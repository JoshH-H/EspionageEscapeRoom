using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechRec : MonoBehaviour
{
    [SerializeField] private enum RecordingState { recording, startRecord, closed, waiting, disabled };
    [SerializeField] private RecordingState rState = RecordingState.waiting;

    [SerializeField] private GameObject codeText;
    [SerializeField] private GameObject recordText;
    [SerializeField] private Text speechTxt;

    private DictationRecognizer dictationRecognizer;
    private Arduino_PlayerDetect playerDetection;

    private void Start()
    {
        playerDetection = GetComponent<Arduino_PlayerDetect>();

        recordText.SetActive(true);
        codeText.SetActive(false);

        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;
    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.LogFormat("Dictation result: {0}", text);
        speechTxt.text += text + "  // VALUE NOT RECOGNISED" + "\n";
    }

    private void DictationRecognizer_DictationHypothesis(string text)
    {
        Debug.LogFormat("Dictation result: {0}", text);

        if (text == "skydance")
        {
            Debug.Log("GetCode");
            recordText.SetActive(false);
            codeText.SetActive(true);

            rState = RecordingState.disabled;
        }
    }

    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        if (cause != DictationCompletionCause.Complete)
        {
            Debug.LogErrorFormat("Dictation completed unsuccesfully: {0}.", cause);
            rState = RecordingState.waiting;
        }
    }

    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        Debug.LogErrorFormat("Dictation error: {0}. HResults = {1}.", error, hresult);
        rState = RecordingState.waiting;
    }

    private void Update()
    {
        if (speechTxt.text.Length >= 1000)
        {
            Debug.Log("Clear");
            speechTxt.text = "";
        }

        switch (rState)
        {
            case RecordingState.waiting:

                WaitForDetection();

                break;

            case RecordingState.startRecord:

                StartRecording();

                break;

            case RecordingState.recording:

                RecordUntilNotDetected();

                break;

            case RecordingState.closed:

                StopRecording();

                break;

            case RecordingState.disabled:

                DisableListening();

                break;

            default:

                rState = RecordingState.waiting;

                break;
        }
    }

    private void WaitForDetection()
    {
        if (playerDetection.playerDetected == true)
        {
            rState = RecordingState.recording;
        }

        else
        {
            rState = RecordingState.waiting;
        }
    }

    private void StartRecording()
    {
        rState = RecordingState.recording;
    }

    private void RecordUntilNotDetected()
    {
        dictationRecognizer.Start();

        if (playerDetection.playerDetected == false)
        {
            rState = RecordingState.closed;
        }
    }

    private void StopRecording()
    {
        dictationRecognizer.Stop();

        rState = RecordingState.waiting;
    }

    private void DisableListening()
    {
        dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;
        dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
        dictationRecognizer.DictationHypothesis -= DictationRecognizer_DictationHypothesis;
        dictationRecognizer.DictationError -= DictationRecognizer_DictationError;

        dictationRecognizer.Stop();
        dictationRecognizer.Dispose();
    }
}
