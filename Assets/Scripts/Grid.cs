using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject tilePrefab;
    public static int gridW = 10, gridH = 13;
    public int spacing = 1;
    private Tile[,] tiles;
    public static int minesAmount = 5;
    public static Tile[,] elements = new Tile[gridW, gridH];
    public static bool gameEndedBool;
    public static bool isFinished = false;
    public static int count;

    Tile SpawnTile(Vector3 pos)
    {
        GameObject clone = Instantiate(tilePrefab);
        clone.transform.position = pos;
        return clone.GetComponent<Tile>();
    }

    public static bool MineAt(int x, int y)
    {
        //coordinates in range then check for mine
        if (x >= 0 && y >= 0 && x < gridW && y < gridH)
        {
            return elements[x, y].mine;
        }
        return false;
    }

    void GenerateTiles()
    {
        tiles = new Tile[gridW, gridH];
        for (int x = 0; x < gridW; x++)
        {
            for (int y = 0; y < gridH; y++)
            {
                Vector2 pos = new Vector2(x, y);

                pos *= spacing;
                Tile tile = SpawnTile(pos);
                tile.transform.SetParent(transform);
                tile.x = x;
                tile.y = y;
                tiles[x, y] = tile;
            }
        }
    }
    #region mine functions
    void GenerateMines()
    {
        for (int i = 0; i < minesAmount; i++)
        {
            Tile t = tiles[Random.Range(0, gridW), Random.Range(0, gridH)];
            if (t.mine)
            {
                i -= 1;
            }
            else
            {
                t.mine = true;
            }
        }
    }
    void Start()
    {
        GenerateTiles();
        GenerateMines();
        print(safeTile);
    }

    public static int AdjacentMines(int x, int y)
    {
        int count = 0;

        if (MineAt(x, y + 1)) ++count;//top
        if (MineAt(x + 1, y + 1)) ++count;//top-right
        if (MineAt(x + 1, y)) ++count;//right
        if (MineAt(x + 1, y - 1)) ++count;//bottom-right
        if (MineAt(x, y - 1)) ++count;//bottom
        if (MineAt(x - 1, y - 1)) ++count;//bottom-left
        if (MineAt(x - 1, y)) ++count;//left
        if (MineAt(x - 1, y + 1)) ++count;//top-left
        return count;
    }
    //uncovers all mines
    public static void UncoverMines()
    {
        foreach (Tile elem in elements)
        {
            if (elem.mine)
            {
                elem.loadTexture(0);
                elem.uncovered = true;
            }
        }
    }
    #endregion
    //removes all the colliders of elements sets gameEndedBool to true
    #region game over
    public static void GameEnded()
    {
        foreach (Tile elem in elements)
        {
            elem.collider.enabled = false;
        }
        gameEndedBool = true;
    }
    #endregion

    // if a large cluster of tiles is empty it will remove them all
    #region uncover empty mines
    public static void FFuncover(int x, int y, bool[,] visited)
    {
        //coordinates in range
        if (x >= 0 && y >= 0 && x < gridW && y < gridH)
        {
            //visited already?
            if (visited[x, y])
            {
                return;
            }
            //uncover element
            elements[x, y].loadTexture(AdjacentMines(x, y));
            elements[x, y].uncovered = true;

            //close to a mine? then no more work needed
            if (AdjacentMines(x, y) > 0)
            {
                return;
            }

            //set visited flag
            visited[x, y] = true;
            //recursion
            FFuncover(x - 1, y, visited);
            FFuncover(x + 1, y, visited);
            FFuncover(x, y - 1, visited);
            FFuncover(x, y + 1, visited);
            FFuncover(x + 1, y + 1, visited);
            FFuncover(x + 1, y - 1, visited);
            FFuncover(x - 1, y - 1, visited);
            FFuncover(x - 1, y + 1, visited);
        }
    }
    #endregion

    public static bool IsFinished()
    {

        foreach (Tile elem in elements)
        {
            if (!elem.uncovered && !elem.mine)
            {
                return false;
            }
        }
        return true;
    }
}
