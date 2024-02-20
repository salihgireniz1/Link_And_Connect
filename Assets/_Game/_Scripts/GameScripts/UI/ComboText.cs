using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboText : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public float scaleAmount = 0.2f;
    public void Initialize(int count, Color32 color)
    {
        textField.color = color;
        textField.text = count.ToString() + "X Combo!";
        transform.localScale = Vector3.one + Vector3.one * scaleAmount * (count - 2);
    }
    public void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
}
