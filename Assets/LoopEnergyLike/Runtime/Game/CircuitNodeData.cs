using UnityEngine;

[System.Serializable]
public struct CircuitNodeData
{
    [Tooltip("x is the row and y is the column")]
    [SerializeField] public Vector2Int rowAndColumn;
    [SerializeField] public SquareNode node;
    [SerializeField] public SquareNodeDirections expectedDirections;
    [SerializeField] public bool match;
}