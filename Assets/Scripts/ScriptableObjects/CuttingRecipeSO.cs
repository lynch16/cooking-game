using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO uncutKitchenObjectSO;
    public KitchenObjectSO cutKitchenObjectSO;
    public int maxCutsRequired;
}
