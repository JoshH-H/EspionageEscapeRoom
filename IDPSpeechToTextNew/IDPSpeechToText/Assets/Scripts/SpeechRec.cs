using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechRec : MonoBehaviour
{
    [SerializeField] private enum RecordingState { recording, closed, waiting };
    [SerializeField] private RecordingState rState = RecordingState.waiting;

    [SerializeField] private GameObject codeText;
    [SerializeField] private GameObject othertext;

    [SerializeField] private bool isEntered = false;
    [SerializeField] private Text speechTxt;

    private DictationRecognizer dictationRecognizer;
    private Arduino_PlayerDetect playerDetection => GetComponent<Arduino_PlayerDetect>();

    private void Start()
    {
        othertext.SetActive(true);
        codeText.SetActive(false);

        dictationRecognizer = new DictationRecognizer();
    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.LogFormat("Dictation result: {0}", text);
        speechTxt.text += text + "  // Entry not Recognised" + "\n";
    }

    private void DictationRecognizer_DictationHypothesis(string text)
    {
        Debug.LogFormat("Dictation result: {0}", text);

        if (text == "12345")
        {
            Debug.Log("GetCode");
            othertext.SetActive(false);
            codeText.SetActive(true);
        }
    }

    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        if (cause != DictationCompletionCause.Complete)
        {
            Debug.LogErrorFormat("Ductation completed unsuccesfully: {0}.", cause);
        }
    }

    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        Debug.LogErrorFormat("Ductation error: {0}. HResults = {1}.", error, hresult);
    }

    private void Update()
    {
        switch (rState)
        {
            case RecordingState.waiting:

                WaitForDetection();

                break;

            case RecordingState.recording:

                StartRecording();

                break;

            case RecordingState.closed:

                StopRecording();

                break;

            default:

                rState = RecordingState.waiting;

                break;
        }
    }

    private void StartRecording()
    {
        if (playerDetection.playerDetected == true && isEntered == false)
        {
            Debug.Log("player in");

            dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
            dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
            dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
            dictationRecognizer.DictationError += DictationRecognizer_DictationError;

            dictationRecognizer.Start();

            isEntered = true;
        }

        if (playerDetection.playerDetected == false)
        {
            rState = RecordingState.closed;
        }
    }

    private void StopRecording()
    {
        if (playerDetection.playerDetected == false && isEntered == true)
        {
            dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;
            dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
            dictationRecognizer.DictationHypothesis -= DictationRecognizer_DictationHypothesis;
            dictationRecognizer.DictationError -= DictationRecognizer_DictationError;

            dictationRecognizer.Stop();
            dictationRecognizer.Dispose();

            rState = RecordingState.waiting;
        }
    }

    private void WaitForDetection()
    {
        isEntered = false;

        if (playerDetection.playerDetected == true)
        {
            rState = RecordingState.recording;
        }
    }








    // BELOW WORKS TOO LEAVE FOR NOW

    //public Text speechTxt;
    //private DictationRecognizer Recogniser; 

    //private Arduino_PlayerDetect playerDetection => GetComponent<Arduino_PlayerDetect>();

    //private void Start()
    //{
    //    Recogniser = new DictationRecognizer();

    //    Recogniser.DictationResult += (text, confidence) =>
    //    {
    //        Debug.LogFormat("Dictation result: {0}", text);
    //        speechTxt.text += text + "\n";
    //    };

    //    Recogniser.DictationComplete += (completionCause) =>
    //    {
    //        if (completionCause != DictationCompletionCause.Complete)
    //        {
    //            Debug.LogErrorFormat("Ductation completed unsuccesfully: {0}.", completionCause);
    //        }
    //    };

    //    Recogniser.DictationError += (error, hresult) =>
    //    {
    //        Debug.LogErrorFormat("Ductation error: {0}. HResults = {1}.", error, hresult);
    //    };
    //}

    //private void Update()
    //{
    //    if (playerDetection.playerDetected == true)
    //    {
    //        Recogniser.Start();
    //    }

    //    else if (playerDetection.playerDetected == false)
    //    {
    //        Recogniser.Stop();
    //    }
    //}
}
