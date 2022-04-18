using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLAGUEV.Control {

    public class CardController : MonoBehaviour, iRaycastable {





        ///*    iRAYCASTABLE    *///

        public bool HandleRaycast() {
            if (Input.GetKey(KeyCode.Mouse0)) {

                // print("card iray");
                
                //  player controller performs some stuff
            }

            // if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {

            // }

            // left and right keys ?

            return true;
        }
    }
}