using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Inventory : MonoBehaviour
{
    // Held Objects (Pointers)
    [Header("Held Objects (DO NOT USE)")]
    [Tooltip("Held Item")]
    public int heldItm = 0;
    [Tooltip("Held Ammunition")]
    public int heldAmm = 0;

    // Item Counter (Array)
    [Header("Item/Ammo Counters")]
    [Tooltip("0 = Repair Kit | 1 = Mine Sensor | 2 = Other")]
    public int[] items;
    [Tooltip("0 = Normal | 1 = Shield | 2 = Smoke Screen | 3 = EMP (P.E.M.) ")]
    public int[] ammo;

    // Prefab Array
    [Header("Prefabs List")]
    [Tooltip("Ammunition Prefabs")]
    public List<GameObject> aPref = new List<GameObject>();
    [Tooltip("Mine Detector Prefab")]
    public GameObject mdPref;

    [Header("Repair")]
    private float rTimer = 0;
    [Range(0, 60f)]
    public float rtLimit = 2f;

    // Start is called before the first frame update
    void Start()
    {
        ammo = new int[aPref.Count];
        for(int i = 0; i < aPref.Count; i++)
        {
            ammo[i] = 10;
        }

        items = new int[2];
        for (int i = 0; i < 2; i++)
        {
            items[i] = 1;
        }

    }
    void Update()
    {
        if(heldItm != 1) mdPref.SetActive(false);
    }

    // Change Held Item
    public void ChangeHeld(string ar)
    {
        switch (ar)
        {
            case "items":
                heldItm++;
                if (heldItm >= items.Length) heldItm = 0;
                Debug.Log("H.I: " + heldItm.ToString());
                break;

            case "ammo":
                heldAmm++;
                if (heldAmm >= ammo.Length) heldAmm = 0;
                Debug.Log(heldAmm.ToString());
                break;
        }
    }

    // Use Held Item
    public void UseHeld(string ar)
    {
        switch (ar)
        {
            case "items":
                if (items[heldItm] > 0)
                {
                    switch (heldItm)
                    {
                        case 0:
                            if (Input.GetKey(KeyCode.LeftShift))
                            {
                                if (rTimer >= rtLimit)
                                {
                                    this.gameObject.GetComponent<Scr_Controls_PROT>().CallRepair(50);
                                    items[heldItm]--;
                                    rTimer = 0;
                                }
                                else rTimer += Time.deltaTime;
                            }
                            else rTimer = 0;

                            break;

                        case 1:
                            mdPref.SetActive(true);
                            break;
                    }
                }
                else items[heldItm] = 0;
                break;

            case "ammo":
                GameObject temp;
                Vector3 impforce;
                switch (heldAmm)
                {
                    case 0:
                        temp = Object.Instantiate(aPref[heldAmm], this.gameObject.GetComponent<Scr_Controls_PROT>().sSpawn.transform.position, this.gameObject.GetComponent<Scr_Controls_PROT>().cannon.transform.rotation, null);
                        impforce = this.GetComponent<Rigidbody>().velocity;
                        temp.GetComponent<Rigidbody>().AddForce(impforce, ForceMode.VelocityChange);
                        break;

                    default:
                        if (ammo[heldAmm] > 0)
                        {
                            temp = Object.Instantiate(aPref[heldAmm], this.gameObject.GetComponent<Scr_Controls_PROT>().sSpawn.transform.position, this.gameObject.GetComponent<Scr_Controls_PROT>().cannon.transform.rotation, null);
                            impforce = this.GetComponent<Rigidbody>().velocity;
                            temp.GetComponent<Rigidbody>().AddForce(impforce, ForceMode.VelocityChange);
                            ammo[heldAmm]--;
                        }
                        else ammo[heldAmm] = 0;
                        break;
                }
                break;
        }
    }
}
