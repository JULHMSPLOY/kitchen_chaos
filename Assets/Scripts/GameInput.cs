using UnityEngine;
using UnityEngine.InputSystem;
using System; // ต้องมีเพื่อใช้ EventHandler

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    // สร้าง Event สำหรับส่งสัญญาณการกดปุ่ม Interact
    public event EventHandler OnInteractAction;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // ลงทะเบียน: เมื่อปุ่ม Interact ถูกกด (performed) ให้ไปเรียกฟังก์ชัน Interact_performed
        playerInputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        // ส่งสัญญาณ (Invoke) ออกไปให้สคริปต์ที่รอฟังอยู่ (เช่น Player.cs)
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}