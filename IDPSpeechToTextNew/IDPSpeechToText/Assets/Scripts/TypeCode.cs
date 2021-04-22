using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeCode : MonoBehaviour
{
    [SerializeField] private float timer = 0.1f;

    [SerializeField] private string partOne;
    [SerializeField] private string ptOneCurrent = "";

    // Start is called before the first frame update
    private void Start() => StartCoroutine(IntroTextEntry());

    private IEnumerator IntroTextEntry()
    {
        for (int i = 0; i < partOne.Length; i++)
        {
            ptOneCurrent = partOne.Substring(0, i);
            this.GetComponent<Text>().text = ptOneCurrent;

            yield return new WaitForSeconds(timer);
        }
    }
}
