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


    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;

    public Camera playerCamera;
    public bool canJump;
    public bool isHolding;
    public GameObject questObjectPrefab;
    public GameObject? questObject;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;
        if (GameObject.Find("QuestManager").GetComponent<QuestManager>().hasQuestObject)
        {
                questObject = Instantiate(questObjectPrefab);
                questObject.GetComponent<PickUp>().OnInteract();
        }
        else
        {
            questObject = this.gameObject;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canJump)
            {
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                canJump = false;
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
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
        rb.linearVelocity = transform.TransformDirection(new Vector3(moveInput.x * moveSpeed, rb.linearVelocity.y, moveInput.y * moveSpeed));
        //Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        //rb.AddForce(move * moveSpeed * Time.deltaTime * 100, ForceMode.Force);
        InteractionCheck();
    }

    private void Update()
    {
        if (transform.position.y < -100)
        {
            transform.position = new Vector3(0, 10, 0);
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
}
