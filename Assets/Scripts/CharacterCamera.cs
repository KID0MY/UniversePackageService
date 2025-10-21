using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterCamera : MonoBehaviour
{
    [SerializeField] float minViewDistance = 25f;
    [SerializeField] Transform playerOrientation;

    private PlayerInput playerInput;

    public float mouseSensitivity = 100f;

    float xRotation = 0f;

    public Vector2 mouseInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse = mouseInput * Time.deltaTime * mouseSensitivity;

        this.transform.Rotate(Vector3.up * mouse.x);

        this.xRotation -= mouse.y;
        this.xRotation = Mathf.Clamp(this.xRotation, -90f, minViewDistance);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerOrientation.Rotate(Vector3.up * mouse.x);
    }
}
