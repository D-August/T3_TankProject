using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Sentence", menuName = "Dialog/CreateSentences")]
public class Scr_Dialogue : ScriptableObject
{
    public string char_name;
    [TextArea(3, 5)]
    public string[] sentences;
}
