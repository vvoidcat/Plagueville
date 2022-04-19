using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLAGUEV.Cards {

    [CreateAssetMenu(fileName = "Card", menuName = "Cards/New Card", order = 0)]
    public class CardData : ScriptableObject {

        public string characterName = null;
        public Sprite sprite = null;        
        public CardType type;
        public bool canAppearEverywhere = false;
        public GameLocation[] locations = new GameLocation[1];        // hidable?.. i need a custom editor.........


        // CardEvent dialogueTreeMain = null;
        // CardEvent dialogueTreeAlternate = null;
        // CardProgression progression = null;

        public bool hasClassTrees = false;
        public bool useMainTree = true;
        

        // this should go to dialogue choices
        // [Range(-100, 100)] [SerializeField] int money = 0;
        // [Range(-100, 100)] [SerializeField] int knowledge = 0;
        // [Range(-100, 100)] [SerializeField] int glory = 0;
        // [Range(-100, 100)] [SerializeField] int faith = 0;
    }
}
