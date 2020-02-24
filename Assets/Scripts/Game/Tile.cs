using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y;
    public static float r, g, b;
    public bool mine, uncovered = false, flagged = false;
    public Sprite[] defaultTiles;
    public Sprite[] colourTiles;
    public Sprite[] emptyTextures;
    public Sprite[] mineTextures;
    public new BoxCollider2D collider;
    public Color lerpedOver = Color.white;
    public Color lerpedOff = new Color(r, g, b);


    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        Grid.elements[x, y] = this;
    }

    public void LoadTexture(int adjacentCount)
    {
        if (GameManager.gamemode == "Default")
        {
            if (mine)
            {
                GetComponent<SpriteRenderer>().sprite = mineTextures[0];
            }
            else if (flagged)
            {
                FlagTileActive();
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];
                collider.enabled = false;
            }
        }
        else if (GameManager.gamemode == "Diagonals")
        {
            if (mine)
            {
                GetComponent<SpriteRenderer>().sprite = mineTextures[0];
            }
            else if (flagged)
            {
                FlagTileActive();
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];
                collider.enabled = false;
            }
        }
        else if (GameManager.gamemode == "Colour")
        {
            if (mine)
            {
                GetComponent<SpriteRenderer>().sprite = mineTextures[0];
            }
            else if (flagged)
            {
                FlagTileActive();
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = colourTiles[adjacentCount];
                collider.enabled = false;
            }
        }
    }

    public void LoadExploded()
    {
        if (mine)
        {
            GetComponent<SpriteRenderer>().sprite = mineTextures[1];
        }
    }

    public void FlagTileActive()
    {
        GetComponent<SpriteRenderer>().sprite = defaultTiles[1];
        flagged = true;
    }
    public void FlagTileInactive()
    {
        GetComponent<SpriteRenderer>().sprite = defaultTiles[0];
        flagged = false;
    }

    void OnMouseOver()
    {
        GetComponent<SpriteRenderer>().color = Color.Lerp(lerpedOff, lerpedOver, 0f);
        if (Input.GetMouseButtonDown(0))
        {
            if (!flagged)
            {
                if (mine)
                {
                    print("lose");
                    Grid.UncoverMines();
                    LoadExploded();
                    Grid.GameEnded();
                }
                else
                {
                    int x = (int)transform.position.x;
                    int y = (int)transform.position.y;
                    LoadTexture(Grid.AdjacentMines(x, y));
                    Grid.FFuncover(x, y, new bool[Grid.w, Grid.h]);
                    if (Grid.IsFinishedNoSafe())
                    {
                        print("winner no safe");
                        Grid.GameEnded();
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (flagged == true)
            {
                FlagTileInactive();
                Grid.flagAmount++;
            }
            else
            {
                if (Grid.flagAmount >= 1)
                {
                    FlagTileActive();
                    Grid.flagAmount--;
                }
            }
        }
        else if (Grid.IsFinishedBombsFlagged())
        {
            print("winner bombs flagged");
            Grid.GameEnded();
        }
    }

    public void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = Color.Lerp(lerpedOver, lerpedOff, 0f);
    }
}
