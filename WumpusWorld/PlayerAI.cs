/*****************************************************************************/
/*  Name:           www.prozhe.com                                        */
/*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WumpusWorld
{
    class PlayerAI
    {
        //struct that simulates a cell in the AI replication of World Grid
        struct cellCharacteristics  
        {
            public int pitPercentage;
            public int wumpusPercentage;
            public bool isPit;
            public bool neighborsMarked;
            public int numTimesvisited;
        }
        
        private cellCharacteristics[ , ] AIGrid;                    //array that simulates World Grid for the AI
        private enum Move {Up, Down, Right, Left, Enter, Escape};   //enum that represents integers that trigger movement in WumpusWorldForm class
        Stack<int> returnPath;                                      //keeps track of each move of AI to trace its path back
        bool returntoBeg;                                           //flag that is triggered when AI finds gold
        int numRandomMoves;                                         //keeps track of the number of random moves that are done

        public PlayerAI()
        {
            AIGrid = new cellCharacteristics[ 5, 5 ];
            cellCharacteristics c;
            returntoBeg = false;
            returnPath = new Stack<int>();
            numRandomMoves = 0;


            for( int y = 0; y < 5; y++ )
            {
                for( int x = 0; x < 5; x++ )
                {
                    c = new cellCharacteristics();
                    c.isPit = false;
                    c.neighborsMarked = false;
                    c.numTimesvisited = 0;

                    AIGrid[ x, y ] = c;
                }
            }
        }

        /// <summary>
        /// Returns the AI's next move
        /// </summary>
        /// <returns></returns>
        public int NextMove()
        {
            Random rand = new Random();

            //if gold has not been found, calculate next step
            if (!returntoBeg)
            {
                //calculate odds of the wumpus or a pit being in a neighboring square
                FuzzyLogic();

                //check to see if the player starts off in a square with a breeze or stench
                //if so and risky mode is enabled, randomly choose direction to go
                if (WumpusWorldForm.playerIndex.X == 0 && WumpusWorldForm.playerIndex.Y == 4
                    && WumpusWorldForm.riskyMode
                    && (WumpusWorldForm.worldGrid[WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y].hasBreeze
                    || WumpusWorldForm.worldGrid[WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y].hasStench) )
                {
                    switch (rand.Next(2))
                    {
                        case 0:
                            AIGrid[ WumpusWorldForm.playerIndex.X + 1, WumpusWorldForm.playerIndex.Y ].numTimesvisited++;
                            return Convert.ToInt32( Move.Right );                                                                                    
                            break;
                        case 1:
                            AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y - 1 ].numTimesvisited++;
                            return Convert.ToInt32( Move.Up );                                                                                       
                            break;
                    }
                }
                //if gold is found, activate return path
                else if( Rule1() )
                {
                    returntoBeg = true;
                }
                //if two spots indicate the location of the wumpus and wumpus is not already dead, take shot
                else if( Rule2() && !WumpusWorldForm.wumpusDead )
                {
                    return Convert.ToInt32( Move.Enter );
                }
                //if AI is most likely trapped and there is a good chance there is not a pit ahead and risky mode is enabled
                //take risk and move ahead
                else if (Rule3() && WumpusWorldForm.riskyMode)
                {
                    //if spot to right is not likely a pit, move there
                    if( WumpusWorldForm.playerIndex.X != 4 &&
                        AIGrid[ WumpusWorldForm.playerIndex.X + 1, WumpusWorldForm.playerIndex.Y ].pitPercentage < 60 )
                    {
                        returnPath.Push( Convert.ToInt32( Move.Left ) );
                        numRandomMoves = 0;
                        return Convert.ToInt32( Move.Right );
                    }
                    //else if spot above is not likely a pit, move there
                    else if( WumpusWorldForm.playerIndex.Y != 0 &&
                        AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y - 1 ].pitPercentage < 60 )
                    {
                        returnPath.Push( Convert.ToInt32( Move.Down ) );
                        numRandomMoves = 0;
                        return Convert.ToInt32( Move.Up );
                    }
                    //else f spot to the left is not likely a pit, move there
                    else if( WumpusWorldForm.playerIndex.X != 0 &&
                        AIGrid[ WumpusWorldForm.playerIndex.X - 1, WumpusWorldForm.playerIndex.Y ].pitPercentage < 60 )
                    {
                        returnPath.Push( Convert.ToInt32( Move.Right ) );
                        numRandomMoves = 0;
                        return Convert.ToInt32( Move.Left );
                    }
                    //else if spot below is not likely a pit, move there
                    else if( WumpusWorldForm.playerIndex.Y != 4 &&
                        AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y + 1 ].pitPercentage < 60 )
                    {
                        returnPath.Push( Convert.ToInt32( Move.Up ) );
                        numRandomMoves = 0;
                        return Convert.ToInt32( Move.Down );
                    }
                }
                //if currently on a space that has a stench or breeze
                //move to safe spot
                else if( Rule4() )
                {
                    //if safe spot to left, move back there
                    if( WumpusWorldForm.playerIndex.X != 0
                            && WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X - 1, WumpusWorldForm.playerIndex.Y ].spotVisited )
                    {
                        returnPath.Push( Convert.ToInt32( Move.Right ) );
                        AIGrid[ WumpusWorldForm.playerIndex.X - 1, WumpusWorldForm.playerIndex.Y ].numTimesvisited++;
                        return Convert.ToInt32( Move.Left );
                    }
                    //else if there is a safe spot down, move back there
                    else if( WumpusWorldForm.playerIndex.Y != 4
                        && WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y + 1 ].spotVisited )
                    {
                        returnPath.Push( Convert.ToInt32( Move.Up ) );
                        AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y + 1 ].numTimesvisited++;
                        return Convert.ToInt32( Move.Down );
                    }
                    //else if there is a safe spot to the right, move back there
                    else if( WumpusWorldForm.playerIndex.X != 4
                        && WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X + 1, WumpusWorldForm.playerIndex.Y ].spotVisited )
                    {
                        returnPath.Push( Convert.ToInt32( Move.Left ) );
                        AIGrid[ WumpusWorldForm.playerIndex.X + 1, WumpusWorldForm.playerIndex.Y ].numTimesvisited++;
                        return Convert.ToInt32( Move.Right );
                    }
                    //else if there is a safe spot above, move back there
                    else if( WumpusWorldForm.playerIndex.Y != 0
                        && WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y - 1 ].spotVisited )
                    {
                        returnPath.Push( Convert.ToInt32( Move.Down ) );
                        AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y - 1 ].numTimesvisited++;
                        return Convert.ToInt32( Move.Up );
                    }
                }
                //if there is not currently a stench or breeze on spot
                //search for unexplored neighbors to move to
                else if( Rule5() )
                {
                    //if space to right is unexplored, move there
                    if( WumpusWorldForm.playerIndex.X != 4 
                        && !WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X + 1, WumpusWorldForm.playerIndex.Y ].spotVisited )
                    {
                        returnPath.Push( Convert.ToInt32( Move.Left ) );
                        AIGrid[ WumpusWorldForm.playerIndex.X + 1, WumpusWorldForm.playerIndex.Y ].numTimesvisited++;
                        return Convert.ToInt32( Move.Right );
                    }
                    //else if space above is unexplored, move there
                    else if( WumpusWorldForm.playerIndex.Y != 0 
                        && !WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y - 1 ].spotVisited )
                    {
                        returnPath.Push( Convert.ToInt32( Move.Down ) );
                        AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y - 1 ].numTimesvisited++;
                        return Convert.ToInt32( Move.Up );
                    }
                    //else if space to left is unexplored, move there
                    else if( WumpusWorldForm.playerIndex.X != 0 
                        && !WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X - 1, WumpusWorldForm.playerIndex.Y ].spotVisited )
                    {
                        returnPath.Push( Convert.ToInt32( Move.Right ) );
                        AIGrid[ WumpusWorldForm.playerIndex.X - 1, WumpusWorldForm.playerIndex.Y ].numTimesvisited++;
                        return Convert.ToInt32( Move.Left );
                    }
                    //else if space below is unexplored, move there
                    else if( WumpusWorldForm.playerIndex.Y != 4 
                        && !WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y + 1 ].spotVisited )
                    {
                        returnPath.Push( Convert.ToInt32( Move.Up ) );
                        AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y + 1 ].numTimesvisited++;
                        return Convert.ToInt32( Move.Down );
                    }
                    //else if all neighbor spaces have been explored, choose random direction to move to
                    else
                    {
                        while( true )
                        {
                            switch( rand.Next( 0, 4 ) )
                            {
                                //if selected, move right
                                case 0:
                                    if( WumpusWorldForm.playerIndex.X != 4
                                         )
                                    {
                                        returnPath.Push( Convert.ToInt32( Move.Left ) );
                                        AIGrid[ WumpusWorldForm.playerIndex.X + 1, WumpusWorldForm.playerIndex.Y ].numTimesvisited++;
                                        numRandomMoves++;
                                        return Convert.ToInt32( Move.Right );
                                    }
                                    break;
                                //if selected, move up
                                case 1:
                                    if( WumpusWorldForm.playerIndex.Y != 0
                                         )
                                    {
                                        returnPath.Push( Convert.ToInt32( Move.Down ) );
                                        AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y - 1 ].numTimesvisited++;
                                        numRandomMoves++;
                                        return Convert.ToInt32( Move.Up );
                                    }
                                    break;
                                //if selected, move left
                                case 2:
                                    if( WumpusWorldForm.playerIndex.X != 0
                                         )
                                    {
                                        returnPath.Push( Convert.ToInt32( Move.Right ) );
                                        AIGrid[ WumpusWorldForm.playerIndex.X - 1, WumpusWorldForm.playerIndex.Y ].numTimesvisited++;
                                        numRandomMoves++;
                                        return Convert.ToInt32( Move.Left );
                                    }
                                    break;
                                //if selected, move down
                                case 3:
                                    if( WumpusWorldForm.playerIndex.Y != 4
                                         )
                                    {
                                        returnPath.Push( Convert.ToInt32( Move.Up ) );
                                        AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y + 1 ].numTimesvisited++;
                                        numRandomMoves++;
                                        return Convert.ToInt32( Move.Down );
                                    }
                                    break;
                            }
                        }
                    }     
                }
            }
            //if gold has been found, move next step in return path
            // since the player always starts off in the lower left hand corner, all spaces in relation to 
            //  it are above and to the right.  Thus, to get back to the starting space we can go left and
            //  down and we will eventually reach where we came from. This does not work however, if there
            //  are any obstacles in the way.  This is why the stack that kept track of all the AI's moves
            //  is used as a last resort if the way is blocked. This method is not foolproof and needs improvement.
            else
            {
                //if there is a safe opening to the left, move there
                if( WumpusWorldForm.playerIndex.X != 0 
                    && WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X - 1, WumpusWorldForm.playerIndex.Y ].spotVisited )
                {
                    return Convert.ToInt32( Move.Left );
                }
                //else if there is a safe opening below, move there
                else if (WumpusWorldForm.playerIndex.Y != 4 
                    && WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y + 1 ].spotVisited )
                {
                    return Convert.ToInt32( Move.Down );
                }
                //if both the left and below spaces are unsafe, perform opposite of last step in move stack
                else
                {
                    return returnPath.Pop();
                }
                
            }

            return 5;
        }

        /// <summary>
        /// If gold has been found, return true
        /// </summary>
        /// <returns></returns>
        private bool Rule1()
        {
            if (WumpusWorldForm.foundGold)
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        /// <summary>
        /// If two or more spaces suggest a wumpus is in a neighboring space, return true
        /// </summary>
        /// <returns></returns>
        private bool Rule2()
        {
            //if wumpus is suspected to be above, adjust shooting direction to up and return true
            if( WumpusWorldForm.playerIndex.Y != 0
                && AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y - 1 ].wumpusPercentage >= 60 )
            {
                WumpusWorldForm.shootUp = true;
                return true;
            }
            //else if wumpus is suspected to be to the right, adjust shooting direction to right and return true
            else if( WumpusWorldForm.playerIndex.X != 4
                && AIGrid[ WumpusWorldForm.playerIndex.X + 1, WumpusWorldForm.playerIndex.Y ].wumpusPercentage >= 60 )
            {
                WumpusWorldForm.shootRight = true;
                return true;
            }
            //if wumpus is suspected to be to the left, adjust shooting direction to left and return true
            else if( WumpusWorldForm.playerIndex.X != 0
                && AIGrid[ WumpusWorldForm.playerIndex.X - 1, WumpusWorldForm.playerIndex.Y ].wumpusPercentage >= 60 )
            {
                WumpusWorldForm.shootLeft = true;
                return true;
            }
            //if wumpus is suspected to be below, adjust shooting direction to down and return true
            else if( WumpusWorldForm.playerIndex.Y != 4
                && AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y + 1 ].wumpusPercentage >= 60 )
            {
                WumpusWorldForm.shootDown = true;
                return true;
            }
            //if not enough evidence for wumpus, return false
            else
            {
                return false;
            }
        }

        /// <summary>
        /// If there is reason to believe player is trapped and there is a reasonable chance there is not
        ///     a pit in a neighboring square and if risky mode is enabled, take risk and move to space
        /// </summary>
        /// <returns></returns>
        private bool Rule3()
        {
            if( numRandomMoves > 0
                && AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y ].numTimesvisited > 1
                && WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y ].hasBreeze )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// If player is in a space that has a stench or breeze, return true
        /// </summary>
        /// <returns></returns>
        private bool Rule4()
        {
            //if space contains both a stench and a breeze and wumpus is not dead, return true
            if( WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y ].hasBreeze
                && WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y ].hasStench
                && !WumpusWorldForm.wumpusDead )
            {
                return true;
            }
            //else if space only contains breeze, return true
            else if( WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y ].hasBreeze )
            {
                return true;
            }
            //else if space only contains stench and wumpus is not dead, return true
            else if( WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y ].hasStench
                && !WumpusWorldForm.wumpusDead )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// If space does not contain a breeze or stench, return true
        /// </summary>
        /// <returns></returns>
        private bool Rule5()
        {
            //if space does not contain a breeze or stench or it does not contain a breeze and wumpus is dead, return true
            if( ( !WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y ].hasBreeze
                && !WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y ].hasStench)
                || ( !WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y ].hasBreeze 
                && WumpusWorldForm.wumpusDead ) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Calculates odds of neighboring sqaces being either a wumpus or pit based on detecting a breeze or stench
        /// </summary>
        private void FuzzyLogic()
        {
            //if spaces neighbors have not already been calculated
            if( !AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y ].neighborsMarked )
            {
                //if current space has a breeze, calculate odds of pit
                if( WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y ].hasBreeze )
                {
                    CalcPitWumpusPercentages( true, false );
                }

                //if current space has a stench, calculate odds of wumpus
                if( WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y ].hasStench
                    && !WumpusWorldForm.wumpusDead)
                {
                    CalcPitWumpusPercentages( false, true );
                }
            }
        }

        /// <summary>
        /// Add 30% chance of pit or wumpus to current % chance for neighboring spaces
        /// </summary>
        /// <param name="pit"></param>
        /// <param name="wumpus"></param>
        private void CalcPitWumpusPercentages( bool pit, bool wumpus)
        {
            //if space above has not been visited, add 30% chance that it is either a pit or wumpus
            if( WumpusWorldForm.playerIndex.Y != 0
                    && !WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y - 1 ].spotVisited )
            {
                if (pit)
                {
                    AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y - 1 ].pitPercentage += 30;
                }

                if (wumpus)
                {
                    AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y - 1 ].wumpusPercentage += 30;
                }                
            }

            //if space to the right has not been visited, add 30% chance that it is either a pit or wumpus
            if( WumpusWorldForm.playerIndex.X != 4
                && !WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X + 1, WumpusWorldForm.playerIndex.Y ].spotVisited )
            {
                if( pit )
                {
                    AIGrid[ WumpusWorldForm.playerIndex.X + 1, WumpusWorldForm.playerIndex.Y ].pitPercentage += 30;
                }

                if( wumpus )
                {
                    AIGrid[ WumpusWorldForm.playerIndex.X + 1, WumpusWorldForm.playerIndex.Y ].wumpusPercentage += 30;
                }
            }

            //if space to the left has not been visited, add 30% chance that it is either a pit or wumpus
            if( WumpusWorldForm.playerIndex.X != 0
                && !WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X - 1, WumpusWorldForm.playerIndex.Y ].spotVisited )
            {
                if( pit )
                {
                    AIGrid[ WumpusWorldForm.playerIndex.X - 1, WumpusWorldForm.playerIndex.Y ].pitPercentage += 30;
                }

                if( wumpus )
                {
                    AIGrid[ WumpusWorldForm.playerIndex.X - 1, WumpusWorldForm.playerIndex.Y ].wumpusPercentage += 30;
                }
            }

            //if space below has not been visited, add 30% chance that it is either a pit or wumpus
            if( WumpusWorldForm.playerIndex.Y != 4
                && !WumpusWorldForm.worldGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y + 1 ].spotVisited )
            {
                if( pit )
                {
                    AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y + 1 ].pitPercentage += 30;
                }

                if( wumpus )
                {
                    AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y + 1 ].wumpusPercentage += 30;
                }
            }

            //mark that neighbors have been marked for specific cell
            AIGrid[ WumpusWorldForm.playerIndex.X, WumpusWorldForm.playerIndex.Y ].neighborsMarked = true;
        }

    }
}
