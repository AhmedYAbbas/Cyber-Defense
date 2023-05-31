using DG.Tweening;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AttackerDefenderPanelTween : MonoBehaviour
{
    // private variables
    private Tweener _panelTweener;
    [SerializeField] private GameObject _panel;
    [SerializeField] private float _delayTime;

    // public variables


    private void Start()
    {
        _panel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _panelTweener?.Kill(); // Kill any previous tweens
            _panel.SetActive(true);
            _panelTweener = _panel.transform.DOLocalMove(Vector3.zero, 1f);
        }
        else if (Input.GetMouseButtonUp(0) && _panel.activeSelf)
        {
            //_panelTweener?.Kill(); // Kill any previous tweens
            _panelTweener = _panel.transform.DOLocalMove(new Vector3(0, -2000f, 0f), 10f).OnComplete(() =>
            {
                _panel.SetActive(false);
            }).SetDelay(_delayTime);
        }
    }
}
