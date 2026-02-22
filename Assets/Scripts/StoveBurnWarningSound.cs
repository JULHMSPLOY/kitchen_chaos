using UnityEngine;

public class StoveBurnWarningSound : MonoBehaviour 
{
    [SerializeField] private StoveCounter stoveCounter;

    private float warningSoundTimer;
    [SerializeField] private float warningSoundTimerMax = 0.25f; 

    private void Update() 
    {
        if (stoveCounter == null) return;

        bool isBurnWarning = 
            stoveCounter.IsFried();

        if (isBurnWarning) 
        {
            float progress = stoveCounter.GetBurnProgress();
            if (progress < 0.3f)
                warningSoundTimerMax = 0.45f;
            else if (progress < 0.7f)
                warningSoundTimerMax = 0.25f;
            else
                warningSoundTimerMax = 0.15f;

            warningSoundTimer -= Time.deltaTime;

            if (warningSoundTimer <= 0f) 
            {
                warningSoundTimer = warningSoundTimerMax;

                if (AudioManager.Instance == null) return;

                AudioClipRefsSO audioClipRefs = 
                    AudioManager.Instance.GetAudioClipRefsSO();

                AudioManager.Instance.PlaySound(
                    AudioManager.Instance
                        .GetAudioClipRefsSO()
                        .stoveBurnWarning,
                    transform.position
                );
            }
        }
        else
        {
            // รีเซ็ตให้พร้อมเริ่มนับใหม่แบบไม่เล่นทันที
            warningSoundTimer = 0;
        }
    }
}