using TMPro;
using UnityEngine;

public class UIBodyPartName : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Start()
    {
        text.text = NavigationSystem.Instance.CurrentBodyPart.partName;
    }
}
