using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLAGUEV.Core {
    public enum PlayerClass {
        THIEF,
        WIZARD,
        GUARD,
        NUN,
    }

    public static class PlayerProperties {

        public static bool charChosen {get; private set; }
        
        public static int playerClass { get; private set; }
        public static bool isMale { get; private set; }

        
        private static void SetPlayerClass(PlayerClass pClass) {
            if (pClass == PlayerClass.THIEF) {
                playerClass = 1;
                isMale = false;
            } else if (pClass == PlayerClass.WIZARD) {
                playerClass = 2;
                isMale = true;
            } else if (pClass == PlayerClass.GUARD) {
                playerClass = 3;
                isMale = true;
            } else if (pClass == PlayerClass.NUN) {
                playerClass = 4;
                isMale = false;
            }

            charChosen = true;
        }
    }
}
