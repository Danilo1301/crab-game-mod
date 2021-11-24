using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BepInEx.Logging;

namespace CrabGameMod
{
    public class ModComponent : MonoBehaviour
    {
        //public static GameObject obj = null;
        //public static ModComponent Instance;
        private static BepInEx.Logging.ManualLogSource log;


        internal static GameObject Create(string name)
        {
            var obj = new GameObject(name);

            DontDestroyOnLoad(obj);

            var component = new ModComponent(obj.AddComponent(UnhollowerRuntimeLib.Il2CppType.Of<ModComponent>()).Pointer);

            //toolTipComp = new TooltipGUI(obj.AddComponent(UnhollowerRuntimeLib.Il2CppType.Of<TooltipGUI>()).Pointer);
            //toolTipComp.enabled = false;

            return obj;
        }

        public ModComponent(IntPtr ptr) : base(ptr)
        {
            log = Loader.log;
            log.LogMessage("ModComponent Loaded");

            //Instance = this;
        }



        public void OnGUI()
        {
            var str = "";
            str += "" + gameObject.name + "\n";
            //str += "root name: " + gameObject.transform.root.gameObject.name + "\n";
            //str += "parent name: " + gameObject.transform.parent.gameObject.name + "\n";

            var position = WorldToGuiPoint(transform.position);

            var distance = Vector3.Distance(Camera.main.transform.position, transform.position);

            if (distance > 0.1f && distance < 10.0f)
            {
                GUI.color = Color.yellow;
                GUI.Label(new Rect(position.x, position.y, 200, 200), str);
            }

        }

        public static bool IsOnScreen(Vector3 position)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            return onScreen;
        }

        public static Vector3 WorldToGuiPoint(Vector3 position)
        {
            var guiPosition = Camera.main.WorldToScreenPoint(position);
            guiPosition.y = Screen.height - guiPosition.y;
            return guiPosition;
        }
    }
}
