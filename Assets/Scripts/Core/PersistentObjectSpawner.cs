// creates a prefab that doesn't get destroyed on load
// do i need it though? there's going to be just one scene in this game


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLAGUEV.Core {
    public class PersistentObjectSpawner : MonoBehaviour {
        [SerializeField] GameObject persistentObjectPrefab;
        static bool hasSpawned;


        void Awake() {
            if (hasSpawned) {
                return;
            }

            if (SpawnPersistentObject() != null) {
                hasSpawned = true;
            } else {
                print("PersistentObjectSpawner.cs: failed to spawn persistentObject");
            }
        }

        GameObject SpawnPersistentObject() {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);

            return persistentObject;
        }
    }
}
