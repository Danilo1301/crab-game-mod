using HarmonyLib;
using System;
using UnityEngine;
using UnhollowerRuntimeLib;

namespace CrabGameMod
{
    public class Mod
    {
        public static GameObject gameObject;

        private static bool dontSendServerMessage = false;

        public static bool loadCustomMap = true;
        public static int customMapId = 1;
        public static int customModeId = 7;

        public static int animId = 0;

        [HarmonyPostfix]
        public static void Update()
        {
            
            if(Input.GetKeyDown(KeyCode.R))
            {
                MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, "READY", "INFO");

                //MonoBehaviourPublicInpabyInInInUnique.TryInteract(0);

                foreach (var pair in Server.GetPlayers())
                {
                    var p = pair.Value;

                    EmulateInteractReadyButton(p.clientId);
                }

                

            }

            /*
            if (Input.GetKeyDown(KeyCode.T))
            {
                MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, "GROCK", "INFO");

                int itemId = 1;

                MonoBehaviourPublicInpabyInInInUnique.SendActiveItem(itemId);
                MonoBehaviourPublicInpabyInInInUnique.UseItem(itemId, Vector3.forward);


            }

            if (Input.GetKeyDown(KeyCode.Y))
            {

                MonoBehaviourPublicInInUnique.SyncObject(GetMySteamId(), Server.testSync, true);
                MonoBehaviourPublicInInUnique.PhysicsObjectSnapshot(GetMySteamId(), Server.testSync, Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.identity);

                MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, "sync " + Server.testSync, "INFO");

                int itemId = 1;

                MonoBehaviourPublicInpabyInInInUnique.TryBuyItem(itemId);
                MonoBehaviourPublicInpabyInInInUnique.TryDropItem(itemId, 0, 60);
                MonoBehaviourPublicInpabyInInInUnique.TryDropItem(itemId, 1, 60);
                MonoBehaviourPublicInpabyInInInUnique.TryDropItem(itemId, 100, 60);


            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, "AN " + animId, "INFO");

                MonoBehaviourPublicInpabyInInInUnique.PlayerAnimation(animId, true);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, "resp", "INFO");

                MonoBehaviourPublicInpabyInInInUnique.GameRequestToSpawn(true);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                animId--;
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                animId++;
            }

            */

            //if (GetMySteamId() != GetLobbyOwnerSteamId()) return;

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

            

            Server.Update();
        }

        public static bool TryInteract(ulong param_0, ObjectPublicIDisposableLi1ByInByBoUnique param_1)
        {
            var newPacket = new ObjectPublicIDisposableLi1ByInByBoUnique(param_1.field_Private_ArrayOf_Byte_0);
            newPacket.field_Private_Int32_0 = 8;

            int objectId = newPacket.Method_Public_Int32_Boolean_0(false);

            //if (objectId == 0) return false;

            if (Server.PlayerExists(param_0))
            {
                var player = Server.GetPlayer(param_0);

                if (Server.m_PhysicObjects.ContainsKey(objectId))
                {
                    return Server.m_PhysicObjects[objectId].OnInteract(player);
                }
            }

            
            MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, "TryInteract", "PACKET");
            MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, "readpos=" + param_1.field_Private_Int32_0, "PACKET");
            MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, "objectId=" + objectId, "PACKET");
            printByteArray(param_1.field_Private_ArrayOf_Byte_0);
            

            //printByteArray(param_1.field_Private_ArrayOf_Byte_0);

               

            return true;

        }
        private static void printByteArray(byte[] array)
        {
            var str = "";
            foreach (var b in array)
            {
                str += b + ", ";
            }

            MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, str, "PACKET ARRAY:");
            BepInExLoader.log.LogMessage(str);
        }

        public static void EmulateWeapon(byte weaponId, int uniqueId)
        {
            byte[] readableBuffer = { 12, 0, 0, 0, 30, 0, 0, 0, weaponId, 0, 0, 0, (byte)uniqueId, 39, 0, 0 };

            var newPacket = new ObjectPublicIDisposableLi1ByInByBoUnique(readableBuffer);

            newPacket.field_Private_Int32_0 = 8;
            //testPacket.field_Private_ArrayOf_Byte_0[8] = 4;
            //testPacket.field_Private_List_1_Byte_0[8] = 4;

            MonoBehaviourPublicUIInUIByUIUnique.ForceGiveItem(newPacket);

            MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, "ForceGiveItem EMULATED", "PACKET");

            printByteArray(newPacket.Method_Public_ArrayOf_Byte_0());
        }

        public static void EmulateInteractReadyButton(ulong fromClient)
        {
            byte[] readableBuffer = { 8, 0, 0, 0, 19, 0, 0, 0, 4, 0, 0, 0};

            var newPacket = new ObjectPublicIDisposableLi1ByInByBoUnique(readableBuffer);
            newPacket.field_Private_Int32_0 = 8;

            MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, "TryInteract EMULATED", "PACKET");

            MonoBehaviourPublicPlVoUI9StVoUI9PlLoUnique.TryInteract(fromClient, newPacket);

        }

        public static bool block = true;
        public static ObjectPublicIDisposableLi1ByInByBoUnique testPacket = null;

        private static byte[] copy = null;

        public static bool GameSpawnPlayer(ObjectPublicIDisposableLi1ByInByBoUnique param_0)
        {
            if(copy == null)
            {
                copy = param_0.Method_Public_ArrayOf_Byte_0();
            }
            /*
            MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, "GameSpawnPlayer", "PACKET");
            MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, "readpos=" + param_0.field_Private_Int32_0, "PACKET");
            

            printByteArray(param_0.Method_Public_ArrayOf_Byte_0());
            */
            return true;
        }

        public static bool ReceiveChatMessage(ObjectPublicIDisposableLi1ByInByBoUnique param_0)
        {
            /*
            MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, "ReceiveChatMessage", "PACKET");

            printByteArray(param_0.Method_Public_ArrayOf_Byte_0());
            */

            return true;
        }

        public static bool ForceGiveItem(ObjectPublicIDisposableLi1ByInByBoUnique param_0)
        {
            var array = param_0.Method_Public_ArrayOf_Byte_0();

            /*
            printByteArray(array);
            */
           

            //var newPacket = new ObjectPublicIDisposableLi1ByInByBoUnique(new byte[] { 1, 2, 3});


            //12, 0, 0, 0, 30, 0, 0, 0, 3, 0, 0, 0, 18, 39, 0, 0 doze
            //12, 0, 0, 0, 30, 0, 0, 0, 1, 0, 0, 0, 16, 39, 0, 0 glock

            //12, 0, 0, 0, 30, 0, 0, 0, 1, 0, 0, 0, 22, 39, 0, 0



            return true;
        }

        public static bool OnLoadMap(int param_0, int param_1, ulong param_2)
        {
            Server.spawnedPlayers.Clear();

            //BepInExLoader.log.LogMessage("OnLoadMap_1 " + param_0 + ", " + param_1 + ", " + param_2);
            return true;
        }

        public static bool OnLoadMap(int param_0, int param_1)
        {
            Server.spawnedPlayers.Clear();

            /*
            BepInExLoader.log.LogMessage("== OnLoadMap == ");
            BepInExLoader.log.LogMessage(param_0 + ", " + param_1);
            BepInExLoader.log.LogMessage("> " + loadCustomMap + " - " + customMapId + " - " + customModeId);
            BepInExLoader.log.LogMessage("====");

            

            if(loadCustomMap && param_1 != 0)
            {
                loadCustomMap = false;
                BepInExLoader.log.LogMessage("loading custom");

                MonoBehaviourPublicInInUnique.LoadMap(customMapId, customModeId);
                return false;
            }

            loadCustomMap = true;

            */


            return true;
        }


        public static bool OnSendChatMessage(ulong param_0, string param_1)
        {
            if (GetMySteamId() != GetLobbyOwnerSteamId()) return true;

            if(!Server.PlayerExists(param_0))
            {
                if(dontSendServerMessage)
                {
                    AppendLocalMessage("Server: Private: " + param_1);
                    return false;
                } else
                {
                    return true;
                }
            }

            var player = Server.GetPlayer(param_0);

            

            if (!player.canMessageBypass)
            {
                BepInExLoader.log.LogMessage("OnSendChatMessage param_0=" + param_0 + " param_1=" + param_1);

                if(player.dontStreamMessage)
                {
                    AppendLocalMessage("Private: " + param_1);

                    dontSendServerMessage = true;
                } else
                {
                    player.canMessageBypass = true;
                    Server.SendChatMessage(param_0, "["+ player.GetSelector() +"] " + param_1);
                    
                }

                Server.ProcessMessage(player, param_1);

                dontSendServerMessage = false;

                return false;
            }
            
            BepInExLoader.log.LogMessage("OnSendChatMessage bypassed");
            player.canMessageBypass = false;

           

            return true;
        }

        [HarmonyPrefix]
        public static bool OnTryBanPlayer(ulong param_1)
        {
            if (GetMySteamId() != GetLobbyOwnerSteamId()) return true;

            ulong clientId = param_1;
            return Server.OnTryBanPlayer(clientId);
        }

        public static void AppendLocalMessage(string message)
        {
            MonoBehaviourPublicRaovTMinTemeColoonCoUnique.Instance.AppendMessage(0, message, "NAME");
        }

        [HarmonyPostfix]
        public static bool OnAppendMessage(ulong param_1, string param_2, string param_3)
        {
            if (GetMySteamId() != GetLobbyOwnerSteamId()) return true;

            return true;

            /*
            ulong fromUser = param_1;
            string message = param_2;
            string fromUsername = param_3;

            try
            {
                return Server.ProcessMessage(fromUser, message, fromUsername);
            }
            catch (Exception e)
            {
                Server.SendServerMessage("Error: " + e.Message);
            }

            return false;
            */
        }

        public static bool OnPlayerDied(ulong param_0, ulong param_1, Vector3 param_2)
        {
            if (GetMySteamId() != GetLobbyOwnerSteamId()) return true;
                
            ulong deadClient = param_0;
            ulong damageDoerId = param_1;
            Vector3 damageDir = param_2;

            if(Server.PlayerExists(deadClient) && Server.PlayerExists(damageDoerId))
            {
                var deadPlayer = Server.GetPlayer(deadClient);
                var damageDoerPlayer = Server.GetPlayer(damageDoerId);

                deadPlayer.SetAlive(false);

                if (deadPlayer == damageDoerPlayer)
                {
                    

                    if (!deadPlayer.autoDie)
                        Server.SendServerMessage(deadPlayer.username + " died");

                }
                else
                {
                    damageDoerPlayer.kills++;


                    Server.SendServerMessage(damageDoerPlayer.username + " killed " + deadPlayer.username);
                    //Server.SendServerMessage(damageDoerPlayer.username + " killed " + deadPlayer.username + "(" + damageDoerPlayer.kills + "kills)");

                }
            }

            return true;
        }

        public static void OnGameSpawnPlayer(ulong param_0, ulong param_1, Vector3 param_2, int param_3, bool param_4, UnhollowerBaseLib.Il2CppStructArray<byte> param_5, int param_6)
        {
            /*
            SendServerMessage("param_0=" + param_0);
            SendServerMessage("param_1=" + param_1);
            SendServerMessage("param_2=" + "Vector3");
            SendServerMessage("param_3=" + param_3);
            SendServerMessage("param_4=" + param_4);
            SendServerMessage("param_5=" + "byte_arr");
            SendServerMessage("param_6=" + param_6);
            */

            var clientId = param_0;
            var numberId = param_6;

            Server.OnGameSpawnPlayer(param_0, param_6);
        }

        public static ulong ChatUserIdToSteamId(ulong userId)
        {
            ulong id = userId;

            //SteamManager.Instance.originalLobbyOwnerId;
            var originalLobbyOwnerId = MonoBehaviourPublicObInUIgaStCSBoStcuCSUnique.Instance.originalLobbyOwnerId;

            if (id == 0) id = GetLobbyOwnerSteamId();

            return id;
        }
        
        public static ulong GetLobbyOwnerSteamId()
        {
            return MonoBehaviourPublicObInUIgaStCSBoStcuCSUnique.Instance.originalLobbyOwnerId.m_SteamID;
        }

        public static ulong GetMySteamId()
        {
            return MonoBehaviourPublicObInUIgaStCSBoStcuCSUnique.Instance.prop_CSteamID_0.m_SteamID;
        }

        public static string GetGameModeName()
        {
            try
            {
                return MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.gameMode.GetScriptClassName();
            }
            catch
            {

            }

            return "Error";
        }
    }
}


/*
 * ClientHandle? = MonoBehaviourPublicUIInUIByUIUnique
 * LobbyManager = MonoBehaviourPublicCSDi2UIInstObUIloDiUnique
 * GameServer = MonoBehaviourPublicObInVoUIVeUIInBoVoByUnique
 * ServerSend = MonoBehaviourPublicInInUnique
 * GameManager = MonoBehaviourPublicDi2UIObacspDi2UIObUnique
 * ClientSend = MonoBehaviourPublicInpabyInInInUnique
 * ServerHandle = MonoBehaviourPublicPlVoUI9StVoUI9PlLoUnique
*/