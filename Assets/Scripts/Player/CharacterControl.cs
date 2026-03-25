using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterControl : MonoBehaviour
{
    private Rigidbody rb;

    private PlayerInput playerInput;

    private Vector2 moveInput;

    private Interactable currentInteractable;

    public Interactable currentPickup;

    public bool _isDropDisabled = false;
    //private float _timeMoved = 0f; Originally used to track tutorial stuff but due to altering how the tutorial works this doesnt need to exist
    private float punishment = 0f;


    public float moveSpeed;
    public float gravVal = -9.2f;
    [SerializeField] float jumpSpeed;
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;

    public Camera playerCamera;
    public bool canJump;
    public bool isHolding;
    public bool _cutsceneMovementLock;
    public bool isExploding = false;
    public GameObject questObjectPrefab;
    public GameObject? questObject;
    public QuestManager _questManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;
        _questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        if (_questManager.hasQuestObject)
        {
            questObject = Instantiate(questObjectPrefab);
            questObject.GetComponent<PickUp>().OnInteract();
        }
        else
        {
            questObject = this.gameObject;
        }
    }

    private void Start()
    {
        if (_questManager._tutorialFlagsCompleted <= 0)
        {
            _questManager.FinishTutorialFlag();
        }
        else if (_questManager._tutorialFlagsCompleted == 4)
        {
            _questManager.FinishTutorialFlag();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!_cutsceneMovementLock)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && !_cutsceneMovementLock && canJump)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            canJump = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && !_cutsceneMovementLock)
        {
            if (currentPickup != null && !_isDropDisabled)
            {
                currentPickup.CheckThrow();
            }
        }
        if (context.canceled && !_cutsceneMovementLock)
        {
            if (currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer) && (currentInteractable.GetComponent<PickUp>() == null || currentPickup == null))
            {
                currentInteractable.OnInteract();
            }
            else if (currentPickup != null && !_isDropDisabled)
            {
                currentPickup.OnInteract();
            }
        }
    }

    private void InteractionCheck()
    {
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable") && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);

                if (currentInteractable)
                {
                    currentInteractable.OnFocus();
                }
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isExploding)
        {
            rb.linearVelocity = transform.TransformDirection(new Vector3(moveInput.x * moveSpeed, rb.linearVelocity.y, moveInput.y * moveSpeed));
        }
        //if (_timeMoved < 2f && (moveInput != Vector2.zero))
        //{
        //    _timeMoved += Time.deltaTime;
        //    if (_timeMoved >= 2f && _questManager._tutorialFlagsCompleted <= 0)
        //    {
        //        _questManager.FinishTutorialFlag();
        //    }
        //}
        if (!canJump)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y + (gravVal * Time.deltaTime), rb.linearVelocity.z);
        }
        //Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        //rb.AddForce(move * moveSpeed * Time.deltaTime * 100, ForceMode.Force);
        InteractionCheck();
    }

    private void Update()
    {
        if (transform.position.y < -100)
        {
            transform.position = new Vector3(0, 10, 0);
            transform.rotation = Quaternion.identity;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            isExploding = false;
        }
        if (isExploding == true)
        {
            
            if (punishment > 10f)
            {
                transform.position = new Vector3(0, 10, 0);
                transform.rotation = Quaternion.identity;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                rb.linearVelocity = Vector3.zero;
                isExploding = false;
                punishment = 0f;
            }
            else
            {
                punishment += Time.deltaTime;
            }
        }
    }

    public RaycastHit CanDropObject() //Not used lmao
    {
        Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance + 1);
        return hit;
    }

    public void PickUpObject(Interactable obj)
    {
        currentPickup = obj;
        if (obj == null)
        {
            isHolding = false;
        }
        else
        {
            isHolding = true;
        }
    }

    public Interactable GetPickUp()
    {
        return currentPickup;
    }

    //temp
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ExitDoor"))
        {
            GameObject.Find("SceneManager").GetComponent<sceneManager_>().loadSpaceScene();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Office_Trigger")
        {
            if (_questManager._tutorialFlagsCompleted <= 1)
            {
                _questManager.FinishTutorialFlag();
            }
        }
    }
    public void BlowUpPlayer(GameObject other)
    {
        isExploding = true;
        rb.AddExplosionForce(100000, other.transform.position, 100000);
        rb.constraints = RigidbodyConstraints.None;
    }
}
