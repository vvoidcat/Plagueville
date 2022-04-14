using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PLAGUEV.Core {

    public class PlayerSettings : MonoBehaviour {
        public static bool isMale { get; private set; }

        [System.Serializable] struct Player {
            public PlayerClass playerClass;
            public StatType mainStat;
            public bool isMale;
            public bool isNull;
        }

        [SerializeField] PlayerClass playerClass;
        Player player = new Player();

        void Awake() {
            InitializePlayer(playerClass);          // REMOVE WHEN THE MAIN MENU IS READY
        }

        public void InitializePlayer(PlayerClass playerClass) {
            if (!player.isNull) return;

            player.playerClass = playerClass;

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

            player.isNull = false;

            // if (SceneManager.GetActiveScene().name == "Main") {
            // }
        }
    }
}
