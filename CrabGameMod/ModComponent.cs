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
        public static GameObject Create()
        {
            GameObject prefabGameObject = new GameObject("PrefabGO");
            var component = new ModComponent(prefabGameObject.AddComponent(UnhollowerRuntimeLib.Il2CppType.Of<ModComponent>()).Pointer);

            GameObject clone = Instantiate(prefabGameObject, prefabGameObject.transform.position, prefabGameObject.transform.rotation) as GameObject;

            return clone;
        }

        public ModComponent(IntPtr ptr) : base(ptr)
        {}

        public void OnGUI()
        {
            GUI.Box(new Rect(75, 200, 150, 150), "This is a title");

            var y = 240;

            if (GUI.Button(new Rect(95, y += 30, 460, 30), "0")) 
            {
                Mod.EmulateWeapon(0, 0);
            }

            if (GUI.Button(new Rect(95, y += 30, 460, 30), "1"))
            {
                Mod.EmulateWeapon(1, 0);
            }

            if (GUI.Button(new Rect(95, y += 30, 460, 30), "4"))
            {
                Mod.EmulateWeapon(4, 0);
            }
            if (GUI.Button(new Rect(95, y += 30, 460, 30), "12"))
            {
                Mod.EmulateWeapon(12, 0);
            }

            var str = "";

            try
            {
                var gamemode = MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.gameMode;
                str += "modeState=" + gamemode.modeState + "\n";
            } catch { }

            //109775240999816905

            str += Mod.GetGameModeName() + "\n";
            str += VoteSystem.GetTimeLeft() + "\n";
            str += "onlyAtLobby= " + Server.onlyAtLobby + "\n"; 
            str += "weaponsDisabled= " + Server.weaponsDisabled + "\n";
            str += "noClipEnabled= " + Server.noClipEnabled + "\n";
            str += "lobbyOwner=" + Mod.GetLobbyOwnerSteamId() + "\n";
            str += "myId=" + Mod.GetMySteamId() + "\n";
            str += "loadCustomMap?=" + Mod.loadCustomMap + " > " + Mod.customMapId + " | " + Mod.customModeId + "\n";

            if(Server.PlayerExists(Mod.GetMySteamId()))
            {
                var player = Server.GetPlayer(Mod.GetMySteamId());

                str += "player=" + player.GetTestMessage() + "\n";
                str += "autoDie=" + player.autoDie + "\n";
            }

            GUI.color = Color.yellow;
            GUI.Label(new Rect(10, 10, 200, 200), str);


            

            /*
            GUI.color = Color.red;

            foreach (var o in UnityEngine.Object.FindObjectsOfType<UnityEngine.Rigidbody>())
            {
                var gameObject = o.gameObject;
                var text = "> " + gameObject.name + "\n";

                if (gameObject.name.Contains("Proximity")) continue;

                var components = gameObject.GetComponents<UnityEngine.Component>();

                foreach(var c in components)
                {
                    text += "* " + c.GetType().ToString() + "|" + c.ToString() + "\n";
                }

                var cs = gameObject.GetComponents<MonoBehaviourPublicObRiSiupVeSiQuVeLiQuUnique>();

                foreach (var c in cs)
                {
                    var id = c.field_Private_MonoBehaviourPublicInidUnique_0.GetId();

                    text += "*S " + id + "\n";
                }

                //MonoBehaviourPublicObRiSiupVeSiQuVeLiQuUnique PhysicsObject
                //MonoBehaviourPublicInidUnique SharedObject

                var position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                var textSize = GUI.skin.label.CalcSize(new GUIContent(text));
                GUI.Label(new Rect(position.x, Screen.height - position.y, textSize.x, textSize.y), text);


            }
            */


            DrawPlayerNames();

        }

        private static void DrawPlayerNames()
        {
            GUI.color = Color.blue;

            foreach (var a in MonoBehaviourPublicDi2UIObacspDi2UIObUnique.Instance.activePlayers)
            {
                var playerManager = a.Value;

                if (!Server.PlayerExists(playerManager.steamProfile.m_SteamID)) continue;
                var player = Server.GetPlayer(playerManager.steamProfile.m_SteamID);

                var text = player.username + " [" + player.GetSelector() + "]";

                var position = Camera.main.WorldToScreenPoint(playerManager.gameObject.transform.position);
                var textSize = GUI.skin.label.CalcSize(new GUIContent(text));
                GUI.Label(new Rect(position.x, Screen.height - position.y, textSize.x, textSize.y), text);
            }
        }

        public static void Print()
        {
            BepInExLoader.log.LogMessage("Print");


            var objs = UnityEngine.Object.FindObjectsOfType<WeaponComponent>();
            foreach (var o in objs)
            {
                var gameObject = o.gameObject;
                var text = "WC> " + gameObject.name + "\n";

                if (gameObject.name.Contains("Proximity")) continue;

                var components = gameObject.GetComponents<UnityEngine.Component>();

                var c = gameObject.GetComponent<ItemData>();

                if(c)
                {
                    text += c.itemName + "\n";
                }

                /*
                foreach (var c in components)
                {
                    text += "* " + c.GetType().ToString() + "|" + c.ToString() + "\n";
                }
                */


                BepInExLoader.log.LogMessage(text);


            }
        }
    }

    
}
