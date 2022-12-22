using UnityEngine;

public class UITouchInputs : MonoBehaviour
{
    [SerializeField]
    private GameObject inputs;

    private void OnEnable()
    {
        GameManager.OnTouchUIEnabled += OnTouchUIEnabled;
        GameManager.OnTouchUIDisabled += OnTouchUIDisabled;
    }

    private void Start()
    {
        if (GameManager.Instance.IsTouchUIEnabled)
        {
            //Debug.Log("Showing Touch Input!");
            inputs.SetActive(true);
        }
        else
        {
            //Debug.Log("Hiding Touch Input!");
            inputs.SetActive(false);
        }
    }

    private void OnTouchUIEnabled()
    {
        inputs.SetActive(true);
    }

    private void OnTouchUIDisabled()
    {
        inputs.SetActive(false);
    }
}
