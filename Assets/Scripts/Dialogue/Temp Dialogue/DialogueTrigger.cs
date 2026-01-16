using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
        public GameObject visualCue;
        
        public GameObject dialogueBox;

        public GameObject questObjectPrefab;

        public bool isQuestGiver;

        public bool isQuestReciever;

        private GameObject questObject;

        private bool playerInRange;

        private bool hasQuest;
   
        private void Awake()
        {
            playerInRange = false;
            visualCue.SetActive(false);
        }

        private void Update()
        {
        if (playerInRange)
        {
            if (isQuestGiver)
            {
                visualCue.SetActive(true);
            }
            else if (isQuestReciever)
            {
                visualCue.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueBox.SetActive(true);
                if (!hasQuest && isQuestGiver)
                {
                    hasQuest = true;
                    questObject = Instantiate(questObjectPrefab, this.transform.position + Vector3.right, Quaternion.identity);
                    questObject.GetComponent<PickUp>().OnInteract();
                }
                if (hasQuest && GameObject.Find("QuestManager").GetComponent<QuestManager>().hasQuestObject && GameObject.Find("Player").GetComponent<CharacterControl>().isHolding)
                {
                    Destroy(GameObject.Find("HoldPosition").GetComponentInChildren<GameObject>());
                }
            }
        }
        else
        {
            if (isQuestGiver)
            {
                visualCue.SetActive(false);
            }
            else if (isQuestReciever)
            {
                visualCue.SetActive(true);
            }
            dialogueBox.SetActive(false);
        }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                playerInRange = true;
            }
            if (isQuestReciever && other.gameObject.tag == "Package")
            {
                Destroy(other.gameObject);
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
