using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrabGameMod
{
    class Server
    {
        public static bool onlyAtLobby = false;
        public static bool weaponsDisabled = false;
        public static int uniqueItemId = 10000;
        public static int uniqueNumberId = 10000;
        public static int testUniqueItemId = 10000;
        public static float changeTime = 0;
        public static Dictionary<ulong, bool> spawnedPlayers = new Dictionary<ulong, bool>();
        private static Dictionary<ulong, Player> m_Players = new Dictionary<ulong, Player>();

        private static Dictionary<string, UnityEngine.Vector3> m_TestPos = new Dictionary<string, UnityEngine.Vector3>();
        private static Dictionary<string, float> m_TestAngle = new Dictionary<string, float>();

        private static UnityEngine.Vector3 noClipPosition = new UnityEngine.Vector3(0, 10, 0);

        public static bool noClipEnabled = false;
        public static int testSync = 0;

        private static List<RotatingObject> m_RotatingObjects = new List<RotatingObject>();


        public static Dictionary<int, PhysicsObject> m_PhysicObjects = new Dictionary<int, PhysicsObject>();

        //private static bool hasEnded = false;

        private static float checkAutoDieTime = 0;

        public static Dictionary<ulong, Player> GetPlayers()
        {
            return m_Players;
        }

        public static Ball CreateBall(int objectId)
        {
            var ball = new Ball(objectId);
            m_PhysicObjects.Add(objectId, ball);
            return ball;
        }

        public static RotatingObject CreateRotatingObject(int objectId)
        {
            var o = new RotatingObject(objectId);
            m_PhysicObjects.Add(objectId, o);
            return o;
        }

        public static void OnGameSpawnPlayer(ulong spawnedClientId, int numberId)
        {
            if (!PlayerExists(spawnedClientId)) CreatePlayer(spawnedClientId, numberId);

            GetPlayer(spawnedClientId).SetAlive(true);


            Server.SendServerMessage("spawn");

            if(!spawnedPlayers.ContainsKey(spawnedClientId))
            {
                Server.spawnedPlayers.Add(spawnedClientId, true);

                SendServerMessage(Server.spawnedPlayers + " / 2");
            }
            

        }

        public static void CreatePlayer(ulong clientId, int numberId)
        {
            //var playerManager = MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.activePlayers[(ulong)numberId];
            //steamIdToUID
            //var playerInfo = MonoBehaviourPublicCSDi2UIInstObUIloDiUnique.lobbyPlayers[clientId];

            var player = new Player(clientId, numberId);

            var playersToRemove = new List<Player>();

            foreach(var pair in GetPlayers())
            {
                var p = pair.Value;
                if(p.uid == player.uid)
                {

                    playersToRemove.Add(p);
                }
            }

            foreach(var p in playersToRemove)
            {
                RemovePlayer(p);
            }

            m_Players.Add(clientId, player);
            BepInExLoader.log.LogMessage("Added player " + player.uid);
        }

        public static void RemovePlayer(Player player)
        {
            m_Players.Remove(player.clientId);


            BepInExLoader.log.LogMessage("Removed player " + player.uid);

        }

        public static Player GetPlayer(ulong clientId)
        {
            return m_Players[clientId];
        }

        public static bool PlayerExists(ulong clientId)
        {
            return m_Players.ContainsKey(clientId);
        }

        public static float testSpeed = 0.01f;
        public static float testRadius = 5f;
        public static float testAngle = 0;

        public static void Update()
        {
            changeTime += UnityEngine.Time.deltaTime;

            if(changeTime >= 3f && !IsAtLobby())
            {
                Server.spawnedPlayers.Clear();

                changeTime = 0;

                SendServerMessage("set time");

                MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.gameMode.SetGameModeTimer(2, 0);
            }
            
            if (Server.spawnedPlayers.Count() >= 2)
            {
                Server.spawnedPlayers.Clear();

                int i = 0;
                foreach (var a in MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.activePlayers)
                {
                    i++;
                }

                if( i >= 2)
                {
                    foreach (var pair in Server.GetPlayers())
                    {
                        Server.SendServerMessage("ready");
                        
                        var p = pair.Value;

                        Mod.EmulateInteractReadyButton(p.clientId);
                    }
                }
                    

            }

            foreach (var pair in m_PhysicObjects)
            {
                pair.Value.physicsObject = null;
                pair.Value.rigidbody = null;
            }

            foreach (var o in UnityEngine.Object.FindObjectsOfType<UnityEngine.Rigidbody>())
            {
                var gameObject = o.gameObject;
              
                foreach (var c in gameObject.GetComponents<MonoBehaviourPublicObRiSiupVeSiQuVeLiQuUnique>())
                {
                    var id = c.field_Private_MonoBehaviourPublicInidBoskUnique_0.GetId();


                    if(m_PhysicObjects.ContainsKey(id))
                    {
                        m_PhysicObjects[id].physicsObject = c;
                        m_PhysicObjects[id].rigidbody = gameObject.GetComponent<UnityEngine.Rigidbody>();
                    }

                }

                //MonoBehaviourPublicObRiSiupVeSiQuVeLiQuUnique PhysicsObject
                //MonoBehaviourPublicInidUnique SharedObject



            }

            foreach(var pair in m_PhysicObjects)
            {
                pair.Value.Update();
            }



            /*
            foreach (var a in MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.activePlayers)
            {

                var playerManager = a.value;

                if (playerManager.steamProfile.m_SteamID != Mod.GetMySteamId())
                {
                    toPos = playerManager.gameObject.transform.position;

                }





                i++;
            }

            */

            /*
            var gos = new List<UnityEngine.GameObject>();

            foreach (var p in m_TestAngle.Keys)
            {
                gos.Add(p);
            }

            foreach (var p in gos)
            {
                m_TestAngle[p] += 0.1f;
            }
            */

            //m_TestAngle.up .AddOrUpdate(id, 1, (id, count) => count + 1);

            /*

            var objs = UnityEngine.Object.FindObjectsOfType<UnityEngine.Rigidbody>();
            foreach (var o in objs)
            {
                var gameObject = o.gameObject;

                if (gameObject.name.Contains("Player") || gameObject.name.Contains("Prox")) continue;


                BepInExLoader.log.LogMessage("o.name" + gameObject.name);

                if(!m_TestPos.ContainsKey(gameObject.name))

                {
                    BepInExLoader.log.LogMessage("o.adding" + gameObject.name);


                    m_TestPos.Add(gameObject.name, gameObject.transform.position);
                    m_TestAngle.Add(gameObject.name, 0);
                }

                BepInExLoader.log.LogMessage("calc" + gameObject.name);

                m_TestAngle[gameObject.name] += 0.1f;


                var lDirection = new UnityEngine.Vector3(UnityEngine.Mathf.Sin(m_TestAngle[gameObject.name]), 0, UnityEngine.Mathf.Cos(m_TestAngle[gameObject.name]));

                BepInExLoader.log.LogMessage("calc2" + gameObject.name + ", " + m_TestAngle[gameObject.name]);

                var targetPos = UnityEngine.Vector3.zero;

                
                foreach (var a in MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.activePlayers)
                {
                    var playerManager = a.value;

                    targetPos = playerManager.gameObject.transform.position;
                }
                

                targetPos += lDirection * 3.0f;

                var newPos = UnityEngine.Vector3.Lerp(gameObject.transform.position, targetPos, 1f);

                BepInExLoader.log.LogMessage("calc3" + gameObject.name);

                gameObject.transform.position = newPos;// + (lDirection * testRadius);

                BepInExLoader.log.LogMessage("calc4" + gameObject.name);

            }
            */

            /*
            var toPos = UnityEngine.Vector3.zero;

            var i = 0;
            foreach (var a in MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.activePlayers)
            {
                
                    var playerManager = a.value;

                    if(playerManager.steamProfile.m_SteamID != Mod.GetMySteamId())
                    {
                        toPos = playerManager.gameObject.transform.position;

                    }

                

       

                i++;
            }

            testAngle += testSpeed;

            var lDirection = new UnityEngine.Vector3(UnityEngine.Mathf.Sin(testAngle), 0, UnityEngine.Mathf.Cos(testAngle));
            toPos += (lDirection * testRadius);


            foreach (var a in MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.activePlayers)
            {
                var playerManager = a.value;



                playerManager.transform.position = toPos;

                break;
            }
            */


            //
            if (noClipEnabled)
            {
                var f = (UnityEngine.Input.GetKey(UnityEngine.KeyCode.W) ? 1 : 0) + (UnityEngine.Input.GetKey(UnityEngine.KeyCode.S) ? -1 : 0);
                var r = (UnityEngine.Input.GetKey(UnityEngine.KeyCode.D) ? 1 : 0) + (UnityEngine.Input.GetKey(UnityEngine.KeyCode.A) ? -1 : 0);


                foreach (var a in MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.activePlayers)
                {
                    var playerManager = a.value;

                    if(playerManager.steamProfile.m_SteamID == Mod.GetLobbyOwnerSteamId())
                    {
                        noClipPosition += UnityEngine.Camera.main.transform.forward * f * 0.5f;
                        noClipPosition += UnityEngine.Camera.main.transform.right * r * 0.5f;

                        //playerManager.transform.position = noClipPosition;
                        var rb = playerManager.gameObject.GetComponent<UnityEngine.Rigidbody>();
                        rb.velocity.Set(0, 0, 0);
                        rb.MovePosition(noClipPosition);

                        break;

                    }


                }
            }
            //

            VoteSystem.Update();

            foreach(var pair in GetPlayers()) {
                var player = pair.Value;
                player.isActive = false;

                player.Update();
            }

            foreach (var a in MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.activePlayers)
            {
                var playerManager = a.value;
                var clientId = playerManager.steamProfile.m_SteamID;

                if(playerManager.dead)
                {
                    //playerManager.dead = false;
                    //SendServerMessage("was dead");
                }

                if (PlayerExists(clientId))
                {
                    var rb = playerManager.GetComponent<UnityEngine.Rigidbody>();

                    var player = GetPlayer(clientId);
                    player.SetUsername(playerManager.username);
                    player.position = playerManager.transform.position;
                    player.rigidbody = rb ? rb : null;
                    player.isActive = true;
                }
            }

            if (IsAtLobby())
            {
                foreach (var pair in GetPlayers())
                {
                    pair.Value.kills = 0;
                }
            }

            /*

            try
            {
                var gamemode = MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.gameMode;

                if(gamemode.modeState == GameMode.EnumNPublicSealedvaFrPlEnGa5vUnique.Playing)
                {
                    hasEnded = false;
                }

                if(gamemode.modeState == GameMode.EnumNPublicSealedvaFrPlEnGa5vUnique.GameOver || gamemode.modeState == GameMode.EnumNPublicSealedvaFrPlEnGa5vUnique.Ended)
                {
                    if(!hasEnded)
                    {
                        hasEnded = true;

                        SendServerMessage("end");

                        var winner = Mod.GetLobbyOwnerSteamId();
                        var winner_kills = 0;

                        foreach (var pair in GetPlayers())
                        {
                            
                            var player = pair.Value;

                            if(player.kills > winner_kills)
                            {
                                winner = player.clientId;
                                winner_kills = player.kills;
                            }
                        }

                        MonoBehaviourPublicInInUnique.SendWinner(winner, 1000000 * (ulong)winner_kills);
                    }
                }

                
            }
            catch { }
            */



            var lrobjects = UnityEngine.Object.FindObjectsOfType<ItemData>();
            foreach (var obj in lrobjects)
            {
                obj.currentAmmo = 1000;
            }

            checkAutoDieTime += UnityEngine.Time.deltaTime;
            if(checkAutoDieTime >= 1f)
            {
                checkAutoDieTime = 0f;

                foreach(var pair in GetPlayers())
                {
                    if(pair.Value.autoDie)
                    {
                        KillPlayer(pair.Value);
                    }
                }
            }
        }

        public static bool OnTryBanPlayer(ulong clientId)
        {
            if (!PlayerExists(clientId)) return true;
            return GetPlayer(clientId).canBeBanned;
        }

        public static List<Player> FindPlayers(string selector, bool isAlive = false)
        {
            var players = new List<Player>();

            //*
            //#213
            //a

            foreach (var pair in GetPlayers())
            {
                var player = pair.Value;

                if(isAlive)
                {
                    if (!player.isActive) continue;
                }

                if (selector == "*")
                {
                    players.Add(player);
                    continue;
                }

                if (selector.StartsWith("#"))
                {
                    var id = Int16.Parse(selector.Replace("#", ""));

                    if (player.uid + 1 == id)
                    {
                        players.Add(player);
                    }

                    continue;
                }

                if (player.username.ToLower().Contains(selector.ToLower()))
                {
                    players.Add(player);
                }
            }

            return players;
        }

        public static void ProcessMessage(Player player, string message)
        {
            BepInExLoader.log.LogMessage("ProcessMessage=" + player.username + ", message=" + message);
        
            var guns = new Dictionary<int, string>();
            guns.Add(0, "ak");
            guns.Add(1, "glock");
            guns.Add(2, "revolver");
            guns.Add(3, "dual");
            guns.Add(4, "bat");
            guns.Add(5, "bomb");
            guns.Add(6, "katana");
            guns.Add(7, "knife");
            guns.Add(8, "pipe");
            guns.Add(9, "stick");
            guns.Add(12, "granada");

   

            var gameMode = MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.gameMode;
            //GameModeManager
            //MonoBehaviourPublicGadealGaLi1pralObInUnique.Instance.allGameModes.IndexOf(gameMode);


            var hasPermission = player.clientId == Mod.GetLobbyOwnerSteamId();

            if (message == "!p")
            {
                foreach (var a in MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.activePlayers)
                {
                    SendServerMessage("activePlayers " + a.value.username);
                }
            }

            if (message == "!gm")
            {
                SendServerMessage(gameMode.GetScriptClassName());
            }


            if (message.StartsWith("!"))
            {
                var command = new Command(message);
                var cmd = command.GetCmd();

                int i = 0;
                foreach (var value in guns.Values)
                {
                    if (cmd == value)
                    {
                        var pair = guns.ElementAt(i);
                        var weaponId = pair.Key;
                        var str = "!w ";

                        if (command.HasArg(0))
                        {
                            str += command.GetArgString(0) + " ";
                        }

                        str += weaponId;

                        //SendServerMessage(" >" + str + "<");

                        command = new Command(str);
                        cmd = command.GetCmd();

                        break;
                    }

                    i++;
                }

                if (cmd == "help")
                {
                    SendServerMessage("Comandos: !armas, !ready, !ball [id], !head [id]");
                }

                if (cmd == "kill" && hasPermission)
                {
                    if (!command.HasArg(0)) return;

                    var selector = command.GetArgString(0);

                    FindPlayers(selector, true).ForEach(targetPlayer =>
                    {
                        KillPlayer(targetPlayer);
                    });
                }

                if (cmd == "votekick")
                {
                    if (VoteSystem.HasAnyVoting())
                    {
                        SendServerMessage("Votacao em andamento");
                        return;
                    }

                    var selector = "";

                    if (command.HasArg(0))
                    {
                        selector = command.GetArgString(0);
                    }

                    if (!selector.StartsWith("#") || selector == "")
                    {
                        SendServerMessage("Digite: !votekick #ID");
                        return;
                    }

                    var targetPlayers = FindPlayers(selector);

                    if (targetPlayers.Count == 0)
                    {
                        SendServerMessage("Player nao encontrado");
                        return;
                    }

                    var targetPlayer = targetPlayers[0];

                    var msg = "Expulsar " + targetPlayer.username + " (" + targetPlayer.GetSelector() + ")?  !vote y  |  !vote n";

                    VoteSystem.StartVoting(20, () =>
                    {
                        if (targetPlayer.clientId == Mod.GetLobbyOwnerSteamId())
                        {
                            SendServerMessage("Can't kick this player");
                            return;
                        }

                        KickPlayer(player);
                    }, () =>
                    {
                        SendServerMessage("Sem votos suficientes");
                    });

                    SendServerMessage(msg);
                }

                if (cmd == "vote")
                {
                    if (!command.HasArg(0)) return;

                    var c = command.GetArgString(0).ToLower();

                    if (!VoteSystem.HasAnyVoting())
                    {
                        return;
                    }

                    VoteSystem.RegisterVote(player, c.StartsWith("y"));


                }

                if (cmd == "r" && hasPermission)
                {
                    MonoBehaviourPublicInInUnique.StartGame();
                }


                if (cmd == "over" && hasPermission)
                {
                    MonoBehaviourPublicInInUnique.GameOver(0);
                }

                if (cmd == "win" && hasPermission)
                {
                    ulong money = 100000000;

                    if (!command.HasArg(0)) return;

                    var selector = command.GetArgString(0);

                    var targetPlayers = FindPlayers(selector);
                    if (targetPlayers.Count == 0)
                    {
                        SendServerMessage("Not found");
                        return;
                    }
                    var targetPlayer = targetPlayers[0];

                    if (command.HasArg(1))
                    {
                        money = command.GetArgUlong(1);
                    }

                    MonoBehaviourPublicInInUnique.SendWinner(targetPlayer.clientId, money);
                }

                if (cmd == "tp")
                {
                    MonoBehaviourPublicInInUnique.PlayerPosition(player.clientId, UnityEngine.Vector3.zero);
                }

                if (cmd == "print")
                {
                    SendServerMessage("printed");
                    ModComponent.Print();
                }

                if (cmd == "tp2")
                {
                    var objs = UnityEngine.Object.FindObjectsOfType<UnityEngine.Rigidbody>();
                    foreach (var o in objs)
                    {

                        SendServerMessage("respo");

                        o.MovePosition(UnityEngine.Vector3.up * 5.0f);

                    }
                }

                if (cmd == "map" && hasPermission)
                {
                    var map = command.GetArgInt(0);
                    var mode = command.GetArgInt(1);
                    MonoBehaviourPublicInInUnique.LoadMap(map, mode);
                }

                if (cmd == "cmap" && hasPermission)
                {
                    var map = command.GetArgInt(0);
                    var mode = command.GetArgInt(1);

                    Mod.customMapId = map;
                    Mod.customModeId = mode;
                    Mod.loadCustomMap = true;

                    MonoBehaviourPublicInInUnique.LoadMap(map, mode);
                }

                if (cmd == "radius")
                {
                    var r = command.GetArgFloat(0);

                    SendServerMessage("r" + r);

                    testRadius = r;
                }

                if (cmd == "speed")
                {
                    var r = command.GetArgFloat(0);

                    testSpeed = r / 100;

                    SendServerMessage("s" + r);

                }

                if (cmd == "fix")
                {
                    SendServerMessage("-------------------------------");
                    SendServerMessage("[PT] Mais info em:");
                    SendServerMessage("[EN] More info at:");
                    SendServerMessage("* https://bit.ly/fixcrabgame");
                    SendServerMessage("-------------------------------");

                }

                if (cmd == "sync")
                {
                    var r = command.GetArgInt(0);

                    testSync = r;

                    SendServerMessage("sync=" + r);
                }

                if (cmd == "ready")
                {
                    foreach (var p in MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.activePlayers)
                    {
                        var playerManager = p.value;
                        playerManager.waitingReady = true;
                    }

                }

                if (cmd == "time" && hasPermission)
                {
                    if (!command.HasArg(0)) return;

                    int time = command.GetArgInt(0);

                    //GameManager.Instance.gameMode.modeTime = 200;
                    MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.gameMode.SetGameModeTimer(time, 0);
                }

                if (cmd == "ban" && hasPermission)
                {
                    if (!command.HasArg(0)) return;

                    var selector = command.GetArgString(0);
                    int time = 9999999;

                    if (command.HasArg(1))
                    {
                        time = command.GetArgInt(1);
                    }

                    var players = FindPlayers(selector);

                    if (players.Count > 1)
                    {
                        SendServerMessage("Multiple players");
                        return;
                    }

                    BanPlayer(players[0], time);
                }

                if (cmd == "re")
                {
                    var a = command.GetArgInt(0);

                    if(a == 1)
                    {
                        MonoBehaviourPublicInInUnique.RespawnPlayer(player.clientId, UnityEngine.Vector3.zero);
                    }

                    if (a == 2)
                    {
                        MonoBehaviourPublicInInUnique.PlayerPosition(player.clientId, UnityEngine.Vector3.zero);
                    }

                    if (a == 3)
                    {
                        var arr = new UnhollowerBaseLib.Il2CppStructArray<byte>(0);
                        var n = Server.uniqueNumberId++;
                        MonoBehaviourPublicInInUnique.GameSpawnPlayer(player.clientId, player.clientId, UnityEngine.Vector3.zero, n, false, arr, n);
                    }

                    if (a == 4)
                    {
                        MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.RespawnPlayer(player.clientId, UnityEngine.Vector3.zero);
                    }

                    if (a == 5)
                    {
                        var arr = new UnhollowerBaseLib.Il2CppStructArray<byte>(0);
                        var n = Server.uniqueNumberId++;
                        MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.SpawnPlayer(player.clientId, UnityEngine.Vector3.zero, n, false, arr, n);
                    }


                    SendServerMessage("re " + a);
                }

                    if (cmd == "respawn" && hasPermission)
                {
                    var selector = player.GetSelector();

                    if (command.HasArg(0))
                    {
                        selector = command.GetArgString(0);
                    }

                    FindPlayers(selector).ForEach(targetPlayer =>
                    {
                        RespawnPlayer(targetPlayer);



                    });
                }

                if (cmd == "c")
                {
                    SendServerMessage("<#ff0000>a");
                }

                if (cmd == "t1")
                {
                    var id = MonoBehaviourPublicObInUIgaStCSBoStcuCSUnique.gameAppId;

                    SendServerMessage("lobbyPlayers[]");
                    foreach(var a in MonoBehaviourPublicCSDi2UIInstObUIloDiUnique.lobbyPlayers)
                    {
                        SendServerMessage("lobbyPlayers=" + a.value);
                    }
                }

                if (cmd == "t2")
                {
                    MonoBehaviourPublicObInUIgaStCSBoStcuCSUnique.gameAppId = 85716235;

                    var id = MonoBehaviourPublicObInUIgaStCSBoStcuCSUnique.gameAppId;

                    SendServerMessage("ID = " + id);
                }

                if (cmd == "y1")
                {

                    foreach (var a in MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.activePlayers)
                    {
                        var playerManager = a.value;

                        var id = playerManager.playerNumber;

                        SendServerMessage("ID = " + id);

                    }
                }

                if (cmd == "y2")
                {
                    foreach (var a in MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.activePlayers)
                    {
                        var playerManager = a.value;

                        playerManager.playerNumber = 156238;

                        var id = playerManager.playerNumber;

                        SendServerMessage("ID = " + id);

                    }
                }

               


                if (cmd == "ball")
                {
                    int itemId = 5;

                    if (command.HasArg(0))
                    {
                        itemId = command.GetArgInt(0);
                    }


                    int objectId = DropItemFromPlayer(player, itemId);
                    var ball = CreateBall(objectId);
                    ball.canBePicked = true;
                }

                if (cmd == "head")
                {
                    int itemId = 5;

                    if (command.HasArg(0))
                    {
                        itemId = command.GetArgInt(0);
                    }

                    int objectId = DropItemFromPlayer(player, itemId);

                    var o = CreateRotatingObject(objectId);
                    o.canBePicked = false;
                    o.followPlayer = player;
                }

                if (cmd == "test")
                {
                    var selector = "*";

                    if (command.HasArg(0))
                    {
                        selector = command.GetArgString(0);
                    }

                    FindPlayers(selector).ForEach(targetPlayer =>
                    {
                        SendServerMessage(targetPlayer.GetTestMessage());
                    });
                }

                if (cmd == "lobby" && hasPermission)
                {
                    onlyAtLobby = !onlyAtLobby;
                }

                if (cmd == "tw" && hasPermission)
                {
                    weaponsDisabled = !weaponsDisabled;
                }

                if (cmd == "w")
                {
                    bool canUse = true;

                    if (!IsAtLobby())
                    {
                        if (onlyAtLobby)
                        {
                            if (!hasPermission) canUse = false;
                        }
                    }

                    if (weaponsDisabled)
                    {
                        if (!hasPermission) canUse = false;
                    }

                    if (!canUse) return;

                    //if (!IsAtLobby()) return;


                    if (!command.HasArg(0)) return;

                    var selector = player.GetSelector();
                    var weaponId = 0;

                    if (command.HasArg(1))
                    {
                        selector = command.GetArgString(0);
                        weaponId = command.GetArgInt(1);
                    }
                    else
                    {
                        weaponId = command.GetArgInt(0);
                    }



                    FindPlayers(selector, true).ForEach(targetPlayer =>
                    {
                        GiveWeapon(targetPlayer.clientId, weaponId);
                    });


                }


                if (cmd == "nc") {

                    foreach (var a in MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.activePlayers)
                    {
                        var playerManager = a.value;

                        if (playerManager.steamProfile.m_SteamID == Mod.GetLobbyOwnerSteamId())
                        {
                            noClipPosition = playerManager.transform.position;
                            break;

                        }


                    }

                    noClipEnabled = !noClipEnabled;
                }
                
                    

                if (cmd == "v" && hasPermission)
                {
                    var selector = player.GetSelector();

                    if (command.HasArg(0))
                    {
                        selector = command.GetArgString(0);
                    }

                    FindPlayers(selector).ForEach(targetPlayer =>
                    {
                        targetPlayer.dontStreamMessage = !targetPlayer.dontStreamMessage;
                    });


                }

                if (cmd == "d" && hasPermission)
                {
                    var selector = player.GetSelector();

                    if (command.HasArg(0))
                    {
                        selector = command.GetArgString(0);
                    }

                    FindPlayers(selector).ForEach(targetPlayer =>
                    {
                        targetPlayer.autoDie = !targetPlayer.autoDie;
                    });


                }


                if (cmd == "armas")
                {
                    var str = "Armas: ";

                    foreach (var value in guns.Values)
                    {
                        str += "!" + value + ", ";

                        if (str.Length > 30)
                        {
                            SendServerMessage(str);
                            str = "";
                        }
                    }

                    SendServerMessage(str);
                }

                if (cmd == "a")
                {
                    var lrobjects = UnityEngine.Object.FindObjectsOfType<ItemData>();
                    foreach (var obj in lrobjects)
                    {
                        obj.currentAmmo = 1000;
                    }
                }
            }

            /*

            if (message.StartsWith("!banned"))
            {
                SendServerMessage("Banned:");
                var bannedPlayers = MonoBehaviourPublicCSDi2UIInstObUIloDiUnique.bannedPlayers;
                foreach (ulong id in bannedPlayers)
                {
                    SendServerMessage("- " + id);
                }

           
                Il2CppSystem.Type thisType = UnhollowerRuntimeLib.Il2CppType.Of<MonoBehaviourPublicInInUnique>();
                var theMethod = thisType.GetMethod("SendChatMessage");

                var a = new UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>(0);

      
                a.AddItem("a");
                a.AddItem(");

                theMethod.Invoke(null, Il2CppSystem.Object[] { 1, 2});

                

                //MonoBehaviourPublicInInUnique.SendChatMessage(1, message);



            }


            if (message.StartsWith("!j"))
            {
                SendServerMessage("!j");

                var field_Private_CSteamID_0 = MonoBehaviourPublicObInUIgaStCSBoStcuCSUnique.Instance.field_Private_CSteamID_0;
                var field_Private_CSteamID_1 = MonoBehaviourPublicObInUIgaStCSBoStcuCSUnique.Instance.field_Private_CSteamID_1;

                var originalLobbyOwnerId = MonoBehaviourPublicObInUIgaStCSBoStcuCSUnique.Instance.originalLobbyOwnerId;

                SendServerMessage("field_Private_CSteamID_0: " + field_Private_CSteamID_0.m_SteamID);
                SendServerMessage("field_Private_CSteamID_1: " + field_Private_CSteamID_1.m_SteamID);
                SendServerMessage("originalLobbyOwnerId: " + originalLobbyOwnerId.m_SteamID);

                var lrobjects = UnityEngine.Object.FindObjectsOfType<ItemData>();
                foreach (var obj in lrobjects)
                {
                    //var distanace = Vector3.Distance(Camera.main.transform.position, obj.transform.position);

                    obj.currentAmmo = 1000;

                    SendServerMessage("ItemData ammo");
                }


            }

            */
        }

        public static void SendServerMessage(string message)
        {
            SendChatMessage(1, message);
        }

 
        public static int DropItemFromPlayer(Player player, int itemId)
        {
            int objectId = uniqueItemId++;
            MonoBehaviourPublicObInVoUIVeUIInBoVoByUnique.ForceGiveWeapon(player.clientId, itemId, objectId);
            int newObjectId = uniqueItemId++;
            MonoBehaviourPublicObInVoUIVeUIInBoVoByUnique.ForceGiveWeapon(player.clientId, itemId, newObjectId);
            MonoBehaviourPublicObInVoUIVeUIInBoVoByUnique.ForceRemoveItem(player.clientId, newObjectId);

            return objectId;
        }

        public static void RespawnPlayer(Player player)
        {
            var arr = new UnhollowerBaseLib.Il2CppStructArray<byte>(0);
            var id = testUniqueItemId++;

            try
            {
                MonoBehaviourPublicObInVoUIVeUIInBoVoByUnique.Instance.QueueRespawn(player.clientId, 0.5f);
                MonoBehaviourPublicObInVoUIVeUIInBoVoByUnique.PlayerSpawnRequest(player.clientId, false, arr, id);
                MonoBehaviourPublicInInUnique.GameSpawnPlayer(player.clientId, player.clientId, UnityEngine.Vector3.zero, id, false, arr, id);

            }
            catch
            {

            }
        }

        public static void SendChatMessage(ulong clientId, string message)
        {
            //ServerSend.SendChatMessage(1, message);
            MonoBehaviourPublicInInUnique.SendChatMessage(clientId, message);
        }

        public static void KillPlayer(Player player)
        {
            MonoBehaviourPublicInInUnique.PlayerDied(player.clientId, player.clientId, UnityEngine.Vector3.up);
        }

        public static void BanPlayer(Player player, int time)
        {
            player.canBeBanned = true;
            MonoBehaviourPublicCSDi2UIInstObUIloDiUnique.Instance.BanPlayer(player.clientId);
        }

        public static void KickPlayer(Player player)
        {
            MonoBehaviourPublicCSDi2UIInstObUIloDiUnique.Instance.KickPlayer(player.clientId);
        }

        public static void GiveWeapon(ulong clientId, int weaponId)
        {
            //GameServer.ForceGiveWeapon(clientId, weaponId, uniqueItemId++);

            try
            {
                int objectId = uniqueItemId++;
                MonoBehaviourPublicObInVoUIVeUIInBoVoByUnique.ForceGiveWeapon(clientId, weaponId, objectId);
                //SendServerMessage("Created object " + objectId);

            } catch
            {

            }
        }

        public static bool IsAtLobby()
        {
            return Mod.GetGameModeName().Contains("Waiting");
        }

    }
}
