using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    Transform target;

    float timeStamp;
    float maxTimePerFrame;
    public float targetFrameRate = 60.0f;

    private Vector3 dirToTarget;
    private Vector3 forward;
    private Vector3 normalToTarget;
    private Vector3 cross;

    private Rigidbody rb;

    private void Awake()
    {
        target = GameObject.Find("Spawner").GetComponent<Transform>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        forward = transform.forward;
        dirToTarget = target.transform.position - transform.position;
        normalToTarget = Vector3.Cross(dirToTarget, forward);
        rb = GetComponent<Rigidbody>();
        StartCoroutine(orbit());
    }
    
    IEnumerator orbit()
    {
        timeStamp = Time.realtimeSinceStartup;
        while (true) 
	{
            dirToTarget = target.transform.position - transform.position;
            cross = Vector3.Cross(dirToTarget, normalToTarget);
            rb.AddForce(cross.normalized * 50);
            //transform.LookAt(dirToTarget.normalized);
            //rb.AddForce(dirToTarget.normalized * 50);
            if (Time.realtimeSinceStartup > timeStamp + maxTimePerFrame)
            {
                yield return null; // wait for next frame of gameplay
                timeStamp = Time.realtimeSinceStartup;
            }
        }
    }
}
