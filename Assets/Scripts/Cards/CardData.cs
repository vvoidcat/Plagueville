using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PLAGUEV.Dialogue;
using PLAGUEV.Quests;

namespace PLAGUEV.Cards {

    [CreateAssetMenu(fileName = "Card", menuName = "Cards/New Card", order = 0)]
    public class CardData : ScriptableObject {

        public string characterName = null;
        public Sprite sprite = null;
        public CardType type;
        public bool canAppearEverywhere = false;
        public LocationType[] locations = new LocationType[1];
        public bool hasClassTrees = false;
        public DialogueTree dialogueTreeMain = null;
        public DialogueTree dialogueTreeAlternate = null;
        DialogueTree dialogueTree = null;
        public Quest quest = null;

        int counter = 0;
        int maxCounter = 0;
        bool isReady = true;
        

        // this should go to dialogue choices
        // [Range(-100, 100)] [SerializeField] int money = 0;
        // [Range(-100, 100)] [SerializeField] int knowledge = 0;
        // [Range(-100, 100)] [SerializeField] int glory = 0;
        // [Range(-100, 100)] [SerializeField] int faith = 0;


        public void SetDialogueTree(CardType cardType) {
            if (hasClassTrees && type == cardType) {
                dialogueTree = dialogueTreeAlternate;
            } else {
                dialogueTree = dialogueTreeMain;
            }
        }

        public void SetMaxCounter(int defaultMax, int uniqueMax) {
            if (type == CardType.UNIQUE) {
                maxCounter = uniqueMax;
            } else {
                maxCounter = defaultMax;
            }
        }

        public bool CanBeChosen(LocationType currentLocation) {
            bool canBeChosen = false;

            if (isReady) {
                if (canAppearEverywhere) {
                    canBeChosen = true;
                } else {
                    foreach (LocationType cardLocation in locations) {
                        if (cardLocation == currentLocation) {
                            canBeChosen = true;
                            break;
                        }
                    }
                }
            }

            // UpdateCounter();
            return canBeChosen;
        }

        public void UpdateCounter() {
            counter++;

            if (counter > maxCounter) {
                counter = 0;
                isReady = true;
            } else {
                isReady = false;
            }
        }

        // public DialogueNode GetNodeToDisplay() {
        //     DialogueNode root = dialogueTree.GetRootNode();
        //     DialogueNode[] results = (DialogueNode[])dialogueTree.GetAllChildren(root);

        //     // randomly choose a node
        //     // check if node is available
        //     // if it isn't, get another node
        //     // return node
        // }
    }
}
