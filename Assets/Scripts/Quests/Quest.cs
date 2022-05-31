using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLAGUEV.Quests {

    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 0)]
    public class Quest : ScriptableObject {

        [System.Serializable] public struct Objective {
            public string name;
            public string description;
        };

        [SerializeField] QuestType type;
        [SerializeField] QuestProgression progression;
        [SerializeField] Objective[] objectives;


        void Awake() {

        }


        public QuestType GetQuestType() {
            return type;
        }

        public QuestProgression GetProgression() {
            return progression;
        }

        public string[] GetObjectives() {
            string[] result = new string[objectives.Length];

            for (int i = 0; i < objectives.Length; i++) {
                result[i] = objectives[i].name;
            }

            return result;
        }

        public string GetObjectiveByIndex(int index) {
            return (objectives.Length == 0) ? null : GetObjectives()[index];
        }

        public int GetIndexObjective(string obj) {
            int result = 0;

            for (int i = 0; i < objectives.Length; i++) {
                if (obj == objectives[i].name) {
                    result = i;
                    break;
                }
            }

            return result;
        }

        public string GetQuestName() {
            return name;
        }
    }
}