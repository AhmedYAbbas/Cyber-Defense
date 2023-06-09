using UnityEngine;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;

public class DamageTextAnimation : MonoBehaviour
{
    // private variables
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float duration = 1f;
    [SerializeField] float delay = 1f;
    private Tweener myTween;


    public void GotDamaged(int damage)
    {
        text.gameObject.SetActive(true);
        text.transform.localScale = Vector3.one * .5f;
        text.text = damage.ToString();
        myTween?.Kill();
        TextTween();
    }

    private void TextTween()
    {
        myTween = text.transform.DOScale(Vector3.one, duration)
            .SetEase(Ease.OutBack)
            .SetDelay(delay)
            .OnComplete(() =>
            {
                Dissapear();
            });
    }

    private void Dissapear()
    {
        myTween = text.transform.DOScale(Vector3.one * .5f, duration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                // Set the text object to inactive
                text.gameObject.SetActive(false);
            });
    }


}