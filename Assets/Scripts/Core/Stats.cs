// all variables for stats are stored here


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PLAGUEV.Core {
    public enum Stat {
        MONEY,
        KNOW,
        GLORY,
        FAITH
    }

    public static class Stats {
        public static bool valuesExist { get; set; }
        public static bool valuesModified { get; set; }

        public static int money { get; private set; }
        public static int knowledge { get; private set; }
        public static int glory { get; private set; }
        public static int faith { get; private set; }

        public const int statMax = 100;


        public static void SetInitValues(int initValue) {
            money = initValue;
            knowledge = initValue;
            glory = initValue;
            faith = initValue;
        }

        public static void SetValue(int value, Stat stat) {
            if (stat == Stat.MONEY) {
                money = value;
            } else if (stat == Stat.KNOW) {
                knowledge = value;
            } else if (stat == Stat.GLORY) {
                glory = value;
            } else if (stat == Stat.FAITH) {
                faith = value;
            }
        }

        public static int GetValue(Stat stat) {
            int value = 0;

            if (stat == Stat.MONEY) {
                value = money;
            } else if (stat == Stat.KNOW) {
                value = knowledge;
            } else if (stat == Stat.GLORY) {
                value = glory;
            } else if (stat == Stat.FAITH) {
                value = faith;
            }

            return value;
        }

        public static float GetPercentage(Stat stat) {
            return CalculatePercentage(GetValue(stat));
        }

        public static float GetFraction(Stat stat) {
            return CalculateFraction(GetValue(stat));
        }

        private static float CalculateFraction(int stat) {
            return (float)stat / statMax;
        }

        private static float CalculatePercentage(int stat) {
            return (float)stat / statMax * 100;
        }
    }
}
