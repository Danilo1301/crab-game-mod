using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CrabGameMod
{
    class Bootstrapper : MonoBehaviour
    {
        private static Bootstrapper Instance = null;
        private static GameObject gameObj = null;

        public static Il2CppSystem.Type modComponentType;

        private static float checkTime = 0f;

        private static bool firstTime = true;

        internal static GameObject Create(string name)
        {
            var obj = new GameObject(name);
            DontDestroyOnLoad(obj);
            var component = new Bootstrapper(obj.AddComponent(UnhollowerRuntimeLib.Il2CppType.Of<Bootstrapper>()).Pointer);
            return obj;
        }

        public Bootstrapper(IntPtr intPtr) : base(intPtr)
        {
            Instance = this;
            modComponentType = UnhollowerRuntimeLib.Il2CppType.Of<ModComponent>();

            Loader.log.LogMessage("Bootstrapper Constructor() Fired!");
        }

        public void Awake()
        {
            // Note: You can't create the trainer in Awake() or OnEnable(). It just won't Intstatiate. However, BepInEx will hook Awake()
            Loader.log.LogMessage("Bootstrapper Awake() Fired!");
        }

        [HarmonyPostfix]
        public static void Update()
        {
            checkTime += Time.deltaTime;




            /*
            if(checkTime >= 5)
            {
                checkTime = 0;

                Loader.log.LogMessage("==========: ");

                GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
                foreach (var go in allObjects)
                {
                    var distanace = Vector3.Distance(Camera.main.transform.position, go.transform.position);

                    if (go.activeInHierarchy)
                    {
                        var component = go.GetComponent(UnhollowerRuntimeLib.Il2CppType.Of<ModComponent>());

                        if (!component)
                        {
                            var c = new ModComponent(go.AddComponent(UnhollowerRuntimeLib.Il2CppType.Of<ModComponent>()).Pointer);
                        }



                        Loader.log.LogMessage("GameObject: " + go.name);

                        var components = go.GetComponents(UnhollowerRuntimeLib.Il2CppType.Of<Component>());

                        foreach (var c in components)
                        {
                            Loader.log.LogMessage("- Component: " + c.GetType().ToString());
                        }
                    }


                    

                    
                    

                }
            }

 
            */

            /*
            if (gameObj == null)
            {
                Loader.log.LogMessage(" ");
                Loader.log.LogMessage("Bootstrapping Trainer...");
                try
                {
                    GameObject prefabGameObject = new GameObject("PrefabGO");
                    var component = new ModComponent(prefabGameObject.AddComponent(UnhollowerRuntimeLib.Il2CppType.Of<ModComponent>()).Pointer);


                    GameObject clone = Instantiate(prefabGameObject, prefabGameObject.transform.position, prefabGameObject.transform.rotation) as GameObject;



                    gameObj = clone;
                }
                catch (Exception e)
                {
                    Loader.log.LogMessage("ERROR Bootstrapping Trainer: " + e.Message);
                    Loader.log.LogMessage(" ");
                }
            }
            */
        }
    }
}