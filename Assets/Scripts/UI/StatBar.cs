using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PLAGUEV.Core;


namespace PLAGUEV.UI {
   public class StatBar : MonoBehaviour {
        [SerializeField] Stat stat;
        RectTransform rect;

        void Awake() {
            rect = transform.GetChild(0).GetComponent<RectTransform>();
        }

        void Start() {
            if (Stats.valuesExist) {
                rect.localScale = new Vector2(Stats.GetFraction(stat), 1);
            }
        }

        public void UpdateDepiction() {
            rect.localScale = new Vector2(Stats.GetFraction(stat), 1);
        }

    }
}