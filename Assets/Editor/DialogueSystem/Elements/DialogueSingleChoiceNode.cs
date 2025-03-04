using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogueSingleChoiceNode : DialogueNode
{
    public override void Initialize(DialogueGraphView graphView, Vector2 position)
    {
        base.Initialize(graphView, position);

        Type = DialogueType.SingleChoice;

        DialogueChoiceSaveData choiceData = new()
        {
            Text = "Next Dialogue",
        };

        Choices.Add(choiceData);
    }

    public override void Draw()
    {
        base.Draw();

        foreach (DialogueChoiceSaveData choice in Choices)
        {
            Port choicePort = this.CreatePort(choice.Text);
            choicePort.userData = choice;
            outputContainer.Add(choicePort);
        }

        RefreshExpandedState();
    }

}
