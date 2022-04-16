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
            if (InteractWithGameObject()) return;
            if (InteractWithComponent()) return;

            SetCursor(CursorType.INACTIVE);
        }


        private bool InteractWithGameObject() {
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
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero);
            
            iRaycastable raycastable = hit.transform.GetComponent<iRaycastable>();

            if (raycastable != null && raycastable.HandleRaycast()) {
                SetCursor(CursorType.HOVERING);
                isInterracting = true;
            } else {
                SetCursor(CursorType.DEFAULT);
                isInterracting = true;
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

