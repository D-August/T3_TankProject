﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PickItem : MonoBehaviour
{
    public enum Type
    {
        Item,
        Ammo
    }

    public enum Amit
    {
        // Items
        Repair_Item,

        // Ammunitions
        Commom_Ammo,
        Shield_Ammo,
        Smoke_Ammo,
        Emp_Ammo
    }


    [Header("Asign Values")]
    public int amount;
    public Type getT;
    public Amit ammoItem;

    [Header("Do NOT USE")]
    [Tooltip("item or ammo")]
    public string type;
    [Tooltip("for item: repair | for ammo: common, shield, smoke or emp")]
    public string oname;
    

    public float rotatingSpeed = 25f;

    void Start()
    {
        //Adapting to make it easier to create the object, but making it work with the old code
        switch(getT)
        {
            case Type.Ammo:
                type = "ammo";

                switch(ammoItem)
                {
                    case Amit.Commom_Ammo:
                        oname = "common";
                        break;

                    case Amit.Shield_Ammo:
                        oname = "shield";
                        break;

                    case Amit.Smoke_Ammo:
                        oname = "smoke";
                        break;

                    case Amit.Emp_Ammo:
                        oname = "emp";
                        break;

                }

                break;
            case Type.Item:
                type = "item";

                switch (ammoItem)
                {
                    case Amit.Repair_Item:
                        oname = "repair";
                        break;

                }
                break;
        }
    }
    void Update()
    {
        transform.Rotate(new Vector3(0, rotatingSpeed * Time.deltaTime,0));
    }
}
