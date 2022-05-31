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
        [SerializeField] QuestProgression progression = QuestProgression.INACTIVE;
        [SerializeField] Objective[] objectives = new Objective[1] { new Objective() };

        // readonly static Objective first = new Objective { name = "init", description = "quest inactive // DO NOT EDIT" };
        // readonly static Objective last = new Objective { name = "end", description = "fin // DO NOT EDIT" };



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
            return objectives[index].name;
        }

        public string[] GetDescriptions() {
            string[] result = new string[objectives.Length];

            for (int i = 0; i < objectives.Length; i++) {
                result[i] = objectives[i].description;
            }

            return result;
        }

        public string GetDescriptionByIndex(int index) {
            return objectives[index].description;
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