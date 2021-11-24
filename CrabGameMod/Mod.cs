using HarmonyLib;
using System;
using UnityEngine;

namespace CrabGameMod
{
    public class Mod
    {
        public static int uniqueItemId = 10000;
        public static GameObject gameObject;

        [HarmonyPostfix]
        public static void Update()
        {


            if (gameObject == null)
            {
                BepInExLoader.log.LogMessage(" ");
                BepInExLoader.log.LogMessage("Bootstrapping Trainer...");

                try
                {
                    gameObject = ModComponent.Create();
                }
                catch (Exception e)
                {
                    BepInExLoader.log.LogMessage("ERROR Bootstrapping Trainer: " + e.Message);
                    BepInExLoader.log.LogMessage(" ");
                }
            }
        }

        [HarmonyPrefix]
        public static bool OnTryBanPlayer(ulong LJNEHPIHNPF)
        {
            ulong id = LJNEHPIHNPF;

            return false;
        }

        [HarmonyPostfix]
        public static void OnAppendMessage(ulong BBJFDBDNGDM, string ABLKDBJILLJ, string EFPJEIMLJGE)
        {
            ulong fromUser = BBJFDBDNGDM;
            string message = ABLKDBJILLJ;
            string fromUsername = EFPJEIMLJGE;

            try
            {
                ProcessMessage(fromUser, message, fromUsername);
            }
            catch (Exception e)
            {
                SendServerMessage("Error: " + e.Message);
            }
        }

        private static void ProcessMessage(ulong fromUser, string message, string fromUsername)
        {
            if (fromUser == 1) return;

            ulong fromClient = ChatUserIdToSteamId(fromUser);
            
            SendServerMessage("from user " + fromUser + ", client=" + fromClient);

            //fromUser == 1 ? LobbyManager.Instance.LEOKPGOOIEC[(int)fromUser] : fromUser;

            if (message.StartsWith("!ak"))
            {
                GiveWeapon(fromClient, 0);
            }

            if (message.StartsWith("!glock"))
            {
                GiveWeapon(fromClient, 1);
            }

            if (message.StartsWith("!doze"))
            {
                GiveWeapon(fromClient, 2);
            }

            if (message.StartsWith("j"))
            {
                SendServerMessage("j?");

                var objects = UnityEngine.Object.FindObjectsOfType<PhysicsObject>();
                foreach (var obj in objects)
                {
                    var distanace = Vector3.Distance(Camera.main.transform.position, obj.transform.position);

                    obj.CKKNFEJKMPI.Set(0, 0, 0);
                    obj.FBOIADADDNB.Set(0, 0, 0);
                    obj.FJJBNFLEEHM.Set(0, 0, 0);
                    obj.NJJFPBLLHMI.Set(0, 0, 0);

                    obj.LIJFNNCNDOL.Set(0, 0, 0);
                    obj.FBOIADADDNB.Set(0, 0, 0);

                    SendServerMessage("SharedObject");
                }

                var lrobjects = UnityEngine.Object.FindObjectsOfType<ItemData>();
                foreach (var obj in lrobjects)
                {
                    //var distanace = Vector3.Distance(Camera.main.transform.position, obj.transform.position);


                    obj.currentAmmo = 10;

                    SendServerMessage("ItemData ammo");
                }

                
            }
        }

        public static void GiveWeapon(ulong clientId, int weaponId)
        {
            GameServer.ForceGiveWeapon(clientId, weaponId, uniqueItemId++);
        }

        public static void SendServerMessage(string message)
        {
            ServerSend.SendChatMessage(1, message);
        }

        private static ulong ChatUserIdToSteamId(ulong userId)
        {
            ulong id = userId;

            if (id == 0) id = SteamManager.Instance.LOLNFHPADPD.m_SteamID;

            return id;
        }

        public static ulong GetMySteamId()
        {
            return SteamManager.Instance.LOLNFHPADPD.m_SteamID;
        }
    }
}
