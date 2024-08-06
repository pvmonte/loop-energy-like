using UnityEngine;

[System.Serializable]
public class CircuitData
{
    [field: SerializeField] public CircuitNodeData[] NodesData { get; private set; }
    public bool isClosed;
}