using UnityEngine;

public class Quest : MonoBehaviour
{
    public string _questName;
    public string _destination;
    public string _description;

    void Start()
    {
        
    }

    public void GenerateRandomQuest()
    {
        GenerateDeliveryType();
        this.name = _questName;
    }

    public void GenerateDeliveryType()
    {
        GenerateDeliveryDestination();
        _description = "Yup, this sure is a package.";
        int _questIndex = Random.Range(0, 4);
        switch (_questIndex)
        {
            case 0:
                _questName = "Basic Delivery";
                _description = "Bring this package to " + _destination + ".";
                break;
            case 1:
                _questName = "Speedy Delivery";
                _description = "Bring this package to " + _destination + ", make it speedy, those contents aren't gonna last long.";
                break;
            case 2:
                _questName = "Fragile Delivery";
                _description = "Bring this package to " + _destination + " carefully. That package is fragile, and I'm not paying you if it breaks.";
                break;
            case 3:
                _questName = "Obscure Delivery";
                _description = "Bring this package to " + _destination + ", specifically to the core of the planet, cause that's where the recipient lives.";
                break;
        }
    }

    public void GenerateDeliveryDestination()
    {
        int _planetIndex = Random.Range(0, 2);
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
}
