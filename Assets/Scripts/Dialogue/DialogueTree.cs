using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PLAGUEV.Dialogue {

    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue", order = 0)]
    public class DialogueTree : ScriptableObject {

        [SerializeField] public List<DialogueNode> nodes = new List<DialogueNode>();
        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        bool isPlot = false;
        // bool allCleared = false;
        string characterName;


        private void OnValidate() {
            nodeLookup.Clear();

            foreach (DialogueNode node in GetAllNodes()) {
                nodeLookup[node.GetID()] = node;
            }
        }

#if UNITY_EDITOR
        private void Awake() {
            if (nodes.Count == 0) {
                AddRootNode();
            }

            OnValidate();
        }


        private void AddRootNode() {
            DialogueNode rootNode = new DialogueNode();  //CreateInstance<DialogueNode>();
            rootNode.SetID(Guid.NewGuid().ToString());

            nodes.Add(rootNode);
        }

        public void CreateNode(DialogueNode parent) {
            DialogueNode newNode = new DialogueNode();
            newNode.SetID(Guid.NewGuid().ToString());
            parent.GetChildren().Add(newNode.GetID());
            nodes.Add(newNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode deadNode) {
            nodes.Remove(deadNode);

            foreach (DialogueNode node in GetAllNodes()) {
                node.GetChildren().Remove(deadNode.GetID());
            }

            OnValidate();
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

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode) {
            foreach (string childID in parentNode.GetChildren()) {
                if (nodeLookup.ContainsKey(childID)) {
                    yield return nodeLookup[childID];
                }
            }
        }

        public bool IsPlot() {
            return isPlot;
        }

        public string GetCharacterName() {
            return characterName;
        }
    }
}
