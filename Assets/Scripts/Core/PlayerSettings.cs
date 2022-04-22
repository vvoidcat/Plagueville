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
            public bool isNull;         // do i need this?.. for what?
        }

        Player player = new Player();


        public PlayerClass GetPlayerClass() {
            return player.playerClass;
        }

        public void InitializePlayer(PlayerClass choiceClass) {
            player.playerClass = choiceClass;
            player.isNull = false;

            switch (player.playerClass) {
                case PlayerClass.THIEF:
                    player.mainStat = StatType.MONEY;
                    player.isMale = false;
                    break;
                case PlayerClass.WIZARD:
                    player.mainStat = StatType.KNOW;
                    player.isMale = true;
                    break;
                case PlayerClass.GUARD:
                    player.mainStat = StatType.GLORY;
                    player.isMale = true;
                    break;
                case PlayerClass.NUN:
                    player.mainStat = StatType.FAITH;
                    player.isMale = false;
                    break;       
            }
        }
    }
}
