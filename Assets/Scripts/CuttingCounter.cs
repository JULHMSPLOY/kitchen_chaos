using System;
using UnityEngine;
using System.Linq;

public class CuttingCounter : MonoBehaviour, IKitchenObjectParent, IInteractable
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    [SerializeField] private KitchenObjectSO breadKitchenObjectSO;

    private KitchenObject kitchenObject;

    private float cuttingProcess;
    public float cuttingSpeed = 5f;
    public float knifeSpeed = 0.2f;
    private bool isCutting = false;

    private Animator animator;
    private ProcessBar processBar;
    private float timer = 0f;

    private readonly string[] cuttableObjects = { "Tomato", "Cheese", "Cabbage" };

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        processBar = GetComponentInChildren<ProcessBar>();
    }

    private void Update()
    {
        if (!HasKitchenObject()) return;
        if (!isCutting) return;

        cuttingProcess += cuttingSpeed * Time.deltaTime;
        Cutting_FX(Time.deltaTime);

        string objectName = kitchenObject.GetKitchenObjectname();
        int index = Array.IndexOf(cuttableObjects, objectName);

        if (index < 0 || index >= cuttingRecipeSOArray.Length) return;

        int cuttingMax = cuttingRecipeSOArray[index].cutCount;

        if (processBar != null)
        {
            float percent = cuttingProcess / cuttingMax;
            processBar.CuttingCounter_OnProcessChanged(percent);
        }

        if (cuttingProcess >= cuttingMax)
        {
            KitchenObjectSO outputSO = cuttingRecipeSOArray[index].to;

            kitchenObject.DestroySelf();

            KitchenObject.SpawnKitchenObject(outputSO, this);

            if (processBar != null)
                processBar.CuttingCounter_OnProcessChanged(0f);

            cuttingProcess = 0f;
            isCutting = false;
        }
    }

    private void Cutting_FX(float duration)
    {
        timer += duration;

        if (timer >= knifeSpeed)
        {
            if (animator != null)
                animator.SetTrigger("Cut");

            AudioManager.Instance.PlaySound(
                AudioManager.Instance.GetAudioClipRefsSO().chop,
                transform.position,
                0.8f
            );

            timer = 0f;
        }
    }

    public void Interact(Player player)
    {
        // ===== Player ถือของ =====
        if (player.HasKitchenObject())
        {
            // ===== Counter มีของ (ผักหั่นแล้ว) =====
            if (HasKitchenObject())
            {
                // ถ้า Player ถือ Plate
                if (player.GetKitchenObject() is PlateKitchenObject plate)
                {
                    if (plate.TryAddIngredient(kitchenObject.GetKitchenObjectSO()))
                    {
                        kitchenObject.DestroySelf();
                        isCutting = false;
                    }
                }
            }
            else
            {
                // ===== วางของลงเพื่อหั่น =====
                KitchenObject playerObject = player.GetKitchenObject();

                if (!cuttableObjects.Contains(playerObject.GetKitchenObjectname()))
                {
                    return;
                }

                playerObject.SetKitchenObjectParent(this);

                cuttingProcess = 0f;
                timer = 0f;
                isCutting = true;

                if (processBar != null)
                    processBar.CuttingCounter_OnProcessChanged(0f);
            }
        }
        else
        {
            // ===== Player ไม่ถืออะไร =====
            if (HasKitchenObject())
            {
                kitchenObject.SetKitchenObjectParent(player);
            }
        }
    }

    // ===== IKitchenObjectParent =====

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