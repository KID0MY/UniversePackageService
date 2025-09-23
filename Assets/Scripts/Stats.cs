using UnityEngine;

public class Stats : MonoBehaviour
{
    public int collectibles = 0;

    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed = Mathf.Floor(this.GetComponent<Rigidbody>().linearVelocity.magnitude * 3.6f);
        Debug.Log(speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Package"))
        {
            Destroy(other.gameObject);
            collectibles++;
            Debug.Log(collectibles);
        }
    }

    public void Reset()
    {
        collectibles = 0;
    }
}
