using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace PLAGUEV.Quests {

    public class QuestManager : MonoBehaviour {

        Quest[] quests = null;


        void Awake() {
            quests = LoadQuests();
        }


        public Quest[] GetAllQuests() {
            return quests;
        }

        public Quest[] LoadQuests() {
            return Resources.LoadAll<Quest>("Quests");
        }
    }
}