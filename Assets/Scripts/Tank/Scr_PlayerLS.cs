using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_PlayerLS : MonoBehaviour
{
    public List<string> validscenes = new List<string>();
    public string lastscene;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (!validscenes.Contains(SceneManager.GetActiveScene().name)) Destroy(gameObject);
    }
}
