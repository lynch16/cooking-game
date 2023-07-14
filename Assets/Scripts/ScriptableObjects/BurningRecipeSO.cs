using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject
{
    public KitchenObjectSO cookedKitchenObjectSO;
    public KitchenObjectSO burnedKitchenObjectSO;
    public float maxBurningTimer;
}
