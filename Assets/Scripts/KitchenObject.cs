using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] KitchenObjectSO kitchenobject;

    protected virtual void Awake()
    {
        // ถ้ายังไม่ต้องทำอะไร ก็ปล่อยว่างไว้ได้
    }

    public string GetKitchenObjectname()
    {
        return kitchenobject.objectname;
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenobject;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
    return kitchenObjectParent;
    }   

    public static KitchenObject SpawnKitchenObject(
        KitchenObjectSO kitchenObjectSO,
        IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(
        kitchenObjectSO.prefab
        );

        KitchenObject kitchenObject =
            kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent newParent)
    {
        if (kitchenObjectParent != null)
        {
            kitchenObjectParent.ClearKitchenObject();
        }

        kitchenObjectParent = newParent;

        newParent.SetKitchenObject(this);

        transform.parent = newParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }   

    public void DestroySelf()
    {
        if (kitchenObjectParent != null)
        {
            kitchenObjectParent.ClearKitchenObject();
        }
        Destroy(gameObject);
    }

    private IKitchenObjectParent kitchenObjectParent;
}