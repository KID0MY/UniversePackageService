using UnityEngine;

public class ParticleGenerator : MonoBehaviour
{
    [SerializeField] SpaceshipControl ship;
    [SerializeField] float xOffset;
    bool enabled = false;

    private void Awake()
    {
        
    }

    private void Update()
    {
        transform.rotation = ship.transform.rotation;
    }
}
