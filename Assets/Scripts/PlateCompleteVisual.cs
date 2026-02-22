using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

    private void Start()
    {
        // 1. ซ่อนโมเดลวัตถุดิบทั้งหมดตอนเริ่ม
        foreach (KitchenObjectSO_GameObject item in kitchenObjectSOGameObjectList) {
            if (item.gameObject != null) item.gameObject.SetActive(false);
        }

        // 2. แก้ไขชื่อฟังก์ชันให้ตรงกับด้านล่าง และใช้โครงสร้าง Event ให้ถูกตาม PlateKitchenObject
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded; 
    }

    // 3. ปรับ Parameter ให้รับ (object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        // 4. วนลูปหาโมเดลที่ตรงกับส่วนผสมที่เพิ่มเข้ามา (e.kitchenObjectSO)
        foreach (KitchenObjectSO_GameObject item in kitchenObjectSOGameObjectList) {
            if (item.kitchenObjectSO == e.kitchenObjectSO) {
                item.gameObject.SetActive(true);
            }
        }
    }
}