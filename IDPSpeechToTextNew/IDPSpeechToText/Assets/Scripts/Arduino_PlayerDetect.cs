using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.UI;

public class Arduino_PlayerDetect : MonoBehaviour
{
    public static SerialPort sp = new SerialPort("COM3", 9600); // Change this to match your Arduino's COM Port.
    Thread readThread = new Thread(ReadData);
    static bool checking = true;
    static public Text value;

    private bool busy = false;
    private bool _isPlayed = false;
    public bool playerDetected = false;

    public AudioClip motionFx;
    public AudioSource audio1;

    // Start is called before the first frame update
    void Start()
    {
        value = GameObject.Find("ValueText").GetComponent<Text>();
        value.text = "Detecting Player";
        OpenConnection();
        readThread.Start();

    }

    // Update is called once per frame
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
                    value.text = "Player Detected";
                    if (!busy && !_isPlayed)
                    {

                        StartCoroutine(playSound());
                        busy = true;
                        playerDetected = true;

                    }

                }
                else if (myBool == false)
                {
                    value.text = "Player Not In Range";
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
        checking = false;
    }

    void OpenConnection()
    {
        sp.Open();
        sp.ReadTimeout = 5000;
        print("Opening port");
    }


    public static void ReadData()
    {
        while (checking)
        {
            try
            {
                //string message = sp.ReadLine();
                //print(message);

            }
            catch
            {
                print("Caught Error!");
            }
        }
    }

    IEnumerator playSound()
    {
        AudioClip clip = motionFx;
        audio1.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        busy = false;
        _isPlayed = true;

    }

}

