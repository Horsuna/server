using UnityEngine;
using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DataHolder
{
	public class FileDatabase
	{
		private readonly static String DATE_PATTERN = "MM-dd-yy H:mm:ss";
		private readonly static String BAN_DATABASE_FILE = "Unturned_Data/Database/Bans.txt";
		private readonly static String STRUCTURE_DATABASE_FILE = "Unturned_Data/Database/Structures.txt";
		private readonly static String DATA_DIRECTORY = "Unturned_Data/Database";
		
		static FileDatabase ()
		{
			if (!Directory.Exists (DATA_DIRECTORY)) {
				Directory.CreateDirectory ("Unturned_Data/Database");
			}
		}
		
		public static void SaveBans (Dictionary<String, NetworkBanned> bans)
		{
			FileStream banFile = new FileStream (BAN_DATABASE_FILE, FileMode.Create);
			StreamWriter writer = new StreamWriter (banFile);
			
			foreach (KeyValuePair<String, NetworkBanned> pair in bans) {
				NetworkBanned bannedPlayer = pair.Value;
				writer.WriteLine ("{0} {1} {2} [{3}] [{4}]",
				                 bannedPlayer.id,
				                 bannedPlayer.banTime.ToString (DATE_PATTERN),
				                 bannedPlayer.bannedBy,
				                 bannedPlayer.name,
				                 bannedPlayer.reason);
			}
			
			writer.Flush ();
			writer.Close ();
			banFile.Close ();
		}
		
		public static Dictionary<String, NetworkBanned> LoadBans ()
		{
			FileStream banFile = new FileStream (BAN_DATABASE_FILE, FileMode.OpenOrCreate);
			StreamReader reader = new StreamReader (banFile);
			
			Dictionary<String, NetworkBanned> steamBans = new Dictionary<String, NetworkBanned> ();
			
			String line = "";
			do {
				line = reader.ReadLine ();
				if (line != null) {
				
					string re1 = "([0-9]{17})";	// Steam ID < banned
					string re2 = "(\\s+)";	// White Space 1
					string re3 = "((?:[0]?[1-9]|[1][012])[-:\\/.](?:(?:[0-2]?\\d{1})|(?:[3][01]{1}))[-:\\/.](?:(?:\\d{1}\\d{1})))(?![\\d])";	// MMDDYY 1
					string re4 = "(\\s+)";	// White Space 2
					string re5 = "((?:(?:[0-1][0-9])|(?:[2][0-3])|(?:[0-9])):(?:[0-5][0-9])(?::[0-5][0-9])?(?:\\s?(?:am|AM|pm|PM))?)";	// HourMinuteSec 1
					string re6 = "(\\s+)";	// White Space 3
					string re7 = "([0-9]{17})";	// Steam ID < banned by
					string re8 = "(\\s+)";	// White Space 4
					string re9 = "(\\[.*?\\])";	// Square Braces 1
					string re10 = "(\\s+)";	// White Space 5
					string re11 = "(\\[.*?\\])";	// Square Braces 2
					
					String regexString = re1 + re2 + re3 + re4 + re5 + re6 + re7 + re8 + re9 + re10 + re11;
					
					Regex r = new Regex (regexString, RegexOptions.IgnoreCase | RegexOptions.Singleline);
					Match m = r.Match (line);
					if (m.Success) {
						DateTime banTime;
						String steamId = m.Groups [1].ToString ();
						String dateString = m.Groups [3].ToString ();
						String timeString = m.Groups [5].ToString ();
						String bannedBy = m.Groups [7].ToString ();
						// TODO: make it better
						String nick = m.Groups [9].ToString ().Replace ("[", "").Replace ("]", "");
						String reason = m.Groups [11].ToString ().Replace ("[", "").Replace ("]", "");
					
						DateTime.TryParseExact (
							dateString + " " + timeString, 
							DATE_PATTERN, 
							null, 
							DateTimeStyles.None, 
							out banTime
						);
						steamBans.Add (steamId, new NetworkBanned (nick, steamId, reason, bannedBy, banTime));
					}
				}				
			} while (!reader.EndOfStream);
			    
			reader.Close ();
			banFile.Close ();			
			return steamBans;
		}
		
		// First version!
		public static void SaveStructures (String structureStr)
		{
			FileStream structureFile = new FileStream (STRUCTURE_DATABASE_FILE, FileMode.Create);
			StreamWriter writer = new StreamWriter (structureFile);

			String[] structures = structureStr.Split (';');

			// Writing line by line
			foreach (String structure in structures) {
				writer.WriteLine (structure);
			}

			Debug.Log (structures.Length + " structures saved.");

			writer.Close ();
			structureFile.Close ();
		}
	}
}

