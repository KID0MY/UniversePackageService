using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public CharacterControl _player;
    private TMPro.TextMeshProUGUI _dialogueText;

    private string _currentText;
    private string _targetText;
    private List<string> _textQueue = new List<string>();
    private bool _isRendering;
    private bool _canProgress;
    [SerializeField] private float _timeBetweenLetters; //In milliseconds

    private KeyCode _textProgressKey1 = KeyCode.Space;
    private KeyCode _textProgressKey2 = KeyCode.E;
    private KeyCode _textSkipKey1 = KeyCode.Space;
    private KeyCode _textSkipKey2 = KeyCode.E;

    void Awake()
    {
        if (_dialogueText == null)
        {
            _dialogueText = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        }
        GameObject.Find("QuestManager").GetComponent<QuestManager>()._dialoguer = this;
        this.gameObject.SetActive(false);
        _isRendering = false;
    }

    void Update()
    {
        _dialogueText.text = _currentText;
        if ((Input.GetKeyDown(_textProgressKey1) || Input.GetKeyDown(_textProgressKey2)) && _canProgress)
        {

            audioManager.Instance.PlaySFX("UI-Select/ContinueDialogue");
            if (_textQueue.Count == 0)
            {
                if (_player != null)
                {
                    _player._cutsceneMovementLock = false;
                }
                _currentText = "";
                _targetText = "";
                this.gameObject.SetActive(false);
                _isRendering = false;
            }
            else
            {
                _currentText = "";
                _targetText = _textQueue[0];
                _textQueue.RemoveAt(0);
                StartCoroutine(PrintText());
            }
        }
        else if ((Input.GetKeyDown(_textSkipKey1) || Input.GetKeyDown(_textSkipKey2)) && !_currentText.Equals(_targetText) && _currentText.Length > 1)
        {
            _currentText = _targetText;
        }
    }

    public void CreateDialogue(string dialogue)
    {
        _textQueue.Add(dialogue);
        if (!_isRendering)
        {
            if (_player != null)
            {
                _player._cutsceneMovementLock = true;
            }
            _currentText = "";
            _targetText = _textQueue[0];
            _textQueue.RemoveAt(0);
            this.gameObject.SetActive(true);
            _isRendering = true;
            StartCoroutine(PrintText());
        }
    }

    IEnumerator PrintText()
    {
        _canProgress = false;
        int _nextLetter = 0;
        while (!_currentText.Equals(_targetText)) {
            _currentText = _currentText + _targetText.Substring(_nextLetter, 1);
            yield return new WaitForSeconds(_timeBetweenLetters / 1000);
            _nextLetter++;
        }
        _canProgress = true;
    }
}
