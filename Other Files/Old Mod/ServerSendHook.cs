using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CrabGameMod
{
    class ServerSendHook
    {
        private static Harmony m_Harmony;

        public static void HookAll(Harmony harmony)
        {
            m_Harmony = harmony;

            HookMethod("ChangeColor", typeof(ServerSend));
            HookMethod("FreezePlayers", typeof(ServerSend));
            HookMethod("GameModeAlert", typeof(ServerSend));
            HookMethod("GameOver", typeof(ServerSend));
            HookMethod("GameSpawnPlayer", typeof(ServerSend));
            HookMethod("RespawnPlayer", typeof(ServerSend));
            HookMethod("SpectatorSpawn", typeof(ServerSend));
            HookMethod("GameStartedCooldown", typeof(ServerSend));
            HookMethod("Interact", typeof(ServerSend));
            HookMethod("LoadingSendAllIntoGame", typeof(ServerSend));
            HookMethod("LoadingSendIntoGame", typeof(ServerSend));
            HookMethod("LoadingState", typeof(ServerSend));
            HookMethod("LoadMap", typeof(ServerSend), new Type[] { typeof(int), typeof(int) });
            HookMethod("SendGameModeTimer", typeof(ServerSend), new Type[] { typeof(ulong), typeof(float), typeof(int) });
            HookMethod("SendModeState", typeof(ServerSend));
            HookMethod("SyncClock", typeof(ServerSend));
            HookMethod("PlayerDied", typeof(ServerSend));
            HookMethod("LobbyMapUpdate", typeof(ServerSend));

            //HookMethod("SyncClock", typeof(ServerSend));



            //ServerSend.StartGame
            //ServerSend.StartLobby

            //ServerSend.PlayerActiveItem


            //ServerSend.SendPlayerReady
            //ServerSend.SendReadyPlayers



            //ServerSend.UseItem

        }

        private static void LogMessage(string message)
        {
            MainGUI.WriteLine(message);
        }

        private static void HookMethod(string name, Type type)
        {
            try
            {
                m_Harmony.Patch(
                    AccessTools.Method(type, name),
                    prefix: new HarmonyMethod(AccessTools.Method(typeof(ServerSendHook), name)));
            } catch (Exception e)
            {
                Loader.log.LogMessage("Failed to apply hook " + name);
                Loader.log.LogMessage(e.Message);
            }
            
        }

        private static void HookMethod(string name, Type type, Type[] generics)
        {
            try
            {
                m_Harmony.Patch(
                    AccessTools.Method(type, name, generics),
                    prefix: new HarmonyMethod(AccessTools.Method(typeof(ServerSendHook), name, generics)));
            }
            catch (Exception e)
            {
                Loader.log.LogMessage("Failed to apply hook " + name);
                Loader.log.LogMessage(e.Message);
            }

        }

        [HarmonyPrefix]
        public static bool ChangeColor(ulong steamId, int colorId)
        {
            LogMessage("ChangeColor steamId=" + steamId);
            return true;
        }

        [HarmonyPrefix]
        public static bool FreezePlayers(bool freeze)
        {
            LogMessage("FreezePlayers " + freeze);
            return true;
        }

        [HarmonyPrefix]
        public static bool GameModeAlert(ulong toClient, int i)
        {
            LogMessage("GameModeAlert toClient=" + toClient + ", i=" + i);
            return true;
        }

        [HarmonyPrefix]
        public static bool GameOver(ulong winnerId)
        {
            LogMessage("GameOver winnerId=" + winnerId);
            return true;
        }

        [HarmonyPrefix]
        public static bool GameSpawnPlayer(ulong toClient, ulong spawnedClient, Vector3 spawnPos, int numberId, bool streamerMode)
        {
            LogMessage("GameSpawnPlayer toClient=" + toClient + ", spawnedClient=" + spawnedClient + ", numberId=" + numberId + ", streamerMode=" + streamerMode);

            ModManager.Server.OnGameSpawnPlayer(spawnedClient, numberId);

            return true;
        }

        [HarmonyPrefix]
        public static bool RespawnPlayer(ulong deadClient, Vector3 spawnPos)
        {
            LogMessage("RespawnPlayer deadClient=" + deadClient);

            return true;
        }

        [HarmonyPrefix]
        public static bool SpectatorSpawn(ulong toClient, ulong fromClient)
        {
            LogMessage("SpectatorSpawn toClient=" + toClient + ", fromClient=" + fromClient);

            return true;
        }
      
        [HarmonyPrefix]
        public static bool GameStartedCooldown(ulong toClient, float time)
        {
            LogMessage("GameStartedCooldown toClient=" + toClient + ", time=" + time);

            return true;
        }

        [HarmonyPrefix]
        public static bool Interact(ulong fromClient, int objectId)
        {
            LogMessage("Interact fromClient=" + fromClient + ", objectId=" + objectId);

            return true;
        }

        [HarmonyPrefix]
        public static bool LoadingSendAllIntoGame(ulong steamId)
        {
            LogMessage("LoadingSendAllIntoGame steamId=" + steamId);

            return true;
        }

        [HarmonyPrefix]
        public static bool LoadingSendIntoGame(ulong steamId)
        {
            LogMessage("LoadingSendIntoGame steamId=" + steamId);

            return true;
        }

        [HarmonyPrefix]
        public static bool LoadingState(ulong steamId, LoadingScreen.LoadingScreenState state)
        {
            LogMessage("LoadingState steamId=" + steamId + ", state=" + state);

            return true;
        }

        [HarmonyPrefix]
        public static bool LoadMap(int mapId, int gameModeId)
        {
            LogMessage("LoadMap mapId=" + mapId + ", gameModeId=" + gameModeId);

            if (ModManager.isAtWaitingLobby)
            {
                ModManager.isAtWaitingLobby = false;
                ServerSend.LoadMap(7, 10);


                ModManager.isWaitingForStart = true;
                /*
                 mode.SetGameModeTimer(1000, 1);
                    mode.UpdateTimer();
                */

                return false;
            }

            

            return true;
        }

        [HarmonyPrefix]
        public static bool SendGameModeTimer(ulong toClient, float freezeTime, int modeState)
        {
            LogMessage("SendGameModeTimer toClient=" + toClient + ", freezeTime=" + freezeTime + ", modeState=" + modeState);

            return true;
        }

        [HarmonyPrefix]
        public static bool SendModeState(int state)
        {
            LogMessage("SendModeState state=" + state);

            return true;
        }

        [HarmonyPrefix]
        public static bool SyncClock(ulong toClient, float time)
        {
            LogMessage("SyncClock toClient=" + toClient + ", time=" + time);

            return true;
        }

        [HarmonyPrefix]
        public static bool PlayerDied(ulong deadClient, ulong damageDoerId, Vector3 damageDir)
        {
            LogMessage("PlayerDied deadClient=" + deadClient + ", damageDoerId=" + damageDoerId);

            ModManager.Server.RespawnPlayer(ModManager.Server.GetPlayer(deadClient));

            return false;
        }

        [HarmonyPrefix]
        public static bool LobbyMapUpdate(ulong workshopId, ulong toPlayer)
        {
            LogMessage("LobbyMapUpdate workshopId=" + workshopId + ", toPlayer=" + toPlayer);

            return true;
        }
    }
}
