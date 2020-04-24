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

    [Header("Death Explosion")]
    public GameObject dExplo;

    [Header("Main Camera")]
    public Camera mCam;

    [Header("ADS Camera")]
    public Camera sCam;

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

    [Header("Reset Tank Rotation")]
    private float rtr = 0f;
    [Tooltip("Time Needed to Hold R")]
    [Range(0, 60f)]
    public float maxRTR = 10f;

    // Stats
    [Header("Status")]
    public float hitPoints = 100f;
    public float maxHP = 100f;

    //public CInventory invent;

    public enum PossibleStates
    {
        PAUSED,
        INVINCIBLE,
        NORMAL,
        DIALOG
    }

    [Header("State Machine")]
    public PossibleStates currentState;

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
        Movement();
        CameraMovement();
        Shoot();
        Death();

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
            if (rtr >= maxRTR)
            {
                transform.position = new Vector3(0, .75f, 0) + transform.position;
                transform.rotation = new Quaternion(0, 0, 0, transform.rotation.w);

                rtr = 0;

                // Comentar Depois
                Debug.Log("Rotation Reset");
            }
            else rtr += Time.deltaTime;

            // Comentar Depois
            Debug.Log(rtr.ToString());
        }

        if (Input.GetKeyUp(KeyCode.R)) rtr = 0;
    }

    [System.Obsolete]
    public void Shoot()
    {
        // Aim
        GameObject tempCanon = cannon.transform.FindChild("Cannon").gameObject;

        if (sCam && Input.GetMouseButton(1))
        {
            mCam.enabled = false;
            mCam.GetComponent<AudioListener>().enabled = false;

            sCam.enabled = true;
            sCam.GetComponent<AudioListener>().enabled = true;

            tempCanon.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            mCam.enabled = true;
            mCam.GetComponent<AudioListener>().enabled = true;

            sCam.enabled = false;
            sCam.GetComponent<AudioListener>().enabled = false;

            tempCanon.GetComponent<MeshRenderer>().enabled = true;
        }

        // Use Item / Shot
        if(Input.GetMouseButtonDown(0))
        {
            this.gameObject.GetComponent<Scr_Inventory>().UseHeld("ammo");
        }
        this.gameObject.GetComponent<Scr_Inventory>().UseHeld("items");

        if (Input.GetKeyDown(KeyCode.Q)) this.gameObject.GetComponent<Scr_Inventory>().ChangeHeld("ammo");
        if (Input.GetKeyDown(KeyCode.E)) this.gameObject.GetComponent<Scr_Inventory>().ChangeHeld("items");
    }

    // Trigger Collider Events
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Turret") other.GetComponentInParent<Scr_TrackingSystem>().target = this.gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Smoke") Debug.Log("Covered by Smoke");
        if (other.transform.tag == "EMP") Debug.Log("HIT by EPM");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Turret") other.GetComponentInParent<Scr_TrackingSystem>().target = null;
    }

    // Hit Points Manipulation
    public void CallDamage(float amount)
    {
        hitPoints -= amount;
        
        Death();
    }
    public void CallRepair(float amount)
    {
        hitPoints += amount;
        if(hitPoints > maxHP) hitPoints = maxHP;
    }

    // Events
    public void Death()
    {
        if (hitPoints > maxHP) hitPoints = maxHP;

        if (hitPoints <= 0)
        {
            if(!mCam.enabled) mCam.enabled = true;
            mCam.transform.SetParent(null);

            GameObject temp = Instantiate(dExplo, transform.position, transform.rotation);
            temp.transform.localScale *= 2;
            temp.transform.SetParent(null);

            Destroy(this.gameObject);
        }
    }

}
