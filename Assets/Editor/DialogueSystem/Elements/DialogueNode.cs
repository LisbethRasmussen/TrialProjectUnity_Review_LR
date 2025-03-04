using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueNode : Node
{
    [field: SerializeField] public string ID { get; set; }
    [field: SerializeField] public string DialogueName { get; set; }
    [field: SerializeField] public string DialogueText { get; set; }
    [field: SerializeField] public List<string> Choices { get; set; }
    [field: SerializeField] public DialogueType Type { get; set; }

    private Color defaultBackgroundColor;

    public virtual void Initialize(Vector2 position)
    {
        ID = Guid.NewGuid().ToString();
        DialogueName = "DialogueName";
        Choices = new List<string>();
        DialogueText = "DialogueText";

        defaultBackgroundColor = new Color(29 / 255f, 29 / 255f, 30 / 255f);

        SetPosition(new Rect(position, Vector2.zero));

        mainContainer.AddToClassList("ds-node__main-container");
        extensionContainer.AddToClassList("ds-node__extension-container");
    }

    public virtual void Draw()
    {
        // Title container
        TextField dialogueNameTextField = DialogueElementUtility.CreateTextField(DialogueName, evt => {
            DialogueName = evt.newValue;
        });
        dialogueNameTextField.AddClasses(
            "ds-node__textfield",
            "ds-node__filename-textfield",
            "ds-node__textfield__hidden"
            );
        titleContainer.Insert(0, dialogueNameTextField);

        // Input container
        Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Dialogue Connection";
        inputContainer.Add(inputPort);


        // Extensions container

        VisualElement customDataContainer = new VisualElement();

        customDataContainer.AddToClassList("ds-node__custom-data-container");

        Foldout textFoldout = DialogueElementUtility.CreateFoldout("Dialogue Text");

        TextField textTextField = DialogueElementUtility.CreateTextArea(DialogueText);

        textTextField.AddClasses(
            "ds-node__textfield",
            "ds-node__quote-textfield"
            );

        textFoldout.Add(textTextField);
        customDataContainer.Add(textFoldout);
        extensionContainer.Add(customDataContainer);
    }

    public void SetErrorStyle(Color color)
    {
        mainContainer.style.backgroundColor = color;
    }

    public void ResetStyle()
    {
        mainContainer.style.backgroundColor = defaultBackgroundColor;
    }
}
