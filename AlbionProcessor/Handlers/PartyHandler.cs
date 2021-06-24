using AlbionMarshaller;
using AlbionMarshaller.MemoryStorage;
using AlbionMarshaller.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbionProcessor.Handlers
{
    public class PartyHandler
    {
        [AlbionMarshaller.EventHandler(EventCodes.PartyOnClusterPartyJoined)]
        public static void OnPartyOnClusterPartyJoined(Dictionary<byte, object> parameters, ILog log)
        {
            byte[][] partyMembers = parameters[0] as byte[][];

            foreach(byte[] partyMemberBytes in partyMembers)
            {
                Guid playerGuid = new Guid(partyMemberBytes);
                CharacterDB.Instance.PartyMembers.Add(playerGuid);

                Player player = CharacterDB.Instance.FindByGuid(playerGuid);

                if (player != null)
                {
                    Debug.WriteLine("PartyOnClusterPartyJoined: " + player.Name);
                    player.InParty = true;
                }
            }
        }

        [AlbionMarshaller.EventHandler(EventCodes.PartyJoined)]
        public static void OnPartyJoined(Dictionary<byte, object> parameters, ILog log)
        {
            byte[][] partyMembers = parameters[4] as byte[][];

            foreach (byte[] partyMemberBytes in partyMembers)
            {
                Guid playerGuid = new Guid(partyMemberBytes);

                CharacterDB.Instance.PartyMembers.Add(playerGuid);

                Player player = CharacterDB.Instance.FindByGuid(playerGuid);
                if (player != null)
                {
                    Debug.WriteLine("PartyJoined: " + player.Name);
                    player.InParty = true;
                }
            }
        }

        [AlbionMarshaller.EventHandler(EventCodes.PartyDisbanded)]
        public static void OnPartyDisbanded(Dictionary<byte, object> parameters, ILog log)
        {
            foreach(Guid playerGuid in CharacterDB.Instance.PartyMembers)
            {
                Player player = CharacterDB.Instance.FindByGuid(playerGuid);
                if (player != null)
                {
                    player.InParty = false;
                }
            }
            CharacterDB.Instance.PartyMembers.Clear();
        }

        [AlbionMarshaller.EventHandler(EventCodes.PartyPlayerJoined)]
        public static void OnPartyPlayerJoined(Dictionary<byte, object> parameters, ILog log)
        {
            Guid playerGuid = new Guid(parameters[1] as byte[]);
            CharacterDB.Instance.PartyMembers.Add(playerGuid);

            Player player = CharacterDB.Instance.FindByGuid(playerGuid);

            if(player != null)
            {
                Debug.WriteLine("PartyPlayerJoined: " + player.Name);
                player.InParty = true;
            }
        }

        [AlbionMarshaller.EventHandler(EventCodes.PartyPlayerLeft)]
        public static void OnPartyPlayerLeft(Dictionary<byte, object> parameters, ILog log)
        {
            Guid playerGuid = new Guid(parameters[1] as byte[]);
            CharacterDB.Instance.PartyMembers.Remove(playerGuid);

            Player player = CharacterDB.Instance.FindByGuid(playerGuid);

            if (player != null)
            {
                player.InParty = false;
            }
            else
            {
                if (playerGuid.Equals(CharacterDB.Instance.FindByID(-1000).Guid))
                {
                    CharacterDB.Instance.PartyMembers.Clear();
                }
            }
        }
    }
}
