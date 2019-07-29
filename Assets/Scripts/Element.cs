using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    //is this a mine?
    public bool mine;
    public bool gameEnded = false;
    public Sprite[] emptyTextures;
    public Sprite mineTexture;
    public Sprite explodedMine;
    public BoxCollider2D collider;
    public Sprite flagTile;
    public bool tileFlagged;
    void Start()
    {
        //randomly decide if it's a mine or not
        mine = Random.value < 0.15;
        collider = GetComponent<BoxCollider2D>();
        //register in grid
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        Playfield.elements[x, y] = this;
    }
    public void loadTexture(int adjacentCount)
    {
        if (mine)
        {
            GetComponent<SpriteRenderer>().sprite = mineTexture;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];
        }
    }

    public void loadExploded()
    {
        if (mine)
        {
            GetComponent<SpriteRenderer>().sprite = explodedMine;
        }
    }

    public void flagTileActive()
    {
        GetComponent<SpriteRenderer>().sprite = flagTile;
        this.collider.enabled = false;
        this.tileFlagged = true;
    }
    public void flagTileInactive()
    {
        isCovered();
        this.collider.enabled = true;
        this.tileFlagged = false;
    }

    public bool isCovered()
    {
        return GetComponent<SpriteRenderer>().sprite.texture.name == "defaultTile";
    }

    void OnMouseUpAsButton()
    {
        if (flagButton.flagTile == true)
        {
            flagTileActive();
            if (this.tileFlagged == true)
            {
                flagTileInactive();
            }
            else
            {
                flagTileActive();
            }
        }
        else
        {
            if (mine)
            {
                Playfield.gameEnded();
                //uncovers all mines
                Playfield.uncoverMines();
                this.loadExploded();
                //game over
                print("you lose");
            }
            //it's not a mine
            else
            {
                int x = (int)transform.position.x;
                int y = (int)transform.position.y;
                loadTexture(Playfield.adjacentMines(x, y));

                Playfield.FFuncover(x, y, new bool[Playfield.w, Playfield.h]);

            }
        }
    }
}
