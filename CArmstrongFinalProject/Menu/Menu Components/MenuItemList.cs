/* MenuItem.cs
 * Description: MenuItemList is a class that inherits from the DrawableGameComponent class. 
 * It stores and draws a a list of objects to the screen a specified positon, and contains 
 * logic to navigate through them.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.05: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// MenuItemList: a class that inherits from the DrawableGameComponent class. 
    /// It stores and draws a a list of objects to the screen a specified positon, and contains
    /// logic to navigate through them.
    /// </summary>
    internal class MenuItemList : DrawableGameComponent
    {
        private Game1 parent;
        private SpriteFont regularFont;
        private SpriteFont hilightFont;
        private Color regularColor;
        private Color hilightColor;

        private List<object> menuItems;
        private List<Rectangle> menuItemRects;
        private int selectedIndex;
        private Vector2 originPosition;

        private int spaceBetweenMenuItems;
        private int hilightPulseAmount;
        private int hilightPulseRate;

        /// <summary>
        /// Primary constructor of the MenuItemList class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the game parent of this class.</param>
        /// <param name="menuItems">A list of all the Menu Item values.</param>
        /// <param name="originPosition">The top left position of the Menu List.</param>
        /// <param name="regularFont">Font for regular items.</param>
        /// <param name="hilightFont">Font for the hilighted item.</param>
        /// <param name="spaceBetweenMenuItems">The amount of space in pixels between each menu item.</param>
        /// <param name="hilightPulseAmount">An int of how much the selected text will pulsate.</param>
        /// <param name="hilightPulseRate">An int of how often the selected text will pulsate.</param>
        public MenuItemList(Game game,
            List<object> menuItems,
            Vector2 originPosition,
            SpriteFont regularFont, 
            SpriteFont hilightFont,
            int spaceBetweenMenuItems,
            int hilightPulseAmount,
            int hilightPulseRate) 
            : base(game)
        {
            this.parent = (Game1)game;
            this.menuItems = menuItems;
            selectedIndex = -1;
            this.originPosition = originPosition;
            this.regularFont = regularFont;
            this.hilightFont = hilightFont;
            this.spaceBetweenMenuItems = spaceBetweenMenuItems;
            this.hilightPulseAmount = hilightPulseAmount;
            this.hilightPulseRate = hilightPulseRate;
            SetColor(Color.White, Color.Red);
            RebuildMenuItemRects();
        }

        /// <summary>
        /// SetColor is a method that sets the colors of the text to be drawn, both the default and hilight Colors.
        /// </summary>
        /// <param name="regularColor">Color for normal drawing. Default White.</param>
        /// <param name="hilightColor">Color for selected index menu item. Default Red.</param>
        public void SetColor(Color regularColor, Color hilightColor)
        {
            this.regularColor = regularColor;
            this.hilightColor = hilightColor;
        }

        /// <summary>
        /// UpdateMenuItem updates a specific menu item's value.
        /// </summary>
        /// <param name="index">The index of the specifc menu item.</param>
        /// <param name="updatedValue">The new value for the menu item.</param>
        public void UpdateMenuItem(int index, object updatedValue)
        {
            menuItems[index] = updatedValue;
        }

        /// <summary>
        /// RebuildMenuItemRects is a method that recreates the list of rectangles that are used to 
        /// detect collision with the mouse.
        /// </summary>
        private void RebuildMenuItemRects()
        {
            menuItemRects = new List<Rectangle>();
            Vector2 tempPos = originPosition;
            for (int i = 0; i < menuItems.Count; i++)
            {
                Vector2 stringSize = regularFont.MeasureString(menuItems[i].ToString());
                int lineSpacing = regularFont.LineSpacing;
                if (i == selectedIndex)
                {
                    stringSize = hilightFont.MeasureString(menuItems[i].ToString());
                    lineSpacing = hilightFont.LineSpacing;
                }
                Rectangle newRect = new Rectangle((int)originPosition.X, (int)tempPos.Y, (int)stringSize.X, (int)stringSize.Y);
                tempPos.Y += lineSpacing + spaceBetweenMenuItems;
                menuItemRects.Add(newRect);
            }
        }

        /// <summary>
        /// UpdateSelectedIndex sets the selected item in the list of menu items and calls 
        /// the RebuildMenuItemRects method.
        /// </summary>
        /// <param name="newIndex">The number of the newly selected item.</param>
        public void UpdateSelectedIndex(int newIndex)
        {
            selectedIndex = newIndex;
            RebuildMenuItemRects();
        }

        /// <summary>
        /// CheckForMenuItemClickOrPress is a method that checks if a specific item is clicked on or enter is pressed. 
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>True if a specific item is clicked on or enter is pressed.</returns>
        public bool CheckForMenuItemClickOrPress(int index)
        {
            if (menuItemRects[index].Contains(parent.InputManager.Ms.Position))
            {
                if (parent.InputManager.SingleLeftClick())
                    return true;
            }
            if (parent.InputManager.SingleKeyPress(Keys.Enter))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// CheckHoverMenuItem is a method that checks if any menu item is being hovered over by the mouse.
        /// </summary>
        /// <returns>Returns -1 if no menu item is being hovered, 
        /// else returns the number of the menu item that is being hovered.</returns>
        public int CheckHoverMenuItem()
        {
            if (parent.InputManager.Ms.Position != parent.InputManager.OldMS.Position)
                for (int i = 0; i < menuItemRects.Count; i++)
                {
                    if (menuItemRects[i].Contains(parent.InputManager.Ms.Position))
                    {
                        return i;
                    }
                }
            return -1;
        }

        /// <summary>
        /// Draw is an overriden method that all DrawableGameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Draw method the menu items values the positons stored.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Draw(GameTime gameTime)
        {
            parent.SpriteBatch.Begin();
            Vector2 tempPos = originPosition;
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (selectedIndex == i)
                {
                    float scale = 1f + ((float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / hilightPulseAmount) / hilightPulseRate);
                    if (hilightPulseAmount == 0 || hilightPulseRate == 0)
                        scale = 1f;
                    parent.SpriteBatch.DrawString(hilightFont, menuItems[i].ToString(), tempPos, hilightColor, 0f, Vector2.Zero,
                        scale, SpriteEffects.None, 0);
                    tempPos.Y += hilightFont.LineSpacing + spaceBetweenMenuItems;
                }
                else
                {
                    parent.SpriteBatch.DrawString(regularFont, menuItems[i].ToString(), tempPos, regularColor);
                    tempPos.Y += regularFont.LineSpacing + spaceBetweenMenuItems;
                }
            }
            parent.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
