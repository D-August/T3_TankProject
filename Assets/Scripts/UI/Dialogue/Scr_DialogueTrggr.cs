using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_DialogueTrggr : MonoBehaviour
{
    public Scr_Dialogue[] dialogue;
    public bool oneTimeOnly = false;

    public void TriggerDialogue()
    {
        Scr_DialogueMngr.mngr.StartDialogue(dialogue);
        if (oneTimeOnly) Destroy(gameObject);
    }
}