using TMPro;
using UnityEngine;

public class UIVersion : MonoBehaviour
{
    private TMP_Text versionText;

    private void Awake()
    {
        versionText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        string version = Application.version;

        versionText.text = $"v{version}";
    }
}
