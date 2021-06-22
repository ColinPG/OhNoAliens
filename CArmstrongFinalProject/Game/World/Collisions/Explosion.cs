/* Explosion.cs
 * Description: Explosion.cs is a class file at contains the Explosion class.
 * The Explosion class contains the fields and logic to play an animated explosion image.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.04: Created
 *      
 * Note: This class is based off the example covered in PROG2370- "Mono Game Bouncing Ball Game" by Reaz Ahmed
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// Explosion: This class contains the fields and logic to play an animated explosion image.
    /// </summary>
    class Explosion
    {
        private Game1 parent;
        private Texture2D tex;
        private int width;
        private int height;
        private int currentX;
        private int currentY;
        private Vector2 position;

        /// <summary>
        /// Primary constructor for the Explosion Class.
        /// </summary>
        /// <param name="game">The Game class that is the game parent class of this object.</param>
        /// <param name="image">The explosion tileset image to be used.</param>
        /// <param name="rows">The number of rows of frames in the texture to draw.</param>
        /// <param name="cols">The number of columns of frames in the texture to draw.</param>
        public Explosion(Game game,
            Texture2D image,
            int rows,
            int cols)
        {
            parent = (Game1)game;
            tex = image;
            this.width = tex.Width / cols;
            this.height = tex.Height / rows;
            currentX = 0;
            currentY = 0;
        }

        /// <summary>
        /// StartAnimation starts the Explosion animation at a specified Vector2 location.
        /// </summary>
        /// <param name="newPosition">The new location of the Explosion.</param>
        public void StartAnimation(Vector2 newPosition)
        {
            this.position = newPosition;
            currentX = 0;
            currentY = 0;
        }

        /// <summary>
        /// AnimationFinished a method that checks if the explosion animation is completed yet.
        /// </summary>
        /// <returns>Boolean, returns true if animation is done, false if animation is still playing.</returns>
        public bool AnimationFinished()
        {
            currentX += width;
            if(currentX >= tex.Width)
            {
                currentX = 0;
                currentY += height;
                if(currentY >= tex.Height)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Draw is a method that draw the current frame of the explosion.
        /// </summary>
        public void Draw()
        {
            Rectangle sourceRectangle = new Rectangle(currentX, currentY, width, height);
            parent.SpriteBatch.Draw(tex, position, sourceRectangle, Color.White);
        }
    }
}
