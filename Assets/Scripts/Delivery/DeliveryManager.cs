using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<OrderData> waitingOrderList;

    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeFailed;
    public event EventHandler OnRecipeListChanged;

    private float spawnRecipeTimer;
    private float firstSpawnDelay = 2f;
    private float minSpawnDelay = 15f;
    private float maxSpawnDelay = 22f;

    private int successfulDeliveries;

    private void Awake()
    {
        Instance = this;
        waitingOrderList = new List<OrderData>();
        spawnRecipeTimer = firstSpawnDelay;
    }

    private void Update()
    {
        HandleSpawning();
        HandleOrderTimers();
    }

    private void HandleSpawning()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0f)
        {
            if (waitingOrderList.Count < 4)
            {
                RecipeSO recipeSO =
                    recipeListSO.recipeSOList[
                        UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)
                    ];

                waitingOrderList.Add(new OrderData(recipeSO));
                OnRecipeListChanged?.Invoke(this, EventArgs.Empty);

                spawnRecipeTimer = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);
            }
        }
    }

    private void HandleOrderTimers()
    {
        for (int i = waitingOrderList.Count - 1; i >= 0; i--)
        {
            waitingOrderList[i].remainingTime -= Time.deltaTime;

            if (waitingOrderList[i].remainingTime <= 0f)
            {
                waitingOrderList.RemoveAt(i);
                OnRecipeFailed?.Invoke(this, EventArgs.Empty);
                OnRecipeListChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public List<OrderData> GetWaitingOrderList()
    {
        return waitingOrderList;
    }

    public bool DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingOrderList.Count; i++)
        {
            RecipeSO recipeSO = waitingOrderList[i].recipeSO;

            if (recipeSO.kitchenObjectSOList.Count ==
                plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateMatches = true;

                foreach (KitchenObjectSO recipeIngredient in recipeSO.kitchenObjectSOList)
                {
                    bool found = false;

                    foreach (KitchenObjectSO plateIngredient in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateIngredient == recipeIngredient)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        plateMatches = false;
                        break;
                    }
                }

                if (plateMatches)
                {
                    successfulDeliveries++;
                    waitingOrderList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeListChanged?.Invoke(this, EventArgs.Empty);

                    return true;
                }
            }
        }

        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        return false;
    }

    public int GetSuccessfulDeliveries()
    {
        return successfulDeliveries;
    }
}