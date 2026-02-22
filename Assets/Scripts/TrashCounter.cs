using UnityEngine;

public class TrashCounter : MonoBehaviour, IInteractable
{
    public void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            Debug.Log("Destroy Item!");

            player.GetKitchenObject().DestroySelf();
        }
    }
}
