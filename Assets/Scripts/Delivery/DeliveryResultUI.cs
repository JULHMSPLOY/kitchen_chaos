using UnityEngine;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private GameObject Background;
    [SerializeField] private GameObject Icon;

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += OnSuccess;
        DeliveryManager.Instance.OnRecipeFailed += OnFail;

        Hide();
    }

    private void OnSuccess(object sender, System.EventArgs e)
    {
        Background.SetActive(true);
        Invoke(nameof(Hide), 1f);
    }

    private void OnFail(object sender, System.EventArgs e)
    {
        Icon.SetActive(true);
        Invoke(nameof(Hide), 1f);
    }

    private void Hide()
    {
        Background.SetActive(false);
        Icon.SetActive(false);
    }
}
