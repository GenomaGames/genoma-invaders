using UnityEngine;

public class UIDiseaseMeter : MonoBehaviour
{
    [SerializeField]
    private RectTransform progressRectTransform;

    private RectTransform meterRectTransform;
    private float maxWidth;

    private void Awake()
    {
        meterRectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        maxWidth = meterRectTransform.sizeDelta.x;

        UpdateLevel(DiseaseManager.Instance.DiseaseLevel);
    }

    private void OnEnable()
    {
        DiseaseManager.Instance.OnLevelUpdated += UpdateLevel;
    }

    private void OnDisable()
    {
        DiseaseManager.Instance.OnLevelUpdated += UpdateLevel;
    }

    private void UpdateLevel(float level)
    {
        float newWidth = level * .01f * maxWidth;
        Vector2 newSize = new Vector2(newWidth, progressRectTransform.sizeDelta.y);

        progressRectTransform.sizeDelta = newSize;
    }
}
