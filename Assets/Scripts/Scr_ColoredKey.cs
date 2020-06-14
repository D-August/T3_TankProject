using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "new colored key", menuName = "Items/Create Colored Key")]
public class Scr_ColoredKey : ScriptableObject
{
    public string color;
    public Image sprite;
}
