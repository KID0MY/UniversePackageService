using Unity.VisualScripting;
using UnityEngine;

public class PickUp : Interactable
{
    public GameObject player;
    public Transform holdPos;
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
        }
        else if (player.GetComponent<CharacterControl>().isHolding == true && player.GetComponent<CharacterControl>().GetPickUp() == this)
        {
            player.GetComponent<CharacterControl>().isHolding = false;
            this.gameObject.layer = 6;
            player.GetComponent<CharacterControl>().PickUpObject(null);
            this.transform.localScale = new Vector3(1f, 1f, 1f);
            this.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }

    public override void OnLoseFocus()
    {

    }

    private void Update()
    {
        if (holdPos != null && player.GetComponent<CharacterControl>().GetPickUp() == this)
        {
            this.transform.position = holdPos.position;
            this.transform.rotation = holdPos.rotation;
            this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
}
