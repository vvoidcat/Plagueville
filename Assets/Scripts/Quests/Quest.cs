using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLAGUEV.Quests {

    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 0)]
    public class Quest : ScriptableObject {

        [SerializeField] QuestType type;
        [SerializeField] QuestProgression progression;
        [SerializeField] string[] objectives;       // create a struct


        public QuestProgression GetProgression() {
            return progression;
        }

        public string[] GetObjectives() {
            return objectives;
        }

        public string GetObjectiveByIndex(int index) {
            return (objectives.Length == 0) ? null : objectives[index];
        }

        public string GetQuestName() {
            return "New Quest";             // customize
        }
    }
}