using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TSARSTVO.Saving
{
    public class SavingSystem : MonoBehaviour
    {

        public IEnumerator LoadLastScene(string saveFile)
        {
            // 1. get state
            // 2. load last scene
            // 3. restore the state of that scene

            
            Dictionary<string, object> state = LoadFile(saveFile);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;

            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                buildIndex = (int)state["lastSceneBuildIndex"];
            }

            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreState(state);
        }


        public void Save (string saveFile)
        {
            // taking the state of the old file
            Dictionary<string, object> state = LoadFile(saveFile);

            // updating the state
            CaptureState(state);

            // overwriting it
            SaveFile(saveFile, state);
        }
        public void Load (string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }
        public void Delete (string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }



        private void SaveFile(string saveFile, object capturedState)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);

            // creates or opens (aka writes inside it) a file on the given path
            // using (filestream){operations with the filestream}  serves as protection against memory leaks
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                // creating a binary formatter, serializing
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, capturedState);
            }
        }
        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);

            if (!File.Exists(path))
            {
                // returning a new dictionary, which is basically an empty state
                return new Dictionary<string, object>();
            }

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                // creating a formatter and returning the deserialized stream (and also performing a cast)
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }



        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }

            // update the state dictionary so that it has information about the last scene the (quicksave) was made in
            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }
        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();

                if (state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
            }
        }



        // returns the path of the save file
        private string GetPathFromSaveFile(string saveFile)
        {
            // combines a path and a string
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}




// this saving system works between scenes too (not that i need it to right now, buuut still)


/*
        Application,persistentDataPath = "default" saving directory of any platform

        do i need another directory for windows?? if i do, how do i make it work?

*/