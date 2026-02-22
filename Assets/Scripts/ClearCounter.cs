using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent, IInteractable
{
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public void Interact(Player player)
    {
        // กรณี Counter มีของ
        if (HasKitchenObject())
        {
            // Player มือว่าง → หยิบของ
            if (!player.HasKitchenObject())
            {
            GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                // Player ถือ Plate → ใส่ ingredient ลงจาน
                if (player.GetKitchenObject() is PlateKitchenObject plateKitchenObject)
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
        }
        else // Counter ว่าง
        {
            // Player มีของ → วางของ
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
    }

    // ===== IKitchenObjectParent Implementation =====

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
