﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    //is this a mine?
    public bool mine;
    public Sprite[] emptyTextures;
    public Sprite mineTexture;
    public Sprite explodedMine;
    public BoxCollider2D collider;
    public Sprite flagTile;
    public bool tileFlagged;
    public static bool playfieldTileFlagged;
    public Sprite tile;
    public static int w = 10;//width
    public static int h = 13;//height
    public static Element[,] elements = new Element[w, h];
    void Start()
    {
        //randomly decide if it's a mine or not
        mine = Random.value < 0.05;
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
        else if (tileFlagged)
        {
            flagTileActive();
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];
            this.collider.enabled = false;
        }
    }

    public void flagTexture()
    {
        GetComponent<SpriteRenderer>().sprite = tile;
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
        this.tileFlagged = true;
    }
    public void flagTileInactive()
    {
        isCovered();
        GetComponent<SpriteRenderer>().sprite = tile;
        this.tileFlagged = false;
        playfieldTileFlagged = tileFlagged;
    }

    public bool isCovered()
    {
        return GetComponent<SpriteRenderer>().sprite.texture.name == "defaultTile";
    }

    void OnMouseUpAsButton()
    {
        if (flagButton.flagTile == true)
        {
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
            if (!tileFlagged)
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
}
