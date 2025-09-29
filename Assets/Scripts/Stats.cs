using UnityEngine;

public class Stats : MonoBehaviour
{
    public int collectibles = 0;

    public float speed;

    [SerializeField] AudioClip grabClip;

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed = Mathf.Floor(this.GetComponent<Rigidbody>().linearVelocity.magnitude * 3.6f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Package"))
        {
            Destroy(other.gameObject);
            collectibles++;
            audioSource.PlayOneShot(grabClip);
        }
    }

    public void Reset()
    {
        collectibles = 0;
    }
}
