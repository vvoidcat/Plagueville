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

        private const float playerHeight = 380;
        private const float cardHeight = 320;
        private const float defaultWidth = 300;

        private const float rootHeight = 100;
        private const float rootWidth = 150;


        public static void DrawConnections(DialogueTree selectedDialogue, DialogueNode node) {
            Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);

            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node)) {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
                Color lineColor = Color.white;

                if (childNode.GetChainedState()) {
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
            } else if (node.GetRootState()) {
                newRect.height = rootHeight;
                newRect.width = rootWidth;
            } else {
                newRect.height = playerHeight;
            }

            node.SetRect(newRect);
        }

        // public static void DrawButtons(DialogueNode node, DialogueNode parentNode, DialogueNode deadNode, DialogueNode linkerNode) {
        //     parentNode = DrawAddChildButton(node, parentNode);
        //     linkerNode = DrawLinkButtons(node, linkerNode);
        //     deadNode = DrawDeleteButton(node, deadNode);
        // }

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
            bool newChainedState = node.GetChainedState();
            bool newCustomState = node.GetCustomState();

            GUILayout.BeginHorizontal();
            GUI.enabled = !selectedDialogue.GetPlotState();
            DrawLabel("Customizable", 105);
            newCustomState = EditorGUILayout.Toggle(node.GetCustomState());
            GUI.enabled = true;

            DrawLabel("Chained", 65);
            newChainedState = EditorGUILayout.Toggle(node.GetChainedState());
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
            if (node.GetCustomState()) {
                GUI.enabled = true;
            } else if (!selectedDialogue.GetPlotState()) {
                GUI.enabled = false;
            }

            DrawCharacterField(node);
            DrawSpriteField(node);

            GUI.enabled = true;
        }

        // TODO move to a dedicated class ??
        public static void DrawQuestSettings(DialogueTree selectedDialogue, DialogueNode node) {
            int indexQuest = 0;
            int indexObjective = 0;

            bool[] newStates = node.GetQuestStates();
            Quest newQuest = node.GetQuest();
            QuestProgression newProgression = node.GetQuestProgression();
            string newObjective = node.GetQuestObjective();

            EditorGUILayout.Space(2);

            EditorGUILayout.BeginHorizontal();
            DrawLabel("Quest Changer", 130);
            newStates[0] = EditorGUILayout.Toggle(newStates[0]);
            GUI.enabled = newStates[0];
            indexQuest = EditorGUILayout.Popup(indexQuest, selectedDialogue.GetQuestList(), GUILayout.Width(100));
            newQuest = selectedDialogue.GetQuestByIndex(indexQuest);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            DrawLabel("Update Objective", 130);
            newStates[1] = EditorGUILayout.Toggle(newStates[1]);
            GUI.enabled = newStates[1];
            indexObjective = EditorGUILayout.Popup(indexObjective, newQuest.GetObjectives(), GUILayout.Width(100));
            newObjective = newQuest.GetObjectiveByIndex(indexObjective);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUI.enabled = newStates[0];
            DrawLabel("Update Progression", 130);
            newStates[2] = EditorGUILayout.Toggle(newStates[2]);
            GUI.enabled = newStates[2];
            newProgression = (QuestProgression)EditorGUILayout.EnumPopup(node.GetQuestProgression(), GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            GUI.enabled = true;
            EditorGUILayout.Space(2);

            node.SetQuest(newQuest, newObjective, newProgression, newStates);
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

        public static void DrawLabel(string labelText, float width) {
            EditorGUILayout.LabelField(labelText + ":", EditorStyles.boldLabel, GUILayout.Width(width));
        }
    }
}
