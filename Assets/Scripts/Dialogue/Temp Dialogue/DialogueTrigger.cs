using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
        public GameObject visualCue;
        
        public GameObject dialogueBox;

        private bool playerInRange;

   
        private void Awake()
        {
            playerInRange = false;
            visualCue.SetActive(false);
        }

        private void Update()
        {
        if (playerInRange)
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueBox.SetActive(true);
            }
        }
        else
        {
            visualCue.SetActive(false);
            dialogueBox.SetActive(false);
        }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                playerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                playerInRange = false;
            }
        }
}
