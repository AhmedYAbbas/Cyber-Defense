using UnityEngine;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;

public class DamageTextAnimation : MonoBehaviour
{
    // private variables
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Slider Hp;
    [SerializeField] float moveDistance = 3f;
    [SerializeField] float duration = 1f;
    [SerializeField] float delay = 1f;

    public void GotDamaged()
    {
        text.gameObject.SetActive(true);

        // Move the text upwards
        text.transform.DOMoveY(text.transform.position.y + moveDistance, duration)
            .SetEase(Ease.OutQuad)
            .SetDelay(delay)
            .OnComplete(() =>
            {
                // Set the text object to inactive
                text.gameObject.SetActive(false);
            });
    }
}