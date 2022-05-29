using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PLAGUEV.Quests;

namespace PLAGUEV.Dialogue.Editor {

    public static class DialogueGUILayout {

        [NonSerialized] static Vector3 controlPointOffset = new Vector2(20, 0);
        static Color chainColor = new Color(0.3f, 0.5f, 1f);

        private const float playerHeight = 362;
        private const float cardHeight = 300;
        private const float defaultWidth = 300;

        private const float rootHeight = 100;
        private const float rootWidth = 150;


        public static void DrawDialogueSettings(DialogueTree selectedDialogue) {
            EditorGUI.BeginChangeCheck();

            GUILayout.BeginHorizontal(GUILayout.Width(1050));
            EditorGUILayout.LabelField("dialogue selected: " + selectedDialogue.name, EditorStyles.boldLabel, GUILayout.Width(300));
            DrawDialogueConditionsSettings(selectedDialogue);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Width(700));
            DrawLabel("Is Plot", 50);
            bool newPlot = EditorGUILayout.Toggle(selectedDialogue.IsPlot());

            GUI.enabled = !selectedDialogue.IsPlot();
            DrawLabel("Character Name", 115);
            string newName = EditorGUILayout.TextField(selectedDialogue.GetCharacterName(), GUILayout.Width(200));
            GUI.enabled = true;

            DrawLabel("   Canvas Size", 90);
            EditorGUILayout.LabelField("x", GUILayout.Width(10));
            float newWidth = EditorGUILayout.FloatField(selectedDialogue.GetCanvasWidth(), GUILayout.Width(50));
            EditorGUILayout.LabelField("y", GUILayout.Width(10));
            float newHeight = EditorGUILayout.FloatField(selectedDialogue.GetCanvasHeight(), GUILayout.Width(50));
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(4);

            selectedDialogue.SetPlotRelation(newPlot);
            selectedDialogue.SetCharacterName(newName);
            selectedDialogue.SetCanvasSize(newWidth, newHeight);
        }

        private static void DrawDialogueConditionsSettings(DialogueTree selectedDialogue) {
            Quest newQuest = selectedDialogue.GetAllQuests()[selectedDialogue.GetIndexQuest()];
            QuestProgression newProgression;
    
            DrawLabel("Always Available", 115);
            bool newAvailability = EditorGUILayout.Toggle(selectedDialogue.IsAlwaysAvailable(), GUILayout.Width(50));

            GUI.enabled = selectedDialogue.IsAlwaysAvailable();
            EditorGUILayout.LabelField("Quest: ", GUILayout.Width(50));
            int newIndexQuest = EditorGUILayout.Popup(selectedDialogue.GetIndexQuest(), selectedDialogue.GetQuestList(), GUILayout.Width(100));
            newQuest = selectedDialogue.GetQuestByIndex(newIndexQuest);

            EditorGUILayout.LabelField("At Objective: ", GUILayout.Width(80));
            int newIndexObjective = EditorGUILayout.Popup(selectedDialogue.GetIndexObjective(), newQuest.GetObjectives(), GUILayout.Width(100));

            EditorGUILayout.LabelField("At Progress Stage: ", GUILayout.Width(110));
            newProgression = (QuestProgression)EditorGUILayout.EnumPopup(selectedDialogue.GetConditionProgression());
            GUI.enabled = true;

            selectedDialogue.SetAvailability(newAvailability);
            selectedDialogue.SetQuestConditions(new int[] {newIndexQuest, newIndexObjective}, newProgression);
        }

        public static void DrawBackground(DialogueTree selectedDialogue) {
            Rect canvas = GUILayoutUtility.GetRect(selectedDialogue.GetCanvasWidth(), selectedDialogue.GetCanvasHeight());
            Texture2D bgTexture = Resources.Load("background") as Texture2D;
            Rect bgTextureCoords = new Rect(0, 0, selectedDialogue.GetCanvasWidth() / DialogueGUIStyles.bgTextureSize,
                                                    selectedDialogue.GetCanvasHeight() / DialogueGUIStyles.bgTextureSize);
            GUI.DrawTextureWithTexCoords(canvas, bgTexture, bgTextureCoords);
        }

        public static DialogueNode[] DrawNode(DialogueTree selectedDialogue, DialogueNode node, DialogueNode[] specialNodes) {
            DialogueGUIStyles.SetNodeStyle(node.GetSpeaker());

            GUILayout.BeginArea(node.GetRect(), DialogueGUIStyles.GetNodeStyle());

            if (!node.IsRoot()) {
                specialNodes = DrawRegularNode(selectedDialogue, node, specialNodes);
            } else {
                specialNodes = DrawRootNode(node, specialNodes);
            }

            GUILayout.EndArea();

            return specialNodes;
        }

        public static DialogueNode[] DrawRegularNode(DialogueTree selectedDialogue, DialogueNode node, DialogueNode[] specialNodes) {
            DrawSpeakerField(node);

            if (node.GetSpeaker() == SpeakerType.CARD) {
                DrawCardToggles(selectedDialogue, node);
                DrawAdditionalFields(selectedDialogue, node);
                DrawQuestSettings(selectedDialogue, node);
            } else {
                DrawActionField(node);
                DrawLocationField(node);
                DrawStats(node);
                DrawQuestSettings(selectedDialogue, node);
            }

            DrawText(node);

            EditorGUILayout.BeginHorizontal();
            specialNodes[0] = DrawAddChildButton(node, specialNodes[0]);
            specialNodes[1] = DrawLinkButtons(node, specialNodes[1]);
            specialNodes[2] = DrawDeleteButton(node, specialNodes[2]);
            EditorGUILayout.EndHorizontal();

            ResetNodeHeight(selectedDialogue, node);

            return specialNodes;
        }

        public static DialogueNode[] DrawRootNode(DialogueNode node, DialogueNode[] specialNodes) {
            DialogueNode parentNode = specialNodes[0];
            DialogueNode linkerNode = specialNodes[1];
    
            EditorGUILayout.LabelField("<START>", DialogueGUIStyles.GetRootLabelStyle());

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            parentNode = DrawAddChildButton(node, parentNode);
            linkerNode = DrawLinkButtons(node, linkerNode);
            GUILayout.EndHorizontal();

            return new DialogueNode[] {parentNode, linkerNode, null};
        }

        public static void DrawConnections(DialogueTree selectedDialogue, DialogueNode node) {
            Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);

            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node)) {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
                Color lineColor = Color.white;

                if (childNode.IsChained()) {
                    lineColor = chainColor;
                }

                Handles.DrawBezier(startPosition, endPosition,
                                   startPosition + controlPointOffset,
                                   endPosition - controlPointOffset,
                                   lineColor, null, 4f);
            }
        }

        public static void ResetNodeHeight(DialogueTree selectedDialogue, DialogueNode node) {
            Rect newRect = node.GetRect();
            newRect.width = defaultWidth;

            if (node.GetSpeaker() == SpeakerType.CARD) {
                newRect.height = cardHeight;
            } else if (node.IsRoot()) {
                newRect.height = rootHeight;
                newRect.width = rootWidth;
            } else {
                newRect.height = playerHeight;
            }

            node.SetRect(newRect);
        }

        public static DialogueNode DrawAddChildButton(DialogueNode node, DialogueNode parentNode) {
            if (GUILayout.Button("+")) {
                parentNode = node;
            }

            return parentNode;
        }

        public static DialogueNode DrawDeleteButton(DialogueNode node, DialogueNode deadNode) {
            if (GUILayout.Button("x")) {
                deadNode = node;
            }

            return deadNode;
        }

        public static DialogueNode DrawLinkButtons(DialogueNode node, DialogueNode linkerNode) {
            bool isLinkable = true;

            if (linkerNode == null) {
                if (GUILayout.Button("link")) {
                    linkerNode = node;
                }
            } else if (linkerNode == node) {
                if (GUILayout.Button("cancel")) {
                    linkerNode = null;
                }
            } else if (linkerNode.GetChildren().Contains(node.name)) {
                if (GUILayout.Button("unchild")) {
                    linkerNode.RemoveChild(node.name);
                    linkerNode = null;
                }
            } else {
                if (node.GetChildren().Contains(linkerNode.name)) {
                    isLinkable = false;
                }

                GUI.enabled = isLinkable;
                if (GUILayout.Button("child")) {
                    linkerNode.AddChild(node.name);
                    linkerNode = null;
                }
                GUI.enabled = true;
            }

            return linkerNode;
        }

        public static void DrawCardToggles(DialogueTree selectedDialogue, DialogueNode node) {
            bool newChainedState = node.IsChained();
            bool newCustomState = node.IsCustom();

            GUILayout.BeginHorizontal();
            GUI.enabled = !selectedDialogue.IsPlot();
            DrawLabel("Customizable", 105);
            newCustomState = EditorGUILayout.Toggle(node.IsCustom());
            GUI.enabled = true;

            DrawLabel("Chained", 65);
            newChainedState = EditorGUILayout.Toggle(node.IsChained());
            GUILayout.EndHorizontal();

            node.SetCustom(newCustomState);
            node.SetChained(newChainedState);
            foreach (DialogueNode child in selectedDialogue.GetAllChildren(node)) {
                if (child.GetSpeaker() == SpeakerType.PLAYER) {
                    child.SetChained(newChainedState);
                }
            }
        }

        public static void DrawAdditionalFields(DialogueTree selectedDialogue, DialogueNode node) {
            if (node.IsCustom()) {
                GUI.enabled = true;
            } else if (!selectedDialogue.IsPlot()) {
                GUI.enabled = false;
            }

            DrawCharacterField(node);
            DrawSpriteField(node);

            GUI.enabled = true;
        }

        // TODO move to a dedicated class ??
        public static void DrawQuestSettings(DialogueTree selectedDialogue, DialogueNode node) {
            Quest newQuest = selectedDialogue.GetAllQuests()[node.GetIndexQuest()];

            EditorGUILayout.Space(2);

            EditorGUILayout.BeginHorizontal();
            DrawLabel("Quest Changer", 130);
            bool newQuestChanger = EditorGUILayout.Toggle(node.IsQuestChanger());
            GUI.enabled = newQuestChanger;
            int newIndexQuest = EditorGUILayout.Popup(node.GetIndexQuest(), selectedDialogue.GetQuestList(), GUILayout.Width(100));
            newQuest = selectedDialogue.GetQuestByIndex(newIndexQuest);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            DrawLabel("Update Objective", 130);
            bool newUpdObjective = EditorGUILayout.Toggle(node.IsObjectiveUpdater());
            if (newQuestChanger && !newUpdObjective) {
                GUI.enabled = false;
            }
            int newIndexObjective = EditorGUILayout.Popup(node.GetIndexObjective(), newQuest.GetObjectives(), GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;

            EditorGUILayout.Space(2);

            node.SetQuest(new int[] {newIndexQuest, newIndexObjective}, new bool[] {newQuestChanger, newUpdObjective});
        }

        public static void DrawSpeakerField(DialogueNode node) {
            SpeakerType speaker = node.GetSpeaker();

            GUILayout.BeginHorizontal();
            DrawLabel("Speaker", 80);
            speaker = (SpeakerType)EditorGUILayout.EnumPopup(node.GetSpeaker());
            GUILayout.EndHorizontal();

            node.SetSpeaker(speaker);
        }

        public static void DrawActionField(DialogueNode node) {
            ActionType action = node.GetAction();

            GUILayout.BeginHorizontal();
            DrawLabel("Action", 80);
            action = (ActionType)EditorGUILayout.EnumPopup(node.GetAction());
            GUILayout.EndHorizontal();

            node.SetAction(action);
        }

        public static void DrawLocationField(DialogueNode node) {
            LocationType newLocation = node.GetLocationChange();

            if (node.GetAction() != ActionType.MOVE_DEFAULT) {
                GUI.enabled = false;
            }
            GUILayout.BeginHorizontal();
            DrawLabel("Move to", 80);
            newLocation = (LocationType)EditorGUILayout.EnumPopup(node.GetLocationChange());
            GUILayout.EndHorizontal();
            GUI.enabled = true;

            node.SetLocationChange(newLocation);
        }

        public static void DrawCharacterField(DialogueNode node) {
            string newName = node.GetCharacterName();

            GUILayout.BeginHorizontal();
            DrawLabel("Character", 80);
            newName = EditorGUILayout.TextField(node.GetCharacterName());
            GUILayout.EndHorizontal();

            node.SetCharacterName(newName);
        }

        public static void DrawSpriteField(DialogueNode node) {
            Sprite newSprite = node.GetSprite();

            GUILayout.BeginHorizontal();
            DrawLabel("Sprite", 80);
            newSprite = (Sprite)EditorGUILayout.ObjectField(node.GetSprite(), typeof(Sprite), false);
            GUILayout.EndHorizontal();

            node.SetSprite(newSprite);
        }

        public static void DrawStats(DialogueNode node) {
            int[] nodeValues = node.GetStatValues();
            int[] newValues = new int[nodeValues.Length];
            string[] labels = new string[] {"Money", "Knowledge", "Glory", "Faith"};

            EditorGUILayout.Space(2);

            if (node.GetAction() != ActionType.DEFAULT) {
                GUI.enabled = false;
            }
            for (int i = 0; i < nodeValues.Length; i++) {
                GUILayout.BeginHorizontal();
                DrawLabel(labels[i], 80);
                newValues[i] = EditorGUILayout.IntSlider(nodeValues[i], -100, 100);
                GUILayout.EndHorizontal();
            }
            GUI.enabled = true;

            node.SetStatValues(newValues);
        }

        public static void DrawText(DialogueNode node) {
            string newText = node.GetText();

            DrawLabel("Text", 80);
            newText = EditorGUILayout.TextField(node.GetText(), DialogueGUIStyles.GetTextStyle(), GUILayout.Height(70));

            node.SetText(newText);
        }

        private static void DrawLabel(string labelText, float width) {
            EditorGUILayout.LabelField(labelText + ":", EditorStyles.boldLabel, GUILayout.Width(width));
        }
    }
}
