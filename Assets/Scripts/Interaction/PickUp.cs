using Unity.VisualScripting;
using UnityEngine;

public class PickUp : Interactable
{
    public GameObject player;
    public GameObject playerCam;
    public Transform holdPos;
    public Transform baseSize;
    public Collider collider;
    public Rigidbody _body;
    public QuestManager _questManager;
    public float _speed;
    public float _speedLastFrame;
    public bool _canBeThrown = false;
    public bool _shouldBeThrown = true;
    public float _throwTimer = 0.0f;
    public Vector3 _throwForce;

    public override void Awake()
    {
        collider = GetComponent<Collider>();
        _body = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerCam = GameObject.Find("Camera");
        holdPos = GameObject.Find("HoldPosition").transform;
        _questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }

    public override void OnFocus()
    {

    }

    public override void OnInteract()
    {
        if (player.GetComponent<CharacterControl>().isHolding == false)
        {
            _throwTimer = 0f;
            this.gameObject.layer = 7;
            collider.enabled = false;
            player.GetComponent<CharacterControl>().PickUpObject(this);
            this.transform.SetParent(holdPos);
            this.transform.localPosition = Vector3.zero;
            this.transform.localScale = holdPos.localScale;
            _body.isKinematic = true;
            this.transform.localRotation = holdPos.localRotation;
            audioManager.Instance.PlaySFX("Grab");
        }
        else if (player.GetComponent<CharacterControl>().isHolding == true && player.GetComponent<CharacterControl>().GetPickUp() == this)
        {
            this.gameObject.layer = 6;
            collider.enabled = true;
            player.GetComponent<CharacterControl>().PickUpObject(null);
            holdPos.DetachChildren();
            _body.isKinematic = false;
            this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            if (_canBeThrown)
            {
                transform.rotation = new Quaternion(playerCam.transform.rotation.x, player.transform.rotation.y, 0, player.transform.rotation.w);
                _throwForce = transform.forward * 1000 * _throwTimer;
                _throwTimer = 0f;
                _canBeThrown = false;
                _body.AddForce(_throwForce);
            }
            //transform.position = hit.point;
        }
        if (_questManager._tutorialFlagsCompleted < 3)
        {
            _questManager.FinishTutorialFlag();
        }
    }

    public override void CheckThrow()
    {
        if (player.GetComponent<CharacterControl>().isHolding == true && player.GetComponent<CharacterControl>().GetPickUp() == this)
        {
            _throwTimer = 1.0f;
        }
    }

    public void KILLYOURSELF()
    {
        player.GetComponent<CharacterControl>().PickUpObject(null);
        Destroy(this.gameObject);
    }

    public override void OnLoseFocus()
    {

    }
    private void Update()
    {
        if (_speed < _speedLastFrame && !player.GetComponent<CharacterControl>().isHolding)
        {
            float _speedDelta = _speedLastFrame - _speed;
            if (_questManager._questList.Count > 0)
            {
                _questManager._questList[0].GetComponent<Quest>().TakeDamage(_speedDelta);
            }
        }
    }
    private void FixedUpdate()
    {
        _speed = Vector3.Magnitude(_body.linearVelocity);
        _speedLastFrame = _speed;
        if (transform.position.y < -100)
        {
            transform.position = new Vector3(0, 10, 0);
            _body.linearVelocity = new Vector3(0, 0, 0);
            _speedLastFrame = 0f;
        }
        if (_throwTimer >= 1.0f && _throwTimer < 3.0f)
        {
            _throwTimer += Time.deltaTime;
        }
        if (_throwTimer >= 2.0f)
        {
            _canBeThrown = true;
        }
        if (_throwTimer > 3.0f)
        {
            _throwTimer = 3.0f;
        }
    }
}
