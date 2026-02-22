using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject {
    public KitchenObjectSO input;      // วัตถุดิบที่สุกแล้ว
    public KitchenObjectSO output;     // วัตถุดิบที่ไหม้แล้ว
    public float burningTimerMax;       // เวลาที่ใช้เผา (วินาที)
}