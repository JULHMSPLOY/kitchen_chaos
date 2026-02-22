using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private float movespeed = 5f;
    [SerializeField] private float rotatespeed = 8f;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float footstepInterval = 0.4f;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool isWalking = false;
    private KitchenObject kitchenObject;
    private BaseCounter selectedCounter;
    private IInteractable currentInteractable;
    private bool canInteract = true;
    private float footstepTimer;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
        transform.position += moveDir * movespeed * Time.deltaTime;

        isWalking = moveDir != Vector3.zero;

        if (isWalking)
        {
            transform.forward = Vector3.Slerp(
                transform.forward,
                moveDir,
                Time.deltaTime * rotatespeed
            );

            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                AudioManager.Instance.PlaySound(
                    AudioManager.Instance.GetAudioClipRefsSO().footstep,
                    transform.position,
                    0.6f
                );

                footstepTimer = 0f;
            }
        }
    }

   private void OnCollisionEnter(Collision collision)
    {
        if (!canInteract) return;

        IInteractable interactable =
            collision.collider.GetComponentInParent<IInteractable>();

        if (interactable != null)
        {
            canInteract = false;
            interactable.Interact(this);
            Invoke(nameof(ResetInteract), 0.2f);
        }
    }

    private void ResetInteract()
    {
        canInteract = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        IInteractable interactable =
            collision.collider.GetComponentInParent<IInteractable>();

        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
        }
    }

    // ===== IKitchenObjectParent =====

    public Transform GetKitchenObjectFollowTransform()
    {
        return holdPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}