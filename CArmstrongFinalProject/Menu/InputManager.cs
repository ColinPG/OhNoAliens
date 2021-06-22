/* InputManager.cs
 * Description: InputManager is a class that inherits from the GameComponent class. 
 * It contains and reads all Keyboard and Mouse Inputs.
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
    /// InputManager: A class that inherits the GameComponent class, this class contains
    /// and reads all Keyboard and Mouse Inputs.
    /// </summary>
    public class InputManager : GameComponent
    {
        private KeyboardState ks;
        /// <summary>
        /// Property of the current state of the Keyboard.
        /// </summary>
        public KeyboardState Ks { get => ks; }

        private MouseState ms;
        /// <summary>
        /// Property of the current state of the Mouse.
        /// </summary>
        public MouseState Ms { get => ms; }

        private KeyboardState oldKs;
        /// <summary>
        /// Property of the previous state of the keyboard.
        /// </summary>
        public KeyboardState OldKs { get => oldKs; }

        private MouseState oldMs;
        /// <summary>
        /// Property of the previous state of the Mouse.
        /// </summary>
        public MouseState OldMS { get => oldMs; }

        /// <summary>
        /// The Primary constructor for the InputManager class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the parent of this class.</param>
        public InputManager(Game game) : base(game)
        {
        }

        /// <summary>
        /// SingleKeyPress is a method that checks if a single specified key was pressed once.
        /// </summary>
        /// <param name="key">The key to check for being pressed.</param>
        /// <returns>Returns true if the specified key was pressed once. Else false.</returns>
        public bool SingleKeyPress(Keys key)
        {
            return Ks.IsKeyDown(key) 
                && OldKs.IsKeyUp(key);
        }

        /// <summary>
        /// SingleLeftClick is a method that checks if the left mouse button was pressed once.
        /// </summary>
        /// <returns>Returns true if the left mouse button was pressed once. Else false.</returns>
        public bool SingleLeftClick()
        {
            return Ms.LeftButton == ButtonState.Pressed
                && OldMS.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// SingleRightClick is a method that checks if the right mouse button was pressed once.
        /// </summary>
        /// <returns>Returns true if the right mouse button was pressed once. Else false.</returns>
        public bool SingleRightClick()
        {
            return Ms.RightButton == ButtonState.Pressed
                && OldMS.RightButton == ButtonState.Released;
        }

        /// <summary>
        /// LeftClick is a method that checks if the left mouse button was pressed at all.
        /// </summary>
        /// <returns>Returns true if the left mouse button was pressed at all. Else false.</returns>
        internal bool LeftClick()
        {
            return Ms.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// RightClick is a method that checks if the right mouse button was pressed at all.
        /// </summary>
        /// <returns>Returns true if the right mouse button was pressed at all. Else false.</returns>
        internal bool RightClick()
        {
            return Ms.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method simply ipdates the keyboard and mouse states contained in this class.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            oldKs = ks;
            ks = Keyboard.GetState();
            oldMs = ms;
            ms = Mouse.GetState();
            base.Update(gameTime);
        }
    }
}
