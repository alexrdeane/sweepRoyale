using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //position of tile
    public int x, y;
    //boolean for mine
    public bool mine, tileFlagged;
    public Sprite[] defaultTiles;
    //array of empty and bombsNear(1-8)
    public Sprite[] emptyTextures;
    //array of mine textures (revealed or exploded)
    public Sprite[] mineTextures;
    //box collider 2D of the object
    public new BoxCollider2D collider;
    public int tile = Grid.minesAmount;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        Grid.elements[x, y] = this;
    }

    #region default texture swap functions
    public bool isCovered()
    {
        return GetComponent<SpriteRenderer>().sprite.texture.name == "defaultTile";
    }

    public bool isFlagged()
    {
        return GetComponent<SpriteRenderer>().sprite.texture.name == "markedTile";
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
        isFlagged();
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
                if (mine)
                {
                    Grid.bombTile--;
                    print(Grid.bombTile);
                }
                if (Grid.isFinished() && Grid.bombTile <= 0)
                {
                    print("win");
                }
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
                    Grid.gameEnded();
                    print("lose");
                    //uncovers all mines
                    Grid.uncoverMines();
                    //use the bombExplodedTile texture to signify which bomb the player touched (optional)
                    this.loadExploded();
                }
                //it's not a mine
                else
                {
                    int x = (int)transform.position.x;
                    int y = (int)transform.position.y;
                    loadTexture(Grid.adjacentMines(x, y));
                    Grid.FFuncover(x, y, new bool[Grid.w, Grid.h]);

                    if (Grid.isFinished() && Grid.bombTile <= 0)
                    {
                        print("win");
                    }
                }
            }
        }
    }
    #endregion
    #endregion
}
