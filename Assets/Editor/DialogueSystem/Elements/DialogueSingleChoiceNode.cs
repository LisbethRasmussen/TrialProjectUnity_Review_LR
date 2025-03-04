using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogueSingleChoiceNode : DialogueNode
{
    public override void Initialize(Vector2 position)
    {
        base.Initialize(position);

        Type = DialogueType.SingleChoice;

        Choices.Add("Next Dialogue");
    }

    public override void Draw()
    {
        base.Draw();

        foreach (string choice in Choices)
        {
            Port choicePort = this.CreatePort(choice);
            choicePort.portName = choice;
            outputContainer.Add(choicePort);
        }

        RefreshExpandedState();
    }

}
