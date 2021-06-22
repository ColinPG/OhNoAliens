/* CollisionDetection.cs
 * Description: CollisionDetection.cs is a class file that holds the CollisionDetection class.
 * The CollisionDetection class inherits from the GameComponent class,
 * it contains collision related logic and functions.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.06: Created
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
    /// CollisionDetection: This class inherits from the GameComponent class, 
    /// it contains collision related logic and functions.
    /// </summary>
    class CollisionDetection : GameComponent
    {
        private Game1 game;
        private PlayScreen parent;

        /// <summary>
        /// Primary constructor of the CollisionDetection class.
        /// </summary>
        /// <param name="game">The Game class that is the game parent class of this object.</param>
        /// <param name="playScreen">The PlayScreen class that is the parent class of this object.</param>
        public CollisionDetection(Game game,
            PlayScreen playScreen)
            : base(game)
        {
            this.game = (Game1)game;
            this.parent = playScreen;
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method checks for collisions between the player bullets and enemies, and enemy bullets and the 
        /// player's mothership.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            Circle motherShipCircle = parent.Mothership.GetCircle();
            foreach (Bullet b in parent.BulletManager.ActiveBullets)
            {
                Circle bulletCircle = b.GetCircle();
                if(b.playerOwned) //Player owned, check if hitting any enemies
                {
                    foreach(Enemy e in parent.EnemyManager.Enemies)
                    {
                        if (e.GetCircle().Intersects(bulletCircle))
                        {
                            e.TakeDamage(b.AttackValue);
                            game.AudioManager.PlaySoundEffect("explosion2");
                            b.Die();
                            parent.ExplosionManager.CreateExplosion(b.Position);
                        }
                    }
                }
                else //Enemy owned, check for hitting mothership
                {
                    if(bulletCircle.Intersects(motherShipCircle))
                    {
                        parent.Mothership.TakeDamage(b.AttackValue);
                        game.AudioManager.PlaySoundEffect("explosion1");
                        b.Die();
                        parent.ExplosionManager.CreateExplosion(b.Position);
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}
