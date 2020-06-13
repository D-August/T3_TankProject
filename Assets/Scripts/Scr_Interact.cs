using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scr_Interact : MonoBehaviour
{
    public enum Type
    {
        Play_Dialogue,
        Use_Item,
    }
    public enum Item
    {
        None,
        Key
    }


    public Type type;
    public Item item;

    public string key_color;

    public void Interact()
    {
        switch (type)
        {
            case Type.Play_Dialogue:
                GetComponent<Scr_DialogueTrggr>().TriggerDialogue();
                break;
            case Type.Use_Item:
                switch (item)
                {
                    case Item.Key:
                        
                        break;
                }
                break;
        }
    }

    public void UseKey(Scr_ColoredKey key)
    {
        
    }
}
