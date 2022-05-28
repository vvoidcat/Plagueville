using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace PLAGUEV.Quests {

    public class QuestManager : MonoBehaviour {

        Quest[] quests;

        void Awake() {
            quests = Resources.LoadAll<Quest>("Quests");
        }

        public Quest[] GetAllQuests() {
            return quests;
        }
    }
}