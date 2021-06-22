/* ExplosionManager.cs
 * Description: ExplosionManager is a class that inherits from the GameComponent class. 
 * It contains and controls all Explosion objects for the game.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.04: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// ExplosionManager: This class contains and controls all Explosion objects for the game.
    /// </summary>
    internal class ExplosionManager : GameComponent
    {
        private Game1 parent;
        private List<Explosion> explosions;
        private Texture2D explosionTex;

        /// <summary>
        /// Primary constructor for the ExplosionManager Class.
        /// </summary>
        /// <param name="parent">The Game1 class that is the game parent class of this object.</param>
        public ExplosionManager(Game1 parent) : base(parent)
        {
            this.parent = parent;
            explosions = new List<Explosion>();
            explosionTex = parent.Content.Load<Texture2D>("Images/explosion");
        }

        /// <summary>
        /// CreateExplosion is a method that creates a new Explosion object at a desired location.
        /// </summary>
        /// <param name="position">The location to create the explosion at.</param>
        public void CreateExplosion(Vector2 position)
        {
            Explosion newExplosion = new Explosion(parent,
                explosionTex, 5, 5);
            newExplosion.StartAnimation(position);
            explosions.Add(newExplosion);
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method simply checks if any Explosions are done and removes them from the active explosion list.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (Explosion ex in explosions)
            {
                if (ex.AnimationFinished())
                {
                    explosions.Remove(ex);
                    break;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw is called during the Draw loop, it simply draws all active explosion objects.
        /// </summary>
        public void Draw()
        {
            foreach(Explosion ex in explosions)
            {
                ex.Draw();
            }
        }
    }
}