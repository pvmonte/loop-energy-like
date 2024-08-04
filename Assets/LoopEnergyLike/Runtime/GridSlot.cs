using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GridSlot : MonoBehaviour
{
    [field: SerializeField] public SquareNode node { get; private set; }
    [field: SerializeField] public GridSlot upNeighbor { get; private set; }
    [field: SerializeField] public GridSlot rightNeighbor { get; private set; }
    [field: SerializeField] public GridSlot downNeighbor { get; private set; }
    [field: SerializeField] public GridSlot leftNeighbor { get; private set; }

    public event Action<GridSlot> OnTap;

    private void OnMouseDown()
    {
        node.Spin(this);
    }
}