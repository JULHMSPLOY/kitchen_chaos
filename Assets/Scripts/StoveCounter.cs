using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class StoveCounter : MonoBehaviour, IKitchenObjectParent, IInteractable
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private KitchenObjectSO[] meatObjectOS;
    [SerializeField] private KitchenObjectSO plateSO;

    public float fryingSpeed = 1f;
    public float fryingMax = 100f;

    private float fryingProcess;

    private Animator animator;
    private FryingBar processBar;
    private Transform warningUI;
    private AudioSource sizzlingAudioSource;

    private KitchenObject kitchenObject;
    private StoveState state = StoveState.Idle;

    private enum StoveState
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        processBar = GetComponentInChildren<FryingBar>();
        warningUI = transform.Find("StoveBurnWarningUI");

        sizzlingAudioSource = gameObject.AddComponent<AudioSource>();
        sizzlingAudioSource.loop = true;
        sizzlingAudioSource.spatialBlend = 1f;
        sizzlingAudioSource.maxDistance = 20f;
    }

    private void Start()
    {
        sizzlingAudioSource.outputAudioMixerGroup =
            AudioManager.Instance.GetMixerGroup("SFX");

        sizzlingAudioSource.clip =
            AudioManager.Instance.GetAudioClipRefsSO().sizzling[0];
    }

    private void Update()
    {
        if (!HasKitchenObject()) return;
        if (state != StoveState.Frying && state != StoveState.Fried) return;

        fryingProcess += fryingSpeed * Time.deltaTime;

        processBar?.FryingCounter_OnProcessChanged(fryingProcess / fryingMax);

        if (fryingProcess >= fryingMax)
        {
            kitchenObject.DestroySelf();

            if (state == StoveState.Frying)
            {
                KitchenObject.SpawnKitchenObject(meatObjectOS[1], this);

                sizzlingAudioSource.Stop();
                state = StoveState.Fried;
                fryingProcess = 0f;

                animator?.SetBool("IsFlashing", true);
                warningUI?.gameObject.SetActive(true);
            }
            else if (state == StoveState.Fried)
            {
                KitchenObject.SpawnKitchenObject(meatObjectOS[2], this);
                state = StoveState.Burned;

                animator?.SetBool("IsFlashing", false);
                warningUI?.gameObject.SetActive(false);
            }
            fryingProcess = 0f;
            processBar?.FryingCounter_OnProcessChanged(0f);
        }
    }

    public void Interact(Player player)
    {
        KitchenObject playerObject = null;

        if (player.HasKitchenObject())
            playerObject = player.GetKitchenObject();

        // =========================
        // 🔹 เตาว่าง → วางเนื้อดิบ
        // =========================
        if (!HasKitchenObject())
        {
            if (!player.HasKitchenObject()) return;

            if (playerObject.GetKitchenObjectSO() == meatObjectOS[0])
            {
                playerObject.SetKitchenObjectParent(this);

                fryingProcess = 0f;
                state = StoveState.Frying;
                sizzlingAudioSource.Play();

                processBar?.FryingCounter_OnProcessChanged(0f);
            }
            return;
        }

        // =========================
        // 🔹 เตามีของอยู่
        // =========================

        // 🔹 กรณี Player มือว่าง → หยิบขึ้นมา
        if (!player.HasKitchenObject())
        {
            kitchenObject.SetKitchenObjectParent(player);
            ResetStove();
            return;
        }

        // 🔹 กรณี Player ถือ Plate → ใส่ลงจาน
        if (playerObject.GetKitchenObjectSO() == plateSO)
        {
            PlateKitchenObject plate =
                playerObject.GetComponent<PlateKitchenObject>();

            if (plate != null &&
                plate.TryAddIngredient(kitchenObject.GetKitchenObjectSO()))
            {
                kitchenObject.DestroySelf();
                ResetStove();
            }
        }
    }

    private void ResetStove()
    {
        sizzlingAudioSource.Stop();
        state = StoveState.Idle;
        fryingProcess = 0f;

        if (processBar != null)
            processBar.FryingCounter_OnProcessChanged(0f);

        if (animator != null)
            animator.SetBool("IsFlashing", false);

        if (warningUI != null)
            warningUI.gameObject.SetActive(false);
    }

    // =========================
    // IKitchenObjectParent
    // =========================

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
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

    public bool IsFrying()
    {
        return state == StoveState.Frying;
    }

    public bool IsFried()
    {
        return state == StoveState.Fried;
    }

    public float GetBurnProgress()
    {
        if (state == StoveState.Fried)
        {
            return fryingProcess / fryingMax;
        }

        return 0f;
    }
}