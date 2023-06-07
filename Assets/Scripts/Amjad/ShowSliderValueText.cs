using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowSliderValueText : MonoBehaviour
{
    [SerializeField] private TMP_Text SliderText;
    [SerializeField] private Slider HpSlider;

    public void ValueToText()
    {
        SliderText.text = HpSlider.value.ToString();
    }
}
