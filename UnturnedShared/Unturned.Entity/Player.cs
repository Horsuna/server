//------------------------------------------------------------------------------
// <auto-generated>
//     Ezt a kódot eszköz generálta.
//     Futásidejű verzió:4.0.30319.0
//
//     Ennek a fájlnak a módosítása helytelen viselkedést eredményezhet, és elvész, ha
//     a kódot újragenerálják.
// </auto-generated>
//------------------------------------------------------------------------------
using System;

using System.Xml;
using System.Xml.Serialization;

namespace Unturned.Entity
{
	[XmlRootAttribute("Player")]
	public class Player
	{
		[XmlElement("SteamID")]
		public String SteamID { get; set; }

		[XmlElement("Reputation")]
		public int Reputation { get; set; }

		[XmlElement("Name")]
		public String Name { get; set; }

		[XmlElement("Inventory")]
		public PlayerInventory Inventory { get; set; }

		[XmlElement("Clothes")]
		public PlayerClothes Clothes { get; set; }

		[XmlElement("Life")]
		public PlayerLife Life { get; set; }

		[XmlElement("Skills")]
		public PlayerSkills Skills { get; set; }

		[XmlElement("X")]
		public float PositionX { get; set; }
		[XmlElement("Y")]
		public float PositionY { get; set; }
		[XmlElement("Z")]
		public float PositionZ { get; set; }

		[XmlElement("ViewDirection")]
		public float ViewDirection { get; set; }

		public Player ()
		{
		}
	}
}

