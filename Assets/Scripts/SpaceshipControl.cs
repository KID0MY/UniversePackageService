using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipControl : MonoBehaviour
{
    private Rigidbody rb;

    //Inputs
    private Vector2 moveInput;      // Horizontal (x) + Vertical (y)
    private Vector2 mouseInput;     // Mouse X + Mouse Y
    private float rollInput;
    private PlayerInput playerInput;
    private bool isMoving;
    public bool isBoosting;

    private AudioSource audioSource;

    [SerializeField] private AudioClip thrusterClip;
    [SerializeField] private AudioClip thrusterEndClip;
    [SerializeField] private float speedMult = 1;
    [SerializeField] private float boostMult = 2;
    [SerializeField] private float speedMultAngle = 0.5f;
    [SerializeField] private float speedRollMultAngle = 0.05f;
    [SerializeField] private float timeLoss;

    public Timer time;
    //[SerializeField] private float timeLossVal = 2;
    //public float timeLossMult = 1;




    private void Awake()
    {
        
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;
        rb.angularVelocity = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
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
        if (context.performed)
        {
            rb.AddForce(rb.transform.forward * moveInput.y * boostMult, ForceMode.Impulse);
            rb.AddForce(rb.transform.right * moveInput.x * boostMult, ForceMode.Impulse);
            time.timeRemaining -= timeLoss;
            isBoosting = !isBoosting;
        }
        //if (context.ReadValue<float>() == 1)
        //{
        //    speedMult = boostMult;
        //    timeLossMult = timeLossVal;

        //}
        //else
        //{
        //    speedMult = 1;
        //    timeLossMult = 1;
        //}
    }
    // -------------------------------------------------

    private void FixedUpdate()
    {
        if ((moveInput.y < 0 || moveInput.y > 0 || moveInput.x < 0 || moveInput.x > 0) && !audioSource.isPlaying)
        {
            audioSource.clip = thrusterClip;
            audioSource.Play();
            isMoving = true;
        }
        if (moveInput.y == 0 && moveInput.x ==0 && audioSource.clip != thrusterEndClip)
        {
            audioSource.clip = thrusterEndClip; 
            audioSource.Play();
            isMoving = false;
        }
        // Translation
        rb.AddForce(rb.transform.forward * moveInput.y * speedMult, ForceMode.VelocityChange);
        rb.AddForce(rb.transform.right * moveInput.x * speedMult, ForceMode.VelocityChange);

        // Rotation from mouse
        // might break later
        if (Time.timeSinceLevelLoad > 1)
        {
            rb.AddTorque(rb.transform.right * speedMultAngle * mouseInput.y * -1, ForceMode.VelocityChange);
            rb.AddTorque(rb.transform.up * speedMultAngle * mouseInput.x, ForceMode.VelocityChange);
        }


        // Roll
        rb.AddTorque(rb.transform.forward * speedRollMultAngle * rollInput, ForceMode.VelocityChange);

        
    }
}
