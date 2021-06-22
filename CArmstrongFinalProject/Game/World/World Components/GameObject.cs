/* GameObject.cs
 * Description: GameObject.cs is a class file that holds the GameObject class.
 * The GameObject class is an abstract class that contains all common properties and functions that every
 * object in the game world needs to have.
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
    /// GameObject: This is an abstract class that contains all the common properties and functions that every
    /// object in the game world needs to have.
    /// </summary>
    abstract class GameObject
    {
        protected Game1 parent;
        public Texture2D tex;

        protected Vector2 position;
        /// <summary>
        /// Property for the current position.
        /// </summary>
        public Vector2 Position { get => position; }

        protected float rotation = 0f;
        /// <summary>
        /// Property for the current rotation.
        /// </summary>
        public float Rotation { get => rotation; set { rotation = value; } }

        protected Vector2 origin;

        protected float scale = 1f;
        /// <summary>
        /// Property for the current scale.
        /// </summary>
        public float Scale { get => scale; set { scale = value; } }

        protected int fireRateInMs = 0;
        protected double sinceLastFire = 0;
        public bool canFire = false;

        protected int attackValue;
        /// <summary>
        /// Property for the current attack value.
        /// </summary>
        public int AttackValue { get => attackValue; set { attackValue = value; } }

        protected int currentShields = 1;
        /// <summary>
        /// Property for the current shield points.
        /// </summary>
        public int CurrentShields { get => currentShields; }

        protected int maxShields = 1;
        /// <summary>
        /// Property for the Max shield points.
        /// </summary>
        public int MaxShields { get => maxShields; }

        protected int currentHealth = 1;
        /// <summary>
        /// Property for the current health points.
        /// </summary>
        public int CurrentHealth { get => currentHealth; }
        protected int maxHealth = 1;
        /// <summary>
        /// Property for the max health points.
        /// </summary>
        public int MaxHealth { get => maxHealth; }

        /// <summary>
        /// The amount of time in milliseconds the gameobject has left to live.
        /// </summary>
        public double TimeToLiveMs { get; set; }

        /// <summary>
        /// Primary Constructor for the GameObject class.
        /// </summary>
        /// <param name="game">The Game class that is the game parent class of this object.</param>
        /// <param name="image">A Texture2D that will be the image for this object in the world.</param>
        /// <param name="position">A Vector2 of the position of the object in world coordinates.</param>
        public GameObject(Game game,
            Texture2D image,
            Vector2 position)
        {
            this.parent = (Game1)game;
            this.tex = image;
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
            this.position = position;
            TimeToLiveMs = 1;
        }

        /// <summary>
        /// SetMaxHpAndShields is a method that sets a new max Health and Shield Points.
        /// The current health and shields also get set to the new max.
        /// Do not use if healing the object to full is not intended.
        /// </summary>
        /// <param name="newMaxShields">The maximum Shield points value.</param>
        /// <param name="newMaxHealth">The maximum Health points value.</param>
        public void SetMaxHpAndShields(int newMaxShields, int newMaxHealth)
        {
            currentShields = newMaxShields;
            maxShields = newMaxShields;
            currentHealth = newMaxHealth;
            maxHealth = newMaxHealth;
        }

        /// <summary>
        /// Checks whether a GameObject is Alive or not based on current Health and Time to Live properties.
        /// Health and Time to Live must be above 0 to be alive.
        /// </summary>
        /// <returns>True if alive, False if not alive.</returns>
        public bool isAlive()
        {
            if (TimeToLiveMs > 0 && currentHealth > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Makes the GameObject dead by lowering the health and Time to Live to negative
        /// </summary>
        public void Die()
        {
            currentHealth = -1;
            TimeToLiveMs = -1;
        }

        /// <summary>
        /// GetBound is a method that returns a rectangle based on the GameObject's current position and scale.
        /// </summary>
        /// <returns>Rectangle based on the GameObject's current position and scale.</returns>
        public Rectangle GetBound()
        {
            return new Rectangle(
                (int)(position.X - ((tex.Width / 2)*scale)),
                (int)(position.Y - ((tex.Height / 2)*scale)),
                (int)(tex.Width  * scale),
                (int)(tex.Height * scale));
        }

        /// <summary>
        /// GetCircle is a method that returns a Circle based on the GameObject's current position and scale.
        /// </summary>
        /// <returns>A Circle based on the GameObject's current position and scale.</returns>
        public Circle GetCircle()
        {
            return new Circle(position.X, position.Y,
                Math.Max(tex.Width / 2, tex.Height / 2) * scale);
        }

        /// <summary>
        /// Fired is a method that is called when the GameObject has just fired a shot, setting the 
        /// GameObject's state so that it can not fire again immediately.
        /// </summary>
        internal void Fired()
        {
            canFire = false;
            sinceLastFire = 0;
        }

        /// <summary>
        /// UpdateFireRate updates the fire rate properties of the object, checking if enough time has passed so the
        /// object can be ready to fire again.
        /// </summary>
        /// <param name="gt">A snapshot of how much time has passed.</param>
        internal void UpdateFireRate(GameTime gt)
        {
            if (fireRateInMs != 0)
            {
                if (fireRateInMs < sinceLastFire)
                {
                    sinceLastFire = 0;
                    canFire = true;
                }
                else
                    sinceLastFire += gt.ElapsedGameTime.TotalMilliseconds;
            }
        }

        /// <summary>
        /// TakeDamage is a method that lowers this GameObject's Shields and Health based on a specified amount.
        /// </summary>
        /// <param name="incomingDamage">The amount of damage this GameObject will take.</param>
        public virtual void TakeDamage(int incomingDamage)
        {
            if (currentShields > 0)
            {
                currentShields -= incomingDamage;
                if (currentShields < 0) //check for spillage.
                {
                    currentHealth -= currentShields;
                    currentShields = 0;
                }
            }
            else
                currentHealth -= incomingDamage;

            if (currentHealth < 0)
                Die();
        }

        /// <summary>
        /// RotationToDirectionVector is a method that returns a directional vector based off this 
        /// GameObjects rotation.
        /// </summary>
        /// <param name="offset">An optional parameter, offsetting the rotation by a specified amount in radians.</param>
        /// <returns>Vector2 of the desired direction based on this objects current rotation.</returns>
        internal Vector2 RotationToDirectionVector(float offset = 0f)
        {
            return new Vector2((float)Math.Cos(rotation + offset),
                (float)Math.Sin(rotation + offset));
        }

        /// <summary>
        /// Draw is a method called during the draw loop that simply draws the GameObject to the world.
        /// </summary>
        public void Draw()
        {
            parent.SpriteBatch.Draw(tex, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0);
        }
    }
}
