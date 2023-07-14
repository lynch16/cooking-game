using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent {

    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public virtual void Interact(Player player) {
    }

    public virtual void InteractAlternate(Player player) { }

    public Transform GetKitchenObjectHoldPoint() { return counterTopPoint; }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() { return kitchenObject; }
    public void ClearKitchenObject() { this.SetKitchenObject(null); }
    public bool HasKitchenObject() { return this.kitchenObject != null; }
}
