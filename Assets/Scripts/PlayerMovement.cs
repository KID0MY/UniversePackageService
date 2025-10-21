using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Character movement Values")]
    public float moveSpeed = 10f;
    public float gravity = -9.81f;

    // Components
    private CharacterController controller; 
    private PlayerInput playerInput;
    public sceneManager_ sceneManager;
    
    private bool isInShipCollisionZone;
    private bool isInteracting;
    // Movement
    private Vector2 moveInput;
    private Vector3 velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Input System callback
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnInteract()
    {
        if (isInShipCollisionZone)
        {
            print("GAY PORN"); // 👈 This will happen when the button is pressed while inside the zone
            sceneManager.loadNextScene();
        }
        // You don't need to read the value into isPressing anymore if you use 'context.performed'
    }

    private void Update()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("shipCollision"))
        {
            isInShipCollisionZone = true; // Player has entered the zone
            print("Ship collision");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("shipCollision"))
        {
            isInShipCollisionZone = false; // Player has left the zone
            print("Ship collision out");
        }
    }
}
