using UnityEngine;
using UnityEngine.UI;

public class PlateIconSingleUI : MonoBehaviour
{
    // ตัวแปรสำหรับอ้างอิง Image ที่อยู่ใน Icon Template
    [SerializeField] private Image image; 

    public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        // เปลี่ยนรูปภาพให้ตรงกับข้อมูลของวัตถุดิบนั้นๆ
        image.sprite = kitchenObjectSO.sprite; 
    }
}
