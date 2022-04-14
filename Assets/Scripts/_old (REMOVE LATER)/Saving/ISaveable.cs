using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TSARSTVO.Saving
{

    // there can only be methods or properties in the interface, not their implementations
    // interface is something of a contract or a list, as it doesn't implement anything itself; however, it helps to avoid circular dependencies on lower levels
    // each function needs to be implemented inside the class that's going to use it; thus, each of them can do different things while they all have the same name
    
    public interface ISaveable
    {

        object CaptureState();
        void RestoreState(object state);
    }
}