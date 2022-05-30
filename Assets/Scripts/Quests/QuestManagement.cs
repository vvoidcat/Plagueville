using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace PLAGUEV.Quests {

    public static class QuestManagement {


        // public Quest[] GetAllQuests() {
        //     return questData;
        // }

        // public Quest GetQuestByIndex(int index) {
        //     return questData[index];
        // }

        // public string[] GetQuestList() {
        //     return questList;
        // }

        // public void InitializeQuests() {
        //     if (questData == null || questList == null) {
        //         questData = LoadQuests();
        //         questList = BuildQuestList(questData);
        //     }
        // }

        public static Quest[] LoadQuests() {
            Quest[] result = Resources.LoadAll<Quest>("Quests");
            return result;
        }

        public static string[] BuildQuestList(Quest[] quests) {
            string[] result = new string[quests.Length];

            for (int i = 0; i < quests.Length; i++) {
                result[i] = quests[i].GetQuestName();
            }

            return result;
        }
    }
}