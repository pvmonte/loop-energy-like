using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SquareNode : MonoBehaviour
{
    [SerializeField] private bool isRoot;
    [SerializeField] private SquareNodeConnection currentUp;

    /// <summary>
    /// The sides of this node with connections
    /// </summary>
    [SerializeField] private SquareNodeConnection[] connections;

    /// <summary>
    /// Tells if this part is connected to the power source
    /// </summary>
    [field: SerializeField]
    public bool IsCharged { get; set; }

    /// <summary>
    /// Neighbor nodes starts on absolute up neighbor and follows clockwise
    /// </summary>
    private List<SquareNode> connectedNodes = new List<SquareNode>();

#if UNITY_EDITOR
    [SerializeField] private SquareNodeConnection lastUpSaved;

    private void OnValidate()
    {
        if (currentUp.Equals(lastUpSaved)) return;

        var diferece = currentUp - lastUpSaved;
        print(diferece);

        transform.rotation = Quaternion.Euler(0, 0, (int)currentUp);

        for (int i = 0; i < connections.Length; i++)
        {
            connections[i] += diferece;
                print($"connection {i} == {connections[i]}");

            if (connections[i] is not SquareNodeConnection.Up &&
                connections[i] is not SquareNodeConnection.Right &&
                connections[i] is not SquareNodeConnection.Down &&
                connections[i] is not SquareNodeConnection.Left )
            {
                connections[i] = SquareNodeConnection.Up;
            }
        }
        
        lastUpSaved = currentUp;
    }
#endif

    void Start()
    {
    }

    public void Spin(GridSlot gridSlot)
    {
        connectedNodes.Clear();
        currentUp = currentUp.GetNext();
        transform.rotation = Quaternion.Euler(0, 0, (int)currentUp);

        SpinConnections();

        for (int i = 0; i < connections.Length; i++)
        {
            switch (connections[i])
            {
                case SquareNodeConnection.Up:
                    if (!gridSlot.upNeighbor) break;
                    CheckConnection(connections[i], gridSlot.upNeighbor.node);
                    break;
                case SquareNodeConnection.Right:
                    if (!gridSlot.rightNeighbor) break;
                    CheckConnection(connections[i], gridSlot.rightNeighbor.node);
                    break;
                case SquareNodeConnection.Down:
                    if (!gridSlot.downNeighbor) break;
                    CheckConnection(connections[i], gridSlot.downNeighbor.node);
                    break;
                case SquareNodeConnection.Left:
                    if (!gridSlot.leftNeighbor) break;
                    CheckConnection(connections[i], gridSlot.leftNeighbor.node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (isRoot) return;
        CheckCharge();
    }

    public void CheckCharge()
    {
        IsCharged = connectedNodes.Count > 0 && connectedNodes.Any(x => x.IsCharged);
    }

    private void SpinConnections()
    {
        if (connections.Length == 0) return;

        for (int i = 0; i < connections.Length; i++)
        {
            connections[i] = connections[i].GetNext();
        }
    }

    private void CheckConnection(SquareNodeConnection nodeConnection, SquareNode node)
    {
        if (node.ContainsConnection(nodeConnection.GetOpposite()))
        {
            connectedNodes.Add(node);
        }
    }

    public bool ContainsConnection(SquareNodeConnection connection)
    {
        return connections.Contains(connection);
    }

    public SquareNodeConnection GetNodeRelativeConnection(int index)
    {
        return connections[index] + (int)currentUp;
    }

    public SquareNodeConnection[] GetRelativeConnections()
    {
        var relativeConnections = new SquareNodeConnection[connections.Length];

        for (int i = 0; i < relativeConnections.Length; i++)
        {
            relativeConnections[i] = GetNodeRelativeConnection(i);
        }

        return relativeConnections;
    }
}


public enum SquareNodeConnection
{
    Up = 0,
    Right = -90,
    Down = -180,
    Left = -270
}

public static class EnumExtensions
{
    public static SquareNodeConnection GetNext(this SquareNodeConnection currentValue)
    {
        switch (currentValue)
        {
            case SquareNodeConnection.Up:
                return SquareNodeConnection.Right;
            case SquareNodeConnection.Right:
                return SquareNodeConnection.Down;
            case SquareNodeConnection.Down:
                return SquareNodeConnection.Left;
            case SquareNodeConnection.Left:
                return SquareNodeConnection.Up;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentValue), currentValue, "No next value defined.");
        }
    }

    public static SquareNodeConnection GetOpposite(this SquareNodeConnection currentValue)
    {
        switch (currentValue)
        {
            case SquareNodeConnection.Up:
                return SquareNodeConnection.Down;
            case SquareNodeConnection.Right:
                return SquareNodeConnection.Left;
            case SquareNodeConnection.Down:
                return SquareNodeConnection.Up;
            case SquareNodeConnection.Left:
                return SquareNodeConnection.Right;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentValue), currentValue, "No next value defined.");
        }
    }
}