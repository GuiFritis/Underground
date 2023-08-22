using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_TextSOInt : MonoBehaviour
{
    public SOInt soFloor;
    public TextMeshProUGUI textMesh;
    public string prefix;
    public string sufix;

    void Start()
    {
        soFloor.OnValueChanged += UpdateText;
    }

    private void UpdateText(int value)
    {
        textMesh.text = "";
        textMesh.text += prefix + value.ToString() + sufix;
    }
}
