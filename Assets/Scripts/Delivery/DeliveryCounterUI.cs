using System;
using TMPro;
using UnityEngine;

public class DeliveryCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += UpdateCounter;
        UpdateCounter(null, null);
    }

    private void UpdateCounter(object sender, EventArgs e)
    {
        counterText.text = "Delivered: " +
            DeliveryManager.Instance.GetSuccessfulDeliveries();
    }

    private void OnDestroy()
    {
        if (DeliveryManager.Instance != null)
        {
            DeliveryManager.Instance.OnRecipeCompleted -= UpdateCounter;
        }
    }
}
