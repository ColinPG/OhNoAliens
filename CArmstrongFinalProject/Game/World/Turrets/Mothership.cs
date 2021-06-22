/* Mothership.cs
 * Description: Mothership is a class that inherits from the GameObject class. 
 * It contains the properties and logic for a single Mothership entity that is the center of the game world.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.04: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// Mothership: This class contains the properties and logic for a single Mothership 
    /// entity that is the center of the game world.
    /// </summary>
    internal class Mothership : GameObject
    {
        /// <summary>
        /// Property of whether the Mothership recently took damage or not.
        /// </summary>
        public bool TookDamage { get; internal set; }

        /// <summary>
        /// Primary Constructor for the Mothership Class.
        /// </summary>
        /// <param name="game">The Game class that is the game parent class of this object.</param>
        /// <param name="image">The Texture of the Mothership.</param>
        /// <param name="position">The starting position of the Mothership</param>
        public Mothership(Game game, Texture2D image, Vector2 position)
            : base(game, image, position)
        {
            maxShields = 50;
            currentShields = maxShields;
            maxHealth = 100;
            currentHealth = maxHealth;
        }

        /// <summary>
        /// RestoreShields is a method that restores shields to their maximum value.
        /// </summary>
        /// <param name="increaseMaxShieldsAmount">An optional parameter that increases the maximum value of the shields.</param>
        public void RestoreShields(int increaseMaxShieldsAmount = 0)
        {
            maxShields += increaseMaxShieldsAmount;
            currentShields = maxShields;
        }

        /// <summary>
        /// TakeDamage is an overriden method that simply sets the mothership tookdamage state to true.
        /// </summary>
        /// <param name="incomingDamage">The amount of incoming damage to be taken by the Mothership.</param>
        public override void TakeDamage(int incomingDamage)
        {
            TookDamage = true;
            base.TakeDamage(incomingDamage);
        }
    }
}