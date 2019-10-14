/*****************************************************************************/
/*  Name:           www.prozhe.com                                        */
/*****************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WumpusWorld
{
    public class Cell
    {
        //booleans that determine state of cell
        public bool isWumpus;
        public bool isPit;
        public bool isGold;
        public bool isPlayer;
        public bool hasStench;
        public bool hasBreeze;
        public bool spotVisited;

        //x and y position of cell in picture box
        public int xPos;
        public int yPos;

        public Cell(int xPosition, int yPosition)
        {
            xPos = xPosition;
            yPos = yPosition;
        }

        /// <summary>
        /// Set cell as pit
        /// </summary>
        public void SetAsPit()
        {
            isWumpus = false;
            isPit = true;
            isPlayer = false;
            isGold = false;
        }

        /// <summary>
        /// Set cell as wumpus
        /// </summary>
        public void SetAsWumpus()
        {
            isWumpus = true;
            isPit = false;
            isPlayer = false;
            isGold = false;
        }

        /// <summary>
        /// Set cell as gold
        /// </summary>
        public void SetAsGold()
        {
            isWumpus = false;
            isPit = false;
            isPlayer = false;
            isGold = true;
        }

        /// <summary>
        /// Set cell as player
        /// </summary>
        public void SetAsPlayer()
        {
            isWumpus = false;
            isPit = false;
            isPlayer = true;
            isGold = false;
        }

        /// <summary>
        /// Clear state of cell
        /// </summary>
        public void Clear()
        {
            isWumpus = false;
            isPit = false;
            isPlayer = false;
            isGold = false;
            hasBreeze = false;
            hasStench = false;

            if (xPos != 0 || yPos != 400)
            {
                spotVisited = false;
            } 
        }
    }
}
