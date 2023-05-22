using UnityEngine;

public class Ads : MonoBehaviour
{
    [SerializeField] private GameObject[] ads;
    private int _adsCount;

    private void Awake()
    {
        _adsCount = ads.Length;
    }

    public void OnClose(GameObject obj)
    {
        _adsCount--;
        obj.SetActive(false);

        if (_adsCount == 0)
        {
            gameObject.SetActive(false);

            foreach (GameObject ad in ads)
            {
                ad.SetActive(true);
            }

            _adsCount = ads.Length;
        }

    }
}
