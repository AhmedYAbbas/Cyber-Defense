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
        if (HpSlider.maxValue == 1)
        {
            SliderText.text = $"{(HpSlider.value * 1000).ToString()}";
        }
        else
        {
            SliderText.text = $"{HpSlider.value.ToString()}";
        }
    }
}
