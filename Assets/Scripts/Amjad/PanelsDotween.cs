using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class PanelsDotween : MonoBehaviour
{
    // private variables
    [SerializeField] private GameObject _Parent;
    [SerializeField] private float _transitionDuration = 0.5f;
    private RectTransform _panelRect;
    private GameObject _panel;

    //private void Awake()
    //{
    //    _panel = this.gameObject;
    //    _panelRect = _panel.GetComponent<RectTransform>();
    //}

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

        _panelRect.DOScale(Vector3.one, _transitionDuration)
            .SetEase(Ease.OutBack);
    }

    public void HidePanel()
    {
        _panelRect.DOScale(Vector3.one * 0.5f, _transitionDuration)
            .SetEase(Ease.InBack)
            .OnComplete(DeactivatePanel);
    }

    private void DeactivatePanel()
    {
        //_panel.SetActive(false);
        _Parent.SetActive(false);
    }
}