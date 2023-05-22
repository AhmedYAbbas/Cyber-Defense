using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenericAbility : MonoBehaviour
{
    public Button _card;
    public int cost = 5;
    public float _nextUseTime;
    public const float COOLDOWN_TIME = 10.0f;
    public TextMeshProUGUI cooldownText;
    public float _timer = COOLDOWN_TIME;
    public bool _isOnCooldown;

    public virtual void Update()
    {
        if (_isOnCooldown)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _timer = COOLDOWN_TIME;
        }

        DisplayTime(_timer);

        if (EnergyManager.Instance._energy < cost || _nextUseTime > Time.time)
        {
            _card.interactable = false;
            cooldownText.gameObject.SetActive(true);
            _isOnCooldown = true;
        }
        else
        {
            _card.interactable = true;
            cooldownText.gameObject.SetActive(false);
            _isOnCooldown = false;
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        cooldownText.text = $"{seconds}";
    }
}
