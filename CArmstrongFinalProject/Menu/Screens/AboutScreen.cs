/* AboutScreen.cs
 * Description: AboutScreen is a class that inherits from the SubMenuScreen class. 
 * It displays an informative message about who created the program.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.05: Created
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// AboutScreen: A class that inherits from the SubMenuScreen class. 
    /// It displays an informative message about who created the program.
    /// </summary>
    class AboutScreen : SubMenuScreen
    {
        private Texture2D aboutScreenTex;

        /// <summary>
        /// The Primary constructor for the AboutScreen class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the parent of this class.</param>
        /// <param name="screenManager">A reference to the ScreenManager class that is the controller of this class.</param>
        public AboutScreen(Game game, ScreenManager screenManager) : base(game, screenManager)
        {
            aboutScreenTex = game.Content.Load<Texture2D>("Images/Menu/About");
        }

        /// <summary>
        /// Draw is an overriden method that all DrawableGameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Draw method draws the about screen texture.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Draw(GameTime gameTime)
        {
            parent.SpriteBatch.Begin();
            parent.SpriteBatch.Draw(aboutScreenTex, Vector2.Zero, Color.White);
            parent.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
