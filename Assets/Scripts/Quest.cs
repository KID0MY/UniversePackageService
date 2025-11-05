using UnityEngine;

public class Quest : MonoBehaviour
{
    public string _questName;
    public string _destination;

    void Start()
    {
        
    }

    public void GenerateRandomQuest()
    {
        GenerateDeliveryType();
        name = _questName;
    }

    public void GenerateDeliveryType()
    {
        int _questIndex = Random.Range(0, 4);
        switch (_questIndex)
        {
            case 0:
                _questName = "Basic Delivery";
                break;
        }
    }
}
