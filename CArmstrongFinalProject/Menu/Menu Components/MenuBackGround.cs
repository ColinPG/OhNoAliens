/* MenuBackGround.cs
 * Description: MenuBackGround is a class that inherits from the DrawableGameComponent class. 
 * It is a class that will display as an animated background while the user navigates the Menus.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.05: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// MenuBackGround: This is a class that  inherits from the DrawableGameComponent class and 
    /// will display as an animated background while the user navigates the Menus.
    /// </summary>
    class MenuBackGround : DrawableGameComponent
    {
        private Game1 parent;
        private Texture2D backgroundTex;
        private Texture2D nebulaTex;
        private Vector2 nebulaPosition;
        private float nebulaScale;
        private Vector2 nebulaOrigin;
        private float nebulaRot = 0f;
        private float rotationSpeed = 0f;

        /// <summary>
        /// The Primary constructor for the MenuBackGround class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the parent of this class.</param>
        public MenuBackGround(Game game) : base(game)
        {
            this.parent = (Game1)game;
            backgroundTex = game.Content.Load<Texture2D>("Images/background/starsBackground");//stars background
            nebulaTex = game.Content.Load<Texture2D>("Images/background/galaxy"); //and slowly spinning galaxy
            nebulaPosition = new Vector2(parent.Graphics.PreferredBackBufferWidth * 0.7f,
                parent.Graphics.PreferredBackBufferHeight * 0.5f);
            nebulaOrigin = new Vector2(nebulaTex.Width / 2, nebulaTex.Height / 2);
            this.nebulaScale = 0.25f;
            this.rotationSpeed = 0.0007f;
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method simply updates the nebula rotation.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            nebulaRot += rotationSpeed;
            parent.ClampAngle(nebulaRot);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw is an overriden method that all DrawableGameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Draw method draws the background star texture and a slowly rotating nebula texture.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Draw(GameTime gameTime)
        {
            parent.SpriteBatch.Begin();
            parent.SpriteBatch.Draw(backgroundTex, Vector2.Zero, Color.White);
            parent.SpriteBatch.Draw(nebulaTex, nebulaPosition, null, Color.White, nebulaRot,
                nebulaOrigin, nebulaScale, SpriteEffects.None, 0);
            parent.SpriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Sets the enabled and visible properties to true, as well as starting to play the Menu Music.
        /// </summary>
        internal void Show()
        {
            parent.AudioManager.PlayMenuMusic();
            this.Enabled = true;
            this.Visible = true;
        }

        /// <summary>
        /// Sets the enabled and visible properties to false.
        /// </summary>
        internal void Hide()
        {
            this.Enabled = false;
            this.Visible = false;
        }
    }
}
