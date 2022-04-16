using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLAGUEV.Core {

    public class StatManager : MonoBehaviour {

        public const int statMax = 100;
        public const int statMin = 0;
        public const int statsSize = 4;

        [Range(statMin, statMax)] [SerializeField] int initValue = 25;
        [SerializeField] bool useInitValueOnGameStart = true;

        [System.Serializable] struct Stat {
            public StatType type;
            [Range(statMin, statMax)] public int value;
        }

        [SerializeField] Stat[] stats = new Stat[statsSize];


        void Awake() {
            InitializeStats();
        }

        void FixedUpdate() {

        }


        private void InitializeStats() {
            int i = 0;

            foreach (StatType type in Enum.GetValues(typeof(StatType))) {
                if (useInitValueOnGameStart) {
                    stats[i].value = initValue;
                }
                stats[i].type = type;
                i++;
            }
        }

        public void SetValues(int money, int knowledge, int glory, int faith) {
            int[] values = {money, knowledge, glory, faith};

            for (int i = 0; i < statsSize; i++) {
                stats[i].value = values[i];
            }
        }

        public void SetValue(int value, StatType stat) {
            for (int i = 0; i < statsSize; i++) {
                if (stats[i].type == stat) {
                    stats[i].value = value;
                    break;
                }
            }
        }

        public int GetValue(StatType stat) {
            int value = 0;

            for (int i = 0; i < statsSize; i++) {
                if (stats[i].type == stat) {
                    value = stats[i].value;
                    break;
                }
            }

            return value;
        }

        public float GetPercentage(StatType stat) {
            return CalculatePercentage(GetValue(stat));
        }

        public float GetFraction(StatType stat) {
            return CalculateFraction(GetValue(stat));
        }

        private float CalculateFraction(int stat) {
            return (float)stat / statMax;
        }

        private float CalculatePercentage(int stat) {
            return (float)stat / statMax * 100;
        }
    }
}
