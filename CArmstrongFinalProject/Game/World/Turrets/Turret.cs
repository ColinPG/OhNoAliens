/* Turret.cs
 * Description: Turret.cs is a class file that holds the Turret class.
 * The Turret class inherits from the GameObject class,
 * it contains all turret properties and related logic.
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
    /// Turret is a class that inherits from the GameObject class, it contains all turret properties and related logic.
    /// </summary>
    internal class Turret : GameObject
    {
        private Texture2D activeTex;
        private Texture2D inactiveTex;


        /// <summary>
        /// The primary constructor for the Turret class.
        /// </summary>
        /// <param name="game">The Game class that is the game parent class of this object.</param>
        /// <param name="inactive">The Texture2D of the turret in an inactive state.</param>
        /// <param name="active">The Texture2D of the turret in an active state.</param>
        /// <param name="initPosition">The initial positon of the turret.</param>
        /// <param name="mothership">A reference to the mothership class.</param>
        /// <param name="angle">The angle from the center of the mothership this turret is being spawned at.</param>
        /// <param name="fireRateInMs">The fire rate in milliseconds of how often this turret can fire.</param>
        public Turret(Game game, 
            Texture2D inactive, 
            Texture2D active, 
            Vector2 initPosition, 
            Mothership mothership, 
            float angle, 
            int fireRateInMs) 
            : base(game, inactive, initPosition)
        {
            this.inactiveTex = inactive;
            this.activeTex = active;
            this.position = mothership.GetCircle().PositionOnEdgeOfCircle(angle, 0.9f);
            this.rotation = -(angle * (float)(Math.PI / 180) + (float)Math.PI); //Default rotation is point away from center of mothership
            this.fireRateInMs = fireRateInMs;
        }

        /// <summary>
        /// Activate sets the turret to an active State.
        /// </summary>
        public void Activate()
        {
            tex = activeTex;
        }

        /// <summary>
        /// DeActivate sets the turret to an inactive State.
        /// </summary>
        public void DeActivate()
        {
            tex = inactiveTex;
        }
    }
}