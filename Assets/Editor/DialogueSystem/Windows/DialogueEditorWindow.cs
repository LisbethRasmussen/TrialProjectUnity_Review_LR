using UnityEditor;
using UnityEngine.UIElements;

public class DialogueEditorWindow : EditorWindow
{
    [MenuItem("Window/DialogueEditorWindow")]
    public static void ShowExample()
    {
        DialogueEditorWindow wnd = GetWindow<DialogueEditorWindow>("DialogueEditorWindow");
        //wnd.titleContent = new GUIContent();
    }

    private void OnEnable()
    {
        AddGraphView();
        AddStyles();
    }

    private void AddStyles()
    {
        rootVisualElement.AddStyleSheets("DialogueSystem/DialogueVariables.uss");
    }

    private void AddGraphView()
    {
        DialogueGraphView graphView = new DialogueGraphView();
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }
}
