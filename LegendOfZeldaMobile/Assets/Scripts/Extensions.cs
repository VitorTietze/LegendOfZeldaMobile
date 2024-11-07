using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void SnapToGrid(this Transform transform, Grid grid)
    {
        Vector3 worldPosition = transform.position;
        Vector3Int snappedCellPosition = grid.WorldToCell(worldPosition);
        transform.position = new Vector3(snappedCellPosition.x, snappedCellPosition.y, transform.position.z);
    }
}
