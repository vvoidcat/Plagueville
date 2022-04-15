using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PLAGUEV.Cards {

    public class Card : MonoBehaviour {
        
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
    }
}
