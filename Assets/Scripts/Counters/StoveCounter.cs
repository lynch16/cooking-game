using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs: EventArgs {
        public State state;
    }

    public enum State {
        Idle,
        Frying,
        Fried,
        Burned
    }
    private State state;

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOs;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOs;

    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Update() {
        HandleCooking();
    }

    private void Start() {
        this.state = State.Idle;
    }

    private void HandleCooking() {
        if (HasKitchenObject()) {
            switch (state) {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.maxFryingTimer
                    });

                    if (fryingTimer > fryingRecipeSO.maxFryingTimer) {

                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.cookedKitchenObjectSO, this);

                        SetState(State.Fried);
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = burningTimer / burningRecipeSO.maxBurningTimer
                    });

                    if (burningTimer > burningRecipeSO.maxBurningTimer) {

                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.burnedKitchenObjectSO, this);

                        SetState(State.Burned);
                    }
                    break;
                case State.Burned:
                    break;
            }
        }

    }

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {

                KitchenObject kitchenObject = player.GetKitchenObject();

                if (IsFryableObjectSO(kitchenObject.GetKitchenObjectSO())) {
                    FryingRecipeSO fryingRecipeSO = GetFryingRecipeSO(kitchenObject.GetKitchenObjectSO());

                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    this.fryingRecipeSO = fryingRecipeSO;
                    SetState(State.Frying);
                    this.fryingTimer = 0f;
                }
            }
        } else {
            if (!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);

                SetState(State.Idle);

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0f
                });
            }
        }
    }

    private void SetState(State state) {
        this.state = state;

        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
            state = state
        });
    }

    
    private bool IsFryableObjectSO(KitchenObjectSO kitchenObject) {
        return !!GetFryingRecipeSO(kitchenObject);
    }

    private KitchenObjectSO GetCutKitchenObject(KitchenObjectSO rawKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSO(rawKitchenObjectSO);

        if (fryingRecipeSO != null) {
            return fryingRecipeSO.cookedKitchenObjectSO;
        }

        return null;
    }

    private FryingRecipeSO GetFryingRecipeSO(KitchenObjectSO rawKitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOs) {
            if (fryingRecipeSO.rawKitchenObjectSO == rawKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }

        return null;
    }

    private BurningRecipeSO GetBurningRecipeSO(KitchenObjectSO cookedKitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOs) {
            if (burningRecipeSO.cookedKitchenObjectSO == cookedKitchenObjectSO) {
                return burningRecipeSO;
            }
        }

        return null;
    }
}
