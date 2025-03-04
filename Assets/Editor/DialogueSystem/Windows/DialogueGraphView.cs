using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{

    private SerializableDictionary<string, DialogueNodeErrorData> ungroupedNodes;
    private SerializableDictionary<string, DialogueGroupErrorData> groups;
    private SerializableDictionary<Group, SerializableDictionary<string, DialogueNodeErrorData>> groupedNodes;

    public DialogueGraphView()
    {
        ungroupedNodes = new SerializableDictionary<string, DialogueNodeErrorData>();
        groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, DialogueNodeErrorData>>();
        groups = new SerializableDictionary<string, DialogueGroupErrorData>();

        AddManipulators();
        AddGridBackground();

        OnElementsDeleted();
        OnGroupElementsAdded();
        OnGroupElementsRemoved();
        OnGroupRenamed();

        AddStyles();
    }

    #region Overrided Methods
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new();
        ports.ForEach((port) =>
        {
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
        ContextualMenuManipulator contextualMenu = new ContextualMenuManipulator((evt) =>
        {
            evt.menu.AppendAction(actionTitle, (e) => AddElement(CreateNode(dialogueType, e.eventInfo.localMousePosition)));
        });
        return contextualMenu;
    }

    private DialogueNode CreateNode(DialogueType dialogueType, Vector2 position)
    {
        Type nodeType = Type.GetType($"Dialogue{dialogueType}Node");
        DialogueNode node = (DialogueNode)Activator.CreateInstance(nodeType);
        node.Initialize(this, position);
        node.Draw();
        node.title = "Dialogue Node";

        AddUngroupedNode(node);

        return node;
    }

    public void AddGroupedNode(DialogueNode node, DialogueGroup group)
    {
        string nodeName = node.DialogueName;
        node.Group = group;

        if (!groupedNodes.ContainsKey(group))
        {
            groupedNodes.Add(group, new SerializableDictionary<string, DialogueNodeErrorData>());
        }

        if (!groupedNodes[group].ContainsKey(nodeName))
        {
            DialogueNodeErrorData nodeErrorData = new();
            nodeErrorData.Nodes.Add(node);
            groupedNodes[group].Add(nodeName, nodeErrorData);
            node.ResetStyle();
            return;
        }

        List<DialogueNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;
        groupedNodesList.Add(node);
        Color errorColor = groupedNodes[group][nodeName].ErrorData.Color;
        node.SetErrorStyle(errorColor);
        if (groupedNodesList.Count > 1)
        {
            foreach (DialogueNode n in groupedNodes[group][nodeName].Nodes)
            {
                n.SetErrorStyle(errorColor);
            }
        }
        else
        {
            node.ResetStyle();
        }
    }

    public void RemoveGroupedNode(DialogueNode node, Group group)
    {
        string nodeName = node.DialogueName;
        node.Group = null;
        List<DialogueNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;
        groupedNodesList.Remove(node);

        node.ResetStyle();

        if (groupedNodesList.Count == 0)
        {
            // Node was not in an error state (no other nodes with the same name)
            groupedNodes[group].Remove(nodeName);

            // Remove the group if there are no nodes in it
            if (groupedNodes[group].Count == 0)
            {
                groupedNodes.Remove(group);
            }
        }
        else if (groupedNodesList.Count == 1)
        {
            // Reset the style of the remaining node if there is only one node with the same name
            groupedNodesList[0].ResetStyle();
        }
    }

    #endregion

    #region Ungrouped Nodes

    public void AddUngroupedNode(DialogueNode node)
    {
        string nodeName = node.DialogueName;

        if (!ungroupedNodes.ContainsKey(nodeName))
        {
            // If the name does not exist, create a new list with the node
            DialogueNodeErrorData errorData = new DialogueNodeErrorData();
            errorData.Nodes.Add(node);
            ungroupedNodes.Add(nodeName, errorData);
            node.ResetStyle();
            return;
        }

        List<DialogueNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

        // If the name already exists, add the node to the list of nodes with the same name
        // This is used to show the error message when there are nodes with the same name
        ungroupedNodesList.Add(node);
        node.SetErrorStyle(ungroupedNodes[nodeName].ErrorData.Color);
        if (ungroupedNodes[nodeName].Nodes.Count > 1)
        {
            foreach (DialogueNode n in ungroupedNodes[nodeName].Nodes)
            {
                n.SetErrorStyle(ungroupedNodes[nodeName].ErrorData.Color);
            }
        }
        else
        {
            node.ResetStyle();
        }
    }

    public void RemoveUngroupedNode(DialogueNode node)
    {
        string nodeName = node.DialogueName;

        if (!ungroupedNodes.ContainsKey(nodeName))
        {
            Debug.LogError("Tried to remove a non-existing node.");
            return;
        }

        List<DialogueNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;
        ungroupedNodesList.Remove(node);
        if (ungroupedNodesList.Count == 0)
        {
            ungroupedNodes.Remove(nodeName);
        }
        else if (ungroupedNodesList.Count == 1)
        {
            ungroupedNodesList[0].ResetStyle();
        }
    }

    #endregion

    #region Callbacks

    private void OnElementsDeleted()
    {
        deleteSelection = (operationName, askUser) =>
        {
            Type groupType = typeof(DialogueGroup);
            List<DialogueNode> nodesToDelete = new();
            List<DialogueGroup> groupsToDelete = new();
            foreach (ISelectable selectedElement in selection)
            {
                if (selectedElement is DialogueNode node)
                {
                    nodesToDelete.Add(node);
                    continue;
                }

                if (selectedElement.GetType() == groupType)
                {
                    DialogueGroup group = (DialogueGroup)selectedElement;
                    groupsToDelete.Add(group);
                }
            }

            foreach (DialogueGroup group in groupsToDelete)
            {
                RemoveGroup(group);
                RemoveElement(group);
            }

            foreach (DialogueNode node in nodesToDelete)
            {
                // Remove the node from the group if it is in one
                node.Group?.RemoveElement(node);

                RemoveUngroupedNode(node);

                RemoveElement(node);
            }
        };
    }

    private void OnGroupElementsAdded()
    {
        elementsAddedToGroup = (group, elements) =>
        {
            foreach (GraphElement element in elements)
            {
                if (element is not DialogueNode)
                {
                    continue;
                }

                DialogueGroup nodeGroup = (DialogueGroup)group;
                DialogueNode node = (DialogueNode)element;

                RemoveUngroupedNode(node);
                AddGroupedNode(node, nodeGroup);
            }
        };
    }

    private void OnGroupElementsRemoved()
    {
        elementsRemovedFromGroup = (group, elements) =>
        {
            foreach (GraphElement element in elements)
            {
                if (element is not DialogueNode)
                {
                    continue;
                }

                DialogueNode node = (DialogueNode)element;
                RemoveGroupedNode(node, group);
                AddUngroupedNode(node);
            }
        };
    }

    private void OnGroupRenamed()
    {
        groupTitleChanged = (group, newTitle) =>
        {
            DialogueGroup dialogueGroup = (DialogueGroup)group;
            RemoveGroup(dialogueGroup);
            dialogueGroup.oldTitle = newTitle;
            AddGroup(dialogueGroup);
        };
    }

    #endregion

    #region Groups
    private IManipulator CreateGroupContextualMenu()
    {
        ContextualMenuManipulator contextualMenu = new ContextualMenuManipulator((evt) =>
        {
            evt.menu.AppendAction("Add Group", (e) => AddElement(CreateGroup("DialogueGroup", e.eventInfo.localMousePosition)));
        });
        return contextualMenu;
    }

    private DialogueGroup CreateGroup(string title, Vector2 position)
    {
        DialogueGroup group = new DialogueGroup(title, position);
        AddGroup(group);
        return group;
    }

    private void AddGroup(DialogueGroup group)
    {
        string groupName = group.title;
        if (!groups.ContainsKey(groupName))
        {
            DialogueGroupErrorData groupErrorData = new();
            groupErrorData.Groups.Add(group);
            groups.Add(groupName, groupErrorData);
            return;
        }

        List<DialogueGroup> groupList = groups[groupName].Groups;
        groupList.Add(group);
        Color errorColor = groups[groupName].ErrorData.Color;
        group.SetErrorStyle(errorColor);

        if (groupList.Count > 1)
        {
            foreach (DialogueGroup g in groupList)
            {
                g.SetErrorStyle(errorColor);
            }
        }
        else
        {
            group.ResetStyle();
        }
    }

    private void RemoveGroup(DialogueGroup group)
    {
        string oldGroupName = group.oldTitle;
        List<DialogueGroup> groupsList = groups[oldGroupName].Groups;
        groupsList.Remove(group);
        group.ResetStyle();

        if (groupsList.Count == 1)
        {
            groupsList[0].ResetStyle();
            return;
        }

        if (groupsList.Count == 0)
        {
            groups.Remove(oldGroupName);
        }
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
