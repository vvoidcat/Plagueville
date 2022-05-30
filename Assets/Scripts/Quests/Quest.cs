using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLAGUEV.Quests {

    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 0)]
    public class Quest : ScriptableObject {

        [SerializeField] QuestType type;
        [SerializeField] QuestProgression progression;
        [SerializeField] string questName;
        [SerializeField] string[] objectives;       // create a struct?
        int indexQuest = 0;
        int indexObjective = 0;

        void Awake() {

        }


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

        public int GetIndexQuest() {
            return indexQuest;
        }

        public int GetIndexObjective(string objective) {
            int result = 0;

            for (int i = 0; i < objectives.Length; i++) {
                if (objective == objectives[i]) {
                    result = i;
                    break;
                }
            }

            return result;
        }
    }
}