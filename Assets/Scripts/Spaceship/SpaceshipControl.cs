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
    private float _timeMoved;
    private float _timeRolled;




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
        _timeRolled += Time.deltaTime;
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        isBoosting = context.performed; 
    }
    // -------------------------------------------------

    private void FixedUpdate()
    {
        if (isBoosting && (moveInput.x != 0 || moveInput.y != 0) && _questManager._tutorialFlagsCompleted >= 6)
        {
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, cameraFOVChange, Time.deltaTime);
            rb.AddForce(rb.transform.forward * moveInput.y * boostMult, ForceMode.Impulse);
            // rb.AddForce(rb.transform.right * moveInput.x * boostMult, ForceMode.Impulse);
            timeLossMult = timeLossVal;
            if (_questManager._tutorialFlagsCompleted <= 6)
            {
                _questManager.FinishTutorialFlag();
            }
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
        if (_questManager._tutorialFlagsCompleted <= 4 && isMoving)
        {
            _timeMoved += Time.deltaTime;
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
        if (_questManager._tutorialFlagsCompleted >= 5)
        {
            rb.AddTorque(rb.transform.forward * speedRollMultAngle * rollInput, ForceMode.VelocityChange);
            if (rollInput != 0 && _questManager._tutorialFlagsCompleted <= 5)
            {
                _timeRolled += Time.deltaTime;
            }
        }
        
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
        if (_questManager._tutorialFlagsCompleted <= 4 && _timeMoved >= 4)
        {
            _questManager.FinishTutorialFlag();
        }
        else if (_questManager._tutorialFlagsCompleted <= 5 && _timeRolled >= 2)
        {
            _questManager.FinishTutorialFlag();
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
        float invalidPlanetPushbackValue = 100000f;
        if (other.gameObject.CompareTag("Planet1"))
        {
            if (_questManager._tutorialFlagsCompleted >= 7)
            {
                sceneMan.loadLastScene();
            }
            else
            {
                rb.AddExplosionForce(invalidPlanetPushbackValue, other.gameObject.transform.position, invalidPlanetPushbackValue); //I just wanted to fucking kill the player in real life if they try to go to a planet before they're supposed to but i didn't know how to do that so this was the next best alternative, anyway this should probably be fixed because just giving the player complete whiplash for going to a planet too quickly might actually be a bad idea i'm not sure don't quote me on this one.
            }
        }
        else if (other.gameObject.CompareTag("Planet2"))
        {
            if (_questManager._tutorialFlagsCompleted >= 7)
            {
                sceneMan.loadNextScene();
            }
            else
            {
                rb.AddExplosionForce(invalidPlanetPushbackValue, other.gameObject.transform.position, invalidPlanetPushbackValue);
            }
        }
    }
}
