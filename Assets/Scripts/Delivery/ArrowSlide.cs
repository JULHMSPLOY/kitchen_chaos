using UnityEngine;

public class ArrowSlide : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveDistance = 2.2f;
    [SerializeField] private float moveDuration = 0.9f;

    [Header("Scale")]
    [SerializeField] private float minScale = 0.4f;
    [SerializeField] private float maxScale = 1.3f;
    [SerializeField] private float scaleDuration = 0.35f;

    private Vector3 startLocalPos;
    private float timer;
    private bool isScaling = true;

    private void Start()
    {
        startLocalPos = transform.localPosition;
        ResetArrow();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (isScaling)
        {
            float t = Mathf.Clamp01(timer / scaleDuration);
            float smooth = Mathf.SmoothStep(0f, 1f, t);

            float scale = Mathf.Lerp(minScale, maxScale, smooth);
            transform.localScale = Vector3.one * scale;

            if (t >= 1f)
            {
                isScaling = false;
                timer = 0f;
            }
        }
        else
        {
            float t = Mathf.Clamp01(timer / moveDuration);
            float smooth = Mathf.SmoothStep(0f, 1f, t);

            float offset = Mathf.Lerp(-moveDistance, moveDistance, smooth);
            transform.localPosition = startLocalPos + new Vector3(0, 0, -offset);

            if (t >= 1f)
            {
                ResetArrow();
            }
        }
    }

    private void ResetArrow()
    {
        timer = 0f;
        isScaling = true;

        transform.localScale = Vector3.one * minScale;
        transform.localPosition = startLocalPos + new Vector3(0, 0, moveDistance);
    }
}