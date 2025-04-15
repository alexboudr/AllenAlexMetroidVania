using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Object = UnityEngine.Object;

//This is code from here! : https://www.youtube.com/watch?v=UR_Rh0c4gbY&ab_channel=ChristinaCreatesGames
public class PrintByCharacter : MonoBehaviour
{
    private TMP_Text _textBox;

    // Basic Typewriter Functionality
    private int _currentVisibleCharacterIndex;
    private Coroutine _typewriterCoroutine;
    //private bool _readyForNewText = true;

    private WaitForSeconds _simpleDelay;
    private WaitForSeconds _interpunctuationDelay;

    [Header("Typewriter Settings")]
    [SerializeField] private float charactersPerSecond = 20;
    [SerializeField] private float interpunctuationDelay = 0.5f;

    [Header("Reference to TMP Text")]
    public TMP_Text textBox;
    private AudioSource audioSource; //audio source...
    public AudioClip type;

    private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();

        _simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
        _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);

        
        
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void SetText()
    {
        if (_typewriterCoroutine != null)
        {
            StopCoroutine(Typewriter());
        }

        _textBox.text = textBox.text; ;
        _textBox.maxVisibleCharacters = 0;
        _currentVisibleCharacterIndex = 0;

        _typewriterCoroutine = StartCoroutine(Typewriter());
    }

    private IEnumerator Typewriter()
    {
        _textBox.ForceMeshUpdate();
        TMP_TextInfo textInfo = _textBox.textInfo;
        int count = 0;
        while (_currentVisibleCharacterIndex < textInfo.characterCount)
        {
            count++;
            if (count % 2 == 0)
            {
                audioSource.PlayOneShot(type, 0.5F);
            }
            
            var lastCharacterIndex = textInfo.characterCount - 1;

            //Event funcitonality
            //if (_currentVisibleCharacterIndex >= lastCharacterIndex)
            //{
            //    _textBox.maxVisibleCharacters++;
            //    yield return _textboxFullEventDelay;
            //    CompleteTextRevealed?.Invoke();
            //    _readyForNewText = true;
            //    yield break;
            //}

            if (_currentVisibleCharacterIndex > lastCharacterIndex)
            {
                Debug.Log("yeah fuck this");
                yield break; // Stop the coroutine safely
            }

            char character = textInfo.characterInfo[_currentVisibleCharacterIndex].character;

            _textBox.maxVisibleCharacters++;

            if (/*!CurrentlySkipping &&*/
                (character == '?' || character == '.' || character == ',' || character == ':' ||
                 character == ';' || character == '!' || character == '-'))
            {
                yield return _interpunctuationDelay;
            }
            else
            {
                yield return /*CurrentlySkipping ? _skipDelay :*/ _simpleDelay;
            }

            //CharacterRevealed?.Invoke(character);
            _currentVisibleCharacterIndex++;
        }
    }



}
