using System.Runtime.CompilerServices;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float groundToleranceDist;

    private BoxCollider boxCollider;

    public float? distanceToGround {  get; private set; }

    public CharacterControl player;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isGroundBelow = Physics.BoxCast(transform.position, (transform.localScale / 2), Vector3.down, out RaycastHit hit, Quaternion.identity, 1000);
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
