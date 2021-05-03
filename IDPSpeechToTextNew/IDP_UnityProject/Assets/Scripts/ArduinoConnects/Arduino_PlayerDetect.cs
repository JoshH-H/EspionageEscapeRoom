using System.Collections;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class Arduino_PlayerDetect : MonoBehaviour
{
    public static SerialPort sp = new SerialPort("COM6", 9600); // Change to match the port used
    static public Text checkState;

    private bool busy = false;
    private bool _isPlayed = false;
    public bool playerDetected = false;

    public AudioClip recSound;
    public AudioSource audio1;

    void Start()
    {
        checkState = GameObject.Find("ValueText").GetComponent<Text>();
        checkState.text = "RECEIVING : OFF";
        OpenConnection();
    }

    void Update()
    {
        if (sp.IsOpen)
        {

            try
            {
                sp.ReadLine();
                string value = sp.ReadLine().ToString();

                if (value == "active")
                {
                    checkState.text = "RECEIVING : ON";

                    if (!busy && !_isPlayed)
                    {
                        StartCoroutine(PlaySound());
                        busy = true;
                    }

                    playerDetected = true;
                }

                else if (value == "inactive")
                {
                    checkState.text = "RECEIVING : OFF";
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
        AudioClip clip = recSound;
        audio1.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        busy = false;
        _isPlayed = true;
    }
}

