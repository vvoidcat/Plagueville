using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PLAGUEV.Core;
using PLAGUEV.UI;

namespace PLAGUEV.Control {
    public class StatManager : MonoBehaviour {
        [Range(0, 100)] [SerializeField] int initValue;
        [SerializeField] bool useInitValueOnGameStart;
        [Range(0, 100)] [SerializeField] int money;
        [Range(0, 100)] [SerializeField] int knowledge;
        [Range(0, 100)] [SerializeField] int glory;
        [Range(0, 100)] [SerializeField] int faith;

        StatBar[] statBars = new StatBar[4];
        [SerializeField] RectTransform statPanel = null;



        void Awake() {
            // if there are no saved games, but what if there are?
            int count = 0;
            foreach (StatBar statBar in statBars) {
                statBars[count] = statPanel.transform.GetChild(count).GetComponent<StatBar>();
            }

            if (!Stats.valuesExist) {
                SetStartingValues();
            }
        }

        void Start() {

        }

        void Update() {
            // checks if value has changed, calls ui things if it has
            if (Stats.money != money) {

            }
        }

        
        private void SetStartingValues() {
            if (useInitValueOnGameStart) {
                Stats.SetInitValues(initValue);
                Stats.valuesExist = true;
            } else {
                Stats.SetValue(money, Stat.MONEY);
                Stats.SetValue(knowledge, Stat.KNOW);
                Stats.SetValue(glory, Stat.GLORY);
                Stats.SetValue(faith, Stat.FAITH);
                Stats.valuesExist = true;
            }
        }
    }
}