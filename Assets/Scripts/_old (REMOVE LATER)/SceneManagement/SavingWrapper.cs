using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TSARSTVO.Saving;


namespace TSARSTVO.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {

        const string save_default = "save";
        const string save_quick = "quicksave";
        const string save_auto = "autosave";

        [SerializeField] float time_fadein = 1f;


        //[SerializeField] GameObject persistent;
        SavingSystem system;
        Fader fader;


        void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene ()
        {
            system = GetComponent<SavingSystem>();
            fader = FindObjectOfType<Fader>();

            fader.FadeOutImmediate();
            yield return system.LoadLastScene(save_quick);  // loads the last quicksave made         // how do i load the last save made, like, in relation to time?? as in, how long should it take??
            yield return fader.FadeIn(time_fadein);
        }


        void Update ()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                QuickSave();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                QuickLoad();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                QuickDelete();
            }
        }



        public void QuickSave()
        {
            system.Save(save_quick);
        }
        public void QuickLoad()
        {
            system.Load(save_quick);
        }
        public void QuickDelete()
        {
            system.Delete(save_quick);
        }



        public void AutoSave()
        {
            system.Save(save_auto);
        }
        public void AutoLoad()
        {
            system.Load(save_auto);
        }
    }
}



// SavingWrapper is responsible for names of the save files and default key bindings




            /* 

                    what i'll PROBABLY need in the future:

                    1. AUTOSAVE - to save the progress & the state of the world & game completion (do i want to allow saving during quests??)
                    2. QUICKSAVE(S)
                    3. CHAPTER SAVES - basically autosaves, but happen after a chapter is completed
                    4. SAVES

                    5. remove QuickDelete entirely after the project is done (deleting should only be possible from the menu, not on holding a key down)

                    *sighs* gotta do some more thinking...

            */


            // for checkpoints in portals, go: RPGcourse / scripts / Portal.cs