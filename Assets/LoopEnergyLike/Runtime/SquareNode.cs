using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SquareNode : MonoBehaviour
{
    private GridSlot slot;
    [SerializeField] private bool isRoot;
    [SerializeField] private SquareNodeDirections currentUp;
    public SquareNodeDirections CurrentUp => currentUp;

    /// <summary>
    /// The sides of this node with connections
    /// </summary>
    [SerializeField] private SquareNodeDirections[] connections;

    /// <summary>
    /// Tells if this part is connected to the power source
    /// </summary>
    [field: SerializeField]
    public bool IsCharged { get; set; }

    /// <summary>
    /// Neighbor nodes starts on absolute up neighbor and follows clockwise
    /// </summary>
    public List<SquareNode> ConnectedNodes { get; private set; } = new List<SquareNode>();

    public event Action OnSpin;

#if UNITY_EDITOR
    [SerializeField] private SquareNodeDirections lastUpSaved;

    private void OnValidate()
    {
        if (currentUp.Equals(lastUpSaved)) return;

        var diferece = currentUp - lastUpSaved;
        print(diferece);

        transform.rotation = Quaternion.Euler(0, 0, (int)currentUp);

        for (int i = 0; i < connections.Length; i++)
        {
            connections[i] += diferece;
                print($"directions {i} == {connections[i]}");

            if (connections[i] is not SquareNodeDirections.Up &&
                connections[i] is not SquareNodeDirections.Right &&
                connections[i] is not SquareNodeDirections.Down &&
                connections[i] is not SquareNodeDirections.Left )
            {
                connections[i] = SquareNodeDirections.Up;
            }
        }
        
        lastUpSaved = currentUp;
    }
#endif

    public void Spin(GridSlot gridSlot)
    {
        ConnectedNodes.Clear();
        currentUp = currentUp.GetNext();
        transform.rotation = Quaternion.Euler(0, 0, (int)currentUp);

        SpinConnections();

        for (int i = 0; i < connections.Length; i++)
        {
            try
            {
                switch (connections[i])
                {
                    case SquareNodeDirections.Up:
                        CheckConnection(connections[i], gridSlot.upNeighbor.node);
                        break;
                    case SquareNodeDirections.Right:
                        CheckConnection(connections[i], gridSlot.rightNeighbor.node);
                        break;
                    case SquareNodeDirections.Down:
                        CheckConnection(connections[i], gridSlot.downNeighbor.node);
                        break;
                    case SquareNodeDirections.Left:
                        CheckConnection(connections[i], gridSlot.leftNeighbor.node);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        if (isRoot) return;
        CheckCharge();
        
        OnSpin?.Invoke();
    }

    public void CheckCharge()
    {
        IsCharged = ConnectedNodes.Count > 0 && ConnectedNodes.Any(x => x.IsCharged);
    }

    private void SpinConnections()
    {
        if (connections.Length == 0) return;

        for (int i = 0; i < connections.Length; i++)
        {
            connections[i] = connections[i].GetNext();
        }
    }

    private void CheckConnection(SquareNodeDirections nodeDirections, SquareNode node)
    {
        if (node.ContainsConnection(nodeDirections.GetOpposite()))
        {
            ConnectedNodes.Add(node);
        }
    }

    public bool ContainsConnection(SquareNodeDirections directions)
    {
        return connections.Contains(directions);
    }
}