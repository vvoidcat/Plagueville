using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PLAGUEV.Core;

namespace PLAGUEV.Control {

    public class PlayerController : MonoBehaviour {

        [System.Serializable] struct Cursor {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] Cursor[] cursors = null;

        void Start() {

        }

        void Update() {
            
        }
    }
}

