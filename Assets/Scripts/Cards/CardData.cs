using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PLAGUEV.Dialogue;

namespace PLAGUEV.Cards {

    [CreateAssetMenu(fileName = "Card", menuName = "Cards/New Card", order = 0)]
    public class CardData : ScriptableObject {

        public string characterName = null;
        public Sprite sprite = null;
        public CardType type;
        public bool canAppearEverywhere = false;
        public LocationType[] locations = new LocationType[1];
        public bool hasClassTrees = false;
        public bool useMainTree = true;
        public DialogueTree dialogueTreeMain = null;
        public DialogueTree dialogueTreeAlternate = null;
        // CardProgression progression = null;
        

        // this should go to dialogue choices
        // [Range(-100, 100)] [SerializeField] int money = 0;
        // [Range(-100, 100)] [SerializeField] int knowledge = 0;
        // [Range(-100, 100)] [SerializeField] int glory = 0;
        // [Range(-100, 100)] [SerializeField] int faith = 0;
    }
}
