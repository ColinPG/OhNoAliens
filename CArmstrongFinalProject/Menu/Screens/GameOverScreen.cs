/* GameOverScreen.cs
 * Description: GameOverScreen is a class that inherits from the SubMenuScreen class. 
 * It is a Screen that displays a list of informative information the player's score for that session of playing.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.06: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// GameOverScreen: A class that inherits from the SubMenuScreen class. 
    /// It is a Screen displays a list of informative information the player's score for that session of playing.
    /// </summary>
    internal class GameOverScreen : SubMenuScreen
    {
        private Score score;
        private HighScoreScreen highScoreScreen;
        private string name;
        private string nameFiller;
        private Keys[] letterKeys;
        private double delayBeforeReadingNameInMs;

        private Game1 game;
        private MenuItem highScoreTitle;
        private const string HighScoreTitleText = "Game Over!";

        private MenuItem enterNameItem;
        private const string enterNameItemText = "Please Enter Your Name.";

        private List<object> descList;
        private MenuItemList descMenuList;

        private List<object> detailsList;
        private MenuItemList detailsMenuList;
        private const int SPACE_BETWEEN_MENUITEMS = 55;
        private const int PULSE_AMOUNT = 0; // 0 = No pulsing
        private const int PULSE_RATE = 0;

        /// <summary>
        /// The Primary constructor for the GameOverScreen class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the parent of this class.</param>
        /// <param name="screenManager">A reference to the ScreenManager class that is the controller of this class.</param>
        public GameOverScreen(Game game, ScreenManager screenManager)
            : base(game, screenManager)
        {
            this.game = (Game1)game;
            descList = new List<object>
            { 
                "Name:",
                "Waves Survived:",
                "Enemy Kills:",
                "Total Score:"
            };
            detailsList = new List<object>
            {
                "-----",
                0,
                0,
                0,
            };

            letterKeys = new Keys[] 
            { Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K,
                Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V,
                Keys.W, Keys.X, Keys.Y, Keys.Z };
            delayBeforeReadingNameInMs = 1000;
            name = "";
            nameFiller = "-----";

            highScoreTitle = new MenuItem(game,
                screenManager,
                this.game.PositionOnScreen(0.50f, 0.05f, xOffsetInPixels: -(screenManager.HighLightMenuFont.MeasureString(HighScoreTitleText).X / 2)),
                HighScoreTitleText,
                screenManager.HighLightMenuFont);
            this.Components.Add(highScoreTitle);

            enterNameItem = new MenuItem(game,
                screenManager,
                this.game.PositionOnScreen(0.50f, 0.55f, xOffsetInPixels: -(screenManager.HighLightMenuFont.MeasureString(enterNameItemText).X / 2)),
                enterNameItemText,
                screenManager.HighLightMenuFont);
            this.Components.Add(enterNameItem);
            
            enterNameItem.Enabled = false;

            descMenuList = new MenuItemList(
                parent,
                descList,
                this.game.PositionOnScreen(0.3f, 0.2f),
                screenManager.MenuFont,
                screenManager.HighLightMenuFont,
                SPACE_BETWEEN_MENUITEMS,
                PULSE_AMOUNT,
                PULSE_RATE);
            descMenuList.UpdateSelectedIndex(3);
            descMenuList.SetColor(Color.Red, Color.Yellow);
            this.Components.Add(descMenuList);

            detailsMenuList = new MenuItemList(
                parent,
                detailsList,
                this.game.PositionOnScreen(0.6f, 0.2f),
                screenManager.MenuFont,
                screenManager.HighLightMenuFont,
                SPACE_BETWEEN_MENUITEMS,
                PULSE_AMOUNT,
                PULSE_RATE);
            detailsMenuList.UpdateSelectedIndex(3);
            detailsMenuList.SetColor(Color.White, Color.Yellow);
            this.Components.Add(detailsMenuList);
        }

        /// <summary>
        /// SetScore is a method that retrieves the score information from an instance of a PlayScreen.
        /// </summary>
        /// <param name="sender">The instance of the PlayScreen that just ended, containing the users score.</param>
        /// <param name="highScoreScreen">The instance of the highScoreScreen, allowing the users score 
        /// to be passed to it and saved if appropriate.</param>
        internal void SetScore(PlayScreen sender, HighScoreScreen highScoreScreen)
        {
            score = sender.Score.GetScore();
            score.Name = nameFiller;
            this.highScoreScreen = highScoreScreen;
            RebuildDetailsList();
        }

        /// <summary>
        /// RebuildDetailsList is a method that rebuilds the score details list.
        /// </summary>
        private void RebuildDetailsList()
        {
            detailsList[0] = name + nameFiller;
            detailsList[1] = score.Wave + " x 100";
            detailsList[2] = score.Kills + " x 10";
            detailsList[3] = score.Total;
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method allows a user to type in their name.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            if(delayBeforeReadingNameInMs > 0)
            {
                delayBeforeReadingNameInMs -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (delayBeforeReadingNameInMs <= 0)
                    enterNameItem.Enabled = true;
            }
            if (enterNameItem.Enabled)
            {
                if (name.Length != 5)
                {
                    //Small optimization to not check for inputs every frame, only when changed. It is quite a big loop and could drain resources.
                    if (parent.InputManager.Ks != parent.InputManager.OldKs)
                        foreach (Keys k in letterKeys)
                            if (parent.InputManager.SingleKeyPress(k))
                            {
                                name += k.ToString();
                                nameFiller = nameFiller.Substring(0, nameFiller.Length - 1);
                                RebuildDetailsList();
                                score.Name = name;
                                if (name.Length >= 5)
                                {
                                    enterNameItem.Enabled = false;
                                    enterNameItem.Visible = false;
                                }    
                                break;
                            }
                }
            }   
            if(returnItem.MouseHoverAndLeftClick() || parent.InputManager.SingleKeyPress(Keys.Escape))
                highScoreScreen.TryToAddHighScore(score);
            base.Update(gameTime);
        }
    }
}