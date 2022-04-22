using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PLAGUEV.Dialogue {

    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue", order = 0)]
    public class DialogueTree : ScriptableObject {

        //
        [SerializeField] public List<DialogueNode> nodes = new List<DialogueNode>();

        bool isPlot = false;
        string characterName;

#if UNITY_EDITOR
        private void Awake() {
            if (nodes.Count == 0) {
                AddRootNode();
            }
        }

        private void AddRootNode() {
            DialogueNode rootNode = new DialogueNode();  //CreateInstance<DialogueNode>();

            nodes.Add(rootNode);
        }

        public void SetPlotRelation(bool state) {
            isPlot = state;
        }

        public void SetCharacterName(string newName) {
            characterName = newName;
        }
#endif

        public IEnumerable<DialogueNode> GetAllNodes() {
            return nodes;
        }

        public DialogueNode GetRootNode() {
            return nodes[0];
        }

        public bool IsPlot() {
            return isPlot;
        }

        public string GetCharacterName() {
            return characterName;
        }
    }
}
