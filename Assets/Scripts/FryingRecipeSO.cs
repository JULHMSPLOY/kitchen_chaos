using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;        // วัตถุดิบดิบ
    public KitchenObjectSO output;       // วัตถุดิบที่สุกแล้ว
    public float fryingTimeMax;          // เวลาใช้ทอด (วินาที)
}
