/* HelpScreen.cs
 * Description: HelpScreen is a class that inherits from the SubMenuScreen class. 
 * It displays an informative pictures about how to play the game.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.06: Created
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// HelpScreen: A class that inherits from the SubMenuScreen class. 
    /// It displays an informative pictures about how to play the game.
    /// </summary>
    class HelpScreen : SubMenuScreen
    {
        private Texture2D[] helpPictures;
        private const int NUM_HELP_SCREENS = 7;
        private int currentHelpScreen = 0;

        private MenuItem title;
        private ClickableMenuItem next;
        private ClickableMenuItem previous;

        /// <summary>
        /// The Primary constructor for the HelpScreen class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the parent of this class.</param>
        /// <param name="screenManager">A reference to the ScreenManager class that is the controller of this class.</param>
        public HelpScreen(Game game, ScreenManager screenManager) : base(game, screenManager)
        {
            helpPictures = new Texture2D[NUM_HELP_SCREENS];
            for (int i = 0; i < NUM_HELP_SCREENS; i++)
            {
                helpPictures[i] = parent.Content.Load<Texture2D>("Images/Menu/help" + i.ToString());
            }

            SpriteFont titleFont = screenManager.HighLightMenuFont;
            SpriteFont itemFont = screenManager.HighLightMenuFont;

            string titleText = "Help:";
            Vector2 titleTextSize = itemFont.MeasureString(titleText);
            title = new MenuItem(game,
                screenManager,
                parent.PositionOnScreen(0.50f, 0.03f, xOffsetInPixels: -(titleTextSize.X / 2)),
                titleText,
                titleFont);
            Components.Add(title);

            string prevText = "< Prev";
            Vector2 prevTextSize = itemFont.MeasureString(prevText);
            previous = new ClickableMenuItem(game,
                screenManager,
                parent.PositionOnScreen(0.02f, 0.50f, yOffsetInPixels: -(prevTextSize.Y / 2)),
                prevText,
                itemFont);
            Components.Add(previous);

            string nextText = "Next >";
            Vector2 nextTextSize = itemFont.MeasureString(nextText);
            next = new ClickableMenuItem(game,
                screenManager,
                parent.PositionOnScreen(0.98f, 0.50f, -nextTextSize.X, -(nextTextSize.Y / 2)),
                nextText,
                itemFont);
            Components.Add(next);
        }

        /// <summary>
        /// Show overrides the based Show method to reset the screenCenter variable on screen show,
        /// incase the screen size has changed since this screen was created.
        /// Also resets the picture index to 0.
        /// </summary>
        public override void Show()
        {
            currentHelpScreen = 0;
            base.Show();
            UpdatePictureIndex(0);
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method checks for user input to navigate through the help pictures.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            if (previous.MouseHoverAndLeftClick() || parent.InputManager.SingleKeyPress(Keys.Left))
            {
                UpdatePictureIndex(-1);
            }
            if (next.MouseHoverAndLeftClick() || parent.InputManager.SingleKeyPress(Keys.Right))
            {
                UpdatePictureIndex(1);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// UpdatePictureIndex is a helper method that changes the selected help image.
        /// </summary>
        /// <param name="offset">The amount of picture index to change.</param>
        private void UpdatePictureIndex(int offset)
        {
            currentHelpScreen += offset;
            currentHelpScreen = MathHelper.Clamp(currentHelpScreen, 0, NUM_HELP_SCREENS - 1);
            if (currentHelpScreen == 0)
                previous.Visible = false;
            else
                previous.Visible = true;

            if (currentHelpScreen == NUM_HELP_SCREENS - 1)
                next.Visible = false;
            else
                next.Visible = true;
        }

        /// <summary>
        /// Draw is an overriden method that all DrawableGameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Draw method draws the current help texture.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Draw(GameTime gameTime)
        {
            parent.SpriteBatch.Begin();
            parent.SpriteBatch.Draw(helpPictures[currentHelpScreen], Vector2.Zero, Color.White);
            parent.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
