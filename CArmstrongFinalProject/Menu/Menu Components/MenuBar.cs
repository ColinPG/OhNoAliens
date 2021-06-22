/* MenuBar.cs
 * Description: MenuBar is a class that inherits from the DrawableGameComponent class. 
 * It contains the properties and logic to display a interactable bar with labels on the screen.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.05: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// MenuBar: A class that inherits from the DrawableGameComponent class. 
    /// It contains the properties and logic to display a interactable bar with labels on the screen.
    /// </summary>
    internal class MenuBar : DrawableGameComponent
    {
        private Game1 parent;
        private ScreenManager screenManager;
        private float barValue;
        public float BarValue { get => barValue; set => barValue = value; }
        
        private Vector2 basePosition;
        private float detailOffset;
        private Vector2 minPosition;
        private Vector2 barPosition;
        private Vector2 barFillOffset;
        private Vector2 maxPosition;
        private const float padding = 10;

        private string descLabelText = "";
        private string minLabelText = "0%";
        private string maxLabelText = "100%";

        private Texture2D barTextureFull;
        private Texture2D barTextureEmpty;
        private Rectangle fillBarRect;

        public bool isSelected;
        public bool inDetail;

        /// <summary>
        /// Primary constructor of the MenuBar class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the game parent of this class.</param>
        /// <param name="screenManager">A reference to the ScreenManager that is the parent of this class.</param>
        /// <param name="barVal">The initial bar value.</param>
        /// <param name="basePos">The top left position of the bar.</param>
        /// <param name="detailOffset">The offset in pixels between the detail and the bar.</param>
        /// <param name="barTextureFull">The texture for a filled bar.</param>
        /// <param name="barTextureEmpty">The texture for an empty bar.</param>
        public MenuBar(Game game, ScreenManager screenManager, float barVal, Vector2 basePos, float detailOffset, Texture2D barTextureFull, Texture2D barTextureEmpty) : base(game)
        {
            this.parent = (Game1)game;
            this.screenManager = screenManager;
            this.barValue = barVal;
            this.basePosition = basePos;
            this.detailOffset = detailOffset;
            this.barTextureFull = barTextureFull;
            this.barTextureEmpty = barTextureEmpty;
            barFillOffset = new Vector2(barTextureEmpty.Width - barTextureFull.Width, barTextureEmpty.Height - barTextureFull.Height) / 2;
            RebuildPositions();
            isSelected = false;
            inDetail = false;
        }

        /// <summary>
        /// SetLabels is a method that sets the text of the labels for the description,
        /// minimum and maximum labels around the menu bar.
        /// </summary>
        /// <param name="description">The new text for the description label.</param>
        /// <param name="minLabel">The new text for the minimum label.</param>
        /// <param name="maxLabel">The new text for the maximum label.</param>
        internal void SetLabels(string description, string minLabel = "0%", string maxLabel = "100%")
        {
            descLabelText = description;
            minLabelText = minLabel;
            maxLabelText = maxLabel;
            RebuildPositions();
        }

        /// <summary>
        /// RebuildPositions is a method that recalculates the positions of the various labels and the menu bar.
        /// </summary>
        private void RebuildPositions()
        {
            Vector2 descSize = screenManager.MenuFont.MeasureString(descLabelText);
            Vector2 minSize = screenManager.MenuDetailFont.MeasureString(minLabelText);

            minPosition = basePosition;
            minPosition.X += detailOffset;
            barPosition = minPosition;
            barPosition.X += minSize.X + padding;
            maxPosition = barPosition;
            maxPosition.X += barTextureEmpty.Width + padding;
            minPosition.Y += minSize.Y / 2;
            maxPosition.Y += minSize.Y / 2;

            fillBarRect = new Rectangle(
                (int)(barPosition.X + barFillOffset.X), (int)(barPosition.Y + barFillOffset.Y), barTextureFull.Width, barTextureFull.Height);
        }

        /// <summary>
        /// CheckForBarInput is a method that checks for relevant mouse or keyboard input to adjust the bar value.
        /// </summary>
        internal void CheckForBarInput()
        {
            if (fillBarRect.Contains(parent.InputManager.Ms.Position))
                if(parent.InputManager.LeftClick())
                {
                    barValue = (float)(parent.InputManager.Ms.X - fillBarRect.X) / 100;
                }
            if(parent.InputManager.SingleKeyPress(Keys.Left))
            {
                barValue -= 0.1f;
            }
            if (parent.InputManager.SingleKeyPress(Keys.Right))
            {
                barValue += 0.1f;
            }
            barValue = MathHelper.Clamp(barValue, 0, 1f);
            barValue = (float)Math.Round((double)barValue * 100) / 100;
        }

        /// <summary>
        /// Draw is an overriden method that all DrawableGameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Draw method draws the description text, minmum label, menubar, and the maximum label.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Draw(GameTime gameTime)
        {
            parent.SpriteBatch.Begin();
            Color descColor = Color.White;
            if (isSelected)
                descColor = Color.Red;
            parent.SpriteBatch.DrawString(screenManager.MenuFont, descLabelText, basePosition, descColor);
            if (!inDetail)
                descColor = Color.White;
            else
                descColor = Color.Red;
            parent.SpriteBatch.DrawString(screenManager.MenuDetailFont, minLabelText, minPosition, descColor);
            parent.SpriteBatch.Draw(barTextureEmpty, barPosition, Color.White);
            Rectangle drawFillRect = new Rectangle((int)(barPosition.X + barFillOffset.X), (int)(barPosition.Y + barFillOffset.Y), (int)(barValue * barTextureFull.Width), barTextureFull.Height);
            parent.SpriteBatch.Draw(barTextureFull, drawFillRect, Color.White);
            parent.SpriteBatch.DrawString(screenManager.MenuDetailFont, maxLabelText, maxPosition, descColor);
            parent.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}