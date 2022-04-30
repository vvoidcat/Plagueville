using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PLAGUEV.Dialogue.Editor {

    public static class DialogueGUIStyles {

        [NonSerialized] static GUIStyle style;
        static GUIStyle defaultNodeStyle;
        static GUIStyle playerNodeStyle;
        static GUIStyle rootLabelStyle;
        static GUIStyle textStyle;
        public const float bgTextureSize = 100f;


        public static GUIStyle GetNodeStyle() {
            return style;
        }

        public static GUIStyle GetTextStyle() {
            return textStyle;
        }

        public static GUIStyle GetRootLabelStyle() {
            return rootLabelStyle;
        }

        public static void SetGUIStyles() {
            style = new GUIStyle();

            // card nodes
            defaultNodeStyle = new GUIStyle();
            defaultNodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            defaultNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            defaultNodeStyle.border = new RectOffset(12, 12, 12, 12);

            // player nodes
            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);

            // <START> node
            rootLabelStyle = new GUIStyle(GUI.skin.label);
            rootLabelStyle.alignment = TextAnchor.MiddleCenter;
            rootLabelStyle.fontStyle = FontStyle.Bold;

            // text blocks
            textStyle = new GUIStyle(GUI.skin.textField);
            textStyle.clipping = TextClipping.Clip;
            textStyle.wordWrap = true;
            textStyle.padding = new RectOffset(8, 8, 8, 8);
        }

        public static void SetNodeStyle(SpeakerType speaker) {
            if (speaker == SpeakerType.PLAYER) {
                style = playerNodeStyle;
            } else {
                style = defaultNodeStyle;
            }
        }
    }
}