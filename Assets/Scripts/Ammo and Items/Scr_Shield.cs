using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Scr_Shield : MonoBehaviour
{
    public string creator;

    private float timer = 0;
    private Vector2 offset = new Vector2(0, 1);

    // Start is called before the first frame update
    void Start()
    {
        Color temp = this.GetComponent<MeshRenderer>().sharedMaterial.color;
        this.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(temp.r, temp.g, temp.b, .45f);

    }

    // Update is called once per frame
    void Update()
    {
        offset += new Vector2(0,1) *  Time.deltaTime;
        this.GetComponent<MeshRenderer>().sharedMaterial.mainTextureOffset = offset;

        timer += Time.deltaTime;
        if (timer >= 10) Destroy(this.gameObject);
    }
}
