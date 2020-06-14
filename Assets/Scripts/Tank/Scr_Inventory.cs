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
    public bool m_cannon = false;
    public List<Scr_ColoredKey> l_keys = new List<Scr_ColoredKey>();

    // Prefab Array
    [Header("Prefabs List")]
    [Tooltip("Ammunition Prefabs")]
    public List<GameObject> aPref = new List<GameObject>();
    [Tooltip("Mine Detector Prefab")]
    public GameObject mdPref;
    [Tooltip("Shot Smoke Prefab")]
    public GameObject pref_smk;

    [Header("Repair")]
    public float rTimer = 0;
    [Range(0, 60f)]
    public float rtLimit = 2f;

    [Header("Shot Cooldown")]
    public float scdTime = 5f;
    public float scdt = 0;

    [Header("Audio")]
    public List<AudioClip> ac_list = new List<AudioClip>();

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
            items[i] = 0;
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

                //ADD AUDIO
                //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, gameObject);

                break;

            case "ammo":
                heldAmm++;
                if (heldAmm >= ammo.Length) heldAmm = 0;
                Debug.Log(heldAmm.ToString());

                //ADD AUDIO TO HIT
                //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, gameObject);

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

                                    //PLAY AUDIO
                                    //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, gameObject);
                                }
                                else rTimer += Time.deltaTime;
                            }
                            else rTimer = 0;

                            break;

                        case 1:
                            mdPref.SetActive(true);

                            //PLAY AUDIO
                            //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, gameObject);

                            break;
                    }
                }
                else items[heldItm] = 0;
                break;

            case "ammo":
                GameObject temp;
                Vector3 impforce;
                if(scdt <= 0 && m_cannon)
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


                            //ADD AUDIO
                            Scr_AudioCon.ac.PlaySound(ac_list[0], 1, false, gameObject, Random.Range(.75f, 1.25f));

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

                                //ADD AUDIO
                                Scr_AudioCon.ac.PlaySound(ac_list[0], 1, false, gameObject, Random.Range(.75f, 1.25f));

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
        // Get Item/Ammo
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
        // Get Key (returns true if a key with the same color exists in the list)
    public bool GetKey(Scr_ColoredKey key)
    {
        foreach(Scr_ColoredKey k in l_keys)
        {
            if (k.color.Equals(key.color)) return true;
        }

        return false;
    }
    public bool GetKey(string color)
    {
        foreach (Scr_ColoredKey k in l_keys)
        {
            if (k.color.Equals(color)) return true;
        }

        return false;
    }

    // Later a per item max capacity will be added
    public void Addheld(string ar, string br, int amount)
    {
        switch(ar)
        {
            case "item":
                //ADD AUDIO
                //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, gameObject);

                switch (br)
                {
                    case "repair":
                        items[0] += amount;
                        break;

                    case "minedetector":
                        if (items[1] == 0) items[1] = 1;
                        break;
                }
                break;

            case "ammo":
                //ADD AUDIO
                //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, gameObject);

                switch (br)
                {
                    case "common":
                        ammo[0] += amount;
                        break;
                    case "shield":
                        ammo[1] += amount;
                        break;
                    case "smoke":
                        ammo[2] += amount;
                        break;
                    case "emp":
                        ammo[3] += amount;
                        break;
                }
                break;

            case "equip":
                switch (br)
                {
                    case "cannon":
                        m_cannon = true;
                        break;
                }
                break;
        }
    }
    // AddKey
    public void AddKey(Scr_ColoredKey key)
    {
        if (!GetKey(key))
        {
            l_keys.Add(key);
        }
    }
}
