using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PLAGUEV.Dialogue {

    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue", order = 0)]
    public class DialogueTree : ScriptableObject, ISerializationCallbackReceiver {

        [SerializeField] public List<DialogueNode> nodes = new List<DialogueNode>();
        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        bool isPlot = false;
        // bool allCleared = false;
        string characterName;
        float canvasWidth = 5000;
        float canvasHeight = 5000;


        private void OnValidate() {
            nodeLookup.Clear();

            if (nodes.Count > 0) {
                for (int i = 0; i < nodes.Count; i++) {
                    if (nodes[i] != null) {
                        if (i == 0) {
                            nodes[i].SetRoot(true);
                        }
                        nodeLookup[nodes[i].name] = nodes[i];
                    }
                }
            }
        }


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

        public bool GetPlotState() {
            return isPlot;
        }

        public string GetCharacterName() {
            return characterName;
        }

        public float GetCanvasWidth() {
            return canvasWidth;
        }

        public float GetCanvasHeight() {
            return canvasHeight;
        }


#if UNITY_EDITOR
        public void Initialize() {
            if (nodes.Count == 0) {
                CreateRootNode();
            }
            OnValidate();
        }

        public void CreateRootNode() {
            DialogueNode rootNode = CreateNodeMaker(null);
            rootNode.SetRoot(true);
            rootNode.SetRect(new Rect (0, 0, 150, 100));
            CreateNodeAdder(rootNode);
        }

        public void CreateNode(DialogueNode parent) {
            DialogueNode newNode = CreateNodeMaker(parent);

            Undo.RegisterCreatedObjectUndo(newNode, "Undo Create Node");
            Undo.RecordObject(this, "Undo Add Node");

            CreateNodeAdder(newNode);
        }

        private DialogueNode CreateNodeMaker(DialogueNode parent) {
            DialogueNode newNode = CreateInstance<DialogueNode>();

            newNode.name = System.Guid.NewGuid().ToString();

            if (parent != null) {
                Rect newRect = parent.GetRect();
                newRect.position = new Vector2(newRect.position.x + 100, newRect.position.y + 50);
                newNode.SetRect(newRect);
                parent.AddChild(newNode.name);
            }

            return newNode;
        }

        private void CreateNodeAdder(DialogueNode newNode) {
            nodes.Add(newNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode deadNode) {
            Undo.RecordObject(this, "Undo Delete Node");
            nodes.Remove(deadNode);

            foreach (DialogueNode node in GetAllNodes()) {
                node.GetChildren().Remove(deadNode.name);
            }

            Undo.DestroyObjectImmediate(deadNode);
            OnValidate();
        }

        public void SetPlotRelation(bool state) {
            isPlot = state;
        }

        public void SetCharacterName(string newName) {
            characterName = newName;
        }

        public void SetCanvasSize(float width, float height) {
            canvasWidth = width;
            canvasHeight = height;
        }
#endif



        // ISerializationCallbackReceiver

        public void OnBeforeSerialize() {
#if UNITY_EDITOR
            if (AssetDatabase.GetAssetPath(this) != "") {
                foreach (DialogueNode node in nodes) {
                    if (AssetDatabase.GetAssetPath(node) == "") {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize() {
            // nothing to do
        }
    }
}
