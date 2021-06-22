/* Enemy.cs
 * Description: Enemy.cs is a class file that holds the Enemystate enum and the Enemy class.
 * The EnemyState enum contains all possible state types that an Enemy object can be in.
 * The Enemy class inherits from the GameObject class,
 * it contains all enemy properties and related logic.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.05: Created
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// Enemystate is an enum that contains all possible states that an Enemy object can be in.
    /// </summary>
    public enum EnemyState
    {
        /// <summary>
        /// Approaching is the state where an enemy is moving full speed towards their destination.
        /// </summary>
        Approaching,
        /// <summary>
        /// Slowing is the state where an enemy is close to their destination, and reduces speed but continues moving.
        /// </summary>
        Slowing,
        /// <summary>
        /// Slowing is the state where an enemy has reached their destination and does not move anymore.
        /// </summary>
        Stopped
    }
    /// <summary>
    /// Enemy is a class that inherits from the GameObject class, it contains all enemy properties and related logic.
    /// </summary>
    internal class Enemy : GameObject
    {
        private Vector2 direction;
        private Vector2 destination;
        private float speed;
        /// <summary>
        /// Property for the float speed, allowing other classes to get and set the speed value of the Enemy.
        /// </summary>
        public float Speed { get { return speed; } set { maxSpeed = value; speed = value; } }
        private float maxSpeed;
        private float brakeSpeed;
        public EnemyState State = EnemyState.Approaching;
        private bool pulse;
        private double sinceLastPulse = 0;
        private const double pulseRate = 160; //Engine pulses every 160ms at full speed
        private const float slowDistance = 500f; //Distance away from destination enemy will move into slowing state.

        /// <summary>
        /// Primary constructor for the Enemy class.
        /// </summary>
        /// <param name="parent">A reference to the Game class that is the parent of this class.</param>
        /// <param name="image">A reference to the Texture2D that this object will use to draw in the game world.</param>
        /// <param name="destination">A Vector2 of where the Enemy's destination will be.</param>
        /// <param name="initPosition">A Vector2 of where the Enemy's starting position will be.</param>
        /// <param name="fireRateInMs">A int of the number of milliseconds needed between every shot the Enemy can fire.</param>
        public Enemy(Game parent,
            Texture2D image,
            Vector2 destination,
            Vector2 initPosition,
            int fireRateInMs) : base(parent, image, initPosition)
        {
            this.direction = -(position - destination);
            direction.Normalize();
            float angle = (float)Math.Atan2(1 - direction.Y, 0 - direction.X) * 2 + (float)Math.PI;
            rotation = angle;
            this.destination = destination;
            canFire = true;
            this.fireRateInMs = fireRateInMs;
        }

        /// <summary>
        /// Move is a method that updates the Enemy's positon based on direction and speed.
        /// </summary>
        private void Move()
        {
            position += direction * speed;
        }

        /// <summary>
        /// Approach is a method that calls Move and checks if the Enemy is close enough to move to the slowing state.
        /// </summary>
        internal void Approach()
        {
            Move();
            if (Vector2.Distance(position, destination) < slowDistance)
            {
                State = EnemyState.Slowing;
                brakeSpeed = speed * 0.005f;
            }
        }

        /// <summary>
        /// Slow is a method that calls Move and reduces the Enemy's speed based on their brakeSpeed. It also checks if 
        /// they have reached their destination or lost all speed, and if so will move them to the Stopped State.
        /// </summary>
        internal void Slow()
        {
            Move();
            speed -= brakeSpeed;
            if (speed < 0 || Vector2.Distance(position, destination) < 20f) //If the object gets with 20 units of their destination, stop, so they don't accidentally move past it.
                State = EnemyState.Stopped;
        }

        /// <summary>
        /// DrawHealthBar is a method called during the Draw loop, drawing a health bar slightly above the 
        /// Enemy in the game world based on it's currentHealth.
        /// </summary>
        /// <param name="healthBarTex">A reference to the Texture2D that will be used to draw the Health Bar.</param>
        internal void DrawHealthBar(Texture2D healthBarTex)
        {
            Vector2 pos = position - (tex.Bounds.Center.ToVector2() * scale); //top left of object
            float heightScale = tex.Height * 0.1f * scale; //health bar height will be 10% of the size of the texture
            pos.Y -= heightScale;
            parent.SpriteBatch.Draw(healthBarTex, pos, new Rectangle(0, 0, (int)(tex.Width * ((float)currentHealth / (float)maxHealth) * scale), (int)heightScale), Color.White);
        }

        /// <summary>
        /// DrawEngineAnimation is a method called during the draw loop that draws an engine image behind the enemy based on their rotation, scale, and position.
        /// </summary>
        /// <param name="engine1">The first possible engine image to be drawn.</param>
        /// <param name="engine2">The second possible engine image to be drawn.</param>
        internal void DrawEngineAnimation(Texture2D engine1, Texture2D engine2)
        {
            Vector2 tempPos = position - (direction * scale * (tex.Height * 0.5f));
            if (pulse)
                parent.SpriteBatch.Draw(engine1, tempPos, null, Color.White, rotation, engine1.Bounds.Center.ToVector2(), scale * (tex.Height / engine1.Height / 2), SpriteEffects.None, 0);
            else
                parent.SpriteBatch.Draw(engine2, tempPos, null, Color.White, rotation, engine1.Bounds.Center.ToVector2(), scale * (tex.Height / engine1.Height / 2.1f), SpriteEffects.None, 0);
        }

        /// <summary>
        /// EngineUpdate is a method called during the Update loop that checks how often to pulse the engine animation based on the speed of the Enemy.
        /// </summary>
        /// <param name="gt">A snapshot of the time passed.</param>
        internal void EngineUpdate(GameTime gt)
        {
            if (pulseRate < sinceLastPulse)
            {
                sinceLastPulse = 0;
                pulse = !pulse;
            }
            else
                sinceLastPulse += (gt.ElapsedGameTime.TotalMilliseconds * (speed / maxSpeed));
        }
    }
}