using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private Transform iconTemplate;

    private PlateKitchenObject plateKitchenObject;

    private void Awake()
    {
        // ซ่อน template ไว้ ไม่ให้โชว์
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        // หา PlateKitchenObject ใน Scene ตอนเริ่มเกม
        plateKitchenObject = FindFirstObjectByType<PlateKitchenObject>();

        if (plateKitchenObject == null)
        {
            Debug.LogError("PlateKitchenObject not found in scene!");
            return;
        }

        // Subscribe Event
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        // อัปเดต UI ครั้งแรก
        UpdateVisual();
    }

    private void OnDestroy()
    {
        // ป้องกัน error ตอน object ถูกลบ
        if (plateKitchenObject != null)
        {
            plateKitchenObject.OnIngredientAdded -= PlateKitchenObject_OnIngredientAdded;
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        // ลบ icon เก่า
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        if (plateKitchenObject == null) return;

        // สร้าง icon ตาม ingredient บนจาน
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);

            iconTransform.gameObject.SetActive(true);

            PlateIconSingleUI plateIconSingleUI = iconTransform.GetComponent<PlateIconSingleUI>();

            if (plateIconSingleUI != null)
            {
                plateIconSingleUI.SetKitchenObjectSO(kitchenObjectSO);
            }
        }
    }
}