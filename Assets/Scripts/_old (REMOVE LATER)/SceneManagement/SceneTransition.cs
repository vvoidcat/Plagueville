using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TSARSTVO.Saving;
using UnityEngine.SceneManagement;

namespace TSARSTVO.SceneManagement
{
    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = -1;            // clear this number later; set it in the EDITOR or BELOW (somewhere..) to the corresponding index of the scene


        [SerializeField] float time_fadeout = 0.5f;
        [SerializeField] float time_fadein = 1f;
        [SerializeField] float time_loading = 1f;

        Fader fader;
        SavingWrapper savingWrapper;


        void Start ()
        {
            fader = FindObjectOfType<Fader>();
            savingWrapper = FindObjectOfType<SavingWrapper>();
        }
        void Update() // should this be on update at all?? maybe on the button's "OnClick" method? maybe both?
        {
            // if (button is clicked) { StartCoroutine(Transition()); }
        }


        public void TransitionStart()
        {

            // if (the scene the player's in = something) {StartCoroutine(Transition(SCENE INDEX));}
            // a switch case statement would probably work better though

        }

        private IEnumerator Transition(int sceneToLoad)
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load is not set.");
                yield break;
            }

            // actions in the current scene
            DontDestroyOnLoad(gameObject);   // DontDestroyOnLoad() only works if the game object is at the root of the scene (not a child of any other objects)
            ///////// remove control from the player
            yield return fader.FadeOut(time_fadeout);
            savingWrapper.AutoSave();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // actions in the next scene
            print("scene " + sceneToLoad + " loaded");
            ///////// remove control again (for the new player gameobject)
            savingWrapper.AutoLoad();
            savingWrapper.AutoSave();
            yield return new WaitForSeconds(time_loading);
            fader.FadeIn(time_fadein);
            ///////// restore control

            // create a new gameobject that'll represent an ancor point in the new scene and update the player's position to that of that object
            // (do it before saving)

            Destroy(gameObject);
        }


        // these are for creating separate chapter autosaves; needs more work (in the SavingWrapper.cs)
        // ok it's been 2 weeks and i don't know what the fuck is going on here; why not the Capture/restore state?
        void SaveProgress (int sceneToLoad)
        {
            savingWrapper.AutoSave();
        }

        void UpdateProgress (int sceneToLoad)
        {
            savingWrapper.AutoLoad();
        }
    }
}