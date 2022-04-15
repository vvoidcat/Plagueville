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
