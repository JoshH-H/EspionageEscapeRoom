using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechRec : MonoBehaviour
{
    public Text spokenTxt;
    public Text recognisitions;
    private DictationRecognizer Recogniser;

    private Arduino_PlayerDetect playerDetection => GetComponent<Arduino_PlayerDetect>();

    private void Start()
    {
        Recogniser = new DictationRecognizer();

        Recogniser.DictationResult += (text, confidence) =>
        {
            Debug.LogFormat("Dictation result: {0}", text);
            recognisitions.text += text + "\n";
        };

        Recogniser.DictationComplete += (completionCause) =>
        {
            if (completionCause != DictationCompletionCause.Complete)
            {
                Debug.LogErrorFormat("Ductation completed unsuccesfully: {0}.", completionCause);
            }
        };

        Recogniser.DictationError += (error, hresult) =>
        {
            Debug.LogErrorFormat("Ductation error: {0}. HResults = {1}.", error, hresult);
        };

        
    }

    private void Update()
    {
        if (playerDetection.playerDetected == true)
        {
            Recogniser.Start();
            Debug.Log("PLayer is being detected");
        }

        else if (playerDetection.playerDetected == false)
        {
            Recogniser.Stop();
            Debug.Log("Offline");
        }
    }
}
