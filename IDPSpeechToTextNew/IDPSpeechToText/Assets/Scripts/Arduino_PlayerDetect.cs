using System.Collections;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class Arduino_PlayerDetect : MonoBehaviour
{
    public static SerialPort sp = new SerialPort("COM3", 9600);
    static public Text value;

    private bool busy = false;
    private bool _isPlayed = false;
    public bool playerDetected = false;

    public AudioClip motionFx;
    public AudioSource audio1;

    void Start()
    {
        value = GameObject.Find("ValueText").GetComponent<Text>();
        value.text = "RECEIVING : OFF";
        OpenConnection();
    }

    void Update()
    {
        if (sp.IsOpen)
        {

            try
            {
                string message = sp.ReadLine();
                bool myBool = bool.Parse(message);

                if (myBool == true)
                {
                    value.text = "RECEIVING : ON";
                    if (!busy && !_isPlayed)
                    {
                        StartCoroutine(PlaySound());
                        busy = true;
                    }

                    playerDetected = true;
                }

                else if (myBool == false)
                {
                    value.text = "RECEIVING : OFF";
                    _isPlayed = false;
                    playerDetected = false;
                }

            }
            catch (System.Exception)
            {
                throw;
            }

        }
    }

    private void OnApplicationQuit()
    {
        sp.Close();
    }

    void OpenConnection()
    {
        sp.Open();
        sp.ReadTimeout = 5000;
        print("Opening port");
    }

    IEnumerator PlaySound()
    {
        AudioClip clip = motionFx;
        audio1.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        busy = false;
        _isPlayed = true;
    }
}

