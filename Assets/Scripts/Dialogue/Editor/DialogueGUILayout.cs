using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace PLAGUEV.Dialogue.Editor {

    public static class DialogueGUILayout {

        [NonSerialized] static Vector3 controlPointOffset = new Vector2(20, 0);
        static Color chainColor = new Color(0.3f, 0.5f, 1f);

        private const float playerHeight = 180;
        private const float cardHeight = 280;
        private const float customHeight = 320;


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

            if (node.GetSpeaker() == SpeakerType.CARD) {            // CHANGE
                if (node.GetCustomState() || selectedDialogue.GetPlotState()) {
                    newRect.height = customHeight;
                } else {
                    newRect.height = cardHeight;
                }
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
            } else if (linkerNode.GetChildren().Contains(node.GetID())) {
                if (GUILayout.Button("unchild")) {
                    linkerNode.RemoveChild(node.GetID());
                    linkerNode = null;
                }
            } else {
                if (node.GetChildren().Contains(linkerNode.GetID())) {
                    isLinkable = false;
                }

                GUI.enabled = isLinkable;
                if (GUILayout.Button("child")) {
                    linkerNode.AddChild(node.GetID());
                    linkerNode = null;
                }
                GUI.enabled = true;
            }

            return linkerNode;
        }

        public static void DrawAdditionalFields(DialogueTree selectedDialogue, DialogueNode node) {
            if (selectedDialogue.GetPlotState() || node.GetCustomState()) {
                DrawCharacterField(node);
                DrawSpriteField(node);
            }
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

            if (node.GetLocationChangerState()) {
                GUI.enabled = false;
            }
            GUILayout.BeginHorizontal();
            DrawLabel("Action", 80);
            action = (ActionType)EditorGUILayout.EnumPopup(node.GetAction());
            GUILayout.EndHorizontal();
            GUI.enabled = true;

            node.SetAction(action);
        }

        public static void DrawLocationToggle(DialogueNode node) {
            bool newState = node.GetLocationChangerState();

            GUILayout.BeginHorizontal();
            DialogueGUILayout.DrawLabel("Location Changer", 117);
            newState = EditorGUILayout.Toggle(node.GetLocationChangerState());
            GUILayout.EndHorizontal();

            node.SetLocationChangerState(newState);
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

            if (node.GetLocationChangerState() || node.GetAction() != ActionType.DEFAULT) {
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

        public static void DrawCardToggles(DialogueTree selectedDialogue, DialogueNode node) {
            bool newChainedState = node.GetChainedState();
            bool newCustomState = node.GetCustomState();

            GUILayout.BeginHorizontal();
            GUI.enabled = !selectedDialogue.GetPlotState();
            DialogueGUILayout.DrawLabel("Customizable", 95);
            newCustomState = EditorGUILayout.Toggle(node.GetCustomState());
            GUI.enabled = true;

            DialogueGUILayout.DrawLabel("Chained", 60);
            newChainedState = EditorGUILayout.Toggle(node.GetChainedState());
            GUILayout.EndHorizontal();

            node.SetChained(newChainedState);
            node.SetCustom(newCustomState);
        }

        public static void DrawLabel(string labelText, float width) {
            EditorGUILayout.LabelField(labelText + ":", EditorStyles.boldLabel, GUILayout.Width(width));
        }
    }
}