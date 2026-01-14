using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movespeed = 5f;
    [SerializeField] private float rotatespeed = 8f;
    [SerializeField] private GameInput gameInput;

    private bool isWalking;

    private void Update() 
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDir * movespeed * Time.deltaTime;

        isWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotatespeed);
    }

    public bool IsWalking() 
    {
        return isWalking;
    }
}
