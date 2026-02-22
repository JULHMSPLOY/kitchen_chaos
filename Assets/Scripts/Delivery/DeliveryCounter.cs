using System;
using UnityEngine;

public class DeliveryCounter : BaseCounter, IInteractable
{
    [SerializeField] private GameObject highlight;
    public event EventHandler OnPlatePlaced;

    private void Start()
    {
        highlight.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            highlight.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            highlight.SetActive(false);
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject()) return;

        if (player.GetKitchenObject() is PlateKitchenObject plate)
        {
            // 🔥 เช็คผลก่อน
            bool isCorrect = DeliveryManager.Instance.DeliverRecipe(plate);

            if (isCorrect)
            {
                // ✅ ส่งถูก → ค่อยยิง Event
                OnPlatePlaced?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                // ❌ ส่งผิด → ไม่ต้องยิง Event
                // (ถ้าจะให้ลูกศรหาย คุมใน Arrow แทน)
            }

            player.GetKitchenObject().DestroySelf();
        }
    }
}