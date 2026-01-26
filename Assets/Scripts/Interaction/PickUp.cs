using Unity.VisualScripting;
using UnityEngine;

public class PickUp : Interactable
{
    public GameObject player;
    public Transform holdPos;
    public Transform baseSize;
    public Collider collider;

    public override void Awake()
    {
        collider = GetComponent<Collider>();
        player = GameObject.Find("Player");
        holdPos = GameObject.Find("HoldPosition").transform;
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
    }

    public void KILLYOURSELF()
    {
        print("sure man");
        Destroy(this.gameObject);
    }

    public override void OnLoseFocus()
    {

    }

    private void Update()
    {

    }
}
