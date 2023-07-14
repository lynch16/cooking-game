using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{

    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs: EventArgs {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private float playerHeight = 2f;
    private float playerRadius = .7f;
    private float interactDistance = 2f;

    private Vector3 lastMoveDirection;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one Player Instance");
        }
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void HandleInteraction() {
        Vector3 moveDirection = GetMoveDirection();

        if (moveDirection != Vector3.zero) {
            lastMoveDirection = moveDirection;
        }

        if (Physics.Raycast(transform.position, lastMoveDirection, out RaycastHit raycastHit, interactDistance)) {
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }
    }

    private Vector3 GetMoveDirection() {
        Vector2 inputVector = gameInput.getMovementVectorNormalized();

        return new Vector3(inputVector.x, 0f, inputVector.y);
    }

    private void HandleMovement() {
        Vector3 heightAtPosition = transform.position + (Vector3.up * playerHeight);
        Vector3 moveDirection = GetMoveDirection();
        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, heightAtPosition, playerRadius, moveDirection, moveDistance);

        if (!canMove) {
            Vector3 moveX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, heightAtPosition, playerRadius, moveX, moveDistance);

            if (canMove) {
                moveDirection = moveX;
            }
        }
        if (!canMove) {
            Vector3 moveZ = new Vector3(0, 0, moveDirection.z).normalized;
            canMove = !Physics.CapsuleCast(transform.position, heightAtPosition, playerRadius, moveZ, moveDistance);

            if (canMove) {
                moveDirection = moveZ;
            }
        }

        if (canMove) {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        isWalking = moveDirection != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void SetSelectedCounter(BaseCounter baseCounter) {
        selectedCounter = baseCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = baseCounter
        });
    }

    public Transform GetKitchenObjectHoldPoint() {
        return this.kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() {
        return this.kitchenObject;
    }

    public void ClearKitchenObject() {
        this.SetKitchenObject(null);
    }

    public bool HasKitchenObject() {
        return this.kitchenObject != null;
    }
}
