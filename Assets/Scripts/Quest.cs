using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public int _dangerLevel;
    public float _health;
    public string _questName;
    public string _destination;
    public string _description;
    public string _recipient;
    public int _payAmount; //Everything that sets this is a placeholder value
    public int _latenessLeeway;
    public float _startTime;

    void Start()
    {
        
    }

    public void GenerateRandomQuest(int planet_exclusion)
    {
        _payAmount = 0;
        GenerateDeliveryDestination(planet_exclusion);
        GenerateRecipient();
        GenerateDeliveryType();
        this.name = _questName;
        print(transform.parent.GetComponent<QuestManager>().IsMatchingRecipient("GAY FURRY PORN"));
    }

    public void GenerateDeliveryType()
    {
        _description = "Yup, this sure is a package.";
        int _questIndex = 0;
        _health = 1; //Default value of health
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
                _latenessLeeway = 30;
                break;
            case 1:
                _questName = "Speedy Delivery";
                _description = "Bring this package to " + _recipient + " on " + _destination + ", make it quick, those contents aren't gonna last long.";
                _payAmount = 30;
                _latenessLeeway = 10;
                break;
            case 2:
                _questName = "Fragile Delivery";
                _description = "Bring this package to " + _recipient + " on " + _destination + " carefully. That package is fragile, and I'm not paying you if it breaks.";
                _payAmount = 30;
                _latenessLeeway = 35;
                break;
            case 3:
                _questName = "Obscure Delivery";
                _description = "Bring this package to " + _recipient + " on " + _destination + ", specifically to the core of the planet, cause that's where the recipient lives.";
                _payAmount = 40;
                _latenessLeeway = 40;
                break;
            case 4:
                _questName = "Final Delivery";
                _description = "Bring this package to " + _recipient + " on " + _destination + ". This package is vital, get it delivered flawlessly.";
                _payAmount = 40;
                _latenessLeeway = 40;
                break;
        }
    }

    public void GenerateDeliveryDestination(int planet_exclusion)
    {
        int _planetIndex = Random.Range(0, 2);
        if (planet_exclusion == _planetIndex)
        {
            _planetIndex++;
        }
        switch (_planetIndex)
        {
            case 0:
                _destination = "Planet 1";
                break;
            case 1:
                _destination = "Planet 2";
                break;
            case 2:
                _destination = "Planet 1";
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

    public void TakeDamage(float amount)
    {
        float _damageMultiplier = 1f;
        if (this.name == "Fragile Delivery")
        {
            _damageMultiplier = 3f;
        }
        _health -= ((amount*_damageMultiplier)/100);
    }
}
