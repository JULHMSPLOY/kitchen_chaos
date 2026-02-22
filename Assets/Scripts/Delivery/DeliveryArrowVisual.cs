using UnityEngine;

public class DeliveryArrowVisual : MonoBehaviour
{
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeListChanged += UpdateArrow;

        UpdateArrow(null, null); // เช็คตอนเริ่มเกม
    }

    private void UpdateArrow(object sender, System.EventArgs e)
    {
        if (DeliveryManager.Instance.GetWaitingOrderList().Count > 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (DeliveryManager.Instance != null)
        {
            DeliveryManager.Instance.OnRecipeListChanged -= UpdateArrow;
        }
    }
}