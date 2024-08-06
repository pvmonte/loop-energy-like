using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CircuitController : MonoBehaviour
{
    [SerializeField] private SquareNode[] nodes;

    [SerializeField] private CircuitData circuitData;
    

    public event Action<CircuitController> OnClose; 
    public event Action<CircuitController> OnOpen; 
    
    public void Initialize(GridSlot[,] slots)
    {
        nodes = new SquareNode[circuitData.NodesData.Length];
        
        for (int i = 0; i < nodes.Length; i++)
        {
            var data = circuitData.NodesData[i];
            var rowAndColumn = data.rowAndColumn;
            var slot = slots[rowAndColumn.x, rowAndColumn.y];
            nodes[i] = Instantiate(data.node, slot.transform);
            slot.node = nodes[i];
            
            nodes[i].OnSpin += Node_OnSpin;
        }
    }

    private void Node_OnSpin()
    {
        CheckCircuit();
    }

    private void CheckCircuit()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            CheckNode(nodes[i], i);
        }

        bool circuitClosedCheck = circuitData.NodesData.All(data => data.match);

        if (circuitClosedCheck && !circuitData.isClosed)
        {
            circuitData.isClosed = true;
            print("Circuit closed");
            OnClose?.Invoke(this);
        }
        else if(!circuitClosedCheck && circuitData.isClosed)
        {
            circuitData.isClosed = false;
            print("Circuit opened");
            OnOpen?.Invoke(this);
        }
    }

    private void CheckNode(SquareNode node, int index)
    {
        circuitData.NodesData[index].match = node.CurrentUp == circuitData.NodesData[index].expectedDirections;
    }
}

[System.Serializable]
public class CircuitData
{
    [field: SerializeField] public CircuitNodeData[] NodesData { get; private set; }
    public bool isClosed;
}

[System.Serializable]
public struct CircuitNodeData
{
    [Tooltip("x is the row and y is the column")]
    [SerializeField] public Vector2Int rowAndColumn;
    [SerializeField] public SquareNode node;
    [SerializeField] public SquareNodeDirections expectedDirections;
    [SerializeField] public bool match;
}