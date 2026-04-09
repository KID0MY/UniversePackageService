using UnityEngine;

public class CutsceneMove : MonoBehaviour
{
    public float smoothTime = 15;
    public GameObject ship;
    public Vector3 velocity = new Vector3(0, 0, 2);
    Vector3 targetPos;
    float waitTime = 0;

    void Start()
    {

        transform.position = new Vector3(0, 0, -20);

        Vector3 targetPos = this.transform.position + new Vector3(0, 0, 50);
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.GetComponent<MeshRenderer>().enabled = false;
        sphere.transform.position = targetPos;

        targetPos = targetPos - new Vector3(0, 0, 2);
    }

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        waitTime += Time.deltaTime;

        if (waitTime > 4.5)
        {
            ship.SetActive(false);
        }

        
    }
}
