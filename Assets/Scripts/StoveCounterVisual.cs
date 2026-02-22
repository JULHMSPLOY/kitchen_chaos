using UnityEngine;

public class StoveCounterVisual : MonoBehaviour {

    [SerializeField] private StoveCounter stoveCounter; // อ้างอิงไปยัง Script เตาหลัก
    [SerializeField] private GameObject stoveOnVisual;  // Object ไฟเตา (StoveOnVisual)
    [SerializeField] private GameObject sizzlingParticles; // Object ควัน (SizzlingParticles)

    private void Start() {
        // รอรับฟัง Event เมื่อสถานะของเตาเปลี่ยน (ถ้าคุณเขียนระบบ State Event ไว้)
        // หรือใช้วิธีเช็คใน Update ง่ายๆ ดังนี้:
    }

    private void Update() {
        // ตรวจสอบสถานะจาก StoveCounter
        bool isFrying = stoveCounter.IsFried() || stoveCounter.IsFrying(); // เพิ่มฟังก์ชันเช็คสถานะใน StoveCounter

        // ควบคุมการแสดงผล Visual
        stoveOnVisual.SetActive(isFrying);
        sizzlingParticles.SetActive(isFrying);
    }
}
