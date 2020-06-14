using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Lock : MonoBehaviour
{
    public bool locked = true;
    
    public void Unlock()
    {
        if(locked) { locked = false; };
    }
}
