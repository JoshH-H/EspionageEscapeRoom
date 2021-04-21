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
    public string txt;
    public Text speechText;
    public GameObject dialoguePanel;


    private void Start()
    {
        codeWords.Add("one two three four five", CodeReceived);
        getSpeech = new KeywordRecognizer(codeWords.Keys.ToArray());
        getSpeech.OnPhraseRecognized += GetCodes;

        getSpeech.Start();
    }

    private void GetCodes(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        codeWords[speech.text].Invoke();
    }

    private void CodeReceived()
    {
        transform.Translate(-1, 0, 0);
        //this is a comment
    }
}
