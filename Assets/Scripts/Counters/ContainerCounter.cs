using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter {
    [SerializeField] private KitchenObjectSO kitchenObjectSo;

    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player) {
        if (!this.HasKitchenObject() && !player.HasKitchenObject()) {
            KitchenObject.SpawnKitchenObject(kitchenObjectSo, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }

}
