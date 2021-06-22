/* PlayScreen.cs
 * Description: PlayScreen is a class that inherits from the GameScreen class. 
 * It is the Screen where the user actually plays the game.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.04: Created
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// PlayScreen: A class that inherits from the GameScreen class. 
    /// It is the Screen where the user actually plays the game.
    /// </summary>
    class PlayScreen : GameScreen
    {
        private GameBackGround background;
        /// <summary>
        /// Property for the GameBackGround of the game world.
        /// </summary>
        internal GameBackGround Background { get => background; }

        private Mothership mothership;
        /// <summary>
        /// Property for the Mothership of the player.
        /// </summary>
        public Mothership Mothership { get => mothership; }

        private TurretManager turretManager;
        /// <summary>
        /// Property for TurretManager instance of the game.
        /// </summary>
        internal TurretManager TurretManager { get => turretManager; }

        private BulletManager bulletManager;
        /// <summary>
        /// Property for the BulletManager instance of the game.
        /// </summary>
        internal BulletManager BulletManager { get => bulletManager; }

        private EnemyManager enemyManager;
        /// <summary>
        /// Property for the EnemyManager instance of the game.
        /// </summary>
        internal EnemyManager EnemyManager { get => enemyManager; }

        private ExplosionManager explosionManager;
        /// <summary>
        /// Property for the ExplosionManager instance of the game.
        /// </summary>
        internal ExplosionManager ExplosionManager { get => explosionManager; }

        private Camera cam;
        /// <summary>
        /// Property for the Camera instance of the game.
        /// </summary>
        internal Camera Cam { get => cam; }

        private CollisionDetection collisionDetection;
        /// <summary>
        /// Property for the CollisionDetection of the game.
        /// </summary>
        internal CollisionDetection CollisionDetection { get => collisionDetection; }

        private HUD hud;
        /// <summary>
        /// Property for the HUD instance of the game.
        /// </summary>
        internal HUD Hud { get => hud; }
        
        private WaveManager waveManager;
        /// <summary>
        /// Property for the Camera instance of the game.
        /// </summary>
        internal WaveManager WaveManager { get => waveManager; }

        private ScoreTracker score;
        /// <summary>
        /// Property for the ScoreTracker instance of the game.
        /// </summary>
        internal ScoreTracker Score { get => score; }

        private const float mapSize = 2000f;
        /// <summary>
        /// Property for the Map Size of the game.
        /// </summary>
        internal float MapSize { get => mapSize; }

        private bool paused;
        /// <summary>
        /// Property for the paused state of the game.
        /// </summary>
        public bool Paused { get => paused; }

        private Cursor cursor;

        /// <summary>
        /// Primary constructor of the PlayScreen class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the parent of this class.</param>
        /// <param name="screenManager">A reference to the ScreenManager that manages this class.</param>
        public PlayScreen(Game game, ScreenManager screenManager) : base(game, screenManager)
        {
            paused = false;
            background = new GameBackGround(game);
            this.Components.Add(background);

            mothership = new Mothership(parent,
                parent.Content.Load<Texture2D>("Images/mothership"),
                Vector2.Zero);

            cam = new Camera(parent, this);
            this.Components.Add(cam);

            bulletManager = new BulletManager(parent);
            this.Components.Add(bulletManager);

            turretManager = new TurretManager(parent, this);
            this.Components.Add(turretManager);

            explosionManager = new ExplosionManager(parent);
            this.Components.Add(explosionManager);

            collisionDetection = new CollisionDetection(game, this);
            this.Components.Add(collisionDetection);

            score = new ScoreTracker();

            enemyManager = new EnemyManager(game, this);
            this.Components.Add(enemyManager);

            waveManager = new WaveManager(game, this);
            this.Components.Add(waveManager);

            hud = new HUD(game, this);
            this.Components.Add(hud);

            cursor = new Cursor(game);
            this.Components.Add(cursor);
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method simply checks if the player wants to exit or pause, and if the mothership has died.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            if (parent.InputManager.SingleKeyPress(Keys.Escape))
            {
                screenManager.ChangeScreen(this, "Menu");
            }
            if(!mothership.isAlive())
            {
                screenManager.ChangeScreen(this, "GameOver");
            }
            if(parent.InputManager.SingleKeyPress(Keys.OemTilde))
            {
                waveManager.Enabled = paused;
                enemyManager.Enabled = paused;
                turretManager.Enabled = paused;
                bulletManager.Enabled = paused;
                paused = !paused;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw is an overriden method that all DrawableGameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Draw method draws the background of the game, then draws the game world, then finally draws the HUD on top.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Draw(GameTime gameTime)
        {
            //It's the background, needs to be drawn first, world is drawn on top of it.
            background.Draw();

            //Everything that gets affected by the camera must be called in here so that it is affected by the camera matrix
            //things like HUD or otherwise can be called separately.
            parent.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, cam.Transform);
            mothership.Draw();
            bulletManager.Draw();
            turretManager.Draw();
            enemyManager.Draw();
            explosionManager.Draw();
            parent.SpriteBatch.End();

            //UI is drawn on top of the world
            hud.Draw();

            base.Draw(gameTime);
        }
    }
}
