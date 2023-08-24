using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_TextSOInt : MonoBehaviour
{
    public SOInt soInt;
    public TextMeshProUGUI textMesh;
    public string prefix;
    public string sufix;

    void Awake()
    {
        soInt.OnValueChanged += UpdateText;
    }

    void OnEnable()
    {
        textMesh.text = prefix + soInt.Value.ToString() + sufix;
    }

    private void UpdateText(int value)
    {
        textMesh.text = prefix + value.ToString() + sufix;
    }
}
