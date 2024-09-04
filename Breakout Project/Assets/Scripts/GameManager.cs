using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject paddle;
    public GameObject blockPrefab;

    [Header("Grid")]
    public Vector2 brickOffset;
    public int gridWidth = 8;
    public int gridHeight = 6;
    public float itemSpacerX = 1;
    public float itemSpacerY = 0.4f;

    public GameObject[,] blocks;
    public static int remainingBricks;

    private void BuildGrid ()
    {
        remainingBricks = gridWidth * gridHeight;
        blocks = new GameObject[gridWidth, gridHeight];
        brickOffset.x += -((gridWidth - 1) * itemSpacerX) / 2.0f;
        brickOffset.y += -((gridHeight - 1) * itemSpacerY) / 2.0f;
        for (int i = 0; i < gridWidth; i++)
            for (int j = 0; j < gridHeight; j++)
                blocks[i, j] = Instantiate(blockPrefab, brickOffset + new Vector2(i * itemSpacerX, j * itemSpacerY), Quaternion.identity);
    }

    void Start()
    {
        BuildGrid();
    }
}
