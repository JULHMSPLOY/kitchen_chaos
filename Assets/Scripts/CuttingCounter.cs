using System;
using UnityEngine;

public class CuttingCounter : MonoBehaviour, IKitchenObjectParent, IInteractable
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private KitchenObject kitchenObject;

    private float cuttingProgress;
    [SerializeField] private float cuttingSpeed = 1f;
    [SerializeField] private float knifeSpeed = 0.2f;

    private bool isCutting = false;

    private Animator animator;
    private ProcessBar processBar;
    private float knifeTimer = 0f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        processBar = GetComponentInChildren<ProcessBar>();
    }

    private void Update()
    {
        if (!HasKitchenObject()) return;
        if (!isCutting) return;

        CuttingRecipeSO recipe =
            GetCuttingRecipeWithInput(kitchenObject.GetKitchenObjectSO());

        if (recipe == null)
        {
            StopCutting();
            return;
        }

        // เพิ่ม progress
        cuttingProgress += cuttingSpeed * Time.deltaTime;

        PlayCutFX(Time.deltaTime);

        if (processBar != null)
        {
            float percent = cuttingProgress / recipe.cutCount;
            processBar.CuttingCounter_OnProcessChanged(percent);
        }

        // เสร็จแล้ว
        if (cuttingProgress >= recipe.cutCount)
        {
            CompleteCut(recipe);
        }
    }

    private void CompleteCut(CuttingRecipeSO recipe)
    {
        KitchenObjectSO outputSO = recipe.to;

        // เก็บตัวเก่าไว้
        KitchenObject oldObject = kitchenObject;

        oldObject.DestroySelf();

        // Spawn ตัวใหม่
        KitchenObject newObject =
            KitchenObject.SpawnKitchenObject(outputSO, this);

        // อัปเดต reference ชัดเจน
        kitchenObject = newObject;

        StopCutting();
    }

    private void StopCutting()
    {
        cuttingProgress = 0f;
        knifeTimer = 0f;
        isCutting = false;

        if (processBar != null)
            processBar.CuttingCounter_OnProcessChanged(0f);
    }

    private void PlayCutFX(float delta)
    {
        knifeTimer += delta;

        if (knifeTimer >= knifeSpeed)
        {
            if (animator != null)
                animator.SetTrigger("Cut");

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(
                    AudioManager.Instance.GetAudioClipRefsSO().chop,
                    transform.position,
                    0.8f
                );
            }

            knifeTimer = 0f;
        }
    }

    public void Interact(Player player)
    {
        // ผู้เล่นถือของอยู่
        if (player.HasKitchenObject())
        {
            // Counter มีของอยู่
            if (HasKitchenObject())
            {
                // ถ้า player ถือจาน
                if (player.GetKitchenObject() is PlateKitchenObject plate)
                {
                    if (plate.TryAddIngredient(kitchenObject.GetKitchenObjectSO()))
                    {
                        kitchenObject.DestroySelf();
                        StopCutting();
                    }
                }
            }
            else
            {
                // วางของลงเพื่อหั่น
                KitchenObject playerObject = player.GetKitchenObject();

                CuttingRecipeSO recipe =
                    GetCuttingRecipeWithInput(playerObject.GetKitchenObjectSO());

                if (recipe == null) return;

                playerObject.SetKitchenObjectParent(this);

                cuttingProgress = 0f;
                knifeTimer = 0f;
                isCutting = true;

                if (processBar != null)
                    processBar.CuttingCounter_OnProcessChanged(0f);
            }
        }
        else
        {
            // ผู้เล่นมือว่าง → หยิบของขึ้น
            if (HasKitchenObject())
            {
                kitchenObject.SetKitchenObjectParent(player);
                StopCutting();
            }
        }
    }

    private CuttingRecipeSO GetCuttingRecipeWithInput(KitchenObjectSO inputSO)
    {
        foreach (CuttingRecipeSO recipe in cuttingRecipeSOArray)
        {
            if (recipe.from == inputSO)
                return recipe;
        }

        return null;
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