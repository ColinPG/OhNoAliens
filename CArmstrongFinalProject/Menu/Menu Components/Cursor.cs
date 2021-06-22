/* Cursor.cs
 * Description: Cursor is a class that inherits from the DrawableGameComponent class. 
 * It contains a cursor texture and draws it to the current mouse position.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.05: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// Cursor: A class that inherits from the DrawableGameComponent class. 
    /// It contains a cursor texture and draws it to the current mouse position.
    /// </summary>
    internal class Cursor : DrawableGameComponent
    {
        private Game1 parent;
        private Texture2D cursorTex;
        private Vector2 position;
        private Vector2 origin;

        /// <summary>
        /// Primary constructor of the Cursor class.
        /// </summary>
        /// <param name="parent">A reference to the Game class that is the game parent of this class.</param>
        public Cursor(Game parent) : base (parent)
        {
            this.parent = (Game1)parent;
            position = Vector2.Zero;
            cursorTex = parent.Content.Load<Texture2D>("Images/Menu/cursor0");
            origin = cursorTex.Bounds.Center.ToVector2();
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method simply updates the Cursor position to the mouse position.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            position = parent.InputManager.Ms.Position.ToVector2();
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw is an overriden method that all DrawableGameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Draw method draws the cursor texture at the position of the mouse.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Draw(GameTime gameTime)
        {
            parent.SpriteBatch.Begin();
            parent.SpriteBatch.Draw(cursorTex, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0);
            parent.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}