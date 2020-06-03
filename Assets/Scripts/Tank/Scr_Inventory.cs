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
    public float rTimer = 0;
    [Range(0, 60f)]
    public float rtLimit = 2f;

    [Header("Shot Cooldown")]
    public float scdTime = 5f;
    public float scdt = 0;

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
        if (scdt > 0) scdt -= Time.deltaTime;
    }

    // Change or Use
        // Change
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
        // Use
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
                            if (Input.GetKey(KeyCode.F))
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
                if(scdt <= 0)
                {
                    switch (heldAmm)
                    {
                        case 0:
                            // Instantiate bullet
                            temp = Object.Instantiate(aPref[heldAmm], this.gameObject.GetComponent<Scr_Controls_PROT>().sSpawn.transform.position, this.gameObject.GetComponent<Scr_Controls_PROT>().cannon.transform.rotation, null);

                            // Add Tank speed to bullet
                            impforce = this.GetComponent<Rigidbody>().velocity;
                            temp.GetComponent<Rigidbody>().AddForce(impforce, ForceMode.Impulse);

                            // Tank Recoil
                            this.gameObject.GetComponent<Rigidbody>().AddForce(-temp.transform.forward * 1500, ForceMode.Impulse);

                            // Set Cooldown Timer
                            scdt = scdTime;
                            break;

                        default:
                            if (ammo[heldAmm] > 0)
                            {
                                // Instantiate bullet
                                temp = Object.Instantiate(aPref[heldAmm], this.gameObject.GetComponent<Scr_Controls_PROT>().sSpawn.transform.position, this.gameObject.GetComponent<Scr_Controls_PROT>().cannon.transform.rotation, null);
                                
                                // Add Tank speed to bullet
                                impforce = this.GetComponent<Rigidbody>().velocity;
                                temp.GetComponent<Rigidbody>().AddForce(impforce, ForceMode.Impulse);
                                
                                // Tank Recoil
                                this.gameObject.GetComponent<Rigidbody>().AddForce(-temp.transform.forward * 1500, ForceMode.Impulse);
                                
                                // Ammo Reduction
                                ammo[heldAmm]--;
                                
                                // Set Cooldown Timer
                                scdt = scdTime;
                            }
                            else ammo[heldAmm] = 0;
                            break;
                    }
                }
                break;
        }
    }
        // Get
    public int GetHeld(string ar)
    {
        switch (ar)
        {
            case "items":
                return heldItm;
            case "ammo":
                return heldAmm;
            default:
                return 99;
        }
    }
        // Add
            // Later a per item max capacity will be added
    public void Addheld(string ar, string br, int amount)
    {
        switch(ar)
        {
            case "item":
                switch (br)
                {
                    case "repair":
                        items[0]++;
                        break;
                }
                break;
            case "ammo":
                switch (br)
                {
                    case "common":
                        ammo[0]++;
                        break;
                    case "shield":
                        ammo[1]++;
                        break;
                    case "smoke":
                        ammo[2]++;
                        break;
                    case "emp":
                        ammo[3]++;
                        break;
                }
                break;
        }
    }
}
