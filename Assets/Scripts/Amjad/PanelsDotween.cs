using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System.Collections.Generic;

public class PanelsDotween : MonoBehaviour
{
    public enum TweenTypes
    {
        Back,
        Elastic,
        Bounce,
        Flash,
        Expo,
        Circ,
        Cubic,
        Quad,
        Quart,
        Quint,
        Sine
    }

    // private variables
    private RectTransform _panelRect;
    private GameObject _panel;
    private Dictionary<TweenTypes, Ease> _inTweenTypes;
    private Dictionary<TweenTypes, Ease> _outTweenTypes;

    // public variables
    [SerializeField] // Add this attribute
    public TweenTypes SelectedOption;

    [SerializeField] // Add this attribute
    public GameObject Parent;

    [SerializeField] // Add this attribute
    public float TransitionDuration = 0.5f;


    private void Awake()
    {
        _inTweenTypes = new Dictionary<TweenTypes, Ease>();
        _outTweenTypes = new Dictionary<TweenTypes, Ease>();

        // in Tweens
        _inTweenTypes.Add(TweenTypes.Back, Ease.InBack);
        _inTweenTypes.Add(TweenTypes.Elastic, Ease.InElastic);
        _inTweenTypes.Add(TweenTypes.Bounce, Ease.InBounce);
        _inTweenTypes.Add(TweenTypes.Flash, Ease.InFlash);
        _inTweenTypes.Add(TweenTypes.Expo, Ease.InExpo);
        _inTweenTypes.Add(TweenTypes.Circ, Ease.InCirc);
        _inTweenTypes.Add(TweenTypes.Cubic, Ease.InCubic);
        _inTweenTypes.Add(TweenTypes.Quad, Ease.InQuad);
        _inTweenTypes.Add(TweenTypes.Quart, Ease.InQuart);
        _inTweenTypes.Add(TweenTypes.Quint, Ease.InQuint);
        _inTweenTypes.Add(TweenTypes.Sine, Ease.InSine);


        // out Tweens
        _outTweenTypes.Add(TweenTypes.Back, Ease.OutBack);
        _outTweenTypes.Add(TweenTypes.Elastic, Ease.OutElastic);
        _outTweenTypes.Add(TweenTypes.Bounce, Ease.OutBounce);
        _outTweenTypes.Add(TweenTypes.Flash, Ease.OutFlash);
        _outTweenTypes.Add(TweenTypes.Expo, Ease.OutExpo);
        _outTweenTypes.Add(TweenTypes.Circ, Ease.OutCirc);
        _outTweenTypes.Add(TweenTypes.Cubic, Ease.OutCubic);
        _outTweenTypes.Add(TweenTypes.Quad, Ease.OutQuad);
        _outTweenTypes.Add(TweenTypes.Quart, Ease.OutQuart);
        _outTweenTypes.Add(TweenTypes.Quint, Ease.OutQuint);
        _outTweenTypes.Add(TweenTypes.Sine, Ease.OutSine);
    }

    void OnEnable()
    {
        _panel = this.gameObject;
        _panelRect = _panel.GetComponent<RectTransform>();
        ShowPanel();
    }

    public void ShowPanel()
    {
        _panelRect.localScale = Vector3.one * 0.5f;
        _panel.SetActive(true);

        _panelRect.DOScale(Vector3.one, TransitionDuration)
            .SetEase(_outTweenTypes[SelectedOption]);
    }

    public void HidePanel()
    {
        _panelRect.DOScale(Vector3.one * 0.5f, TransitionDuration)
            .SetEase(_inTweenTypes[SelectedOption])
            .OnComplete(DeactivatePanel);
    }

    private void DeactivatePanel()
    {
        Parent?.SetActive(false);
    }
}