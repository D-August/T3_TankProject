using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TowerSpawn : MonoBehaviour
{
    [Header("Enemies")]
    [Range(0, 120f)]
    [Tooltip("Time between enemies spawn")]
    public float spawnTimer = 0f;
    private float st = 0f;
    [Tooltip("Max Enemies (Tower Minions) Number")]
    public int maxEnemies = 5;
    [Tooltip("Enemies Prefab")]
    public GameObject pref_enem;
    [Tooltip("List of current enemies")]
    public List<GameObject> enemies = new List<GameObject>();

    [Header("Targets")]
    public Transform minions_parent;

    // Start is called before the first frame update
    void Start()
    {
        st = spawnTimer;
    }
    void Update()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        if(enemies.Count < 5)
        {
            if (st >= spawnTimer)
            {
                GameObject temp_go = Instantiate(pref_enem, minions_parent);
                temp_go.transform.localPosition = new Vector3(0, 0, 0);
                enemies.Add(temp_go);
                st = 0;
            }
            else if (st < spawnTimer) st += Time.deltaTime;
        }
    }
}
