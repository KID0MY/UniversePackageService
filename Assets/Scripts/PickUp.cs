using Unity.VisualScripting;
using UnityEngine;

public class PickUp : Interactable
{
    public GameObject player;
    public Transform holdPos;
    public Transform baseSize;

    private void Start()
    {

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
            Debug.Log("pickedUp");
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
            player.GetComponent<CharacterControl>().PickUpObject(null);
            this.transform.localScale = new Vector3(1f, 1f, 1f);
            this.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            holdPos.DetachChildren();
            this.GetComponent<Rigidbody>().isKinematic = false;
            this.transform.localScale = Vector3.one;
        }
    }

    public override void OnLoseFocus()
    {

    }

    private void Update()
    {

    }
}
