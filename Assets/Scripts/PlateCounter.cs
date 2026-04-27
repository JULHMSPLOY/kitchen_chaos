using UnityEngine;

public class PlateCounter : MonoBehaviour, IKitchenObjectParent, IInteractable
{
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    private void Start()
    {
        SpawnPlate();
    }

    private void SpawnPlate()
    {
        if (HasKitchenObject()) return;

        KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, this);
    }

    public void Interact(Player player)
    {
        if (!HasKitchenObject()) return;

        if (!player.HasKitchenObject())
        {
            kitchenObject.SetKitchenObjectParent(player);
            SpawnPlate();
        }
    }

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