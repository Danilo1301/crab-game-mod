using HarmonyLib;
using System;
using UnityEngine;
using BepInEx;


/*
namespace CrabGameMod
{
    class Command
    {
        private string _text;

        public Command(string text)
        {
            _text = text;
            Console.WriteLine(text);


        }

        public string SplitString(int index)
        {
            return _text.Split(' ')[index];
        }

        public string GetCommand()
        {
            return SplitString(0).Replace("!", "");
        }

        public int GetArgInt(int index)
        {
            return Int32.Parse(SplitString(index+1));
        }

        public ulong GetArgUlong(int index)
        {
            return (ulong)Int32.Parse(SplitString(index + 1));
        }

        public string GetArgString(int index)
        {
            return SplitString(index + 1);
        }
    }

    class TestComponent : MonoBehaviour
    {


        internal static GameObject Create(string name)
        {


            var obj = new GameObject(name);
            DontDestroyOnLoad(obj);
            var component = new TestComponent(obj.AddComponent(UnhollowerRuntimeLib.Il2CppType.Of<TestComponent>()).Pointer);
            return obj;
        }

        public TestComponent(IntPtr intPtr) : base(intPtr) { }


        public static float sendMessageTime = 0.0f;
        public static float resetGunsTime = 0.0f;

        public static void PrintServerMessage(string message)
        {
            ServerSend.SendChatMessage(1, message);

            //ServerSend.chat

        }

        [HarmonyPrefix]
        public static void OnPlayerSpawnOrDespawn(ulong playerId)
        {
            PrintServerMessage("player spawn/despawn");
        }

        [HarmonyPrefix]
        public static bool BanPlayer(ulong id)
        {
            //Loader.log.LogMessage("baan");

            //

            PrintServerMessage("Ban attempt");

            if (SteamManager.Instance.PlayerSteamId.m_SteamID == id) return false;

            

            return true;
        }

        [HarmonyPostfix]
        public static void ReceiveChatMessage(ulong fromClient, Packet packet)
        {
           
        }
            
        public static ulong FindClientIdByIndex(int index)
        {
            var i = 1;
            var clients = LobbyManager.Instance.clients;
            foreach (var client in clients)
            {

                if (i == index)
                {
                    return client.steamId.m_SteamID;
                }
                i++;


            }
            return 0;
        }

        public static void RespawnPlayer(ulong steamId)
        {

            var position = SpawnZoneManager.Instance.FindGroundedSpawnPosition();
            //GameManager.Instance.RespawnPlayer(client.steamId.m_SteamID, position);
            ServerSend.RespawnPlayer(steamId, position);
            ServerSend.PlayerPosition(steamId, position);
        }

        private static ulong ChatUserIdToSteamId(ulong userId)
        {
            ulong id = userId;

            if (id == 0) id = SteamManager.Instance.PlayerSteamId.m_SteamID;

            return id;
        }

        private static void ProcessMessage(ulong fromUser, string message, string fromUsername)
        {
            bool isTester = SteamManager.Instance.PlayerSteamId.m_SteamID == 76561198802428350;

            if (isTester) return;

            //bool isTester = SteamManager.Instance.PlayerSteamId.m_SteamID == 76561198802428350;

            bool isAdmin = fromUsername.EndsWith("Danilo");

            Loader.log.LogMessage("AppendMessage() fromUser=" + fromUser + ", message=" + message + ", fromUsername=" + fromUsername + " isAdmin=" + isAdmin);

            if (message.StartsWith("!"))
            {
                var command = new Command(message);

                var cmd = command.GetCommand();

                if (cmd == "weapon")
                {
                    var toClientIndex = command.GetArgInt(0);
                    ulong toClient = FindClientIdByIndex(toClientIndex);

                    if (toClient == 0)
                    {
                        PrintServerMessage("Player not found");
                    }
                    else
                    {
                        var item = command.GetArgInt(1);
                        //var obj = command.GetArgInt(2);

                        GameServer.ForceGiveWeapon(toClient, item, 0);
                    }


                }

                if(cmd == "myid")
                {
                    PrintServerMessage("ID:" + ChatUserIdToSteamId(fromUser));
                }

                if (cmd == "test1")
                {
                    GameManager.Instance.gameMode.InitMode();
                }

                if (cmd == "test2")
                {

                    GameManager.Instance.gameMode.StartRound();
                }

                if (cmd == "test3")
                {
                    //GameManager.Instance.gameMode.modeTime = 200;
                    GameManager.Instance.gameMode.SetGameModeTimer(100, 0);
                }

                if (cmd == "test4")
                {
                    foreach (var p in GameManager.Instance.activePlayers)
                    {
                        PrintServerMessage("found a player in GameManager");
                    }

                }

                if(cmd == "ready")
                {
                    foreach (var p in GameManager.Instance.activePlayers)
                    {
                        var playerManager = p.value;
                        playerManager.waitingReady = true;
                    }

                }

                if (cmd == "testhp")
                {
                    foreach (var p in GameManager.Instance.activePlayers)
                    {
                        var playerManager = p.value;

                        var status = playerManager.GetComponent<PlayerStatus>();

                        status.currentHp = 30;

                        PrintServerMessage(playerManager.username + ", currentHp=" + status.currentHp + " / " + status.maxHp);
                    }

                }

                if (cmd == "help")
                {
                    PrintServerMessage("Comandos: !glock !bomb !faca !bat !katana !granada !ready");
                }

                if (cmd == "glock")
                {
                    var id = ChatUserIdToSteamId(fromUser);

                    GameServer.ForceGiveWeapon(id, 1, uniqueItemId++);
                }

                if (cmd == "bomb")
                {
                    var id = ChatUserIdToSteamId(fromUser);

                    GameServer.ForceGiveWeapon(id, 4, uniqueItemId++);
                }

                if (cmd == "bat")
                {
                    var id = ChatUserIdToSteamId(fromUser);

                    GameServer.ForceGiveWeapon(id, 3, uniqueItemId++);
                }

                if (cmd == "katana")
                {
                    var id = ChatUserIdToSteamId(fromUser);

                    GameServer.ForceGiveWeapon(id, 5, uniqueItemId++);
                }

                if (cmd == "faca")
                {
                    var id = ChatUserIdToSteamId(fromUser);

                    GameServer.ForceGiveWeapon(id, 6, uniqueItemId++);
                }

                if (cmd == "granada")
                {
                    var id = ChatUserIdToSteamId(fromUser);

                    GameServer.ForceGiveWeapon(id, 11, uniqueItemId++);
                }

                if (cmd == "weaponall")
                {
                    int weaponId = command.GetArgInt(0);
                    GameServer.ForceGiveAllWeapon(weaponId);


                }

                if (cmd == "weaponall")
                {
                    int weaponId = command.GetArgInt(0);
                    GameServer.ForceGiveAllWeapon(weaponId);


                }

                if (cmd == "test5")
                {
                    //GameManager.Instance.
                }

                    if (cmd == "b")
                {
                    Chatbox.Instance.SendMessage("id" + SteamManager.Instance.PlayerSteamId.m_SteamID);



                    foreach (var p in PlayerList.Instance.players)
                    {
                        PrintServerMessage("found a player PlayerList");
                    }


                    foreach (var p in LobbyManager.lobbyPlayers)
                    {
                        PrintServerMessage("found a player LobbyManager");
                    }



                }

                if (cmd == "r")
                {
                    var clients = LobbyManager.Instance.GetClients();
                    foreach (var client in clients)
                    {
                        RespawnPlayer(client.steamId.m_SteamID);
                    }
                    ServerSend.StartGame();
                }

                if (cmd == "start")
                {

                    ServerSend.StartGame();

                }

                if (cmd == "over")
                {
                    ServerSend.GameOver(0);
                }

                if (cmd == "respawnall")
                {
                    var clients = LobbyManager.Instance.GetClients();
                    foreach (var client in clients)
                    {
                        RespawnPlayer(client.steamId.m_SteamID);
                    }
                }

                if (cmd == "test")
                {

                }

                if (cmd == "kill")
                {
                    var toClientIndex = command.GetArgInt(0);
                    ulong toClient = FindClientIdByIndex(toClientIndex);

                    if (toClient == 0)
                    {
                        PrintServerMessage("Player not found");
                    }
                    else
                    {
                        ServerSend.PlayerDied(toClient, 0, Vector3.zero);
                    }


                }

                if (cmd == "respawn")
                {
                    var toClientIndex = command.GetArgInt(0);
                    ulong toClient = FindClientIdByIndex(toClientIndex);

                    if (toClient == 0)
                    {
                        toClient = FindClientIdByIndex(1); ;
                    }

                    var position = SpawnZoneManager.Instance.FindGroundedSpawnPosition();

                    PrintServerMessage("" + toClient);

                    GameServer.Instance.RespawnPlayer(toClient, 1000);
                    GameServer.Instance.RespawnPlayer(0, 1000);

                    //ServerSend.PlayerPosition(toClient, Vector3.zero);

                    //GameManager.Instance.RespawnPlayer(toClient, position);
                    //ServerSend.PlayerPosition(toClient, position);

                }

                if (cmd == "map")
                {
                    var map = command.GetArgInt(0);
                    var mode = command.GetArgInt(1);
                    ServerSend.LoadMap(map, mode);


                }

                if (cmd == "ready")
                {
                    //LobbyManager.Instance.pla
                }

                if (cmd == "p")
                {
                    PrintServerMessage("Clients:");


                    var clients = LobbyManager.Instance.clients;
                    int i = 1;
                    foreach (var client in clients)
                    {
                        ulong steamId = client.steamId.m_SteamID;



                        i++;

                        if (steamId == 0) continue;

                        PrintServerMessage("[" + (i - 1) + "] " + steamId);
                    }

                    //PrintServerMessage("Players:");

                }
            }
        }

        [HarmonyPostfix]
        public static void AppendMessage(ulong fromUser, string message, string fromUsername)
        {
            try
            {
                ProcessMessage(fromUser, message, fromUsername);
            } catch(Exception e)
            {
                PrintServerMessage("Error: " + e.Message);
            }
        }

        public static int uniqueItemId = 0;

        [HarmonyPostfix]
        public static void Update()
        {
            sendMessageTime += Time.deltaTime;
            resetGunsTime += Time.deltaTime;

            if(resetGunsTime >= 30)
            {
                resetGunsTime = 0;

                PrintServerMessage("Digite !help para ver os comandos");

                //GameServer.ForceRemoveAllWeapons();
            }

            if (sendMessageTime <= 2) return;
            sendMessageTime = 0;

            
    
            var clients = LobbyManager.Instance.clients;
            foreach (var client in clients)
            {
                ulong id = client.steamId.m_SteamID;

                if (id == 0) continue;

                if (!GameManager.Instance.activePlayers.ContainsKey(id)) continue;
                
          
                bool hasWeapon = false;

                //PrintServerMessage("Player " + id + ":");

                foreach (var itemData in client.inventory)
                {
                    if (!itemData) continue;

                    //PrintServerMessage("- has " + itemData.itemName + "|" + itemData.itemID + "|" + itemData.objectID);

                    if (itemData.itemID == 1)
                    {
                        hasWeapon = true;

                        //itemData.currentAmmo = 100;
                    }
                }


                if (!hasWeapon)
                {

                    GameServer.ForceGiveWeapon(id, 1, uniqueItemId++);
                    //PrintServerMessage("Giving weapon");
                }

            }


              
        }
    }
}

*/