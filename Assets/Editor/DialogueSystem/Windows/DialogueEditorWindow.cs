using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueEditorWindow : EditorWindow
{
    private readonly string defaultFileName = "DialogueFileName";
    private TextField fileNameTextField;
    private Button saveButton;

    [MenuItem("Window/DialogueEditorWindow")]
    public static void ShowExample()
    {
        DialogueEditorWindow wnd = GetWindow<DialogueEditorWindow>("DialogueEditorWindow");
        //wnd.titleContent = new GUIContent();
    }

    private void OnEnable()
    {
        AddGraphView();
        AddToolBar();
        AddStyles();
    }

    private void AddToolBar()
    {
        Toolbar toolbar = new Toolbar();
        fileNameTextField = DialogueElementUtility.CreateTextField(defaultFileName, "File Name:", callback =>
        {
            fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
        });

        saveButton = DialogueElementUtility.CreateButton("Save", () =>
        {
            Debug.Log("Save button clicked");
        });

        toolbar.Add(fileNameTextField);
        toolbar.Add(saveButton);
        toolbar.AddStyleSheets("DialogueSystem/DialogueToolBarStyles.uss");
        rootVisualElement.Add(toolbar);
    }

    private void AddStyles()
    {
        rootVisualElement.AddStyleSheets("DialogueSystem/DialogueVariables.uss");
    }

    private void AddGraphView()
    {
        DialogueGraphView graphView = new DialogueGraphView(this);
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    public void EnableSaving()
    {
        saveButton.SetEnabled(true);
    }

    public void DisableSaving()
    {
        saveButton.SetEnabled(false);
    }
}
