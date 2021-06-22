/* ClickableMenuItem.cs
 * Description: ClickableMenuItem is a class that inherits from the MenuItem class. 
 * It allows the existing menu item to be interacted with by the user.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.05: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// ClickableMenuItem: A class that inherits from the MenuItem class. 
    /// It allows the existing menu item to be interacted with by the user.
    /// </summary>
    internal class ClickableMenuItem : MenuItem
    {
        private bool mouseHover;
        private Color itemRegularFontColor;
        private Color itemHoverFontColor;
        protected Rectangle itemRect;

        /// <summary>
        /// Primary constructor of the ClickableMenuItem class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the game parent of this class.</param>
        /// <param name="screenManager">A reference to the ScreenManager that is the parent this class.</param>
        /// <param name="position">The top-left position of the menu item.</param>
        /// <param name="text">The text of the menu item.</param>
        /// <param name="itemFont">The Font of the menu item.</param>
        public ClickableMenuItem(Game game, ScreenManager screenManager, Vector2 position, string text, SpriteFont itemFont)
            : base(game, screenManager, position, text, itemFont)
        {
            SetColor(Color.White, Color.Red);
            Vector2 textSize = screenManager.MenuFont.MeasureString(itemText);
            this.itemRect = new Rectangle((int)itemPosition.X, (int)itemPosition.Y, (int)textSize.X, (int)textSize.Y);
        }

        /// <summary>
        /// MouseHover is a method that checks if Mouse if hovering over the item.
        /// </summary>
        /// <returns>True if Mouse if hovering over the item. Otherwise false.</returns>
        internal bool MouseHover()
        {
            return parent.InputManager.SingleLeftClick();
        }

        /// <summary>
        /// MouseHoverAndClick is a method that checks if Mouse if hovering over the item and left clicking.
        /// </summary>
        /// <returns>True if Mouse if hovering over the item and left clicking. Otherwise false.</returns>
        internal bool MouseHoverAndLeftClick()
        {
            return parent.InputManager.SingleLeftClick() && mouseHover;
        }

        /// <summary>
        /// SetColor is a method that Sets the colors of the text to be drawn, both the default and onHover Colors.
        /// </summary>
        /// <param name="regularColor">Color for normal drawing. Default White.</param>
        /// <param name="hoverColor">Color for when mouse is on top of text. Default Red.</param>
        protected void SetColor(Color regularColor, Color hoverColor)
        {
            itemRegularFontColor = regularColor;
            itemHoverFontColor = hoverColor;
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method simply checks if the user if hover over the menu item or not.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            if (itemRect.Contains(parent.InputManager.Ms.Position))
            {
                itemFontColor = itemHoverFontColor;
                mouseHover = true;
            }
            else
            {
                itemFontColor = itemRegularFontColor;
                mouseHover = false;
            }
            base.Update(gameTime);
        }
    }
}