using UnityEngine;

public class ButtonPrompts : MonoBehaviour
{
    public CharacterControl _player;
    public sceneManager_ _sceneMan;
    public QuestManager _questMan;
    public int _currentScene;
    public TMPro.TextMeshProUGUI _text;

    void Start()
    {
        _questMan = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        if (_text == null)
        {
            _text = GetComponent<TMPro.TextMeshProUGUI>();
        }
        if (_sceneMan == null)
        {
            _sceneMan = GameObject.Find("SceneManager").GetComponent<sceneManager_>();
        }
        _currentScene = _sceneMan.GetCurrentSceneIndex();
        if (_currentScene != 2)
        {
            _player = GameObject.Find("Player").GetComponent<CharacterControl>();
        }
        if (_player.isHolding && _currentScene == 3)
        {
            _text.text = "Press E - Drop\nHold E - Throw";
        }
        else if (_currentScene == 2)
        {

        }
        else if (_currentScene == 4)
        {
            if (_questMan._tutorialFlagsCompleted >= 6)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        
    }
}
