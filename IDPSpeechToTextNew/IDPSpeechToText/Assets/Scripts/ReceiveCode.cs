using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class ReceiveCode : MonoBehaviour
{
    private KeywordRecognizer getSpeech;
    private Dictionary<string, Action> codeWords = new Dictionary<string, Action>();
    public GameObject codeText;
    public GameObject othertext;
    public GameObject dialoguePanel;

    private Arduino_PlayerDetect playerDetection => GetComponent<Arduino_PlayerDetect>();


    private void Start()
    {
        codeText.SetActive(false);
        codeWords.Add("one two three four five", CodeReceived);
        getSpeech = new KeywordRecognizer(codeWords.Keys.ToArray());
        getSpeech.OnPhraseRecognized += GetCodes;
    }

    private void GetCodes(PhraseRecognizedEventArgs code)
    {
        Debug.Log(code.text);
        codeWords[code.text].Invoke();
    }

    private void CodeReceived()
    {
        othertext.SetActive(false);
        codeText.SetActive(true);
    }

    private void Update()
    {
        if (playerDetection.playerDetected == true)
        {
            getSpeech.Start();
        }

        else if (playerDetection.playerDetected == false)
        {
            getSpeech.Stop();
        }
    }
}
