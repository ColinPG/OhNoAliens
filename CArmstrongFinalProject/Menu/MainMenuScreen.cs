/* MainMenuScreen.cs
 * Description: MainMenuScreen is a class that inherits from the GameScreen class. 
 * It displays a list of options to choose from to navigate to different Screens.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.03: Created
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
    /// MainMenuScreen: A class that inherits from the GameScreen class. 
    /// It displays a list of options to choose from to navigate to different Screens.
    /// </summary>
    class MainMenuScreen : GameScreen
    {
        private List<object> menuItems =
            new List<object>
            {
                "Start Game",
                "Help",
                "High Score",
                "Settings",
                "About",
                "Quit"
            };

        private int selectedIndex;
        private const int SPACE_BETWEEN_MENUITEMS = 20;
        private const int PULSE_AMOUNT = 150;
        private const int PULSE_RATE = 15;

        private MenuItemList menuItemList;

        private Texture2D logo;
        private Vector2 logoPos;

        private Cursor cursor;

        /// <summary>
        /// The Primary constructor for the MainMenuScreen class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the parent of this class.</param>
        /// <param name="screenManager">A reference to the ScreenManager class that is the controller of this class.</param>
        public MainMenuScreen(Game game, 
            ScreenManager screenManager) : base(game, screenManager)
        {
            Vector2 position = new Vector2(parent.Graphics.PreferredBackBufferWidth / 9,
                parent.Graphics.PreferredBackBufferHeight / 5);
            menuItemList = new MenuItemList(game,
                menuItems,
                position,
                screenManager.MenuFont,
                screenManager.HighLightMenuFont,
                SPACE_BETWEEN_MENUITEMS,
                PULSE_AMOUNT,
                PULSE_RATE);

            menuItemList.UpdateSelectedIndex(0);
            this.Components.Add(menuItemList);

            logo = game.Content.Load<Texture2D>("Images/Menu/logo");
            logoPos = parent.PositionOnScreen(0.5f, 0.05f, -logo.Width / 2);
            cursor = new Cursor(game);
            parent.Components.Add(cursor);
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method simply calls the ProcessNavigation method.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            ProcessNavigation();
            base.Update(gameTime);
        }

        /// <summary>
        /// ProcessNavigation is a method that checks for relevant navigation input 
        /// and updates the selected item based on that input.
        /// </summary>
        private void ProcessNavigation()
        {
            if (parent.InputManager.SingleKeyPress(Keys.Down))
            {
                selectedIndex++;
            }
            if (parent.InputManager.SingleKeyPress(Keys.Up))
            {
                selectedIndex--;
            }

            if (selectedIndex < 0)
                selectedIndex = menuItems.Count - 1;
            if (selectedIndex >= menuItems.Count)
                selectedIndex = 0;

            menuItemList.UpdateSelectedIndex(selectedIndex);
            int hoveredItem = menuItemList.CheckHoverMenuItem();
            if (hoveredItem != -1)
                selectedIndex = hoveredItem;

            if (menuItemList.CheckForMenuItemClickOrPress(selectedIndex))
                screenManager.ChangeScreen(this, menuItems[selectedIndex].ToString());
        }

        /// <summary>
        /// Draw is an overriden method that all DrawableGameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Draw method simply draws the game logo.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Draw(GameTime gameTime)
        {
            parent.SpriteBatch.Begin();
            parent.SpriteBatch.Draw(logo, logoPos, Color.White);
            parent.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
