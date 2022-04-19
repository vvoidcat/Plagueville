using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PLAGUEV.Dialogue {

    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue", order = 0)]
    public class DialogueTree : ScriptableObject {

        //
        public DialogueNode[] nodes;

        public bool isPlot = true;
        public string characterName;
    }
}
