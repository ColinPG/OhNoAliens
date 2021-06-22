/* GameScreen.cs
 * Description: GameScreen is an abstract class that inherits from the DrawableGameComponent class. 
 * It is a Screen that can be displayed by the ScreenManager class.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.04: Created
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
    /// GameScreen: This is an abstract class that is a Screen that can be displayed by the ScreenManager class.
    /// </summary>
    abstract class GameScreen : DrawableGameComponent
    {
        protected Game1 parent;
        protected ScreenManager screenManager;
        /// <summary>
        /// Property for Components, a list of GameComponents.
        /// </summary>
        public List<GameComponent> Components { get; set; }

        /// <summary>
        /// The Primary constructor for the GameScreen class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the parent of this class.</param>
        /// <param name="screenManager">A reference to the Game class that is the controller of this class.</param>
        public GameScreen(Game game, ScreenManager screenManager) : base(game)
        {
            parent = (Game1)game;
            this.screenManager = screenManager;
            Components = new List<GameComponent>();
            Hide();
        }

        /// <summary>
        /// SetState sets the GameScreen's Enabled and Visibility states and all it's subcomponents to a specified state.
        /// </summary>
        /// <param name="state">The state which the scree and it's components will be set to.</param>
        private void SetState(bool state)
        {
            this.Enabled = state;
            this.Visible = state;
            foreach (GameComponent item in Components)
            {
                item.Enabled = state;
                if(item is DrawableGameComponent)
                {
                    DrawableGameComponent comp = (DrawableGameComponent)item;
                    comp.Visible = state;
                }
            }
        }

        /// <summary>
        /// Hide set's the state of the screen and it's components to false.
        /// </summary>
        public virtual void Hide()
        {
            SetState(false);
        }

        /// <summary>
        /// Show set's the state of the screen and it's components to true.
        /// </summary>
        public virtual void Show()
        {
            SetState(true);
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method simply updates all the screen's components.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            foreach(GameComponent item in Components)
            {
                if(item.Enabled)
                {
                    item.Update(gameTime);
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw is an overriden method that all DrawableGameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Draw method simply draw all the screen's drawable components.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Draw(GameTime gameTime)
        {
            DrawableGameComponent comp = null;
            foreach (GameComponent item in Components)
            {
                if (item is DrawableGameComponent)
                {
                    comp = (DrawableGameComponent)item;
                    if(comp.Visible)
                    {
                        comp.Draw(gameTime);
                    }
                }
            }
            base.Draw(gameTime);
        }
    }
}
