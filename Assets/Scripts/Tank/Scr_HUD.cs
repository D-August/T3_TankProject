using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scr_HUD : MonoBehaviour
{
    public List<Canvas> cnvs = new List<Canvas>();

    [Header("Player")]
    public GameObject go_player = null;

    [Header("Canvas")]
    public Canvas c_01;
    public Canvas c_02;

    [Header("Hit Points")]
    public Slider s_hp;
    public float v_maxhp = 1;
    public float v_curhp = 1;

    [Header("Ammo")]
    public Image i_ammo;
    public List<Sprite> sp_ammo = new List<Sprite>();
    public int v_aa = 0;
    public TextMeshProUGUI tmp_ammo;

    [Header("Itens")]
    public Image i_item;
    public List<Sprite> sp_item = new List<Sprite>();
    public int v_ia = 0;
    public TextMeshProUGUI tmp_item;
    public Slider s_cldwn;

    [Header("Map")]
    public Image i_map;

    // Awake, Start & Update
    void Start()
    {
        
    }
    void Update()
    {
        SetPlayer();
        HPController();
        AmmoController();
        ItemController();
        AimController();
        ShowMap();
    }

    // Set Target
    public void SetPlayer()
    {
        if (!go_player) go_player = GameObject.Find("Tank (Player)");
        else  if(!GameObject.Find("Tank (Player)"))
        {
            go_player = null;
        }
    }

    // Hit Points
    public void HPController()
    {
        if(go_player)
        {
            v_maxhp = go_player.GetComponent<Scr_Controls_PROT>().maxHP;
            v_curhp = go_player.GetComponent<Scr_Controls_PROT>().hitPoints;
        }

        s_hp.maxValue = v_maxhp;
        s_hp.value = v_curhp;
    }

    // Ammunition
    public void AmmoController()
    {
        if (go_player)
        {
            v_aa = go_player.GetComponent<Scr_Inventory>().ammo[go_player.GetComponent<Scr_Inventory>().GetHeld("ammo")];
            try
            {
                i_ammo.sprite = sp_item[go_player.GetComponent<Scr_Inventory>().GetHeld("ammo")];
            } catch { }
        }

        tmp_ammo.text = v_aa.ToString();
    }
    public void ItemController()
    {
        if (go_player)
        {
            v_ia = go_player.GetComponent<Scr_Inventory>().items[go_player.GetComponent<Scr_Inventory>().GetHeld("items")];
            try
            {
                i_item.sprite = sp_item[go_player.GetComponent<Scr_Inventory>().GetHeld("items")];
            }
            catch { }
            try
            {
                switch (go_player.GetComponent<Scr_Inventory>().GetHeld("items"))
                {
                    case 0:
                        tmp_item.enabled = true;
                        s_cldwn.maxValue = go_player.GetComponent<Scr_Inventory>().rtLimit;
                        s_cldwn.value = go_player.GetComponent<Scr_Inventory>().rTimer;
                        break;
                    case 1:
                        tmp_item.enabled = false;
                        s_cldwn.maxValue = 1;
                        s_cldwn.value = 1;
                        break;
                }

            } catch { }
        }

        tmp_item.text = v_ia.ToString();
    }

    public void AimController()
    {
        if (go_player)
        {
            if (go_player.GetComponent<Scr_Controls_PROT>().sCam.enabled)
            {
                c_02.enabled = true;
            }
            else c_02.enabled = false;
        }
    }
    public void ShowMap()
    {
        if (Input.GetKeyDown(KeyCode.M) && i_map) i_map.enabled = !i_map.enabled;
    }

}
