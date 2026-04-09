using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SpaceshipControl : MonoBehaviour
{
    private Rigidbody rb;

    //Inputs
    private Vector2 moveInput;      // Horizontal (x) + Vertical (y)
    private Vector2 mouseInput;     // Mouse X + Mouse Y
    private float rollInput;
    private PlayerInput playerInput;
    public GameObject shipMesh;
    public Camera _camera;
    Quaternion targetRotation;
    private bool isMoving;
    public bool isBoosting;

    public sceneManager_ sceneMan;

    private AudioSource audioSource;

    public ParticleSystem BoosterLeft, BoosterRight;

    [SerializeField] private AudioClip thrusterClip;
    [SerializeField] private AudioClip hit;
    [SerializeField] private float speedMult = 1;
    [SerializeField] private float boostMult = 2;
    [SerializeField] private float speedMultAngle = 0.5f;
    [SerializeField] private float speedRollMultAngle = 0.05f;
    [SerializeField] private float _paddingStrength;
    [Range(0.0f,90.0f)] public float speedRollMult = 45;
    [Range(0.0f,90.0f)] public float cameraFOVChange = 45;

    [SerializeField] private float timeLossVal = 2;
    public float timeLossMult = 1;
    public QuestManager _questManager;
    public Quest _questScr;
    float _speedLastFrame;
    public GameObject _drasil;
    bool bossAggression = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;
        rb.angularVelocity = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        _questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }
    private void Start()
    {
        if (_questManager._questList.Count > 0)
        {
            _questScr = _questManager._questList[0].GetComponent<Quest>();
        }
        if (_questManager._tutorialFlagsCompleted <= 3)
        {
            _questManager.FinishTutorialFlag();
        }
    }

    // ---------------- INPUT CALLBACKS ----------------
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        rollInput = context.ReadValue<float>();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        isBoosting = context.performed; 
    }
    // -------------------------------------------------

    private void FixedUpdate()
    {
        if (isBoosting && (moveInput.x != 0 || moveInput.y != 0))
        {
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, cameraFOVChange, Time.deltaTime);
            rb.AddForce(rb.transform.forward * moveInput.y * boostMult, ForceMode.Impulse);
            // rb.AddForce(rb.transform.right * moveInput.x * boostMult, ForceMode.Impulse);
            timeLossMult = timeLossVal;
        }
        else
        {
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, 60.0f, Time.deltaTime);
            timeLossMult = 1;
        }

        
        if ((moveInput.y < 0 || moveInput.y > 0 || moveInput.x < 0 || moveInput.x > 0) && !audioSource.isPlaying)
        {
            audioSource.clip = thrusterClip;
            audioSource.Play();
            isMoving = true;
            BoosterLeft.Play();
            BoosterRight.Play();
        }
        if (moveInput.y == 0 && moveInput.x ==0)
        {
            isMoving = false;
            BoosterLeft.Pause();
            BoosterRight.Pause();
        }
        // Translation
        rb.AddForce(rb.transform.forward * moveInput.y * speedMult, ForceMode.VelocityChange);
        // rb.AddForce(rb.transform.right * moveInput.x * speedMult, ForceMode.VelocityChange);

        // Rotation from mouse
        // might break later
        if (Time.timeSinceLevelLoad > 1)
        {
            rb.AddTorque(rb.transform.right * speedMultAngle * mouseInput.y * -1, ForceMode.VelocityChange);
            rb.AddTorque(rb.transform.up * speedMultAngle * mouseInput.x, ForceMode.VelocityChange);
        }


        // Roll
        rb.AddTorque(rb.transform.forward * speedRollMultAngle * rollInput, ForceMode.VelocityChange);
        
        //if (moveInput.x != 0 && rollInput == 0 )
        //{
        //    targetRotation = Quaternion.Euler(0.0f, 0.0f, -speedRollMult * moveInput.x);
        //}
        //else
        //{
        //    targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f) ;
        //}

        shipMesh.transform.localRotation = Quaternion.Lerp(shipMesh.transform.localRotation,targetRotation,Time.deltaTime * 5f);
        if (_questScr != null)
        {
            float _speed = Vector3.Magnitude(rb.linearVelocity);
            if (_speed < _speedLastFrame)
            {
                float _speedDelta = _speedLastFrame - _speed;
                _speedDelta -= _paddingStrength;
                _speedDelta = _speedDelta / _paddingStrength;
                if (_speedDelta > 0.0f)
                {
                    _questScr.TakeDamage(_speedDelta);
                }
            }
            _speedLastFrame = _speed;
        }
    }

    public void Update()
    {
        if (_questManager._dangerLevel == 2 && (_questManager.hasQuestObject))
        {
            if (Vector3.Distance(transform.position, _drasil.transform.position) > 1800 && !bossAggression)
            {
                
                _questManager._dialoguer.CreateDialogue("Didn'tcha hear me?");
                _questManager._dialoguer.CreateDialogue("I told ya' to go to Drasil, and leave that package there.");
                _questManager._dialoguer.CreateDialogue("When yer done with that, I told ya to come back to me.");
                _questManager._dialoguer.CreateDialogue("And right now, I don't see ya doin' either of those.");
                bossAggression = true;
            }
            //SpaceshipControl.cs
            if (Vector3.Distance(transform.position, _drasil.transform.position) > 5000)
            {
                sceneMan.loadGoodCutscene();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            audioSource.PlayOneShot(hit);
        }
        //temp
        if (collision.gameObject.CompareTag("ShipDoor")) //Slamming into the door damages the package if you have one btw
        {
            sceneMan.loadShipScene();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Planet1"))
        {
            sceneMan.goToOrbitron();
        }
        else if (other.gameObject.CompareTag("Planet2"))
        {
            sceneMan.goToDrasil();
        }


    }
}
