using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        if (Camera.main == null) return;

        Vector3 dir = transform.position - Camera.main.transform.position;
        dir.y = 0f; // ไม่ให้เอียงตามกล้องมากเกิน

        transform.forward = dir;

        // เพิ่มองศาเงยขึ้นเล็กน้อย
        transform.Rotate(15f, 0f, 0f);
    }
}