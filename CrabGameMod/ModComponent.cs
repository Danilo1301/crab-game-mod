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
            var str = "helo";

            GUI.color = Color.yellow;
            GUI.Label(new Rect(10, 10, 200, 200), str);

        }
    }
}
