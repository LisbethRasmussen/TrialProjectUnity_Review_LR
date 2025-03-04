using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueEditorWindow : EditorWindow
{
    private DialogueGraphView graphView;
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
        Toolbar toolbar = new();
        fileNameTextField = DialogueElementUtility.CreateTextField(defaultFileName, "File Name:", callback =>
        {
            fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
        });

        saveButton = DialogueElementUtility.CreateButton("Save", () =>
        {
            Save();
            Debug.Log("Save button clicked");
        });

        Button clearButton = DialogueElementUtility.CreateButton("Clear", () =>
        {
            graphView.ClearGraph();
            fileNameTextField.value = defaultFileName;
        });

        toolbar.Add(fileNameTextField);
        toolbar.Add(saveButton);
        toolbar.Add(clearButton);

        toolbar.AddStyleSheets("DialogueSystem/DialogueToolBarStyles.uss");
        rootVisualElement.Add(toolbar);
    }

    #region Toolbar Methods
    private void Save()
    {
        if (string.IsNullOrEmpty(fileNameTextField.value))
        {
            EditorUtility.DisplayDialog("Invalid file name", "Please enter a valid file name", "Ok");
            return;
        }

        DialogueIOUtility.Initialize(graphView, fileNameTextField.value);
        DialogueIOUtility.Save();
    }
    #endregion

    private void AddStyles()
    {
        rootVisualElement.AddStyleSheets("DialogueSystem/DialogueVariables.uss");
    }

    private void AddGraphView()
    {
        graphView = new DialogueGraphView(this);
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
