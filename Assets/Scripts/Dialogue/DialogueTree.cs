using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PLAGUEV.Quests;

namespace PLAGUEV.Dialogue {

    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class DialogueTree : ScriptableObject, ISerializationCallbackReceiver {

        [SerializeField] bool isPlot = false;
        bool isCleared = false;
        
        [SerializeField] bool alwaysAvailable = true;
        [SerializeField] int indexQuest = 0;
        [SerializeField] int indexObjective = 0;
        [SerializeField] QuestProgression conditionProgression;

        [SerializeField] public List<DialogueNode> nodes = new List<DialogueNode>();
        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        string characterName;
        float canvasWidth = 5000;
        float canvasHeight = 5000;

        Quest[] questData = null;
        string[] questList = null;

        private const float cardHeight = 300;


        void Awake() {
            OnValidate();
        }

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

        public bool IsPlot() {
            return isPlot;
        }

        public bool IsCleared() {
            return isCleared;
        }

        public bool IsAlwaysAvailable() {
            return alwaysAvailable;
        }
        
        public int GetIndexQuest() {
            return indexQuest;
        }

        public int GetIndexObjective() {
            return indexObjective;
        }

        public QuestProgression GetConditionProgression() {
            return conditionProgression;
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
        public Quest GetQuestByIndex(int index) {
            return questData[index];
        }

        public string[] GetQuestList() {
            return questList;
        }

        public void Initialize() {
            if (questData == null || questList == null) {
                questData = QuestManagement.LoadQuests();
                questList = QuestManagement.BuildQuestList(questData);
            }

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
                Vector2 creationOffset = new Vector2(newRect.position.x + 100, newRect.position.y + 50);
                
                if (parent.GetSpeaker() == SpeakerType.PLAYER) {
                    newRect.height = cardHeight;
                    creationOffset.y += 60;
                }

                newRect.position = creationOffset;
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
            Undo.RecordObject(this, "Undo Update Dialogue Plot Relation");
            isPlot = state;
        }

        public void SetAvailability(bool state) {
            Undo.RecordObject(this, "Undo Update Dialogue Conditions Settings");
            alwaysAvailable = state;
        }

        public void SetQuestConditions(int[] newIndexes, QuestProgression newProgression) {
            Undo.RecordObject(this, "Undo Update Dialogue Conditions Settings");
            indexQuest = newIndexes[0];
            indexObjective = newIndexes[1];
            conditionProgression = newProgression;
        }

        public void SetCharacterName(string newName) {
            Undo.RecordObject(this, "Undo Update Dialogue Character Name");
            characterName = newName;
        }

        public void SetCanvasSize(float width, float height) {
            Undo.RecordObject(this, "Undo Update Dialogue Canvas Size");
            canvasWidth = width;
            canvasHeight = height;
        }
#endif

        public void SetCleared(bool state) {
            isCleared = state;
        }



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
