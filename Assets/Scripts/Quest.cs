using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public int _dangerLevel;
    public string _questName;
    public string _destination;
    public string _description;
    public string _recipient;
    public int _payAmount; //Everything that sets this is a placeholder value
    public float _startTime;

    void Start()
    {
        
    }

    public void GenerateRandomQuest()
    {
        _payAmount = 0;
        GenerateDeliveryDestination();
        GenerateRecipient();
        GenerateDeliveryType();
        this.name = _questName;
        print(transform.parent.GetComponent<QuestManager>().IsMatchingRecipient("GAY FURRY PORN"));
    }

    public void GenerateDeliveryType()
    {
        _description = "Yup, this sure is a package.";
        int _questIndex = 0;
        if (_dangerLevel == 0)
        {
            _questIndex = Random.Range(0, 4);
        }
        switch (_questIndex)
        {
            case 0:
                _questName = "Basic Delivery";
                _description = "Bring this package to " + _recipient + " on " + _destination + ".";
                _payAmount = 20;
                break;
            case 1:
                _questName = "Speedy Delivery";
                _description = "Bring this package to " + _recipient + " on " + _destination + ", make it quick, those contents aren't gonna last long.";
                _payAmount = 30;
                break;
            case 2:
                _questName = "Fragile Delivery";
                _description = "Bring this package to " + _recipient + " on " + _destination + " carefully. That package is fragile, and I'm not paying you if it breaks.";
                _payAmount = 30;
                break;
            case 3:
                _questName = "Obscure Delivery";
                _description = "Bring this package to " + _recipient + " on " + _destination + ", specifically to the core of the planet, cause that's where the recipient lives.";
                _payAmount = 40;
                break;
            case 4: //Unused
                _questName = "Courier's Rasher";
                _description = "Bring this over to fucking Choral Chambers in 5 minutes and don't get hit or so help me god I will actually kill you.";
                _payAmount = 0;
                break;
        }
    }

    public void GenerateDeliveryDestination()
    {
        int _planetIndex = Random.Range(1, 2); //Set first number to 0 to include planet 1
        switch (_planetIndex)
        {
            case 0:
                _destination = "Planet 1";
                break;
            case 1:
                _destination = "Planet 2";
                break;
        }
    }

    public void GenerateRecipient()
    {
        List<string> names = new List<string>();
        if (_destination == "Planet 1")
        {
            names = transform.parent.GetComponent<QuestManager>()._planetOneRecipientNames;
        }
        else if (_destination == "Planet 2")
        {
            names = transform.parent.GetComponent<QuestManager>()._planetTwoRecipientNames;
        }
        _recipient = names[Random.Range(0, names.Count)];
    }
}
