using Unity.Collections;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;

public class ContainerCounter : MonoBehaviour, IKitchenObjectParent,IInteractable

{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private KitchenObject kitchenObject;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        Debug.Log("Animator = " + animator);
    }

    public void Interact(Player player)
    {
        // ถ้าเคาน์เตอร์ไม่มีของ → spawn ก่อนเสมอ
        if (!HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, this);
            
            animator?.SetTrigger("OpenClose");
            return;  
        }

        // ถ้า player ไม่มีของ → หยิบตามปกติ
        if (!player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
            return;
        }

        // ถ้า player ถือจาน → ใส่ลงจาน
        if (player.GetKitchenObject() is PlateKitchenObject plateKitchenObject)
        {
            var so = GetKitchenObject().GetKitchenObjectSO();

            if (plateKitchenObject.TryAddIngredient(so))
            {
                GetKitchenObject().DestroySelf();
            }
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