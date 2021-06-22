/* HUD.cs
 * Description: HUD is a class that inherits from the GameComponent class. 
 * It contains and controls all UI objects for the play screen.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.06: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// HUD: Short for Heads-Up-Display, this class inherits from the GameComponent class and contains 
    /// all objects and logic for user interface objects draw on top of the world for the player.
    /// </summary>
    internal class HUD : GameComponent
    {
        private Game1 game;
        private PlayScreen parent;

        private Texture2D hudBackground;

        private Texture2D healthTop;
        private Texture2D healthBottom;
        private Texture2D shieldTop;
        private Texture2D shieldBottom;

        private Texture2D hitIndicator; 
        private double maxTimeToDisplayIndicator = 500; // In Milliseconds
        private double timeLeftToDisplayIndicator = 0;

        private SpriteFont hudFont;
        private SpriteFont healthFont;
        private Vector2 indicatorScale;
        private Minimap minimap;

        /// <summary>
        /// Primary constructor of the HUD class.
        /// </summary>
        /// <param name="game">The Game class that is the game parent class of this object.</param>
        /// <param name="playScreen">The PlayScreen class that is the parent class of this object.</param>
        public HUD(Game game, PlayScreen playScreen)
            : base (game)
        {
            this.game = (Game1)game;
            this.parent = playScreen;
            hudFont = this.game.Content.Load<SpriteFont>("Fonts/hudFont");
            healthFont = this.game.Content.Load<SpriteFont>("Fonts/healthFont");
            hudBackground = this.game.Content.Load<Texture2D>("Images/HUD/hud-background");

            healthTop = this.game.Content.Load<Texture2D>("Images/HUD/health1");
            healthBottom = this.game.Content.Load<Texture2D>("Images/HUD/health2");
            shieldTop = this.game.Content.Load<Texture2D>("Images/HUD/shield1");
            shieldBottom = this.game.Content.Load<Texture2D>("Images/HUD/shield2");

            hitIndicator = this.game.Content.Load<Texture2D>("Images/HUD/hitIndicator");
            indicatorScale = this.game.PositionOnScreen(1, 1) / hitIndicator.Bounds.Size.ToVector2();
            minimap = new Minimap(this.game, playScreen);
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method simply updates the hit indicator timer.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            if (timeLeftToDisplayIndicator > 0)
                timeLeftToDisplayIndicator -= gameTime.ElapsedGameTime.TotalMilliseconds;
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw is a method called by the parent class to Draw all UI objects to the screen.
        /// </summary>
        public void Draw()
        {
            //Map, Mothership Hp, Wave#, Enemies Remaining, Timer for next wave
            game.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            minimap.Draw();
            float currentHealth = parent.Mothership.CurrentHealth;
            float maxHealth = parent.Mothership.MaxHealth;
            float currentShields = parent.Mothership.CurrentShields;
            float maxShields = parent.Mothership.MaxShields;
            int enemiesRemaining = parent.WaveManager.GetEnemyCount();
            int waveNumber = parent.WaveManager.WaveNumber;
            string waveText = "Current Wave: ";
            if (parent.Paused)
            {
                DrawTextInCenterOfScreen("    PAUSED. \nPress ` to resume.");
            }
            else if (!parent.WaveManager.WaveAlive)
            {
                waveText = "Next Wave: ";
                waveNumber++;
                DrawTextInCenterOfScreen("Press Enter to start the next wave.");
            }
            else
            {
                //if player takes damage, draw the start the timer for displaying the hit indicator on the screen
                if (parent.Mothership.TookDamage)
                {
                    timeLeftToDisplayIndicator = maxTimeToDisplayIndicator;
                    parent.Mothership.TookDamage = false;
                }
                //if timer for displaying indicator is active, display it
                if(timeLeftToDisplayIndicator > 0)
                {
                    game.SpriteBatch.Draw(hitIndicator, Vector2.Zero, null, Color.White * (float)(timeLeftToDisplayIndicator / maxTimeToDisplayIndicator), 0f, Vector2.Zero,
                        scale: indicatorScale, SpriteEffects.None, 0);
                }
                //Wave is active, show how many enemies remaining in wave
                DrawTextWithBackGround("Enemies Left: ", enemiesRemaining.ToString(),
                    game.PositionOnScreen(0.01f, 0.14f), Color.Red);
            }
            DrawTextWithBackGround(waveText, waveNumber.ToString(),
                game.PositionOnScreen(0.01f, 0.04f), Color.White);

            Vector2 bottomLeft = game.PositionOnScreen(0, 1f);
            string healthText = "HP: " + currentHealth.ToString() + "/" + maxHealth.ToString();
            string shieldText = "Shields: " + currentShields.ToString() + "/" + maxShields.ToString();

            Vector2 pos = bottomLeft;
            pos.Y -= healthTop.Height;
            Vector2 textPos = (new Vector2(healthTop.Width, healthTop.Height) / 2) - (healthFont.MeasureString(healthText) / 2);

            game.SpriteBatch.Draw(healthBottom, pos, Color.White);
            game.SpriteBatch.Draw(healthTop, pos, new Rectangle(0, 0, (int)(healthTop.Width * (currentHealth / maxHealth)), healthTop.Height), Color.White);
            game.SpriteBatch.DrawString(healthFont, healthText, pos + textPos, Color.White);

            pos.Y -= healthTop.Height;
            textPos = (new Vector2(healthTop.Width, healthTop.Height) / 2) - (healthFont.MeasureString(shieldText) / 2);
            game.SpriteBatch.Draw(shieldBottom, pos, Color.White);
            game.SpriteBatch.Draw(shieldTop, pos, new Rectangle(0, 0, (int)(healthTop.Width * (currentShields / maxShields)), healthTop.Height), Color.White);
            game.SpriteBatch.DrawString(healthFont, shieldText, pos + textPos, Color.White);

            game.SpriteBatch.End();
        }

        /// <summary>
        /// DrawTextInCenterOfScreen is a helper method to draw a specified string centered on the exact center of the screen.
        /// </summary>
        /// <param name="text">The text to be drawn.</param>
        private void DrawTextInCenterOfScreen(string text)
        {
            Vector2 startWaveSize = hudFont.MeasureString(text);
            game.SpriteBatch.DrawString(hudFont, text,
                game.PositionOnScreen(0.5f, 0.5f) - startWaveSize / 2, Color.White);
        }

        /// <summary>
        /// DrawTextWithBackGround is a helper method that draws a string in a specified color at a specified screen location
        /// with a blue background behind it to make it more visibile to the user.
        /// </summary>
        /// <param name="label">The first part of the text to draw.</param>
        /// <param name="value">The second part of the text to draw.</param>
        /// <param name="position">The position of the text.</param>
        /// <param name="color">The color of the text drawn.</param>
        void DrawTextWithBackGround(string label, string value, Vector2 position, Color color)
        {
            Vector2 labelTextSize = hudFont.MeasureString(label);
            Vector2 valueTextSize = hudFont.MeasureString(value);
            Vector2 totalTextSize = hudFont.MeasureString(label + value);
            Vector2 backgroundScale = totalTextSize / 250;
            backgroundScale.X = totalTextSize.X / 300;
            game.SpriteBatch.Draw(hudBackground, position - (totalTextSize / 2), null, Color.White, 0f, Vector2.Zero, backgroundScale, SpriteEffects.None, 0);
            game.SpriteBatch.DrawString(hudFont, label, position, color);
            position.X += labelTextSize.X;
            game.SpriteBatch.DrawString(hudFont, value, position, color);
        }
    }
}