using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    //variables
    #region variables
    //if the tile is a mine or flagged
    public bool mine, tileFlagged;
    //array of default tiles (default or revealed)
    public Sprite[] defaultTiles;
    //array of empty and bombsNear(1-8)
    public Sprite[] emptyTextures;
    //array of mine textures (revealed or exploded)
    public Sprite[] mineTextures; 
    //box collider 2D of the object
    public BoxCollider2D collider;
    #endregion
    //start function / sets mine, gets colliders and location of tiles / sets the GameObject this is attached to as a tile
    #region start
    void Start()
    {
        //randomly decide if it's a mine or not
        mine = Random.value < 0.15;
        //gets colliders from tiles
        collider = GetComponent<BoxCollider2D>();
        //register in grid
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        //sets the GameObject this is attached to as a tile
        Playfield.elements[x, y] = this;
    }
    #endregion
    //these functions are used to swap sprites and deactivate colliders when needed
    #region default texture swap functions
    //boolean used to check if the tile is covered (if it has not been revealed)
    public bool isCovered()
    {
        return GetComponent<SpriteRenderer>().sprite.texture.name == "defaultTile";
    }
    //loads a mine texture if its a mine, flagged texture if the tile is flagged or the empty tile textures if empty
    public void loadTexture(int adjacentCount)
    {
        if (mine)
        {
            //sets the texture to the mine texture
            GetComponent<SpriteRenderer>().sprite = mineTextures[0];
        }
        else if (tileFlagged)
        {
            //sets the texture and bool to flagged
            flagTileActive();
        }
        //if not a mine or flagged
        else
        {
            //sets the texture to empty or bombsNear(1-8)
            GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];
            //disables the collider of empty tiles to stop players from clicking the tile again
            this.collider.enabled = false;
        }
    }

    //sets the texture to the exploded bomb to indicate which bomb was pressed at game over
    public void loadExploded()
    {
        if (mine)
        {
            GetComponent<SpriteRenderer>().sprite = mineTextures[1];
        }
    }
    #endregion
    //if these functions are called the tile pressed will become flagged or unflagged depending on which is referenced
    #region flagging function / unflagging function
    public void flagTileActive()
    {
        GetComponent<SpriteRenderer>().sprite = defaultTiles[1];
        this.tileFlagged = true;
    }
    public void flagTileInactive()
    {
        isCovered();
        GetComponent<SpriteRenderer>().sprite = defaultTiles[0];
        this.tileFlagged = false;
    }
    #endregion
    //mouse input and touch input accessible
    #region OnMouseUpAsButton
    void OnMouseUpAsButton()
    {
        // if the flag button is active any tile the player presses will become a flagged tile, if a flagged tile is pressed whilst the flag button it will reset the tile back to being unflagged
        #region flagging and unflagging
        //if the flagTile bool is active
        if (FlagButton.flagTile == true)
        {
            //if this tile is flagged
            if (this.tileFlagged == true)
            {
                //resets the flagged tile to be a normal covered tile
                flagTileInactive();
            }
            //if the tile is not flagged
            else
            {
                //sets this tile to be flagged
                flagTileActive();
            }
        }
        #endregion
        // if a tile is pressed without the flag button activated it will replace the tiles with the emptyTile sprite or one of the bombsNear(1-8), if the tile is a mine the game will end
        #region removing tiles / game over
        //if the flagTile bool is not active
        else
        {
            //if the tile is not flagged
            if (!tileFlagged)
            {
                //mine = Random.value > .5f;
                //if the tile is a mine
                if (mine)
                {
                    //activate the game ended void in the Playfield script
                    Playfield.gameEnded();
                    //uncovers all mines
                    Playfield.uncoverMines();
                    //use the bombExplodedTile texture to signify which bomb the player touched (optional)
                    this.loadExploded();
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
        #endregion
    }
    #endregion
}
