using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PLAGUEV.Core;

namespace PLAGUEV.Control {

    public class PlayerController : MonoBehaviour {

        [System.Serializable] struct CursorMapping {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        [SerializeField] CursorMapping[] cursorMappings = null;


        void Awake() {
            
        }

        void Start() {

        }

        void Update() {
            if (InteractWithUI()) return;
            if (InteractWithComponent()) return;
            // if interact with background?..

            SetCursor(CursorType.INACTIVE);
        }


        private bool InteractWithUI() {
            bool isInterracting = false;

            if (EventSystem.current.IsPointerOverGameObject()) {        // find a better solution later
                SetCursor(CursorType.DEFAULT);
                isInterracting = true;
            }

            return isInterracting;
        }

        private bool InteractWithComponent() {
            bool isInterracting = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, Vector2.zero);

            foreach (RaycastHit2D hit in hits) {
                iRaycastable raycastable = hit.transform.GetComponent<iRaycastable>();

                if (raycastable != null && raycastable.HandleRaycast()) {
                    SetCursor(CursorType.HOVERING);     // move to CardController?
                    isInterracting = true;
                }
            }

            return isInterracting;
        }

        private void SetCursor(CursorType cursorType) {
            CursorMapping cursorMapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType cursorType) {
            CursorMapping cursorMapping = cursorMappings[0];

            for (int i = 0; i < cursorMappings.Length; i++) {
                if (cursorMappings[i].type == cursorType) {
                    cursorMapping = cursorMappings[i];
                    break;
                }
            }

            return cursorMapping;
        }
    }
}

