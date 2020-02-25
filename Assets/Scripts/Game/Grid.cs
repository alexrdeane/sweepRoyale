using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Tile tile;
    public GameObject tilePrefab;
    public static int w = 15, h = 20;
    public int spacing = 1;
    public static Tile[,] tiles;
    public static int minesAmount = GameManager.mineAmounts[GameManager.mineAmountInt];
    public static Tile[,] elements = new Tile[w, h];
    public static bool gameEndedBool;
    public static int flagAmount;
    public static int minesAmoun;

    public static bool mineImmunity;

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

    public void GenerateTiles()
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

    public void GenerateMines()
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

    public static void RegenerateMines()
    {
        for (int i = 0; i < Tile.mineRegenInt; i++)
        {
            Tile t = tiles[Random.Range(0, w), Random.Range(0, h)];
            if (minesAmount <= 20)
            {
                if (!t.uncovered && !t.mine)
                {
                    t.mine = true;
                    minesAmoun++;
                }
            }
        }
    }

    void Start()
    {
        mineImmunity = true;
        GenerateTiles();
        GenerateMines();
        flagAmount = minesAmount;
    }

    public static int AdjacentMines(int x, int y, bool check = false)
    {
        int count = 0;
        if (check)
        {
            if (MineAt(x, y + 1)) ++count;//top
            if (MineAt(x + 1, y + 1)) ++count;//top-right
            if (MineAt(x + 1, y)) ++count;//right
            if (MineAt(x + 1, y - 1)) ++count;//bottom-right
            if (MineAt(x, y - 1)) ++count;//bottom
            if (MineAt(x - 1, y - 1)) ++count;//bottom-left
            if (MineAt(x - 1, y)) ++count;//left
            if (MineAt(x - 1, y + 1)) ++count;//top-left
        }
        else if (GameManager.gamemode == "Default")
        {
            //if (MineAt(x, y + 1)) ++count;//top
            if (MineAt(x + 1, y + 1)) ++count;//top-right
            //if (MineAt(x + 1, y)) ++count;//right
            if (MineAt(x + 1, y - 1)) ++count;//bottom-right
            //if (MineAt(x, y - 1)) ++count;//bottom
            if (MineAt(x - 1, y - 1)) ++count;//bottom-left
            //if (MineAt(x - 1, y)) ++count;//left
            if (MineAt(x - 1, y + 1)) ++count;//top-left
        }
        else if (GameManager.gamemode == "Colour")
        {
            if (MineAt(x, y + 1)) ++count;//top
            if (MineAt(x + 1, y + 1)) ++count;//top-right
            if (MineAt(x + 1, y)) ++count;//right
            if (MineAt(x + 1, y - 1)) ++count;//bottom-right
            if (MineAt(x, y - 1)) ++count;//bottom
            if (MineAt(x - 1, y - 1)) ++count;//bottom-left
            if (MineAt(x - 1, y)) ++count;//left
            if (MineAt(x - 1, y + 1)) ++count;//top-left
        }
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

            if (AdjacentMines(x, y, true) > 0)
            {
                return;
            }

            visited[x, y] = true;
            if (GameManager.gamemode == "Default")
            {
                FFuncover(x - 1, y, visited);
                FFuncover(x + 1, y, visited);
                FFuncover(x, y - 1, visited);
                FFuncover(x, y + 1, visited);
                FFuncover(x + 1, y + 1, visited);
                FFuncover(x + 1, y - 1, visited);
                FFuncover(x - 1, y - 1, visited);
                FFuncover(x - 1, y + 1, visited);
            }
            else if (GameManager.gamemode == "Colour")
            {
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

        if (Tile.mineImmuneInt <= 0)
        {
            mineImmunity = false;
        }
        minesAmoun = minesAmount;
    }
}
