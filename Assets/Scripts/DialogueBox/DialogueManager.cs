using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _mainText;
    [SerializeField] private InputActionReference _continueAction;

    private void OnEnable()
    {
        _continueAction.action.Enable();
        _continueAction.action.performed += ContinueAction_performed;
    }

    private void OnDisable()
    {
        _continueAction.action.Disable();
        _continueAction.action.performed -= ContinueAction_performed;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gameObject.SetActive(false);
    }

    public void StartDialogue(Story story)
    {
        gameObject.SetActive(true);
        StartCoroutine(GoThroughStory(story));
    }

    public void EndDialogue()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator GoThroughStory(Story story)
    {
        StoryNode currentNode = story.GetCurrentNode();

        while (currentNode != null)
        {
            _titleText.text = currentNode.GetTitle();
            _mainText.text = currentNode.getText();

            yield return new WaitUntil(() => _continueAction.action.triggered);

            story.NextNode();
            currentNode = story.GetCurrentNode();
        }

    }

    private void ContinueAction_performed(InputAction.CallbackContext obj)
    {
        // Continue to the next node
    }
}
