using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public GameObject _playerObject;

    void Start()
    {
        if (_playerObject == null)
        {
            _playerObject = GameObject.Find("Player");
        }
    }

    void Update()
    {
        transform.position = new Vector3(_playerObject.transform.position.x, transform.position.y, _playerObject.transform.position.z);
        transform.rotation = Quaternion.Euler(90, _playerObject.transform.rotation.eulerAngles.y, 0);
    }
}
