/* WaveManager.cs
 * Description: WaveManager is a class that inherits from the GameComponent class. 
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.05: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// WaveManager: Inherits from the GameComponent class, containing all logic for controlling creating waves of enemies.
    /// </summary>
    internal class WaveManager : GameComponent
    {
        private Game1 game;
        private PlayScreen parent;

        private bool waveAlive;
        /// <summary>
        /// A boolean of whether there is a currently a wave happening or not.
        /// </summary>
        public bool WaveAlive { get => waveAlive; }

        private int waveNumber;
        /// <summary>
        /// An int of current Wave Number the player is on.
        /// </summary>
        public int WaveNumber { get => waveNumber; }

        private int maxEnemiesAtOnce;
        private int enemiesAlive;
        private int enemiesToSpawn;

        /// <summary>
        /// Primary constructor for the WaveManager Class.
        /// </summary>
        /// <param name="game">The Game class that is the game parent class of this object.</param>
        /// <param name="parent">The PlayScreen class that is the parent class of this object.</param>
        public WaveManager(Game game,
            PlayScreen parent)
            : base(game)
        {
            this.game = (Game1)game;
            this.parent = parent;
            this.waveAlive = false;
            this.waveNumber = 0;
            this.maxEnemiesAtOnce = 2;
            this.enemiesToSpawn = 0;
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method checks for input to start a wave if there isn't an active wave, or checks if enemies need 
        /// to be spawned if a wave is happening.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            //Check for LevelStart Input
            if (!waveAlive)
            {
                if (game.InputManager.SingleKeyPress(Keys.Enter))
                    StartWave();
                //countdown timer could be added here
            }
            else
            {
                if (enemiesAlive + enemiesToSpawn == 0)
                {
                    waveAlive = false;
                    parent.Score.WaveSurvived();
                    parent.Mothership.RestoreShields();
                    parent.BulletManager.ClearBullets();
                }
                if (enemiesAlive < maxEnemiesAtOnce && enemiesToSpawn > 0)
                {
                    parent.EnemyManager.CreateEnemy();
                    enemiesAlive++;
                    enemiesToSpawn--;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// GetEnemyCount is a method that returns the amount of enemies remaining in a wave, both active 
        /// and enemies yet to be created.
        /// </summary>
        /// <returns>Returns a int of number of enemies alive plus number of enemies left to be create in the current wave.</returns>
        internal int GetEnemyCount()
        {
            enemiesAlive = parent.EnemyManager.Enemies.Count;
            return enemiesAlive + enemiesToSpawn;
        }

        /// <summary>
        /// StartWave is a method called to start a new wave of enemies, adding move enemies to be spawned
        /// and increasing the wave number.
        /// </summary>
        private void StartWave()
        {
            waveAlive = true;
            maxEnemiesAtOnce += 1;
            waveNumber++;
            enemiesToSpawn = waveNumber * 5;
        }
    }
}