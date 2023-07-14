using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {
    [SerializeField] CuttingRecipeSO[] cuttingRecipes;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    private int cuttingProgress;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {

                KitchenObject kitchenObject = player.GetKitchenObject();

                if (IsCuttableKitchenObject(kitchenObject.GetKitchenObjectSO())) {
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(kitchenObject.GetKitchenObjectSO());

                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    cuttingProgress = 0;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.maxCutsRequired
                    });
                }
            }
        } else {
            if (!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player) {
        KitchenObjectSO kitchenObjectSO = GetKitchenObject().GetKitchenObjectSO();
        if (HasKitchenObject() && IsCuttableKitchenObject(kitchenObjectSO)) {
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(kitchenObjectSO);

            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.maxCutsRequired
            });

            if (cuttingProgress >= cuttingRecipeSO.maxCutsRequired) {
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(GetCutKitchenObject(kitchenObjectSO), this);
            }
        }
    }

    private bool IsCuttableKitchenObject(KitchenObjectSO kitchenObject) {
        return !!GetCuttingRecipeSO(kitchenObject);
    }

    private KitchenObjectSO GetCutKitchenObject(KitchenObjectSO uncutKitchenObject) {
        CuttingRecipeSO cuttingRecipeSo = GetCuttingRecipeSO(uncutKitchenObject);

        if (cuttingRecipeSo != null) {
            return cuttingRecipeSo.cutKitchenObjectSO;
        }

        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSO(KitchenObjectSO uncutKitchenObject) {
        foreach (CuttingRecipeSO cuttingRecipe in cuttingRecipes) {
            if (cuttingRecipe.uncutKitchenObjectSO == uncutKitchenObject) {
                return cuttingRecipe;
            }
        }

        return null;
    }
}
