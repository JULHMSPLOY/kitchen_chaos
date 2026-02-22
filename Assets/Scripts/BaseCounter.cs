using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint; // จุดสำหรับวาง KitchenObject บนเคาน์เตอร์

    private KitchenObject kitchenObject;

    // ฟังก์ชันเสมือนสำหรับให้เคาน์เตอร์ลูก (เช่น StoveCounter) นำไปเขียนทับ (Override)
    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    }

    // --- ส่วนการจัดการ Visual (สำหรับระบบ Raycast ของ Player) ---

    // แสดงแถบสีสว่างรอบเคาน์เตอร์เมื่อผู้เล่นมองมา
    public void ShowSelectedVisual() {
        Transform selected = transform.Find("Selected");
        if (selected != null) {
            selected.gameObject.SetActive(true);
        }
    }

    // ซ่อนแถบสีสว่างเมื่อผู้เล่นมองไปที่อื่น
    public void HideSelectedVisual() {
        Transform selected = transform.Find("Selected");
        if (selected != null) {
            selected.gameObject.SetActive(false);
        }
    }

    // --- ส่วนของ IKitchenObjectParent (จัดการการถือ/วางของ) ---

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