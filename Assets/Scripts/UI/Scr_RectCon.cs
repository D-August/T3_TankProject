using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Scr_RectCon : MonoBehaviour
{
    public RectTransform rec;
    public bool changeW = true;
    public bool changeH = true;

    // Update is called once per frame
    void Update()
    {
        if (rec)
        {
            rec.sizeDelta = new Vector2( changeW ? Screen.width : rec.sizeDelta.x, changeH ? Screen.height : rec.sizeDelta.y);
        } else
        {
            try { rec = GetComponent<RectTransform>(); } catch { }
        }
    }
}
