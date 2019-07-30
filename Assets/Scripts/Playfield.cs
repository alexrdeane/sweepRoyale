using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : Element
{
    //width and height of playfield
    public static int w = 10;
    public static int h = 13;
    //
    public static Element[,] elements = new Element[w, h];
    public static bool gameEndedBool;

    public static void gameEnded()
    {
        foreach (Element elem in elements)
        {
            elem.collider.enabled = false;
        }
        gameEndedBool = true;
    }
    public static void uncoverMines()
    {
        foreach (Element elem in elements)
        {
            if (elem.mine) elem.loadTexture(0);
        }
    }

    public static bool mineAt(int x, int y)
    {
        //coordinates in range? then check for mine
        if (x >= 0 && y >= 0 && x < w && y < h)
        {
            return elements[x, y].mine;
        }
        return false;
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

        }
    }
}
