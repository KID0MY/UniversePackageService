using UnityEngine;

public class Asteroid : MonoBehaviour
{
    MeshCollider _collider;
    Rigidbody _body;
    public SpaceshipControl _player;
    int _distanceForCollision = 1000;
    Vector3 _driftDir;

    private void Start()
    {
        _collider = GetComponent<MeshCollider>();
        _player = FindAnyObjectByType<SpaceshipControl>();
        _body = GetComponent<Rigidbody>();
        _driftDir = new Vector3(Random.Range(-10000, 10000), Random.Range(-10000, 10000), Random.Range(-10000, 10000));
        _body.AddForce(_driftDir);
    }

    public void CheckCollisionEnable()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) < _distanceForCollision)
        {
            _collider.enabled = true;
        }
        else
        {
            _collider.enabled = false;
        }
    }

    public void InvertDirection()
    {
        _driftDir = _driftDir * -2;
        _body.AddForce(_driftDir);
    }

    public void TargetPlayer()
    {
        _driftDir = ((_player.transform.position - transform.position).normalized * 2000) + new Vector3(Random.Range(-10000, 10000), Random.Range(-10000, 10000), Random.Range(-10000, 10000));
        _body.AddForce(_driftDir);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 colVeliocity = collision.gameObject.GetComponent<Rigidbody>().linearVelocity;
            Vector3 normal = collision.contacts[0].normal;
            Vector3 bounceDir = Vector3.Reflect(colVeliocity.normalized, normal);
        }
    }
}
