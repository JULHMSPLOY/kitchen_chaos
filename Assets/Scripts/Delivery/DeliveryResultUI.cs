using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Image iconImage;

    private void OnEnable()
    {
        if (DeliveryManager.Instance != null)
        {
            DeliveryManager.Instance.OnRecipeCompleted += OnSuccess;
        }

        Hide();
    }

    private void OnDisable()
    {
        if (DeliveryManager.Instance != null)
        {
            DeliveryManager.Instance.OnRecipeCompleted -= OnSuccess;
        }
    }

    private void OnSuccess(object sender, EventArgs e)
    {
        Debug.Log("UI RECEIVED SUCCESS");
        background.SetActive(true);

        messageText.text = "DELIVERY SUCCESS!";
        messageText.color = Color.white;

        Image bgImage = background.GetComponent<Image>();
        bgImage.color = new Color(0.1f, 0.7f, 0.2f);

        iconImage.gameObject.SetActive(true);

        CancelInvoke();
        Invoke(nameof(Hide), 1f);
    }

    private void Hide()
    {
        background.SetActive(false);
    }
}