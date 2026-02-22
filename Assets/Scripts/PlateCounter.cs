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
        KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, this);
    }

    public void Interact(Player player)
    {
        // ถ้ามีจานบนโต๊ะ และ player ไม่มีของ
        if (HasKitchenObject() && !player.HasKitchenObject())
        {
            kitchenObject.SetKitchenObjectParent(player);
            SpawnPlate(); // สร้างจานใหม่โชว์บนโต๊ะ
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
