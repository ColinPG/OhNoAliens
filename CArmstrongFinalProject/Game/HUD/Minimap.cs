/* Minimap.cs
 * Description: Minimap.cs contains the Minimap class.
 * The Minimap class contains the data and logic for drawing a miniture representation of the game world to the screen.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.05: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// Minimap: The Minimap class contains the data and logic for drawing a miniture representation
    /// of the game world to the screen.
    /// </summary>
    class Minimap
    {
        private Game1 game;
        private PlayScreen playScreen;
        private Texture2D mapTex;
        private Texture2D enemyDotTex;
        private Texture2D cameraBoxTex;

        private Vector2 miniMapPositionOnScreen;
        private Vector2 miniMapCenterOnScreen;
        private float worldToMapScaling;
        private Vector2 bottomRightScreen;
        private Rectangle miniMapBounds;

        /// <summary>
        /// Primary constructor of the Minimap class.
        /// </summary>
        /// <param name="game">The Game class that is the game parent class of this object.</param>
        /// <param name="playScreen">The PlayScreen class that is the parent class of this object.</param>
        public Minimap(Game1 game, PlayScreen playScreen)
        {
            this.game = game;
            this.playScreen = playScreen;
            mapTex = game.Content.Load<Texture2D>("Images/HUD/minimap3");
            enemyDotTex = game.Content.Load<Texture2D>("Images/HUD/enemyDot");
            cameraBoxTex = game.Content.Load<Texture2D>("Images/HUD/cameraBox");

            miniMapPositionOnScreen = game.PositionOnScreen(1f, 0f, -mapTex.Width);
            bottomRightScreen = game.PositionOnScreen(1f, 1f);
            miniMapCenterOnScreen = miniMapPositionOnScreen + mapTex.Bounds.Center.ToVector2();
            worldToMapScaling = playScreen.MapSize / (mapTex.Height / 2);
            //1000 / (200 / 2) = 10 
            //worldToMapScale is 10
            //World Size is (-1000,-1000 to +1000, 1000)
            //MiniMapTex size is 0, 0 to 200, 200
            int mapSizeInMiniMapCoords = (int)(playScreen.MapSize / worldToMapScaling);
            //If MapSize default is 1000... 1000/10 is 100.
            //MiniMapBounds would be
            //-100, -100, 100, 100
            //If MapSize is 2000... WorldtoMapScale is 20 => 2000/20 is 100 still
            miniMapBounds = new Rectangle(
                -mapSizeInMiniMapCoords,
                -mapSizeInMiniMapCoords, 
                mapSizeInMiniMapCoords * 2, 
                mapSizeInMiniMapCoords * 2);
        }

        /// <summary>
        /// Draw is the method called during the draw loop, to draw the minimap on top of the screen.
        /// </summary>
        internal void Draw()
        {
            game.SpriteBatch.Draw(mapTex, miniMapPositionOnScreen, Color.White);
            foreach (Enemy e in playScreen.EnemyManager.Enemies)
            {
                Vector2 ePositionOnMiniMap = e.Position / worldToMapScaling; 
                if (miniMapBounds.Contains(ePositionOnMiniMap))
                    game.SpriteBatch.Draw(enemyDotTex, miniMapCenterOnScreen + ePositionOnMiniMap - enemyDotTex.Bounds.Center.ToVector2(), Color.White);
            }
            Vector2 cameraTopLeftWorld = playScreen.Cam.ScreenToWorld(Vector2.Zero) / worldToMapScaling;
            Vector2 cameraBottomRightWorld = playScreen.Cam.ScreenToWorld(bottomRightScreen) / worldToMapScaling;
            
            //=100 to 100 is 200, thats the size of the minimap texture.
            cameraTopLeftWorld.X = MathHelper.Clamp(cameraTopLeftWorld.X, -100, 100);
            cameraTopLeftWorld.Y = MathHelper.Clamp(cameraTopLeftWorld.Y, -100, 100);

            cameraBottomRightWorld.X = MathHelper.Clamp(cameraBottomRightWorld.X, -100, 100);
            cameraBottomRightWorld.Y = MathHelper.Clamp(cameraBottomRightWorld.Y, -100, 100);

            Rectangle newRect = new Rectangle(
                (int)(miniMapCenterOnScreen.X + cameraTopLeftWorld.X),
                (int)(miniMapCenterOnScreen.Y + cameraTopLeftWorld.Y),
                (int)(cameraBottomRightWorld.X - cameraTopLeftWorld.X),
                (int)(cameraBottomRightWorld.Y - cameraTopLeftWorld.Y));
            game.SpriteBatch.Draw(cameraBoxTex, newRect, Color.White);
        }
    }
}
