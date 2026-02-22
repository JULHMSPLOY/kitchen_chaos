using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;
    [SerializeField] private TextMeshProUGUI timerText;

    private OrderData orderData;

    public void SetOrder(OrderData orderData)
    {
        this.orderData = orderData;

        RecipeSO recipe = orderData.recipeSO;
        recipeNameText.text = recipe.recipeName;

        // ลบ icon เก่า
        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        // สร้าง icon ใหม่
        foreach (KitchenObjectSO kitchenObjectSO in recipe.kitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);

            iconTransform.GetComponent<Image>().sprite =
                kitchenObjectSO.sprite;
        }
    }

    private void Update()
    {
        if (orderData == null) return;

        timerText.text =
            Mathf.Ceil(orderData.remainingTime).ToString("00") + "s";
    }
}