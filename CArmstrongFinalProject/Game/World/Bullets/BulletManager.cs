/* BulletManager.cs
 * Description: BulletManager is a class that inherits from the GameComponent class. 
 * It contains and controls all bullet objects for the game.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.05: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// BulletManager: Inherits the GameComponent class and contains all bullet objects and related logic within.
    /// </summary>
    internal class BulletManager : GameComponent
    {
        private Game1 parent;
        private List<Bullet> inactivebullets;
        private List<Bullet> activebullets;
        /// <summary>
        /// A property of activebullets, allowing other class to get a readonly copy of all the currently active bullet objects.
        /// </summary>
        internal List<Bullet> ActiveBullets { get => activebullets; }

        private Texture2D playerBulletTex;
        private Texture2D enemyBulletTex;

        const int numberOfBullets = 100; //Number of bullets to be created on load, will create more if necessary later.

        /// <summary>
        /// Primary constructor of the BulletManager class.
        /// </summary>
        /// <param name="parent">A reference to the Game1 class that is the parent class of this object.</param>
        public BulletManager(Game1 parent) : base(parent)
        {
            this.parent = parent;
            playerBulletTex = parent.Content.Load<Texture2D>("Images/Bullets/beamBlue");
            enemyBulletTex = parent.Content.Load<Texture2D>("Images/Bullets/beamRed");
            inactivebullets = new List<Bullet>();
            activebullets = new List<Bullet>();
            for (int i = 0; i < numberOfBullets; i++)
            {
                CreateNewInactiveBullet();
            }
        }

        /// <summary>
        /// CreateNewInactiveBullet is a method that create an new bullet instance and adds it to the inactiveBullet list.
        /// </summary>
        private void CreateNewInactiveBullet()
        {
            Bullet newBullet = new Bullet(parent,
                playerBulletTex,
                Vector2.Zero,
                Vector2.Zero,
                10f, 1);
            inactivebullets.Add(newBullet);
        }

        /// <summary>
        /// FireBullet is a method that takes a bullet from the inactive list and move it to the activebullet list.
        /// It creates the bullet based off the triggering Game Objects values.
        /// </summary>
        /// <param name="gameObject">The GameObject that triggered the event, firing a bullet.</param>
        /// <param name="playerOwned">A boolean parameter tagging the bullet as player owned or not.</param>
        internal void FireBullet(GameObject gameObject, bool playerOwned)
        {
            gameObject.Fired();

            if (inactivebullets.Count == 0)
                CreateNewInactiveBullet();
            Bullet firedBullet = inactivebullets[0];
            firedBullet.AttackValue = gameObject.AttackValue;
            
            firedBullet.playerOwned = playerOwned;
            if (playerOwned)
            {
                parent.AudioManager.PlaySoundEffect("playerShoot");
                firedBullet.tex = playerBulletTex;
                firedBullet.TimeToLiveMs = 1000;
            }
            else
            {
                parent.AudioManager.PlaySoundEffect("enemyShoot");
                firedBullet.tex = enemyBulletTex;
                firedBullet.TimeToLiveMs = 2000;
            }
            firedBullet.Fire(gameObject.Position,
                gameObject.RotationToDirectionVector(-(float)MathHelper.Pi / 2));

            //Move it to active list
            activebullets.Add(firedBullet);
            inactivebullets.Remove(firedBullet);
        }

        /// <summary>
        /// ClearBullets is a method that removes all bullet objects from the activebullets list.
        /// </summary>
        internal void ClearBullets()
        {
            activebullets.Clear();
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method simply updates all active bullet objects and removes any that have expired.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            foreach(Bullet b in activebullets)
            {
                b.Update(gameTime);
                if (!b.isAlive())
                {
                    activebullets.Remove(b);
                    //inactivebullets.Add(b);
                    break;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw is a method that calls the Draw method for all active bullet objects.
        /// </summary>
        internal void Draw()
        {
            foreach (Bullet b in activebullets)
            {
                b.Draw();
            }
        }
    }
}