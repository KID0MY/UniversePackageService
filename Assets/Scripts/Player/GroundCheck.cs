using System.Runtime.CompilerServices;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float groundToleranceDist;

    private CapsuleCollider capsule;

    public float? distanceToGround {  get; private set; }

    public CharacterControl player;

    private void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isGroundBelow = Physics.CapsuleCast(capsule.bounds.center, capsule.bounds.center, capsule.radius, Vector3.down, out RaycastHit hit, groundToleranceDist);
        print("Ground check: " + isGroundBelow);
        print("Distance to ground: " + distanceToGround);
        print("Ground check hit point: " + hit.point);
        if (isGroundBelow)
        {
            distanceToGround = transform.position.y - hit.point.y;
        }
        else
        {
            distanceToGround = null;
        }
        player.canJump = isGroundBelow && distanceToGround <= groundToleranceDist;
    }
}
