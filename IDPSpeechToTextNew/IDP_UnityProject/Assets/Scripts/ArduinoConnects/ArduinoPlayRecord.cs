using System.IO.Ports;
using UnityEngine;

public class ArduinoPlayRecord : MonoBehaviour
{
    public static SerialPort sp = new SerialPort("COM3", 9600); // Change to the used COM
    public bool playerDetected = false;

    public AudioClip recordClip;
    public AudioSource recording;

    void Start()
    {
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

                if (value == "playing" && recording.isPlaying == false)
                {
                    recording.Play();
                }

                else if (value != "playing" && recording.isPlaying == true)
                {
                    recording.Pause();
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
}
