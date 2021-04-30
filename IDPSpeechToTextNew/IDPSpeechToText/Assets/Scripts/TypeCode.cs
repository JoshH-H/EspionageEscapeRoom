using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypeCode : MonoBehaviour
{
    public float timer = 0.1f;

    public Text codeText;
    public string partOne;
    public string txtCurrent;

    private void Start()
    {
        StartCoroutine(IntroTextEntry());
    }

    private IEnumerator IntroTextEntry()
    {
        for (int i = 0; i < partOne.Length; i++)
        {
            txtCurrent = partOne.Substring(0, i);
            codeText.text = txtCurrent;

            yield return new WaitForSeconds(timer);
        }
    }
}
