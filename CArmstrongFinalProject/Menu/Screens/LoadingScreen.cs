/* LoadingScreen.cs
 * Description: LoadingScreen is a class that inherits from the GameScreen class. 
 * It is a Screen that is displayed temporarily before the another specified screen is shown.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.05: Created
 */
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// LoadingScreen: A Screen that is displayed temporarily before the another specified screen is shown.
    /// </summary>
    internal class LoadingScreen : GameScreen
    {
        private SpriteFont loadingFont;
        private Color fontColor = Color.White;
        private string loadingText = "Loading";
        private double timeSinceLoadingScreenStartedInMilliseconds = 0;
        private double timeToLoadInMilliseconds = 0;

        private const int DOT_FREQUENCY = 200;
        private int dots = 0;
        private double timeSinceLastDot = 0;

        private string screenToLoadName;

        private Vector2 loadingTextPosition;

        /// <summary>
        /// Primary constructor for the LoadingScreen class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the parent of this class.</param>
        /// <param name="screenManager">A reference to the ScreenManager class that is the controller of this class.</param>
        public LoadingScreen(Game game, ScreenManager screenManager) : base(game, screenManager)
        {
            this.loadingFont = parent.Content.Load<SpriteFont>("Fonts/loadingFont");
        }

        /// <summary>
        /// LoadScreen is a method that reset's the loadingScreen's loading values to default, 
        /// records the name of the screen it is loading, and how long it will take to load.
        /// </summary>
        /// <param name="screenToLoadName">The name of the screen this class will load.</param>
        /// <param name="timeToLoad">An option parameter, the amount of time the Loading Screen will exist before loading the next screen.</param>
        public void LoadScreen(string screenToLoadName, double timeToLoad = 1200)
        {
            //reset values
            timeSinceLoadingScreenStartedInMilliseconds = 0;
            dots = 0;
            loadingTextPosition = parent.PositionOnScreen(0.2f, 0.8f);
            this.timeToLoadInMilliseconds = timeToLoad;
            this.screenToLoadName = screenToLoadName;
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method simply updates the loading text animation properties, and checks if it is 
        /// time to load the next screen.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            timeSinceLoadingScreenStartedInMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;
            timeSinceLastDot += gameTime.ElapsedGameTime.TotalMilliseconds;
            if(timeToLoadInMilliseconds < timeSinceLoadingScreenStartedInMilliseconds)
            {
                screenManager.ChangeScreen(this, screenToLoadName);
            }
            if(timeSinceLastDot > DOT_FREQUENCY)
            {
                timeSinceLastDot -= DOT_FREQUENCY;
                dots++;
                if (dots > 3)
                    dots = 0;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw is an overriden method that all DrawableGameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Draw method simply draws the loading screen text.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Draw(GameTime gameTime)
        {
            parent.SpriteBatch.Begin();
            string loadingTextWithDots = loadingText;
            for (int i = 0; i < dots; i++)
                loadingTextWithDots += ".";
            parent.SpriteBatch.DrawString(loadingFont, loadingTextWithDots, loadingTextPosition, fontColor);
            parent.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}