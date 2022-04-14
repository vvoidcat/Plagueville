using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace TSARSTVO.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";  // creating an unset string
        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();  // this exists for the entire lifetime of the app, bc it's static



        // going through all this pain to make sure that udentifiers for ALL OBJECTS IN THE SCENE (in the scene ONLY) are unique
        // the code that's included in between hashes is going to be stripped out when i go to build

#if UNITY_EDITOR
        void Update ()
        {
            if (Application.IsPlaying(gameObject)) return;                // returns if the scene the gameObject is in is currently playing, which means we're not in the Editor
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;      // returns if the path to the scene is empty, which means we're dealing with a prefab

            // this goes and finds a serialization of THIS MomoBehaviour and stores it in a variable serializedObject
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                // modifying the property (with the newly generated UUID) and notifying Unity about the changes
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;
        }
#endif



        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        bool IsUnique(string candidate)
        {
            // 1. checking if the key exists in the dictionary
            // 2. checking if it is pointing to itself
            // 3. checking if it's null (= has been deleted) and removing it from the static dictionary if that's the case
            // 4. checking if by some miracle its identifier got changed, removing it from the dictionary too

            if (!globalLookup.ContainsKey(candidate)) return true;

            if (globalLookup[candidate] == this) return true;

            if (globalLookup[candidate] == null)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            return false;
        }




        // there go all the things i need to save/restore

        public object CaptureState()
        {
            // getting the list of components that implement the ISaveable
            // creating a dictionary key (type string) for each of the components and asking them to capture their own state

            Dictionary<string, object> state = new Dictionary<string, object>();

            foreach (ISaveable isaveable in GetComponents<ISaveable>())
            {
                state[isaveable.GetType().ToString()] = isaveable.CaptureState();
            }

            return state;
        }

        public void RestoreState(object state)
        {
            // checking if the components in the dictionary have the ISaveable on them
            // launching RestoreState() for each of them through the ISaveable interface

            Dictionary<string, object> stateDictionary = (Dictionary<string, object>) state;

            foreach (ISaveable isaveable in GetComponents<ISaveable>())
            {
                string typeString = isaveable.GetType().ToString();

                if (stateDictionary.ContainsKey(typeString))
                {
                    isaveable.RestoreState(stateDictionary[typeString]);
                }
            }
        }
    }
}








    // the SaveableEntity class needs to be put on EVERY object that needs saving       !!

    //  in this case, it's probably only Tsarevich && the quest objects (to save animation triggers, etc.?)