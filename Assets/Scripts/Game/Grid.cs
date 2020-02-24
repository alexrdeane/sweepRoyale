using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject tilePrefab;
    public static int w = 10, h = 13;
    public int spacing = 1;
    public Tile[,] tiles;
    public static int minesAmount = GameManager.mineAmounts[GameManager.mineAmountInt];
    public static Tile[,] elements = new Tile[w, h];
    public static bool gameEndedBool;
    public static int flagAmount;

    Tile SpawnTile(Vector3 pos)
    {
        GameObject clone = Instantiate(tilePrefab);
        clone.transform.position = pos;
        return clone.GetComponent<Tile>();
    }

    public static bool MineAt(int x, int y)
    {
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
        flagAmount = minesAmount;
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
    public static void UncoverMines()
    {
        foreach (Tile elem in elements)
        {
            if (elem.mine)
            {
                elem.LoadTexture(0);
                elem.uncovered = true;
            }
        }
    }

    public static void GameEnded()
    {
        foreach (Tile elem in elements)
        {
            elem.GetComponent<Collider2D>().enabled = false;
        }
        gameEndedBool = true;
    }

    public static void FFuncover(int x, int y, bool[,] visited)
    {
        if (x >= 0 && y >= 0 && x < w && y < h)
        {
            if (visited[x, y])
            {
                return;
            }
            elements[x, y].LoadTexture(AdjacentMines(x, y));
            elements[x, y].uncovered = true;

            if (AdjacentMines(x, y) > 0)
            {
                return;
            }

            visited[x, y] = true;
            if (GameManager.gamemode == "Default")
            {

            }
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

    public static bool IsFinishedNoSafe()
    {
        foreach (Tile elem in elements)
        {
            if (!elem.uncovered && !elem.mine)
            {
                return false;
            }
        }
        foreach (Tile elem in elements)
        {
            elem.GetComponent<Collider2D>().enabled = false;
            gameEndedBool = true;
        }
        return true;
    }

    public static bool IsFinishedBombsFlagged()
    {
        foreach (Tile elem in elements)
        {
            if (!elem.flagged && elem.mine)
            {
                return false;
            }
        }
        foreach (Tile elem in elements)
        {
            elem.GetComponent<Collider2D>().enabled = false;
            gameEndedBool = true;
        }
        return true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame.GameRestart();
        }
    }
}
