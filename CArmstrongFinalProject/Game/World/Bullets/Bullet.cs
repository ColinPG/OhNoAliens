/* Bullet.cs
 * Description: Bullet is a class that inherits from the of GameObject class. 
 * It contains all bullet properties and logic.
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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// Bullet: Inherits the GameObject class and contains all bullet properties and related logic within.
    /// </summary>
    class Bullet : GameObject
    {
        private float speed;
        private Vector2 direction;
        public bool playerOwned;

        /// <summary>
        /// Primary constructor for the Bullet class.
        /// </summary>
        /// <param name="parent">The Game class that is the game parent class of this object.</param>
        /// <param name="image">The image to be used for the bullet.</param>
        /// <param name="initPosition">The starting position of the bullet in world coords.</param>
        /// <param name="direction">The directionf of the bullet.</param>
        /// <param name="speed">The speed of the bullet.</param>
        /// <param name="damage">The damage value of the bullet.</param>
        public Bullet(Game1 parent,
            Texture2D image,
            Vector2 initPosition,
            Vector2 direction,
            float speed,
            int damage) 
            : base(parent, image, initPosition)
        {
            this.direction = direction;
            this.speed = speed;
            this.AttackValue = damage;
        }
        
        /// <summary>
        /// Update is a method called every frame, updating the position  based on the speed and direction 
        /// of the Bullet object and reducing it's remaining time to live.
        /// </summary>
        /// <param name="gt"></param>
        public void Update(GameTime gt)
        {
            position += direction * speed;
            TimeToLiveMs -= gt.ElapsedGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// Fire is a method that is called when the bullet is set to active, 
        /// </summary>
        /// <param name="newPosition">A Vector2 of the new position the bullet will be at.</param>
        /// <param name="direction">A Vector2 of the direction the bullet will be heading in.</param>
        internal void Fire(Vector2 newPosition, Vector2 direction)
        {
            position = newPosition;
            this.direction = direction;
        }
    }
}
