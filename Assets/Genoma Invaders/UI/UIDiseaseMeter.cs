using UnityEngine;

public class UIDiseaseMeter : MonoBehaviour
{
    [SerializeField]
    private RectTransform progressRectTransform;

    private RectTransform meterRectTransform;
    private float maxWidth;
    private GameManager gameManager;

    private void Awake()
    {
        meterRectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        maxWidth = meterRectTransform.sizeDelta.x;
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        float newWidth = gameManager.DiseaseLevel * .01f * maxWidth;
        Vector2 newSize = new Vector2(newWidth, progressRectTransform.sizeDelta.y);

        progressRectTransform.sizeDelta = newSize;
    }
}
