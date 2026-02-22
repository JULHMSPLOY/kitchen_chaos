using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeListChanged += UpdateVisual;
        UpdateVisual(null, null);
    }

    private void UpdateVisual(object sender, System.EventArgs e)
    {
        // ลบของเก่า
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        // 🔥 ใช้ OrderData แทน RecipeSO
        foreach (OrderData orderData in DeliveryManager.Instance.GetWaitingOrderList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);

            recipeTransform
                .GetComponent<DeliveryManagerSingleUI>()
                .SetOrder(orderData);
        }
    }

    private void OnDestroy()
    {
        if (DeliveryManager.Instance != null)
        {
            DeliveryManager.Instance.OnRecipeListChanged -= UpdateVisual;
        }
    }
}