using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace CrabGameMod
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    class Loader : BepInEx.IL2CPP.BasePlugin
    {
        public const string pluginGuid = "com.danilo1301.crabgamemod";
        public const string pluginName = "Crab Game Mod";
        public const string pluginVersion = "1.0.0.0";

        public static BepInEx.Logging.ManualLogSource log;

        public Loader()
        {
            log = Log;
        }

        public override void Load()
        {
            Log.LogMessage("Working");

            Log.LogMessage("Registering C# Type's in Il2Cpp");

            try
            {
                ClassInjector.RegisterTypeInIl2Cpp<Bootstrapper>();
                ClassInjector.RegisterTypeInIl2Cpp<ModComponent>();
            }
            catch
            {
                log.LogError("FAILED to Register Il2Cpp Type!");
            }

            try
            {
                log.LogMessage(" ");
                log.LogMessage("Inserting Harmony Hooks...");

                var harmony = new Harmony("wh0am15533.trainer.il2cpp");


                #region[Update() Hook - Only Needed for Bootstrapper]

                var originalUpdate = AccessTools.Method(typeof(Chatbox), "Update");
                log.LogMessage("   Original Method: " + originalUpdate.DeclaringType.Name + "." + originalUpdate.Name);
                var postUpdate = AccessTools.Method(typeof(Bootstrapper), "Update");
                log.LogMessage("   Postfix Method: " + postUpdate.DeclaringType.Name + "." + postUpdate.Name);
                harmony.Patch(originalUpdate, postfix: new HarmonyMethod(postUpdate));

                #endregion


                log.LogMessage("Runtime Hooks's Applied");
                log.LogMessage(" ");
            }
            catch { log.LogError("FAILED to Apply Hooks's!"); }

            log.LogMessage("Initializing Il2CppTypeSupport..."); // Helps with AssetBundles
            Il2CppTypeSupport.Initialize();



            //Bootstrapper.Create("BootstrapperGO");

        }
    }
}


/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace CrabGameMod
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    class Loader : BepInEx.IL2CPP.BasePlugin
    {
        public const string pluginGuid = "com.danilo1301.crabgamemod";
        public const string pluginName = "Crab Game Mod";
        public const string pluginVersion = "1.0.0.0";

        public static BepInEx.Logging.ManualLogSource log;

        public Loader()
        {
            log = Log;
        }

        [HarmonyPostfix]
        public static void Update()
        {
            Loader.log.LogMessage("Update called");

        }

        public override void Load()
        {
            Log.LogMessage("Working");

            Log.LogMessage("Registering C# Type's in Il2Cpp");

            try
            {
                ClassInjector.RegisterTypeInIl2Cpp<TestComponent>();
            }
            catch
            {
                log.LogError("FAILED to Register Il2Cpp Type!");
            }




            try
            {
                log.LogMessage(" ");
                log.LogMessage("Inserting Harmony Hooks...");

                var harmony = new Harmony("wh0am15533.trainer.il2cpp");

                #region[Enable/Disable Harmony Debug Log]
                //Harmony.DEBUG = true; (Old)
                //HarmonyFileLog.Enabled = true;
                #endregion


                var originalUpdate = AccessTools.Method(typeof(Chatbox), "Update");
                var postUpdate = AccessTools.Method(typeof(CrabGameMod.TestComponent), "Update");
                harmony.Patch(originalUpdate, postfix: new HarmonyMethod(postUpdate));
                log.LogMessage("   Original Method: " + originalUpdate.DeclaringType.Name + "." + originalUpdate.Name);
                log.LogMessage("   Postfix Method: " + postUpdate.DeclaringType.Name + "." + postUpdate.Name);


                var originalAppendMessage = AccessTools.Method(typeof(Chatbox), "AppendMessage");
                var postAppendMessage = AccessTools.Method(typeof(CrabGameMod.TestComponent), "AppendMessage");
                harmony.Patch(originalAppendMessage, postfix: new HarmonyMethod(postAppendMessage));



                harmony.Patch(
                    AccessTools.Method(typeof(GameMode), "OnPlayerSpawnOrDespawn"),
                    postfix: new HarmonyMethod(AccessTools.Method(typeof(CrabGameMod.TestComponent), "OnPlayerSpawnOrDespawn")));

                harmony.Patch(
                    AccessTools.Method(typeof(GameObject), "Update"),
                    postfix: new HarmonyMethod(AccessTools.Method(typeof(Loader), "Update")));

                harmony.Patch(
                    AccessTools.Method(typeof(ServerHandle), "ReceiveChatMessage"),
                    postfix: new HarmonyMethod(AccessTools.Method(typeof(CrabGameMod.TestComponent), "ReceiveChatMessage")));

                harmony.Patch(
                    AccessTools.Method(typeof(LobbyManager), "BanPlayer"),
                    prefix: new HarmonyMethod(AccessTools.Method(typeof(CrabGameMod.TestComponent), "BanPlayer")));

                log.LogMessage("Runtime Hooks's Applied");
                log.LogMessage(" ");
            }
            catch { log.LogError("FAILED to Apply Hooks's!"); }

            log.LogMessage("Initializing Il2CppTypeSupport..."); // Helps with AssetBundles
            Il2CppTypeSupport.Initialize();

            TestComponent.Create("TestComponentGO");

        }
    }
}

*/