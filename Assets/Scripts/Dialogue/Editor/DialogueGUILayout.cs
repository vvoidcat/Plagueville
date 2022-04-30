using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace PLAGUEV.Dialogue.Editor {

    public static class DialogueGUILayout {

        [NonSerialized] static Vector3 controlPointOffset = new Vector2(20, 0);
        [NonSerialized] static Color chainColor = new Color(0.3f, 0.5f, 1f);

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
            SpeakerType speaker = node.GetSpeaker();
            Rect newRect = node.GetRect();

            if (speaker == SpeakerType.CARD) {
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


        public static SpeakerType DrawSpeakerField(DialogueNode node) {
            SpeakerType speaker = node.GetSpeaker();

            GUILayout.BeginHorizontal();
            DrawLabel("Speaker", 80);
            speaker = (SpeakerType)EditorGUILayout.EnumPopup(node.GetSpeaker());
            GUILayout.EndHorizontal();

            node.SetSpeaker(speaker);

            return speaker;
        }

        public static void DrawAdditionalFields(DialogueTree selectedDialogue, DialogueNode node) {
            SpeakerType speaker = node.GetSpeaker();

            if (speaker == SpeakerType.CARD) {
                if (selectedDialogue.GetPlotState() || node.GetCustomState()) {
                    DialogueGUILayout.DrawCharacterField(node);
                    DialogueGUILayout.DrawSpriteField(node);
                }
                DialogueGUILayout.DrawStats(node);
            }
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

            for (int i = 0; i < nodeValues.Length; i++) {
                GUILayout.BeginHorizontal();
                DrawLabel(labels[i], 80);
                newValues[i] = EditorGUILayout.IntSlider(nodeValues[i], -100, 100);
                GUILayout.EndHorizontal();
            }

            node.SetStatValues(newValues);
        }

        public static void DrawText(DialogueNode node) {
            string newText = node.GetText();

            DrawLabel("Text", 80);
            newText = EditorGUILayout.TextField(node.GetText(), DialogueGUIStyles.GetTextStyle(), GUILayout.Height(70));

            node.SetText(newText);
        }

        public static void DrawToggles(DialogueTree selectedDialogue, DialogueNode node) {
            SpeakerType speaker = node.GetSpeaker();
            bool newChainedState = node.GetChainedState();
            bool newCustomState = node.GetCustomState();
            bool isEnabled = !selectedDialogue.GetPlotState();

            if (speaker == SpeakerType.CARD) {
                GUILayout.BeginHorizontal();
                GUI.enabled = isEnabled;
                DialogueGUILayout.DrawLabel("Customizable", 95);
                newCustomState = EditorGUILayout.Toggle(node.GetCustomState());
                GUI.enabled = true;

                DialogueGUILayout.DrawLabel("Chained", 60);
                newChainedState = EditorGUILayout.Toggle(node.GetChainedState());
                GUILayout.EndHorizontal();
            }

            node.SetChained(newChainedState);
            node.SetCustom(newCustomState);
        }

        private static void DrawLabel(string labelText, float width) {
            EditorGUILayout.LabelField(labelText + ":", EditorStyles.boldLabel, GUILayout.Width(width));
        }
    }
}