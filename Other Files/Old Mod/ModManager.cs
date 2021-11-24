using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CrabGameMod
{
   

    public class ModManager
    {
        public static bool isAtWaitingLobby = false;
        public static bool isWaitingForStart = false;

        public class Player
        {
            public readonly ulong clientId;
            public readonly int numberId;

            public Player(ulong clientId, int numberId)
            {
                this.clientId = clientId;
                this.numberId = numberId;
            }

            public void Test()
            {
                Server.SendServerMessage("clientId= " + clientId);
                Server.SendServerMessage("numberId= " + numberId);
            }

            public void Respawn()
            {
                Server.RespawnPlayer(this);
            }
        }

        public class Server
        {
            private static Dictionary<ulong, Player> _players = new Dictionary<ulong, Player>();

            public static Dictionary<ulong, Player> GetPlayers()
            {
                return _players;
            }

            public static void OnGameSpawnPlayer(ulong spawnedClientId, int numberId)
            {
                if(!PlayerExists(spawnedClientId)) CreatePlayer(spawnedClientId, numberId);
            }

            public static void CreatePlayer(ulong clientId, int numberId)
            {
                var player = new Player(clientId, numberId);
                _players.Add(clientId, player);
            }
            public static Player GetPlayer(ulong clientId)
            {
                return _players[clientId];
            }

            public static bool PlayerExists(ulong clientId)
            {
                return _players.ContainsKey(clientId);
            }

            public static void TestPlayers()
            {
                foreach(var pair in _players)
                {
                    pair.Value.Test();
                }
            }

            public static void SendServerMessage(string message)
            {
                ServerSend.SendChatMessage(1, message);
            }

            public static void ProcessCommand(Player player, Command command)
            {
                if(command.GetCommand() == "test")
                {
                    TestPlayers();
                }

                if(command.GetCommand() == "respawn")
                {
                    SendServerMessage("Respawn");
                    player.Respawn();
                }

                if (command.GetCommand() == "redlight")
                {
                    var b = command.GetArgBool(0);
                    var reactionTime = command.getArgFloat(1);

                    ServerSend.RedLight(player.clientId, b, reactionTime);
                }

                if (command.GetCommand() == "piecefall")
                {
                    var index = command.GetArgInt(0);

                    ServerSend.PieceFall(index);
                }

       
                if(command.GetCommand() == "test1")
                {
                    var allObjects = UnityEngine.Object.FindObjectsOfType<PlayerManager>();
                    foreach (var c in allObjects)
                    {

                        SendServerMessage("re=" + c.waitingReady + " de=" + c.dead);

                        c.dead = false;
                        c.waitingReady = !c.waitingReady;

                    }

    
                }

                if(command.GetCommand() == "gb")
                {
                    var gameMode = GameManager.Instance.gameMode;

                    if(gameMode)
                    {
                        var g = gameMode.GetGoodBadPlayers().Item1;
                        var b = gameMode.GetGoodBadPlayers().Item2;


                        SendServerMessage("g=" + g + "b="+b);

                        /*
                        LobbyManager.lobbyPlayers;
                        LobbyManager.Instance.nextRoundPlayers;

                        GameManager.Instance.activePlayers;
                        */

                        var gamemodeWaiting = gameMode as GameModeWaiting;

                        //LobbyManager.lobbyPlayers[0].
                    }
                }
            }

            public static void RespawnPlayer(Player player)
            {


                /*
                ServerSend.PlayerPosition(player.clientId, position);


                GameServer.Instance.RespawnPlayer(player.clientId, 0.1f);
                GameManager.Instance.RespawnPlayer(player.clientId, position);

                //test:
                //GameServer.Instance.QueueRespawn()

                GameServer.Instance.RespawnPlayer(player.clientId, 0.3f);
                GameServer.Instance.QueueRespawn(player.clientId, 0.6f);

                ServerSend.RespawnPlayer(player.clientId, position);

                ServerSend.LoadingSendIntoGame(player.clientId);

                foreach (var pair in _players)
                {
                    var toSendPlayer = pair.Value;

                    //toSendPlayer.clientId

                    ServerSend.GameSpawnPlayer(toSendPlayer.clientId, player.clientId, position, player.numberId, false);
                }

                ServerSend.PlayerPosition(player.clientId, position);
                */

                var position = SpawnZoneManager.Instance.FindGroundedSpawnPosition();
                position.y += 2.0f;

                //GameServer.Instance.QueueRespawn(player.clientId, 0f);

                ServerSend.RespawnPlayer(player.clientId, position);
                GameManager.Instance.RespawnPlayer(player.clientId, position);
                ServerSend.PlayerPosition(player.clientId, position);
            }

            public static void SendErrorMessageNeedAlive()
            {
                SendServerMessage("Voce precisa estar vivo");
            }
        }

        
    }
}
