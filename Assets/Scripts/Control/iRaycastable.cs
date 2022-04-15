using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PLAGUEV.Control {

    public interface iRaycastable {

        CursorType GetCursorType();
        bool HandleRaycast();   // arguments?..
    }
}