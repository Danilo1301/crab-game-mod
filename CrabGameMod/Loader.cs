using System;
using BepInEx;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace CrabGameMod
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class BepInExLoader : BepInEx.IL2CPP.BasePlugin
    {
        public static BepInEx.Logging.ManualLogSource log;

        public const string
            MODNAME = "Trainer",
            AUTHOR = "wh0am15533",
            GUID = "com." + AUTHOR + "." + MODNAME,
            VERSION = "1.0.0.0";

        public BepInExLoader()
        {
            AppDomain.CurrentDomain.UnhandledException += ExceptionHandler;
            Application.runInBackground = true;
            log = Log;
        }

        private static void ExceptionHandler(object sender, UnhandledExceptionEventArgs e) => log.LogError("\r\n\r\nUnhandled Exception:" + (e.ExceptionObject as Exception).ToString());

        public override void Load()
        {
            Log.LogMessage("Registering C# Type's in Il2Cpp");

            try
            {
                ClassInjector.RegisterTypeInIl2Cpp<ModComponent>();
            }
            catch
            {
                log.LogError("FAILED to Register Il2Cpp Type!");
            }

            try
            {
                var harmony = new Harmony("wh0am15533.trainer.il2cpp");

                harmony.Patch(AccessTools.Method(typeof(Chatbox), "Update"),
                    postfix: new HarmonyMethod(AccessTools.Method(typeof(Mod), "Update")));

                harmony.Patch(AccessTools.Method(typeof(LobbyManager), "BanPlayer"),
                   prefix: new HarmonyMethod(AccessTools.Method(typeof(Mod), "OnTryBanPlayer")));

                harmony.Patch(AccessTools.Method(typeof(Chatbox), "AppendMessage"),
                    postfix: new HarmonyMethod(AccessTools.Method(typeof(Mod), "OnAppendMessage")));

                log.LogMessage("Runtime Hooks's Applied");
                log.LogMessage(" ");
            }
            catch { log.LogError("FAILED to Apply Hooks's!"); }
        }



        
    }
}
