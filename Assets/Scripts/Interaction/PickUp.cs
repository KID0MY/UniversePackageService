using Unity.VisualScripting;
using UnityEngine;

public class PickUp : Interactable
{
    public GameObject player;
    public Transform holdPos;
    public Transform baseSize;
    public Collider collider;
    public Rigidbody _body;
    public QuestManager _questManager;
    public float _speed;
    public float _speedLastFrame;

    public override void Awake()
    {
        collider = GetComponent<Collider>();
        _body = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
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
            this.gameObject.layer = 7;
            collider.enabled = false;
            player.GetComponent<CharacterControl>().PickUpObject(this);
            this.transform.SetParent(holdPos);
            this.transform.localPosition = Vector3.zero;
            this.transform.localScale = holdPos.localScale;
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.transform.localRotation = holdPos.localRotation;
        }
        else if (player.GetComponent<CharacterControl>().isHolding == true && player.GetComponent<CharacterControl>().GetPickUp() == this)
        {
            this.gameObject.layer = 6;
            collider.enabled = true;
            player.GetComponent<CharacterControl>().PickUpObject(null);
            holdPos.DetachChildren();
            this.GetComponent<Rigidbody>().isKinematic = false;
            this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //transform.position = hit.point;
        }
        if (_questManager._tutorialFlagsCompleted < 4)
        {
            _questManager.FinishTutorialFlag();
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

    private void FixedUpdate()
    {
        _speed = Vector3.Magnitude(_body.linearVelocity);
        if (_speed < _speedLastFrame && !player.GetComponent<CharacterControl>().isHolding)
        {
            float _speedDelta = _speedLastFrame - _speed;
            if (_questManager._questList.Count > 0)
            {
                _questManager._questList[0].GetComponent<Quest>().TakeDamage(_speedDelta);
            }
        }
        _speedLastFrame = _speed;
        if (transform.position.y < -100)
        {
            transform.position = new Vector3(0, 10, 0);
            _body.linearVelocity = new Vector3(0, 0, 0);
            _speedLastFrame = 0f;
        }
    }
}
