using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Controls_PROT : MonoBehaviour
{
    // Phisical Parts
    [Header("Tank Parts")]
    public GameObject tTop;
    public GameObject tBase;

    [Header("Cannon Parts")]
    public GameObject cannon;
    public GameObject sSpawn;

    [Header("Main Camera")]
    public Camera mCam;

    // Limiters
    [Header("Cannon Rotation")]
    [Range(0, 50f)]
    [Tooltip("Camera Max Rotation")]
    public float cmax = 10f;
    [Range(0f, 360f)]
    [Tooltip("Top Max Rotation")]
    public float trange = 180;

    [Header("Speeds")] 
    [Range(0, 2f)]
    [Tooltip("Camera Rotation Speed")]
    public float camSpd = .75f;
    [Tooltip("Movement Speed <Unused>")]
    public float mSpd = .5f;

    private Vector3 cos = new Vector3(0, 0, 0); // Cam Offset

    // Stats
    [Header("Status")]
    public float hitPoints = 100f;
    public float maxHP = 100f;

    //public CInventory invent;

    // Awake, Start & Update
    void Awake()
    {
        //this.gameObject.AddComponent<Scr_Inventory>();
    }
    void Start()
    {

    }
    void Update()
    {
        CameraMovement();
        Shoot();

    }

    // Actions
    public void CameraMovement()
    {
        /* Limitar rotação topo a 180º e implementar limite a movimentação do canhão*/
        // Rotação
            // Canhão
        cos += new Vector3(Input.GetAxis("Mouse Y") * -camSpd, 0, 0);
        if (cos.x < -cmax) cos = new Vector3(-cmax, cos.y, 0); if (cos.x > cmax) cos = new Vector3(cmax, cos.y, 0);

        // Topo
        cos += new Vector3(0, Input.GetAxis("Mouse X") * camSpd, 0);
        if (cos.y < -(trange/2)) cos = new Vector3(cos.x, -(trange / 2), 0); if (cos.y > (trange / 2)) cos = new Vector3(cos.x, (trange / 2), 0);

        tTop.transform.localEulerAngles = new Vector3(0, cos.y, 0);
        cannon.transform.localEulerAngles = new Vector3(cos.x, 0, 0);
    }
    public void Movement()
    {
        Rigidbody tempRb = tBase.gameObject.GetComponent<Rigidbody>();
        
        // Foward
        if (Input.GetKey(KeyCode.R))
        {
            // Reload Tank
        }
    }
    public void Shoot()
    {
        if(Input.GetMouseButtonDown(0))
        {
            this.gameObject.GetComponent<Scr_Inventory>().UseHeld("ammo");
        }
        if(Input.GetKeyDown(KeyCode.Q)) this.gameObject.GetComponent<Scr_Inventory>().ChangeHeld("ammo");
        if (Input.GetKeyDown(KeyCode.E)) this.gameObject.GetComponent<Scr_Inventory>().ChangeHeld("items");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Smoke") Debug.Log("Covered by Smoke");
        if (other.transform.tag == "EMP") Debug.Log("HIT by EPM");
    }

    // Hit Points Manipulation
    public void callDamage(float amount)
    {
        hitPoints -= amount;
        if(hitPoints <= 0) {/*Death*/}
    }
    public void callRepair(float amount)
    {
        hitPoints += amount;
        if(hitPoints > maxHP) hitPoints = maxHP;
    }
}

public class CInventory
{
    

    
}
