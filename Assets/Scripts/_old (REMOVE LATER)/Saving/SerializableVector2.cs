using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace TSARSTVO.Saving
{

    [System.Serializable]       // setting a class as serializable, which means that all of its fields will automatically be serialized
    public class SerializableVector2
    {
        float x, y;


        // creating a Constructor
        public SerializableVector2(Vector2 vector)
        {
            x = vector.x;
            y = vector.y;
        }



        // converting deserialized data (floats) back to a vector
        public Vector2 ToVector ()
        {          
            return new Vector2 (x, y);
        }

    }
}
