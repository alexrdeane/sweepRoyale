using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : Tile
{
    public GameObject tilePrefab;
    public static int w = 10, h = 13;
    public int spacing = 1;
    private Tile[,] tiles;
    public static int minesAmount = 15;
    public static Tile[,] elements = new Tile[w, h];
    public static bool gameEndedBool;
    public static bool gameWonBool;

    Tile SpawnTile(Vector3 pos)
    {
        GameObject clone = Instantiate(tilePrefab);
        clone.transform.position = pos;
        return clone.GetComponent<Tile>();
    }

    public static bool mineAt(int x, int y)
    {
        //coordinates in range then check for mine
        if (x >= 0 && y >= 0 && x < w && y < h)
        {
            return elements[x, y].mine;
        }
        return false;
    }

    void GenerateTiles()
    {
        tiles = new Tile[w, h];
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
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
            Tile t = tiles[Random.Range(0, w), Random.Range(0, h)];
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
    }

    public static int adjacentMines(int x, int y)
    {
        int count = 0;

        if (mineAt(x, y + 1)) ++count;//top
        if (mineAt(x + 1, y + 1)) ++count;//top-right
        if (mineAt(x + 1, y)) ++count;//right
        if (mineAt(x + 1, y - 1)) ++count;//bottom-right
        if (mineAt(x, y - 1)) ++count;//bottom
        if (mineAt(x - 1, y - 1)) ++count;//bottom-left
        if (mineAt(x - 1, y)) ++count;//left
        if (mineAt(x - 1, y + 1)) ++count;//top-left
        return count;
    }
    //uncovers all mines
    public static void uncoverMines()
    {
        foreach (Tile elem in elements)
        {
            if (elem.mine) elem.loadTexture(0);
        }
    }
    #endregion
    //removes all the colliders of elements sets gameEndedBool to true
    #region game over
    public static void gameEnded()
    {
        foreach (Tile elem in elements)
        {
            elem.collider.enabled = false;
        }
        gameEndedBool = true;
    }
    #endregion

    public static bool isFinished()
    {
        foreach (Tile elem in elements)
        {
            if (elem.isCovered() && !elem.mine)
            {
                print("false");
                return false;
            }
        }
        print("true");

        return true;
    }

    // if a large cluster of tiles is empty it will remove them all
    #region uncover empty mines
    public static void FFuncover(int x, int y, bool[,] visited)
    {
        //coordinates in range
        if (x >= 0 && y >= 0 && x < w && y < h)
        {
            //visited already?
            if (visited[x, y])
            {
                return;
            }
            //uncover element
            elements[x, y].loadTexture(adjacentMines(x, y));
            //close to a mine? then no more work needed
            if (adjacentMines(x, y) > 0)
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
}
