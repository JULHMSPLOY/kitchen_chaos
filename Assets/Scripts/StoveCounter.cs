using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class StoveCounter : MonoBehaviour, IKitchenObjectParent, IInteractable
{
    [SerializeField] private Transform counterTopPoint;

    [Header("Meat Objects")]
    [SerializeField] private KitchenObjectSO[] meatObjectSO;
    // 0 Raw
    // 1 Cooked
    // 2 Burned

    [Header("Egg Objects")]
    [SerializeField] private KitchenObjectSO[] eggObjectSO;
    // 0 Raw
    // 1 Fried
    // 2 Burned

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
            KitchenObjectSO currentSO = kitchenObject.GetKitchenObjectSO();

            // =========================
            // MEAT
            // =========================
            if (currentSO == meatObjectSO[0] && state == StoveState.Frying)
            {
                kitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(meatObjectSO[1], this);

                state = StoveState.Fried;

                animator?.SetBool("IsFlashing", true);
                warningUI?.gameObject.SetActive(true);
            }
            else if (currentSO == meatObjectSO[1] && state == StoveState.Fried)
            {
                kitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(meatObjectSO[2], this);

                state = StoveState.Burned;

                animator?.SetBool("IsFlashing", false);
                warningUI?.gameObject.SetActive(false);
            }

            // =========================
            // EGG
            // =========================
            else if (currentSO == eggObjectSO[0] && state == StoveState.Frying)
            {
                kitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(eggObjectSO[1], this);

                state = StoveState.Fried;

                animator?.SetBool("IsFlashing", true);
                warningUI?.gameObject.SetActive(true);
            }
            else if (currentSO == eggObjectSO[1] && state == StoveState.Fried)
            {
                kitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(eggObjectSO[2], this);

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
        // เตาว่าง → วางของลงทอด
        // =========================
        if (!HasKitchenObject())
        {
            if (!player.HasKitchenObject()) return;

            KitchenObjectSO playerSO = playerObject.GetKitchenObjectSO();

            if (playerSO == meatObjectSO[0] || playerSO == eggObjectSO[0])
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
        // Player มือว่าง → หยิบขึ้น
        // =========================
        if (!player.HasKitchenObject())
        {
            kitchenObject.SetKitchenObjectParent(player);

            ResetStove();

            return;
        }

        // =========================
        // ใส่ลง Plate
        // =========================
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