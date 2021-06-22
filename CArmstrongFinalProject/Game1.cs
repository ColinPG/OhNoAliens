/* Game1.cs
 * Description: Game1.cs holds the Game1 class, which inherits from the game class and starts when the project loads.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.03: Created
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// Game1: Inherits the Game class and contains all other objects within.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        /// <summary>
        /// Property for graphics, allowing other classes to retrieve graphic based information.
        /// </summary>
        public GraphicsDeviceManager Graphics { get => graphics; }

        private SpriteBatch spriteBatch;
        /// <summary>
        /// Property for spriteBatch, allowing other classes to use the same instance of spritebatch.
        /// </summary>
        public SpriteBatch SpriteBatch { get => spriteBatch; }

        private InputManager inputManager;
        /// <summary>
        /// Property of inputManager, allows other classes to access the same InputManager class, ensuring reliable input data.
        /// </summary>
        public InputManager InputManager { get => inputManager; }

        private GameSettings gameSettings;
        /// <summary>
        /// Property of gameSettings, allowing other classes to access the GameSettings class, which contains various options
        /// chosen and saved by the user.
        /// </summary>
        public GameSettings GameSettings { get => gameSettings; set { gameSettings = value; } }

        private AudioManager audioManager;
        /// <summary>
        /// Property of audioManager, allows other classes usage of the AudioManager class, which allows for consistent
        /// sound and music levels.
        /// </summary>
        public AudioManager AudioManager { get => audioManager; }

        private ScreenManager screenManager;

        /// <summary>
        /// Primary constructor for the Game1 class.
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            gameSettings = new GameSettings(this);
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = gameSettings.FullScreen;
            graphics.ApplyChanges();

            inputManager = new InputManager(this);
            Components.Add(inputManager);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            audioManager = new AudioManager(this);
            Components.Add(audioManager);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// PositionOnScreen is a public method that returns a Vector2 of a pixel location on the screen based on the current
        /// graphics settings and parameter information.
        /// </summary>
        /// <param name="xPercent">The percentage of the point on the screen in the X dimension, starting from the left.</param>
        /// <param name="yPercent">The percentage of the point on the screen in the Y dimension, starting from the top.</param>
        /// <param name="xOffsetInPixels">An optional parameter, allowing to offset the location by an exact number of pixels in the X axis.</param>
        /// <param name="yOffsetInPixels">An optional parameter, allowing to offset the location by an exact number of pixels in the Y axis.</param>
        /// <returns>A Vector2 of a pixel location on the screen.</returns>
        public Vector2 PositionOnScreen(float xPercent,
            float yPercent,
            float xOffsetInPixels = 0f,
            float yOffsetInPixels = 0f)
        {
            return new Vector2((Graphics.PreferredBackBufferWidth * xPercent) + xOffsetInPixels,
                    (Graphics.PreferredBackBufferHeight * yPercent) + yOffsetInPixels);
        }

        /// <summary>
        /// ClampAngle in a simple helper method that keeps an angle within -Pi to Pi. In Radians, this is a complete rotation 
        /// of a circle. Clamping an angle allows for infinite rotation while never maxing out a number variable.
        /// </summary>
        /// <param name="angle">The angle object to be clamped.</param>
        /// <returns>The clamped angle, between -Pi and Pi.</returns>
        public float ClampAngle(float angle)
        {
            while (angle < -MathHelper.Pi)
            {
                angle += MathHelper.TwoPi;
            }
            while (angle > MathHelper.Pi)
            {
                angle -= MathHelper.TwoPi;
            }
            return angle;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
