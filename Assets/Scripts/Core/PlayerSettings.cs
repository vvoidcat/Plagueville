using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PLAGUEV.Core {

    public class PlayerSettings : MonoBehaviour {

        struct Player {
            public PlayerClass playerClass;
            public StatType mainStat;
            public bool isMale;
            public bool isNull;     // do i need this?..
        }
        
        [SerializeField] PlayerClass playerClass;
        Player player = new Player();

        void Awake() {
            InitializePlayer(playerClass);          // REMOVE WHEN THE MAIN MENU IS READY
        }

        public void InitializePlayer(PlayerClass playerClass) {
            player.playerClass = playerClass;
            player.isNull = false;

            if (player.playerClass == PlayerClass.THIEF) {
                player.mainStat = StatType.MONEY;
                player.isMale = false;
            }
            if (player.playerClass == PlayerClass.WIZARD) {
                player.mainStat = StatType.KNOW;
                player.isMale = true;
            }
            if (player.playerClass == PlayerClass.GUARD) {
                player.mainStat = StatType.GLORY;
                player.isMale = true;
            }
            if (player.playerClass == PlayerClass.NUN) {
                player.mainStat = StatType.FAITH;
                player.isMale = false;
            }
        }
    }
}
