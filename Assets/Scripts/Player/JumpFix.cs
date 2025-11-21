using UnityEngine;
using UnityEngine.InputSystem;

public class JumpFix : MonoBehaviour
{
    public float fallMult;
    public float lowJumpMult;

    Rigidbody rb;

    PlayerInput player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocity.y < 0 && rb.position.y > 1)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMult - 1) * Time.deltaTime;
        }
        //else if (rb.linearVelocity.y > 0)
        //{
        //    rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMult - 1) * Time.deltaTime;
        //}
    }
}
