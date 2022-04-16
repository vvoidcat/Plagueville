using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PLAGUEV.Control;



namespace PLAGUEV.Cards {

    public class Card : MonoBehaviour, iRaycastable {
        
        [SerializeField] CardType type;
        CardEvent cardEvent = null;
        CardProgression progression = null;

        // sprite
        // text
        // answers
        
        [Range(0, 100)] [SerializeField] int money;
        [Range(0, 100)] [SerializeField] int knowledge;
        [Range(0, 100)] [SerializeField] int glory;
        [Range(0, 100)] [SerializeField] int faith;




        ///*    iRAYCASTABLE    *///

        public bool HandleRaycast() {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                
                //  player controller performs some stuff
            }

            return true;
        }
    }
}
