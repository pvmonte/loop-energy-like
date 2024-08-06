using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GridSlot : MonoBehaviour
{
    [field: SerializeField] public SquareNode node { get; set; }
    [field: SerializeField] public GridSlot upNeighbor { get; private set; }
    [field: SerializeField] public GridSlot rightNeighbor { get; private set; }
    [field: SerializeField] public GridSlot downNeighbor { get; private set; }
    [field: SerializeField] public GridSlot leftNeighbor { get; private set; }

    public event Action<GridSlot> OnTap;

    private void OnMouseDown()
    {
        if (node is null) return;

        node.Spin(this);
        OnTap?.Invoke(this);
    }

    public void SetNeighbor(SquareNodeDirections directions, GridSlot neighbor)
    {
        switch (directions)
        {
            case SquareNodeDirections.Up:
                upNeighbor = neighbor;
                break;
            case SquareNodeDirections.Right:
                rightNeighbor = neighbor;
                break;
            case SquareNodeDirections.Down:
                downNeighbor = neighbor;
                break;
            case SquareNodeDirections.Left:
                leftNeighbor = neighbor;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(directions), directions, null);
        }
    }
}