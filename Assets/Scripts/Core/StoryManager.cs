using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLAGUEV.Core {

    public class StoryManager : MonoBehaviour {

        [SerializeField] bool prologueOn = true;
        [SerializeField] bool endingOn = false;


        public bool GetPrologueState() {
            return prologueOn;
        }

        public bool GetEndingState() {
            return endingOn;
        }
    }
}
