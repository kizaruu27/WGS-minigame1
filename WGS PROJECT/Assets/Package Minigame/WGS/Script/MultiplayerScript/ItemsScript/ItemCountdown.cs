using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCountdown : MonoBehaviour
{

    [Header("Item References")]
    [SerializeField] Sprite[] imageTypes;

    [Header("Item Game Object")]
    [SerializeField] Image type;
    [SerializeField] TextMeshProUGUI CoundownTime;

    float localTimer;
    [HideInInspector]
    public float time
    {
        private get => localTimer;
        set => localTimer = value;
    }
    [HideInInspector] public bool isItemSpeed { get; set; }

    float deltaTime;
    float countdown;


    private void Update()
    {
        CountDownStart();
        SetItemType();
    }

    public void CountDownStart()
    {
        deltaTime += Time.deltaTime;

        countdown = localTimer - deltaTime;
        Debug.Log(localTimer - deltaTime);
        CoundownTime.text = countdown > 0f ? countdown.ToString("n2") : "";

        if (countdown <= 0f)
        {
            deltaTime = 0f;
            enabled = false;
        }
    }

    void SetItemType()
    {
        type.sprite = imageTypes[isItemSpeed ? 0 : 1];
    }
}