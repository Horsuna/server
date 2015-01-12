//------------------------------------------------------------------------------
// <auto-generated>
//     Ezt a kódot eszköz generálta.
//     Futásidejű verzió:4.0.30319.18408
//
//     Ennek a fájlnak a módosítása helytelen viselkedést eredményezhet, és elvész, ha
//     a kódot újragenerálják.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using Newtonsoft.Json;
using System.Configuration;

using System.IO;
using System.Net;
using System.Text;
using Unturned;

namespace Unturned
{
    public class RemoteDatabase : IDataHolder
    {
        private String m_banUrl;
        private String m_host;

        #region IDataHolder implementation

        public void Init()
        {
            m_host = ConfigurationSettings.AppSettings["host"];
            m_banUrl = ConfigurationSettings.AppSettings["banUrl"];

            Console.WriteLine("Remote Database initialized: Host: {0} BanURL: {1}", 
                m_host, 
                m_banUrl
            );
        }

        public void AddBan(INetworkBanned banEntry)
        {
            String banEntryJSON = JsonConvert.SerializeObject(banEntry);

            // Create a request for the URL. 
            String response;
            if (JSONRequest.request(m_host + m_banUrl, banEntryJSON, "POST", out response))
            {
                JSONResponse rs = JsonConvert.DeserializeObject<JSONResponse>(response);
                if (!rs.Success)
                {
                    Logger.LogDatabase("Add Ban entry failed: " + banEntryJSON);
                }
            }
            else
            {
                Logger.LogDatabase("Failed adding ban entry: " + banEntryJSON);
            }
        }

        public void RemoveBan()
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.List<INetworkBanned> loadBans()
        {
            throw new NotImplementedException();
        }

        public void AddStructure(string structureStr)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.Dictionary<string, INetworkBanned> LoadBans()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

