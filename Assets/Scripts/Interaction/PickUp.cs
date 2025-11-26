using Unity.VisualScripting;
using UnityEngine;

public class PickUp : Interactable
{
    public GameObject player;
    public Transform holdPos;
    public Transform baseSize;
    public Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }
    public override void OnFocus()
    {

    }

    public override void OnInteract()
    {
        if (player.GetComponent<CharacterControl>().isHolding == false)
        {
            player.GetComponent<CharacterControl>().isHolding = true;
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
            player.GetComponent<CharacterControl>().isHolding = false;
            this.gameObject.layer = 6;
            collider.enabled = true;
            player.GetComponent<CharacterControl>().PickUpObject(null);
            this.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 0));
            holdPos.DetachChildren();
            this.GetComponent<Rigidbody>().isKinematic = false;
            this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //transform.position = hit.point;
        }
    }

    public override void OnLoseFocus()
    {

    }

    private void Update()
    {

    }
}
