using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO rawKitchenObjectSO;
    public KitchenObjectSO cookedKitchenObjectSO;
    public float maxFryingTimer;
}
