using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PLAGUEV.Dialogue;
using PLAGUEV.Quests;
using PLAGUEV.Stats;

namespace PLAGUEV.Cards {

    [CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 0)]
    public class CardData : ScriptableObject {

        [System.Serializable] public struct Dialogue {
            public DialogueTree dialogueTree;
            public PlayerClass classRestriction;
            public bool isOneTimer;
            public bool isCleared;
        }

        public string characterName = null;
        public Sprite sprite = null;
        public bool isUnique = false;
        public bool canAppearEverywhere = false;
        public LocationType[] locations = new LocationType[1];

        [SerializeField] Dialogue[] dialogues = new Dialogue[1] { new Dialogue() }; 

        public Quest quest = null;

        int counter = 0;
        int maxCounter = 0;
        bool isReady = true;


        public DialogueTree ChooseDialogueTree() {
            DialogueTree result = null;

            // randomizer

            return result;
        }

        public void SetMaxCounter(int defaultMax, int uniqueMax) {
            if (isUnique) {
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
