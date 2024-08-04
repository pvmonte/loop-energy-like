using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CircuitController : MonoBehaviour
{
    [SerializeField] private GridSlot[] slots;
    [SerializeField] private SquareNode[] nodes;

    [SerializeField] private CircuitNodeData[] nodeDatas;
    private bool isClosed;

    public event Action<CircuitController> OnClose; 
    public event Action<CircuitController> OnOpen; 
    
    void Start()
    {
        nodes = new SquareNode[slots.Length];
        
        for (int i = 0; i < slots.Length; i++)
        {
            nodes[i] = slots[i].node;
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

        bool circuitClosedCheck = nodeDatas.All(data => data.match);

        if (circuitClosedCheck && !isClosed)
        {
            isClosed = true;
            print("Circuit closed");
            OnClose?.Invoke(this);
        }
        else if(!circuitClosedCheck && isClosed)
        {
            isClosed = false;
            print("Circuit opened");
            OnOpen?.Invoke(this);
        }
    }

    private void CheckNode(SquareNode node, int index)
    {
        nodeDatas[index].match = node.CurrentUp == nodeDatas[index].expectedDirections;
    }
}

[System.Serializable]
public struct CircuitNodeData
{
    [SerializeField] public SquareNodeDirections expectedDirections;
    [SerializeField] public bool match;
}