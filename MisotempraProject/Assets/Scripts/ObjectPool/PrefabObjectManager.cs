using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prefab
{
    public class PrefabObjectManager : Singleton.DontDestroySingletonMonoBehaviour<PrefabObjectManager>
    {
        public Dictionary<string, List<PrefabObject>> prefabs = new Dictionary<string, List<PrefabObject>>();

        protected override void Init()
        {

        }

        public PrefabObject Instantiate(GameObject prefab)
        {
            if (!prefabs.ContainsKey(prefab.name))
            {
                prefabs.Add(prefab.name, new List<PrefabObject>());
            }

            return null;
        }

        public PrefabObject Instantiate(MonoBehaviour prefab)
        {
            return null;
        }

    }
}
