/* GameBackGround.cs
 * Description: GameBackGround is a class that inherits from the GameComponent class. 
 * It creates a imitation of a 3D background
 * by using multiple textures and scaling them on Camera movements.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.06: Created
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// GameBackGround: This class inherits GameComponent and creates a imitation of a 3D background 
    /// by using multiple textures and scaling them on Camera movements.
    /// </summary>
    internal class GameBackGround : GameComponent
    {
        private Game1 parent;
        private Texture2D backgroundTex;
        private Texture2D midgroundTex;
        private Texture2D foregroundTex;
        private Vector2 midGroundPosition;
        private Vector2 foreGroundPosition;
        private Vector2 midOrigin;
        private Vector2 foreOrigin;
        private Vector2 offset = new Vector2(200, 200);
        private float scaleMin = 0.9f;
        private float midScale;
        private float foreScale;

        /// <summary>
        /// Primary constructor for the GameBackGround class.
        /// </summary>
        /// <param name="game">The Game class that is the game parent class of this object.</param>
        public GameBackGround(Game game) : base (game)
        {
            string backGroundpath = "Images/background/Nebula";
            string midGroundpath = "Images/background/StarsBig_1PC";
            string foreGroundpath = "Images/background/StarsSmall_1";
            parent = (Game1)game;
            parent.AudioManager.PlayGameMusic();
            backgroundTex = game.Content.Load<Texture2D>(backGroundpath);
            midgroundTex = game.Content.Load<Texture2D>(midGroundpath);
            foregroundTex = game.Content.Load<Texture2D>(foreGroundpath);
            midGroundPosition = Vector2.Zero;
            foreGroundPosition = Vector2.Zero;
            midOrigin = new Vector2(midgroundTex.Width / 2, midgroundTex.Height / 2);
            foreOrigin = new Vector2(foregroundTex.Width / 2, foregroundTex.Height / 2);
        }

        /// <summary>
        /// UpdateBackground is a method that updates background positioning and scaling 
        /// based on changes in camera movement.
        /// </summary>
        /// <param name="position">A Vector2 of the position of the Camera.</param>
        /// <param name="scale">A float of the zoom scaling of the Camera.</param>
        internal void UpdateBackground(Vector2 position, float scale)
        {
            midGroundPosition = (-position / 10) + offset;
            foreGroundPosition = (-position / 5) + offset;
            midScale = (scale / 100) + scaleMin;
            foreScale = (scale / 50) + scaleMin;
        }

        /// <summary>
        /// Draw is a method called during the Draw loop that will draw the background to the screen.
        /// This needs to be called first, before rendering the world, so that the background is not drawn on top.
        /// </summary>
        public void Draw()
        {
            parent.SpriteBatch.Begin();
            parent.SpriteBatch.Draw(backgroundTex, Vector2.Zero, Color.White);
            parent.SpriteBatch.Draw(midgroundTex, midGroundPosition, null, Color.White, 0,
                midOrigin, midScale, SpriteEffects.None, 0);
            parent.SpriteBatch.Draw(foregroundTex, foreGroundPosition, null, Color.White, 0,
                foreOrigin, foreScale, SpriteEffects.None, 0);
            parent.SpriteBatch.End();
        }
    }
}