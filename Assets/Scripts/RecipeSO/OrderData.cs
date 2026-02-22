using UnityEngine;

[System.Serializable]
public class OrderData
{
    public RecipeSO recipeSO;
    public float remainingTime;

    public OrderData(RecipeSO recipeSO)
    {
        this.recipeSO = recipeSO;
        this.remainingTime = recipeSO.recipeTime; 
    }
}