using System;
using UnityEngine;

public class DeliveryCounter : BaseCounter, IInteractable
{
    public event EventHandler OnPlatePlaced;

    [SerializeField] private GameObject deliveryResultUI;

    private void Start()
    {
        if (deliveryResultUI != null)
        {
            deliveryResultUI.SetActive(false);
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject()) return;

        KitchenObject kitchenObject = player.GetKitchenObject();
        bool isCorrect = false;

        // ถ้าเป็น Plate
        if (kitchenObject is PlateKitchenObject plate)
        {
            isCorrect = DeliveryManager.Instance.DeliverRecipe(plate);
        }
        else
        {
            // ถ้าเป็นอาหารธรรมดา → ให้ DeliveryManager ตรวจตรง ๆ
            isCorrect = DeliveryManager.Instance.DeliverSingleItem(kitchenObject.GetKitchenObjectSO());
        }

        if (isCorrect)
        {
            OnPlatePlaced?.Invoke(this, EventArgs.Empty);
            ShowDeliverySuccess();
        }

        kitchenObject.DestroySelf();
    }

    private void ShowDeliverySuccess()
    {
        if (deliveryResultUI != null)
        {
            deliveryResultUI.SetActive(true);
            Invoke(nameof(HideDeliveryResult), 2f);
        }
    }

    private void HideDeliveryResult()
    {
        if (deliveryResultUI != null)
        {
            deliveryResultUI.SetActive(false);
        }
    }
}