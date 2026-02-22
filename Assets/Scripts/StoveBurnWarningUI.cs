using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (stoveCounter == null) return;

        bool showWarning = stoveCounter.GetBurnProgress() >= 0.5f;

        gameObject.SetActive(showWarning);

        if (showWarning && canvasGroup != null)
        {
            float blinkSpeed = 8f;
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            canvasGroup.alpha = alpha;
        }
    }
}