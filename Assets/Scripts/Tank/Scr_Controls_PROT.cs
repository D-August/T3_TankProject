using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scr_Controls_PROT : MonoBehaviour
{
    // Phisical Parts
    [Header("Tank Parts")]
    public GameObject tTop;
    public GameObject tBase;

    [Header("Cannon Parts")]
    public GameObject cannon;
    public MeshRenderer cannonMesh;
    public GameObject sSpawn;

    [Header("Wheels")]
    public List<GameObject> wheels = new List<GameObject>();

    [Header("Death Explosion")]
    public GameObject dExplo;

    [Header("Crosshair")]
    public GameObject obj_ch;
    private Vector3 ch_op;
    private Vector3 ch_lop;
    private float ch_dst;

    [Header("Cameras")]
    public Camera mCam;
    public Camera sCam;
    public GameObject pref_mCam;
    private Vector3 cos = new Vector3(0, 0, 0); // Cam Offset

    [Header("Audio")]
    public List<AudioClip> ac_list = new List<AudioClip>();
    public AudioSource as_motor;
    public AudioSource as_break;

    // Limiters
    [Header("Cannon Rotation")]
    [Range(0, 50f)]
    [Tooltip("Camera Max Rotation")]
    public float cmax = 10f;
    [Range(0f, 360f)]
    [Tooltip("Top Max Rotation")]
    public float trange = 180;

    [Header("Speeds")] 
    [Tooltip("Camera Rotation Speed")]
    public float camSpd = .75f;
    [Tooltip("Movement Speed <Unused>")]
    public float mSpd = .5f;

    [Header("Reset Tank Rotation")]
    private float rtr = 0f;
    [Tooltip("Time Needed to Hold R")]
    [Range(0, 60f)]
    public float maxRTR = 10f;

    // Stats
    [Header("Status")]
    public float hitPoints = 100f;
    public float maxHP = 100f;
    [Range(0f, 500f)]
    [Tooltip("Normal Max Speed")]
    public float maxSpd = 50f;
    [Tooltip("Mine Detector Max Speed")]
    [Range(0f, 100f)]
    public float mdMaxSpd = 25f;

    // State Machine
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
        if (obj_ch)
        {
            ch_op = obj_ch.transform.position;
            ch_lop = obj_ch.transform.localPosition;
            ch_dst = Vector3.Distance(cannon.transform.position, ch_op);
            
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void FixedUpdate()
    {
        if (!mCam)
        {
            try { mCam = GameObject.Find("Main Camera").GetComponent<Camera>(); }
            catch
            {
                if (pref_mCam) mCam = Instantiate(pref_mCam).GetComponent<Camera>();
                mCam.gameObject.transform.SetParent(null);
            }
        }
        Movement();
        TopMovement();
        Shoot();
        HudController();
    }

    // Actions
    public void TopMovement()
    {
        /* Limitar rotação topo a 180º e implementar limite a movimentação do canhão*/
        // Rotação
            // Canhão
        cos += new Vector3(Input.GetAxis("Mouse Y") * (-camSpd * Time.deltaTime), 0, 0);
        if (cos.x < -cmax) cos = new Vector3(-cmax, cos.y, 0); if (cos.x > cmax) cos = new Vector3(cmax, cos.y, 0);

        // Topo
        cos += new Vector3(0, Input.GetAxis("Mouse X") * (camSpd * Time.deltaTime), 0);
        if (cos.y < -(trange/2)) cos = new Vector3(cos.x, -(trange / 2), 0); if (cos.y > (trange / 2)) cos = new Vector3(cos.x, (trange / 2), 0);

        tTop.transform.localEulerAngles = new Vector3(0, cos.y, 0);
        cannon.transform.localEulerAngles = new Vector3(cos.x, 0, 0);


    }
    public void Movement()
    {
        // Reset Position
            // Hold Key
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

            // Debug.Log(rtr.ToString());
        }
            // Release Ket
        if (Input.GetKeyUp(KeyCode.R)) rtr = 0;

        SpeedController();
        
    } 
    public void Shoot()
    {
        if (sCam && Input.GetMouseButton(1))
        {
            mCam.enabled = false;
            mCam.GetComponent<AudioListener>().enabled = false;

            sCam.enabled = true;
            sCam.GetComponent<AudioListener>().enabled = true;

            cannonMesh.enabled = false;
        }
        else if(mCam)
        {
            mCam.enabled = true;
            mCam.GetComponent<AudioListener>().enabled = true;

            sCam.enabled = false;
            sCam.GetComponent<AudioListener>().enabled = false;

            if(GetComponent<Scr_Inventory>().m_cannon) cannonMesh.enabled = true; else cannonMesh.enabled = false;
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

    // Controllers
        // Speed
    public void SpeedController()
    {
        Rigidbody tempRb = gameObject.GetComponent<Rigidbody>();
        bool tempB = false;
        int wig = 0;

        // MOTOR SOUND (The closer to top speed higher is the volume)
        if (as_motor)
        {
            if (!as_motor.clip) as_motor.clip = ac_list[0];

            if ((tempRb.velocity.magnitude / mdMaxSpd) >= .25f && Input.GetKey(KeyCode.W))
            {
                as_motor.volume = (tempRb.velocity.magnitude / mdMaxSpd);
            }
            else if ((tempRb.velocity.magnitude / mdMaxSpd) >= .25f && as_motor.volume >= .25f)
            {
                as_motor.volume -= .25f * Time.deltaTime;
            }
            else as_motor.volume = .25f;

            if (Input.GetKey(KeyCode.W)) as_motor.pitch = 1 + (tempRb.velocity.magnitude / mdMaxSpd);
            else if (as_motor.pitch >= 1) as_motor.pitch -= .25f * Time.deltaTime;
            else as_motor.pitch = 1;

            if (!as_motor.isPlaying && !Scr_PauseMenu.pm.isPaused) as_motor.Play();
        }

        // BREAK SOUND (The higher the speed, the higher is the volume)
        if (as_break && !Scr_PauseMenu.pm.isPaused)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                if (!as_break.clip) as_break.clip = ac_list[1];

                as_break.volume = (tempRb.velocity.magnitude / mdMaxSpd) * 1f;
                
                if (!as_break.isPlaying) as_break.Play();
            }
            else
            {
                as_break.Pause();
            }
        }

        foreach (GameObject g in wheels)
        {
            if (g.GetComponent<Scr_IsGrounded>().isGround())
            {
                wig++;
            }
        }

        if (wig > 2) { tempB = true; Debug.Log("Grounded"); }

        // Speed Limits
        switch (gameObject.GetComponent<Scr_Inventory>().GetHeld("items"))
        {
            case 1:
                if (tempRb.velocity.magnitude > mdMaxSpd)
                {
                    if (Input.GetKey(KeyCode.W) && tempB) tempRb.velocity = tempRb.velocity.normalized * mdMaxSpd;
                }
                break;

            default:
                if (tempRb.velocity.magnitude > maxSpd)
                {
                    if (Input.GetKey(KeyCode.W) && tempB) tempRb.velocity = tempRb.velocity.normalized * maxSpd;
                }
                break;
        }
    }
        // OLD Hud
    public void HudController()
    {
        // Crosshair position
        RaycastHit hit;
        RaycastHit hit2;
        int layerMask = 1 << 12;
        layerMask = ~layerMask;

        bool isHit = Physics.Raycast(cannon.transform.position, cannon.transform.forward, out hit, ch_dst, layerMask);
        bool isHit2 = Physics.Raycast(cannon.transform.position, cannon.transform.forward, out hit2, ch_dst * 1.5f, layerMask);
        if (isHit && isHit2)
        {
            Debug.DrawLine(cannon.transform.position, hit.point, Color.green);

            obj_ch.transform.position = hit.point;
            if (hit.collider.transform.tag == "Des_OBJ") obj_ch.GetComponent<SpriteRenderer>().color = Color.red;
            else obj_ch.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else if(!isHit && isHit2)
        {
            Debug.DrawLine(cannon.transform.position, hit2.point, Color.blue);

            if (hit2.collider.transform.tag == "Des_OBJ") obj_ch.GetComponent<SpriteRenderer>().color = Color.red;
            else obj_ch.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            obj_ch.transform.localPosition = ch_lop;
            obj_ch.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    // Trigger Collider Events
    private void OnTriggerEnter(Collider other)
    {
        switch (other.transform.tag)
        {
            case "Turret":
                other.GetComponentInParent<Scr_TrackingSystem>().target = this.gameObject;
                break;

            case "TankTurret":
                try { other.GetComponent<Scr_TankTurr>().target = this.gameObject; }
                catch
                {
                    if (other.transform.parent != null) other.GetComponentInParent<Scr_TankTurr>().target = this.gameObject;
                    else other.GetComponentInChildren<Scr_TankTurr>().target = this.gameObject;
                }
                break;

            case "Pickable":
                Scr_PickItem temp = other.GetComponent<Scr_PickItem>();
                GetComponent<Scr_Inventory>().Addheld(temp.type, temp.oname, temp.amount);
                Destroy(other.gameObject);
                break;

            case "DialogTrigger":
                try { other.GetComponent<Scr_DialogueTrggr>().TriggerDialogue(); }
                catch
                {
                    if (other.transform.parent != null) { other.GetComponentInParent<Scr_DialogueTrggr>().TriggerDialogue(); }
                    else { other.GetComponentInChildren<Scr_DialogueTrggr>().TriggerDialogue(); }
                }
                break;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Smoke")) Debug.Log("Covered by Smoke");
        if (other.transform.CompareTag("EMP")) Debug.Log("HIT by EPM");
        if (other.transform.CompareTag("Turret")) other.GetComponentInParent<Scr_TrackingSystem>().target = this.gameObject;
        if (other.transform.CompareTag("SpawnPoint"))
        {
            GameObject go_temp = GameObject.Find("SceneTransition");
            DontDestroyOnLoad(mCam);
            go_temp.GetComponent<Scr_SceneLoad>().ChangeScene(other.GetComponentInParent<Scr_SpawnPointTransition>().scenename);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.transform.tag)
        {
            case "Turret":
                other.GetComponentInParent<Scr_TrackingSystem>().target = null;
                break;
            case "TankTurret":
                try { other.GetComponent<Scr_TankTurr>().target = null; }
                catch
                {
                    if (other.transform.parent != null) other.GetComponentInParent<Scr_TankTurr>().target = null;
                    else other.GetComponentInChildren<Scr_TankTurr>().target = null;
                }
                break;
        }
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

            GameObject temp = Instantiate(dExplo, transform.position, transform.rotation);
            temp.transform.localScale *= 2;
            temp.transform.SetParent(null);

            //ADD AUDIO TO EXPLOSION
            //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, temp);

            Destroy(this.gameObject);
        }
    }
    public void PauseAudio()
    {
        if (as_motor.isPlaying) as_motor.Pause();
        if (as_break.isPlaying) as_break.Pause();
    }
}
