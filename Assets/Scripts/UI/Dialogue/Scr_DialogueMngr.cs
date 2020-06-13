using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_DialogueMngr : MonoBehaviour
{
    // Singleton
    public static Scr_DialogueMngr mngr = null;

    [Header("Setences List")]
    public Queue<string> sentences;
    public Queue<string> names;

    [Header("Dialogue Box")]
    public GameObject db;

    [Header("TextMeshes")]
    public TextMeshProUGUI char_name;
    public TextMeshProUGUI dialogue_text;

    void Awake()
    {
        if (mngr == null) mngr = this;
        sentences = new Queue<string>();
        names = new Queue<string>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        db.SetActive(Scr_PauseMenu.pm.onDialogue);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Scr_Dialogue[] dialogue)
    {
        Scr_PauseMenu.pm.SEDialogue(true);

        sentences.Clear();
        names.Clear();
        foreach(Scr_Dialogue d in dialogue)
        {
            foreach (string s in d.sentences)
            {
                sentences.Enqueue(s);
                names.Enqueue(d.char_name);
            }
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count <= 0)
        {
            EndDialogue();
            return;
        }

        dialogue_text.text = sentences.Dequeue();
        char_name.text = names.Dequeue();
    }

    public void EndDialogue()
    {
        Scr_PauseMenu.pm.SEDialogue(false);
    }
}
