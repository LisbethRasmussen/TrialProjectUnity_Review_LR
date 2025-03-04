using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueMultipleChoiceNode : DialogueNode
{
    public override void Initialize(DialogueGraphView graphView, Vector2 position)
    {
        base.Initialize(graphView, position);
        Type = DialogueType.MultipleChoice;
        Choices.Add("New Choice");
    }

    public override void Draw()
    {
        base.Draw();

        // Main Container
        Button addChoiceButton = DialogueElementUtility.CreateButton("Add Choice", () => {
            Port choicePort = CreateChoicePort("New Choice");
            Choices.Add("New Choice");
            outputContainer.Add(choicePort);
        });
        addChoiceButton.AddToClassList("ds-node__button");
        mainContainer.Insert(1, addChoiceButton);

        // Output Container
        foreach (string choice in Choices)
        {
            Port choicePort = CreateChoicePort(choice);
            outputContainer.Add(choicePort);
        }
        RefreshExpandedState();
    }

    private Port CreateChoicePort(string choice)
    {
        Port choicePort = this.CreatePort(choice);

        Button deleteButton = DialogueElementUtility.CreateButton("X", () => {
            Choices.Remove("New Choice");
            choicePort.RemoveFromHierarchy();
        });

        deleteButton.AddToClassList("ds-node__button");

        TextField choiceTextField = DialogueElementUtility.CreateTextField(choice);

        choiceTextField.AddClasses(
            "ds-node__textfield",
            "ds-node__choice-textfield",
            "ds-node__textfield__hidden"
            );

        choicePort.Add(choiceTextField);
        choicePort.Add(deleteButton);

        outputContainer.Add(choicePort);

        return choicePort;
    }
}
