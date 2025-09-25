using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControl : MonoBehaviour
{
    private Rigidbody rb;

    private PlayerInput playerInput;

    private Vector2 moveInput;
    private Vector2 mouseInput;
    private bool canInteract;

    private Interactable currentInteractable;

    private Interactable currentPickup;

    [SerializeField] float moveSpeed;
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;

    public Camera playerCamera;
    public bool isHolding;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer) && !isHolding)
            {
                currentInteractable.OnInteract();
            }
            else if (currentPickup != null)
            {
                currentPickup.OnInteract();
            }
        }
    }

    private void InteractionCheck()
    {
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject.layer == 6 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
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
    void Update()
    {
        rb.linearVelocity = transform.TransformDirection(new Vector3(moveInput.x * moveSpeed, rb.linearVelocity.y, moveInput.y * moveSpeed));
        InteractionCheck();
    }

    public void PickUpObject(Interactable obj)
    {
        currentPickup = obj;
    }

    public Interactable GetPickUp()
    {
        return currentPickup;
    }
}
