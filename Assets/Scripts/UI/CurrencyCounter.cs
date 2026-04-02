using UnityEngine;

public class CurrencyCounter : MonoBehaviour
{
    public QuestManager _questManager;
    int _money;
    int _toBeGained;
    float alphaLevel;
    Color _gainTextColour;
    float _vanishTimer = 0;
    public TMPro.TextMeshProUGUI _totalText;
    public TMPro.TextMeshProUGUI _gainText;

    void Start()
    {
        _questManager = FindFirstObjectByType<QuestManager>();
        _questManager._currencyCounter = this;
        _gainTextColour = _gainText.color;
        this.gameObject.SetActive(false);
    }

    public void ShowGainedMoney(int _gainedMoney, int _totalMoney)
    {
        _money = _totalMoney;
        alphaLevel = 0f;
        gameObject.SetActive(true);
        _totalText.text = "$" + (_money - _gainedMoney).ToString();
        _gainText.text = "+$" + _gainedMoney.ToString();
        _toBeGained = _gainedMoney;
        _vanishTimer = 6;
        if (_questManager._dangerLevel == 2)
        {
            _vanishTimer = 25;
        }
    }

    void FixedUpdate()
    {
        if (_vanishTimer > 0)
        {
            if (alphaLevel < 1f && _vanishTimer > 3)
            {
                alphaLevel += Time.deltaTime * 2;
            }
            if ((_vanishTimer < 5 && _toBeGained > 0) || (_vanishTimer < 24 && _questManager._dangerLevel == 2))
            {
                _toBeGained--;
                _gainText.text = "+$" + _toBeGained.ToString();
                _totalText.text = "$" + (_money - _toBeGained).ToString();
            }
            _totalText.color = new Color(1, 1, 1, alphaLevel);
            _gainText.color = new Color(_gainTextColour.r, _gainTextColour.g, _gainTextColour.b, alphaLevel);
            _vanishTimer -= Time.deltaTime;
            if (_vanishTimer < 1)
            {
                alphaLevel -= Time.deltaTime * 2;
            }
            if (_vanishTimer < 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
