using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLAGUEV.Quests {

    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 0)]
    public class Quest : ScriptableObject {

        [SerializeField] QuestType questType;
        [SerializeField] QuestProgression questProgression;
        [SerializeField] string[] objective;        // create a struct


        
    }
}