/* MenuItem.cs
 * Description: MenuItem is a class that inherits from the DrawableGameComponent class. 
 * It stores and draws a specified string to the screen a specified positon.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.05: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// MenuItem: A class that inherits from the DrawableGameComponent class. 
    /// It stores and draws a specified string to the screen a specified positon.
    /// </summary>
    internal class MenuItem : DrawableGameComponent
    {
        protected Game1 parent;
        protected ScreenManager screenManager;
        protected Color itemFontColor;
        protected Vector2 itemPosition;
        protected string itemText;
        protected SpriteFont itemFont;

        /// <summary>
        /// Primary constructor of the MenuItem class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the game parent of this class.</param>
        /// <param name="screenManager">A reference to the ScreenManager that is the parent this class.</param>
        /// <param name="position">The top-left position of the menu item.</param>
        /// <param name="text">The text of the menu item.</param>
        /// <param name="itemFont">The Font of the menu item.</param>
        public MenuItem(Game game, ScreenManager screenManager, Vector2 position, string text, SpriteFont itemFont) : base(game)
        {
            this.parent = (Game1)game;
            this.screenManager = screenManager;
            this.itemPosition = position;
            this.itemText = text;
            this.itemFont = itemFont;
            SetColor(Color.White);
        }

        /// <summary>
        /// SetColor is a method that sets the color of the text to be drawn.
        /// </summary>
        /// <param name="newColor">The new Color of the text. Default White.</param>
        protected void SetColor(Color newColor)
        {
            itemFontColor = newColor;
        }

        /// <summary>
        /// UpdateText is a method that updates the text of the item.
        /// </summary>
        /// <param name="text">The new text to be draw.</param>
        protected void UpdateText(string text)
        {
            itemText = text;
        }

        /// <summary>
        /// Draw is an overriden method that all DrawableGameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Draw method the item text the positon stored.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Draw(GameTime gameTime)
        {
            parent.SpriteBatch.Begin();
            parent.SpriteBatch.DrawString(itemFont, itemText, itemPosition, itemFontColor);
            parent.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}