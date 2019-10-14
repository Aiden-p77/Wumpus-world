
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WumpusWorld
{
    public partial class WumpusWorldForm : Form
    {
        #region Variables
        PlayerAI AI;

        public static Cell[ , ] worldGrid;  //array of cells that represents world
        public Cell c;                      //individual cell in array
        public static Point playerIndex;    //used to represent the x and y index in array, not actual location pixel wise
        private Point AIMove;

        private Graphics g;                 //graphics object that handles all drawing in picturebox
        private Bitmap myBMP;               //bitmap that is drawn onto in picturebox

        //icons that appear in cells
        Icon wumpusIcon;
        Icon wumpusDeadIcon;
        Icon playerIcon;
        Icon playerDeadIcon;
        Icon playerFallenIcon;
        Icon playerArmedIcon;
        Icon goldIcon;
        Icon pitIcon;
        Icon spearUpIcon;
        Icon spearDownIcon;
        Icon spearRightIcon;
        Icon spearLeftIcon;

        //flags that determine state of cells, games and icons
        public static bool foundGold;
        public static bool riskyMode;
        public static bool wumpusDead;
        public static bool shootUp;
        public static bool shootDown;
        public static bool shootRight;
        public static bool shootLeft;

        bool gameOver;
        bool playerArmed;
        bool AIMode;
        bool playerMode;
        bool revealGrid;
        
        bool showWarnings;

        //changeable parts of labels
        int points;
        int arrows;
        int time;
        #endregion
                

        public WumpusWorldForm()
        {
            InitializeComponent();

            //initializes graphics object and sets picturebox = bitmap
            myBMP = new Bitmap( worldPictureBox.Width, worldPictureBox.Height );
            g = Graphics.FromImage( myBMP );
            worldPictureBox.Image = myBMP;

            playerIndex = new Point();
            AIMove = new Point();

            //initializes every icon
            wumpusIcon = new Icon( "bsd-daemon.ico", 64, 64 );
            wumpusDeadIcon = new Icon( "bsd-daemon-dead.ico", 64, 64 );
            playerIcon = new Icon( "tux.ico", 64, 64 );
            playerArmedIcon = new Icon( "tux-spear.ico", 64, 64 );
            playerDeadIcon = new Icon( "tux-stabbed.ico", 64, 64 );
            playerFallenIcon = new Icon( "tux-flames.ico", 64, 64 );
            goldIcon = new Icon( "gold.ico", 64, 64 );
            pitIcon = new Icon( "flame.ico", 64, 64 );
            spearUpIcon = new Icon( "spear-up.ico", 32, 32 );
            spearDownIcon = new Icon( "spear-down.ico", 32, 32 );
            spearRightIcon = new Icon( "spear-right.ico", 32, 32 );
            spearLeftIcon = new Icon( "spear-left.ico", 32, 32 );

            //set default settings for flags
            gameOver = false;
            revealGrid = true;
            foundGold = false;
            playerArmed = false;
            wumpusDead = false;
            shootUp = false;
            shootDown = false;
            shootRight = false;
            shootLeft = false;

            worldGrid = new Cell[ 5, 5 ];

            int xPos = 0;
            int yPos = 0;

            //create world with blank cells
            for( int y = 0; y < 5; y++ )
            {
                for( int x = 0; x < 5; x++ )
                {
                    c = new Cell( xPos, yPos );
                    worldGrid[ x, y ] = c;
                    xPos += 100;
                }
                yPos += 100;
                xPos = 0;
            }

            //draw newly created grid
            DrawGrid();
        }

        /// <summary>
        /// Randomize the grid with one wumpus and specified density of pits
        /// </summary>
        private void RandomizeGrid()
        {
            //pause any currently running game
            InterfaceTimer.Enabled = false;

            Random rand = new Random();

            int pitDensity = 0;
            int x;
            int y;

            //determine density of random pits to generate based off pit density slider bar
            switch( pitDensityTrackBar.Value )
            {
                case 0:
                    pitDensity = 7;
                    break;
                case 1:
                    pitDensity = 6;
                    break;
                case 2:
                    pitDensity = 5;
                    break;
                case 3:
                    pitDensity = 4;
                    break;
            }

            //clear current setting of every cell
            foreach( Cell c in worldGrid )
            {
                c.Clear();
            }

            //based on density setting, randomly determine if spot will be a pit
            // excluding bottom left spot where player spawns
            for( y = 0; y < 5; y++ )
            {
                for( x = 0; x < 5; x++ )
                {
                    if( rand.Next( pitDensity ) == 0 && ( x != 0 || y != 4 ) )
                    {
                        worldGrid[ x, y ].SetAsPit();
                    }
                }
            }

            //set warnings of breezes in cells surrounding pit
            for( y = 0; y < 5; y++ )
            {
                for( x = 0; x < 5; x++ )
                {
                    if (worldGrid[x, y].isPit)
                    {
                        if (y > 0)
                        {
                            worldGrid[ x, y - 1 ].hasBreeze = true;
                        }
                        if( y < 4 )
                        {
                            worldGrid[ x, y + 1  ].hasBreeze = true;
                        }
                        if( x > 0 )
                        {
                            worldGrid[ x - 1, y ].hasBreeze = true;
                        }
                        if( x < 4 )
                        {
                            worldGrid[ x + 1, y ].hasBreeze = true;
                        }
                    }
                }
            }

            //randomly generate position of wumpus
            while( true )
            {
                x = rand.Next( 0, 5 );
                y = rand.Next( 0, 5 );

                //ensure that randomly generated position is not already a pit
                if( !worldGrid[ x, y ].isPit && ( x != 0 && y != 4 ) )
                {
                    worldGrid[ x, y ].SetAsWumpus();

                    //set warnings of stenches in cells surrounding wumpus
                    if( y > 0 )
                    {
                        worldGrid[ x, y - 1 ].hasStench = true;
                    }
                    if( y < 4 )
                    {
                        worldGrid[ x, y + 1 ].hasStench = true;
                    }
                    if( x > 0 )
                    {
                        worldGrid[ x - 1, y ].hasStench = true;
                    }
                    if( x < 4 )
                    {
                        worldGrid[ x + 1, y ].hasStench = true;
                    }

                    break;
                }
            }

            //randomly generate position of gold
            while( true )
            {
                x = rand.Next( 0, 5 );
                y = rand.Next( 0, 5 );

                //ensure that randomly generated position is not a pit or wumpus
                if( !worldGrid[ x, y ].isPit && !worldGrid[ x, y ].isWumpus && ( x != 0 && y != 4 ) )
                {
                    worldGrid[ x, y ].SetAsGold();
                    break;
                }
            }

            //set player in lower left cell
            worldGrid[ 0, 4 ].SetAsPlayer();
            worldGrid[ 0, 4 ].spotVisited = true;
            playerIndex.X = 0;
            playerIndex.Y = 4;

        }

        /// <summary>
        /// Draw grid and cells with corresponding icons based on set flags
        /// </summary>
        private void DrawGrid()
        {
            //clear the drawing board
            g.Clear( Control.DefaultBackColor );

            //draw each cell based on active flags
            foreach( Cell c in worldGrid )
            {
                //draw black border around cell
                g.DrawRectangle( Pens.Black, c.xPos, c.yPos, 100, 100 );

                //if player is currently in a pit
                if( c.isPit && c.isPlayer )
                {
                    g.DrawIcon( playerFallenIcon, c.xPos + 18, c.yPos + 18 );
                }
                //if player is currently on a live wumpus
                else if( c.isWumpus && c.isPlayer && !wumpusDead)
                {
                    g.DrawIcon( playerDeadIcon, c.xPos + 18, c.yPos + 18 );
                }
                //if player is currently on a dead wumpus
                else if (c.isWumpus && c.isPlayer && wumpusDead)
                {
                    g.DrawIcon( wumpusDeadIcon, c.xPos + 18, c.yPos + 18 );
                    g.DrawIcon( playerIcon, c.xPos + 18, c.yPos + 18 );                    

                    float x = c.xPos;
                    float y = c.yPos;

                    Font myFont = new Font( FontFamily.GenericSansSerif, 12 );

                    //display warnings above player and dead wumpus
                    if( c.hasBreeze && c.hasStench )
                    {
                        g.DrawString( "SB", myFont, Brushes.Black, x + 15, y );
                        g.DrawString( "S", myFont, Brushes.Black, x + 75, y );
                    }
                    else if( c.hasBreeze )
                    {
                        g.DrawString( "SB", myFont, Brushes.Black, x + 35, y );
                    }
                    else if( c.hasStench )
                    {
                        g.DrawString( "S", myFont, Brushes.Black, x + 40, y );
                    }
                }
                //if cell is a pit
                else if( c.isPit )
                {
                    if( revealGrid || c.spotVisited )
                    {
                        g.DrawIcon( pitIcon, c.xPos + 18, c.yPos + 18 );
                    }
                    else
                    {
                        g.FillRectangle( Brushes.Black, c.xPos, c.yPos, 100, 100 );
                    }
                }
                //if cell is only a dead wumpus
                else if( c.isWumpus && wumpusDead )
                {
                    g.DrawIcon( wumpusDeadIcon, c.xPos + 18, c.yPos + 18 );

                    float x = c.xPos;
                    float y = c.yPos;

                    Font myFont = new Font( FontFamily.GenericSansSerif, 12 );

                    //display warnings above dead wumpus
                    if( c.hasBreeze && c.hasStench )
                    {
                        g.DrawString( "SB", myFont, Brushes.Black, x + 15, y );
                        g.DrawString( "S", myFont, Brushes.Black, x + 75, y );
                    }
                    else if( c.hasBreeze )
                    {
                        g.DrawString( "SB", myFont, Brushes.Black, x + 35, y );
                    }
                    else if( c.hasStench )
                    {
                        g.DrawString( "S", myFont, Brushes.Black, x + 40, y );
                    }
                }
                //if cell is a live wumpus
                else if( c.isWumpus )
                {
                    if( revealGrid || c.spotVisited )
                    {
                        g.DrawIcon( wumpusIcon, c.xPos + 18, c.yPos + 18 );
                    }
                    else
                    {
                        g.FillRectangle( Brushes.Black, c.xPos, c.yPos, 100, 100 );
                    }
                }
                //if player is currently on gold
                else if( c.isGold && c.isPlayer)
                {
                    g.DrawIcon( playerIcon, c.xPos + 18, c.yPos );
                    g.DrawIcon( goldIcon, c.xPos + 18, c.yPos + 18 );
                }
                //if cell is gold and gold is unfound
                else if( c.isGold && !foundGold)
                {
                    if( revealGrid || c.spotVisited )
                    {
                        g.DrawIcon( goldIcon, c.xPos + 18, c.yPos + 18 );

                        float x = c.xPos;
                        float y = c.yPos;

                        Font myFont = new Font( FontFamily.GenericSansSerif, 12 );

                        //display warnings above gold
                        if( c.hasBreeze && c.hasStench )
                        {
                            g.DrawString( "SB", myFont, Brushes.Black, x + 15, y );
                            g.DrawString( "S", myFont, Brushes.Black, x + 75, y );
                        }
                        else if( c.hasBreeze )
                        {
                            g.DrawString( "SB", myFont, Brushes.Black, x + 35, y );
                        }
                        else if( c.hasStench )
                        {
                            g.DrawString( "S", myFont, Brushes.Black, x + 40, y );
                        }
                    }
                    else
                    {
                        g.FillRectangle( Brushes.Black, c.xPos, c.yPos, 100, 100 );
                    }
                }
                //if player is armed in cell
                else if( c.isPlayer && playerArmed )
                {
                    g.DrawIcon( playerArmedIcon, c.xPos + 18, c.yPos + 18 );
  
                    //display aiming icon facing specified direction
                    if( shootUp )
                    {
                        g.DrawIcon( spearUpIcon, c.xPos + 34, c.yPos );
                    }
                    else if( shootDown )
                    {
                        g.DrawIcon( spearDownIcon, c.xPos + 34, c.yPos + 68 );
                    }
                    else if( shootRight )
                    {
                        g.DrawIcon( spearRightIcon, c.xPos + 68, c.yPos + 34 );
                    }
                    else if( shootLeft )
                    {
                        g.DrawIcon( spearLeftIcon, c.xPos, c.yPos + 34 );
                    }

                    float x = c.xPos;
                    float y = c.yPos;

                    Font myFont = new Font( FontFamily.GenericSansSerif, 12 );

                    //show warnings above armed player
                    if( c.hasBreeze && c.hasStench )
                    {
                        g.DrawString( "SB", myFont, Brushes.Black, x + 15, y );
                        g.DrawString( "S", myFont, Brushes.Black, x + 75, y );
                    }
                    else if( c.hasBreeze )
                    {
                        g.DrawString( "SB", myFont, Brushes.Black, x + 35, y );
                    }
                    else if( c.hasStench )
                    {
                        g.DrawString( "S", myFont, Brushes.Black, x + 40, y );
                    }
                }
                //if player is by itself in cell
                else if( c.isPlayer )
                {
                    g.DrawIcon( playerIcon, c.xPos + 18, c.yPos + 18 );

                    float x = c.xPos;
                    float y = c.yPos;

                    Font myFont = new Font( FontFamily.GenericSansSerif, 12 );

                    //show warnings above player
                    if( c.hasBreeze && c.hasStench )
                    {
                        g.DrawString( "SB", myFont, Brushes.Black, x + 15, y );
                        g.DrawString( "S", myFont, Brushes.Black, x + 75, y );
                    }
                    else if( c.hasBreeze )
                    {
                        g.DrawString( "SB", myFont, Brushes.Black, x + 35, y );
                    }
                    else if( c.hasStench )
                    {
                        g.DrawString( "S", myFont, Brushes.Black, x + 40, y );
                    }
                }
                //if cell only contains warnings
                else if (showWarnings && (c.hasBreeze || c.hasStench))
                {
                    if( revealGrid || c.spotVisited )
                    {
                        float x = c.xPos;
                        float y = c.yPos;

                        Font myFont = new Font( FontFamily.GenericSansSerif, 12 );

                        if( c.hasBreeze && c.hasStench )
                        {
                            g.DrawString( "SB", myFont, Brushes.Black, x + 15, y + 40 );
                            g.DrawString( "S", myFont, Brushes.Black, x + 75, y + 40 );
                        }
                        else if( c.hasBreeze )
                        {
                            g.DrawString( "SB", myFont, Brushes.Black, x + 35, y + 40 );
                        }
                        else if( c.hasStench )
                        {
                            g.DrawString( "S", myFont, Brushes.Black, x + 40, y + 40 );
                        }
                    }
                    else
                    {
                        g.FillRectangle( Brushes.Black, c.xPos, c.yPos, 100, 100 );
                    }
                }
                else if( !revealGrid && !c.spotVisited )
                {
                    g.FillRectangle( Brushes.Black, c.xPos, c.yPos, 100, 100 );
                }
            }

            //refresh world picture box
            worldPictureBox.Refresh();

            //if in player mode, set focus to picture box so keyboard input is properly recognized
            if( playerMode )
            {
                worldPictureBox.Focus();
                this.KeyPreview = true;
            }
        }

        /// <summary>
        /// If set button is clicked, generate new game based on input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setButton_Click( object sender, EventArgs e )
        {
            //create new AI
            AI = new PlayerAI();

            //reset variables used in any previous game to default values
            startButton.Text = "Start";
            GameTimer.Enabled = true;
            InterfaceTimer.Enabled = false;
            wumpusDead = false;
            gameOver = false;
            foundGold = false;
            points = 0;
            arrows = 1;
            time = 0;
            shootUp = false;
            shootRight = false;
            shootLeft = false;
            shootDown = false;
            pointsLabel.Text = "Points: " + points;
            arrowsLabel.Text = "Arrows: " + arrows;
            timeLabel.Text = "Time: " + time;
            messagesListBox.Items.Clear();
            messagesListBox.Items.Add( "Game Starting... Goodluck!" );

            //change text on button from set to reset if first time pressed
            if( setButton.Text == "Set" )
            {
                setButton.Text = "Reset";
            }

            //determine if risky mode is to be enabled
            if (riskyModeCheckBox.Checked)
            {
                riskyMode = true;
            }
            else
            {
                riskyMode = false;
            }

            //determine if warnings are to be shown
            if (showWarningsCheckBox.Checked)
            {
                showWarnings = true;
            }
            else
            {
                showWarnings = false;
            }

            //determine if grid is to be revealed or blacked out
            if (revealGridCheckBox.Checked)
            {
                revealGrid = true;
            } 
            else
            {
                revealGrid = false;
            }

            //determine if the player is going to play or the AI
            if( aiModeRadioButton.Checked )
            {
                AIMode = true;
                playerMode = false;
            }
            else
            {
                AIMode = false;
                playerMode = true;
            }

            //randomize grid, redraw grid and check current status of game
            RandomizeGrid();
            DrawGrid();
            CheckState();
        }

        /// <summary>
        /// Check current status of game
        /// Determine if game is over, gold found, wumpus killed etc.
        /// Display game messages in message list box
        /// </summary>
        private void CheckState()
        {
            //display message if smoke is detected
            if (worldGrid[playerIndex.X, playerIndex.Y].hasBreeze && !playerArmed)
            {
                messagesListBox.Items.Add( "Smoke Detected!" );
                messagesListBox.SelectedIndex = messagesListBox.Items.Count - 1;
            }

            //display message if stench is detected
            if( worldGrid[ playerIndex.X, playerIndex.Y ].hasStench && !playerArmed && !wumpusDead )
            {
                messagesListBox.Items.Add( "Stench Detected!" );
                messagesListBox.SelectedIndex = messagesListBox.Items.Count - 1;
            }

            //display message if gold is found, add points, disable gold icon and set found gold flag to true
            if( worldGrid[ playerIndex.X, playerIndex.Y ].isGold && !playerArmed )
            {
                messagesListBox.Items.Add( "Gold Found!" );
                messagesListBox.SelectedIndex = messagesListBox.Items.Count - 1;
                worldGrid[ playerIndex.X, playerIndex.Y ].isGold = false;
                foundGold = true;
                points += 1000;
                pointsLabel.Text = "Points: " + points;
            }

            //display message if wumpus killed player, subtract points and set game over flag to true
            if( worldGrid[ playerIndex.X, playerIndex.Y ].isWumpus && !wumpusDead )
            {
                messagesListBox.Items.Add( "You were killed!" );
                messagesListBox.Items.Add( "Press Reset to Restart..." );
                messagesListBox.SelectedIndex = messagesListBox.Items.Count - 1;
                gameOver = true;
                GameTimer.Enabled = false;
                points -= 10000;
                pointsLabel.Text = "Points: " + points;
            }

            //display message if pit killed player, subract points and set game over flag to true
            if( worldGrid[ playerIndex.X, playerIndex.Y ].isPit )
            {
                messagesListBox.Items.Add( "You were killed!" );
                messagesListBox.Items.Add( "Press Reset to Restart..." );
                messagesListBox.SelectedIndex = messagesListBox.Items.Count - 1;
                gameOver = true;
                GameTimer.Enabled = false;
                points -= 10000;
                pointsLabel.Text = "Points: " + points;
            }

            //display message if wumpus is killed, add points
            if (wumpusDead && !messagesListBox.Items.Contains( "You killed the Wumpus!" ) )
            {
                messagesListBox.Items.Add( "You killed the Wumpus!" );
                messagesListBox.SelectedIndex = messagesListBox.Items.Count - 1;
                points += 500;
                pointsLabel.Text = "Points: " + points;
            }

            //display message if gold is delivered to start of world, set game over flag to true
            if (playerIndex.X == 0 && playerIndex.Y == 4 && foundGold)
            {
                messagesListBox.Items.Add( "You Win!" );
                messagesListBox.Items.Add( "Press Reset to Restart..." );
                messagesListBox.SelectedIndex = messagesListBox.Items.Count - 1;
                gameOver = true;
                GameTimer.Enabled = false;
            }
        }

        /// <summary>
        /// If keyboard is pressed, check to see if player is currently playing and if appropriate key was pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WumpusWorldForm_KeyDown( object sender, KeyEventArgs e )
        {
            //if game is in player mode and the game is not over
            if( playerMode && !gameOver)
            {
                //if up arrow is pressed, move player up
                if( e.KeyCode == Keys.Up )
                {
                    MoveUp();                    
                }
                //else if down arrow is pressed, move player down
                else if( e.KeyCode == Keys.Down )
                {
                    MoveDown();                    
                }
                //else if right arrow is pressed, move player right
                else if( e.KeyCode == Keys.Right )
                {
                    MoveRight();
                }
                //else if left arrow is pressed, move player left
                else if( e.KeyCode == Keys.Left )
                {
                    MoveLeft();
                }
                //else if enter button is pressed, either arm player or fire arrow
                else if( e.KeyCode == Keys.Enter )
                {
                    EnterButton();                    
                }
                //else if escape button is pressed, unarm player
                else if (e.KeyCode == Keys.Escape)
                {
                    EscapeButton();
                }

                //redraw grid and check state of game
                DrawGrid();
                CheckState();
            }
        }

        /// <summary>
        /// Increment game timer once per second
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameTimer_Tick( object sender, EventArgs e )
        {
            time++;
            timeLabel.Text = "Time: " + time;
        }

        /// <summary>
        /// Perform one AI move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stepButton_Click( object sender, EventArgs e )
        {
            //if game is not over and is in AI mode, move one step
            if( AIMode && !gameOver)
            {
                AIChoice( AI.NextMove() );                
            }
        }

        /// <summary>
        /// Start AI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startButton_Click( object sender, EventArgs e )
        {
            //if currently in AI mode
            if (AIMode)
            {
                //switch button text between start and stop based on current action
                //enable main loop of AI by starting InterfaceTimer
                if( startButton.Text == "Start" )
                {
                    startButton.Text = "Stop";
                    InterfaceTimer.Enabled = true;
                }
                else
                {
                    startButton.Text = "Start";
                    InterfaceTimer.Enabled = false;
                }
            }
        }

        /// <summary>
        /// InterfaceTimer which ensures buttons are still usable as AI runs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InterfaceTimer_Tick( object sender, EventArgs e )
        {
            //if AI mode is active an game is not over, perform next step for AI
            if( AIMode && !gameOver )
            {
                AIChoice( AI.NextMove() );
            } 
        }

        /// <summary>
        /// Determine which way to move player based on feedback from AI
        /// </summary>
        /// <param name="choice"></param>
        private void AIChoice(int choice)
        {
            switch( choice )
            {
                case 0:
                    MoveUp();
                    break;
                case 1:
                    MoveDown();
                    break;
                case 2:
                    MoveRight();
                    break;
                case 3:
                    MoveLeft();
                    break;
                //if enter button is pressed, arm player and aim player, pause, fire, pause, unarm and continue
                case 4:
                    EnterButton();
                    DrawGrid();
                    Thread.Sleep( 1000 );
                    EnterButton();
                    DrawGrid();
                    Thread.Sleep( 1000 );
                    EscapeButton();
                    break;
                case 5:
                    EscapeButton();
                    break;
            }

            //redraw grid and check game state
            DrawGrid();
            CheckState();
        }

        /// <summary>
        /// Move player up one spot, or if armed, aim arrow
        /// </summary>
        private void MoveUp()
        {
            //if player is armed, aim
            if( playerArmed )
            {
                shootUp = true;
                shootDown = false;
                shootRight = false;
                shootLeft = false;
            }
            //else if player is not armed move player
            else if( playerIndex.Y > 0 )
            {
                worldGrid[ playerIndex.X, playerIndex.Y ].isPlayer = false;
                worldGrid[ playerIndex.X, --playerIndex.Y ].isPlayer = true;
                worldGrid[ playerIndex.X, playerIndex.Y ].spotVisited = true;
            }

            //subtract one point for each move performed
            points--;
            pointsLabel.Text = "Points: " + points;
        }

        /// <summary>
        /// Move player down one spot, or if armed, aim arrow
        /// </summary>
        private void MoveDown()
        {
            //if player is armed, aim
            if( playerArmed )
            {
                shootUp = false;
                shootDown = true;
                shootRight = false;
                shootLeft = false;
            }
            //else if player is not armed move player
            else if( playerIndex.Y < 4 )
            {
                worldGrid[ playerIndex.X, playerIndex.Y ].isPlayer = false;
                worldGrid[ playerIndex.X, ++playerIndex.Y ].isPlayer = true;
                worldGrid[ playerIndex.X, playerIndex.Y ].spotVisited = true;
            }

            //subtract one point for each move performed
            points--;
            pointsLabel.Text = "Points: " + points;
        }

        /// <summary>
        /// Move player right one spot, or if armed, aim arrow
        /// </summary>
        private void MoveRight()
        {
            //if player is armed, aim
            if( playerArmed )
            {
                shootUp = false;
                shootDown = false;
                shootRight = true;
                shootLeft = false;
            }
            //else if player is not armed move player
            else if( playerIndex.X < 4 )
            {
                worldGrid[ playerIndex.X, playerIndex.Y ].isPlayer = false;
                worldGrid[ ++playerIndex.X, playerIndex.Y ].isPlayer = true;
                worldGrid[ playerIndex.X, playerIndex.Y ].spotVisited = true;
            }

            //subtract one point for each move performed
            points--;
            pointsLabel.Text = "Points: " + points;
        }

        /// <summary>
        /// Move player left one spot, or if armed, aim arrow
        /// </summary>
        private void MoveLeft()
        {
            //if player is armed, aim
            if( playerArmed )
            {
                shootUp = false;
                shootDown = false;
                shootRight = false;
                shootLeft = true;
            }
            //else if player is not armed move player
            else if( playerIndex.X > 0 )
            {
                worldGrid[ playerIndex.X, playerIndex.Y ].isPlayer = false;
                worldGrid[ --playerIndex.X, playerIndex.Y ].isPlayer = true;
                worldGrid[ playerIndex.X, playerIndex.Y ].spotVisited = true;
            }

            //subtract one point for each move performed
            points--;
            pointsLabel.Text = "Points: " + points;
        }

        /// <summary>
        /// Arm player, or if armed, fire arrow in specified direction
        /// </summary>
        private void EnterButton()
        {
            //if player is not armed, arm player
            if( !playerArmed )
            {
                playerArmed = true;
            }
            //else if armed and player has arrows remaining, aim and fire
            else if( arrows > 0 )
            {
                if( shootUp && playerIndex.Y > 0 )
                {
                    if( worldGrid[ playerIndex.X, playerIndex.Y - 1 ].isWumpus )
                    {
                        wumpusDead = true;
                    }
                }
                else if( shootDown && playerIndex.Y < 4 )
                {
                    if( worldGrid[ playerIndex.X, playerIndex.Y + 1 ].isWumpus )
                    {
                        wumpusDead = true;
                    }
                }
                else if( shootRight && playerIndex.X < 4 )
                {
                    if( worldGrid[ playerIndex.X + 1, playerIndex.Y ].isWumpus )
                    {
                        wumpusDead = true;
                    }
                }
                else if( shootLeft && playerIndex.X > 0 )
                {
                    if( worldGrid[ playerIndex.X - 1, playerIndex.Y ].isWumpus )
                    {
                        wumpusDead = true;
                    }
                }

                arrows = 0;
                arrowsLabel.Text = "Arrows: " + arrows;
            }
        }

        /// <summary>
        /// Unarm player
        /// </summary>
        private void EscapeButton()
        {
            playerArmed = false;
            shootUp = false;
            shootDown = false;
            shootRight = false;
            shootLeft = false;
        }


    }
}

