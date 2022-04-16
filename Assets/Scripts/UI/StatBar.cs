using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PLAGUEV.Core;


namespace PLAGUEV.UI {

   public class StatBar : MonoBehaviour {

        [SerializeField] StatType stat;

        RectTransform rect = null;
        StatManager statManager = null;

        void Awake() {
            rect = transform.GetChild(0).GetComponent<RectTransform>();
            statManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StatManager>();
        }

        void Start() {
           rect.localScale = new Vector2(statManager.GetFraction(stat), 1);
        }

        void FixedUpdate() {
            // these things should actually fire on card choice but they'll be here for now
            DrawFiller();
        }

        private void DrawFiller() {
            rect.localScale = new Vector2(statManager.GetFraction(stat), 1);
        }
    }
}