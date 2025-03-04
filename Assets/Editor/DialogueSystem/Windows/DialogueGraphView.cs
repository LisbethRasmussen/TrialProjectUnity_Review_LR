using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{

    private SerializableDictionary<string, DialogueNodeErrorData> ungroupedNodes;

    public DialogueGraphView()
    {
        ungroupedNodes = new SerializableDictionary<string, DialogueNodeErrorData>();

        AddManipulators();
        AddGridBackground();
        AddStyles();
    }

    #region Overrided Methods
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new();
        ports.ForEach((port) => {
            // Check if the port is not the same as the start port and if the port is not on the same node as the start port
            if (startPort != port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        });
        return compatiblePorts;
    }
    #endregion

    #region Manipulators
    private void AddManipulators()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DialogueType.SingleChoice));
        this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DialogueType.MultipleChoice));
        this.AddManipulator(CreateGroupContextualMenu());
    }
    #endregion

    #region Nodes Creation
    private IManipulator CreateNodeContextualMenu(string actionTitle, DialogueType dialogueType)
    {
        ContextualMenuManipulator contextualMenu = new ContextualMenuManipulator((evt) => {
            evt.menu.AppendAction(actionTitle, (e) => AddElement(CreateNode(dialogueType, e.eventInfo.localMousePosition)));
        });
        return contextualMenu;
    }
    private DialogueNode CreateNode(DialogueType dialogueType, Vector2 position)
    {
        Type nodeType = Type.GetType($"Dialogue{dialogueType}Node");
        DialogueNode node = (DialogueNode)Activator.CreateInstance(nodeType);
        node.Initialize(position);
        node.Draw();
        node.title = "Dialogue Node";

        AddUngroupedNode(node);

        return node;
    }
    #endregion

    #region Ungrouped Nodes

    private void AddUngroupedNode(DialogueNode node)
    {
        string nodeName = node.DialogueName;

        if (!ungroupedNodes.ContainsKey(nodeName))
        {
            // If the name does not exist, create a new list with the node
            DialogueNodeErrorData errorData = new DialogueNodeErrorData();
            errorData.Nodes.Add(node);
            ungroupedNodes.Add(nodeName, errorData);

            return;
        }

        // If the name already exists, add the node to the list of nodes with the same name
        // This is used to show the error message when there are nodes with the same name
        ungroupedNodes[nodeName].Nodes.Add(node);
        node.SetErrorStyle(ungroupedNodes[nodeName].ErrorData.Color);
        if (ungroupedNodes[nodeName].Nodes.Count > 1)
        {
            foreach (DialogueNode n in ungroupedNodes[nodeName].Nodes)
            {
                n.SetErrorStyle(ungroupedNodes[nodeName].ErrorData.Color);
            }
        }
    }

    #endregion

    #region Groups Creation
    private IManipulator CreateGroupContextualMenu()
    {
        ContextualMenuManipulator contextualMenu = new ContextualMenuManipulator((evt) => {
            evt.menu.AppendAction("Add Group", (e) => AddElement(CreateGroup("DialogueGroup", e.eventInfo.localMousePosition)));
        });
        return contextualMenu;
    }

    private Group CreateGroup(string title, Vector2 position)
    {
        Group group = new Group()
        {
            title = title
        };
        group.SetPosition(new Rect(position, Vector2.zero));
        AddElement(group);
        return group;
    }
    #endregion

    #region Styles and Background
    private void AddStyles()
    {
        this.AddStyleSheets("DialogueSystem/DialogueGraphViewStyles.uss", "DialogueSystem/DialogueNodeStyles.uss");
    }

    private void AddGridBackground()
    {
        GridBackground grid = new GridBackground();
        grid.StretchToParentSize();
        Insert(0, grid);
    }
    #endregion
}
