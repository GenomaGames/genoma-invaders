using UnityEngine;
using UnityEngine.UI;

public class UIBodyPartMarker : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private BodyPart bodyPart;

    private Button button;
    private Animator animator;

    private readonly int animatorActivatedParam = Animator.StringToHash("Activated");

    private void Awake()
    {
        button = GetComponent<Button>();
        animator = GetComponent<Animator>();

        button.onClick.AddListener(LoadLevel);
    }

    private void Start()
    {
        if (levelManager.CurrentBodyPart == bodyPart)
        {
            button.interactable = false;

            animator.SetTrigger(animatorActivatedParam);
        }
    }

    private void LoadLevel()
    {
        SceneLoader.LoadScene("Level_Circulatory");
    }
}
