using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrabGameMod
{
    public class ClientHandleHook
    {
        private static Harmony m_Harmony;

        public static void HookAll(Harmony harmony)
        {
            m_Harmony = harmony;

        }

        private static void HookMethod(string name, Type type)
        {
            try
            {
                m_Harmony.Patch(
                    AccessTools.Method(type, name),
                    prefix: new HarmonyMethod(AccessTools.Method(typeof(ClientHandleHook), name)));
            }
            catch (Exception e)
            {
                Loader.log.LogMessage("Failed to apply hook " + name);
                Loader.log.LogMessage(e.Message);
            }

        }

    }
}
