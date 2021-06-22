/* SubMenuScreen.cs
 * Description: SubMenuScreen is an abstract class that inherits from the GameScreen class. 
 * It is a GameScreen that is a a sub screen of the Main Menu screen.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.04: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// SubMenuScreen: An abstract class that inherits from the GameScreen class.
    /// It is a GameScreen that is a a sub screen of the Main Menu screen.
    /// </summary>
    abstract class SubMenuScreen : GameScreen
    {
        protected ClickableMenuItem returnItem;

        /// <summary>
        /// The Primary constructor for the SubMenuScreen class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the parent of this class.</param>
        /// <param name="screenManager">A reference to the Screenmanager class that is the controller of this class.</param>
        public SubMenuScreen(Game game, ScreenManager screenManager) : base(game, screenManager)
        {
            returnItem = new ClickableMenuItem(game, screenManager, new Vector2(10, 10), "< Main Menu", screenManager.MenuFont);
            this.Components.Add(returnItem);
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method simply checks if the return to main menu button has been clicked or pressed.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            if (parent.InputManager.SingleKeyPress(Keys.Escape) || returnItem.MouseHoverAndLeftClick())
            {
                BackToMainMenu();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// BackToMainMenu calls the screenManager to return back to the MainMenuScreen.
        /// </summary>
        protected virtual void BackToMainMenu()
        {
            screenManager.ChangeScreen(this, "Menu");
        }
    }
}
