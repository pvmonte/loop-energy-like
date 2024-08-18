using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelController : MonoBehaviour
{
    [SerializeField] private CircuitController[] circuitControllers;
    private int circuitsClosed;

    [SerializeField] private Vector2Int gameGridCoords; //Row x Columns
    [SerializeField] private GridSlot slotPrefab;
    private GridSlot[,] gridSlots;

    void Start()
    {
        gridSlots = new GridSlot[gameGridCoords.x, gameGridCoords.y];

        for (int row = 0; row < gameGridCoords.x; row++)
        {
            for (int column = 0; column < gameGridCoords.y; column++)
            {
                gridSlots[row, column] = Instantiate(slotPrefab, new Vector3(column, row, 0), quaternion.identity, transform);
                gridSlots[row, column].name = $"GridSlot-{row}-{column}";
            }
        }

        int rows = gridSlots.GetLength(0);
        int columns = gridSlots.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                SetGridSlotNeighbor(gridSlots[i, j], SquareNodeDirections.Up, i + 1, j);
                SetGridSlotNeighbor(gridSlots[i, j], SquareNodeDirections.Right, i, j + 1);
                SetGridSlotNeighbor(gridSlots[i, j], SquareNodeDirections.Down, i - 1, j);
                SetGridSlotNeighbor(gridSlots[i, j], SquareNodeDirections.Left, i, j - 1);
            }
        }

        for (int i = 0; i < circuitControllers.Length; i++)
        {
            circuitControllers[i].OnClose += Circuits_OnClose;
            circuitControllers[i].OnOpen += controller => circuitsClosed--;
            circuitControllers[i].Initialize(gridSlots);
        }
    }

    private void SetGridSlotNeighbor(GridSlot slot, SquareNodeDirections direction, int i, int j)
    {
        try
        {
            slot.SetNeighbor(direction, gridSlots[i, j]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void Circuits_OnClose(CircuitController obj)
    {
        circuitsClosed++;

        if (circuitsClosed == circuitControllers.Length)
        {
            print("Level finished");
        }
    }
}