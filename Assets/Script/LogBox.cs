using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogBox : MonoBehaviour
{
    private TextMeshProUGUI t;
    private void Start()
    {
        t = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void Text(string s)
    {
        t.text += s + "\n";
    }
}
