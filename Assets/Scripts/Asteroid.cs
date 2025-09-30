using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Timer timer;
    [SerializeField] private float timeLoss;
    [SerializeField] private float bounceStrength;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            timer.timeRemaining -= timeLoss;
            Vector3 colVeliocity = collision.gameObject.GetComponent<Rigidbody>().linearVelocity;
            Vector3 normal = collision.contacts[0].normal;
            Vector3 bounceDir = Vector3.Reflect(colVeliocity.normalized, normal);

            collision.gameObject.GetComponent<Rigidbody>().linearVelocity = bounceDir * bounceStrength;
        }
    }
}
