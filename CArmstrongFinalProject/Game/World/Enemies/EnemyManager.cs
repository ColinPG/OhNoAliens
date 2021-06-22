/* EnemyManager.cs
 * Description: EnemyManager is a class that inherits from the GameComponent class. 
 * It contains and controls all Enemy objects for the game.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.05: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// EnemyManager: Inherits from the GameComponent class, containing and controlling all Enemy objects for the game.
    /// </summary>
    internal class EnemyManager : GameComponent
    {
        private Game1 game;
        private PlayScreen parent;
        private List<Enemy> enemies;
        /// <summary>
        /// A property that returns a list of all currently active Enemies.
        /// </summary>
        public List<Enemy> Enemies { get => enemies; }

        private Texture2D enemyTex;
        private Texture2D healthBarTex;
        private Texture2D engine1;
        private Texture2D engine2;

        /// <summary>
        /// Primary constructor of the EnemyManager class.
        /// </summary>
        /// <param name="game">The Game class that is the game parent class of this object.</param>
        /// <param name="playScreen">The PlayScreen class that is the parent class of this object.</param>
        public EnemyManager(Game game, PlayScreen playScreen) : base(game)
        {
            this.game = (Game1)game;
            this.parent = playScreen;
            enemies = new List<Enemy>();
            enemyTex = this.game.Content.Load<Texture2D>("Images/Enemies/enemy0red");
            healthBarTex = this.game.Content.Load<Texture2D>("Images/HUD/health1");
            engine1 = this.game.Content.Load<Texture2D>("Images/Enemies/flame0"); ;
            engine2 = this.game.Content.Load<Texture2D>("Images/Enemies/flame1"); ;
        }

        /// <summary>
        /// Creates one new instance of an enemy.
        /// </summary>
        public void CreateEnemy()
        {
            Random r = new Random();
            float angle = (float)(r.NextDouble() * Math.PI * 2);
            Circle map = new Circle(0, 0, 464); //Circle radius should be the same as mothership
            Vector2 newPos = map.PositionOnEdgeOfCircle(angle, 6.5f);
            float speed = (float)r.Next(300, 700) / 100;
            Vector2 dest = map.PositionOnEdgeOfCircle((float)angle, 1.1f);
            Enemy newEnemy = new Enemy(game, enemyTex, dest, newPos, 2500)
            {
                Speed = speed,
                Scale = 0.2f,
                AttackValue = 10,
            };
            newEnemy.SetMaxHpAndShields(0, 5);
            enemies.Add(newEnemy);
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method simply updates all active enemy objects and removes any that have died.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            foreach(Enemy e in enemies)
            {
                if (!e.isAlive())
                {
                    enemies.Remove(e);
                    parent.Score.EnemyKilled(); 
                    game.AudioManager.PlaySoundEffect("explosion3");
                    break;
                }
                e.EngineUpdate(gameTime);
                switch (e.State)
                {
                    case EnemyState.Approaching:
                        e.Approach();
                        break;
                    case EnemyState.Slowing:
                        e.Slow();
                        break;
                    case EnemyState.Stopped:
                        if (e.canFire)
                        {
                            parent.BulletManager.FireBullet(e, false);
                        }
                        e.UpdateFireRate(gameTime);
                        break;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw is a method that calls the Draw and DrawHealthBar method for all active enemy objects.
        /// It also calls the DrawEngineAntimation method if it is not stopped.
        /// </summary>
        internal void Draw()
        {
            foreach (Enemy e in enemies)
            {
                e.DrawHealthBar(healthBarTex);
                if (e.State != EnemyState.Stopped)
                    e.DrawEngineAnimation(engine1, engine2);
                e.Draw();
            }
        }
    }
}