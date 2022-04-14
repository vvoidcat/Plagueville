using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TSARSTVO.SceneManagement
{
    public class Fader : MonoBehaviour
    {

        CanvasGroup canvasGroup;
        Coroutine currentActiveFade;

        void Awake ()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }


        // immediately sets the screen to black
        public void FadeOutImmediate ()
        {
            canvasGroup.alpha = 1;
        }


        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }

        public Coroutine Fade(float target, float time)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeRoutine(target, time));   // moved the actual fadeout here to prevent coroutines from overlapping (to be able to cancel a coroutine)
            return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                // this will work in both directions
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);

                yield return null;
            }
        }
    }
}


//      fade OUT blackens the screen
//      fade IN returns the image to the screen


// this script needs to be attached to the FADER game object



            // LOADING SCREEN

            // the same principle:
            // IEnumerator
            // wait for (time it takes to fade out)
            // fade out to the screen
            //          where should i do it? in the SceneTransition.cs and in the SavingWrapper.cs?


            // randomize the screen (if there're more than 1)