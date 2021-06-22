/* Camera.cs
 * Description: Camera is a class that inherits from the GameComponent class. 
 * It contains all fields and logic for the View Matrix a game.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.04: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// Camera: A class containing all fields and logic for the View Matrix a game.
    /// </summary>
    class Camera : GameComponent
    {
        private Game1 game; //ref to Game class

        private float zoom; //Camera Zoom Value
        /// <summary>
        /// Property for the Camera's Zoom amount.
        /// </summary>
        public float Zoom
        {
            get { return zoom; }
        }

        private Matrix transform; //Camera Transform Matrix
        /// <summary>
        /// Property for the Camera's View Matrix.
        /// </summary>
        public Matrix Transform
        {
            get { return transform; }
        }

        private Matrix inverseTransform; //Inverse of Transform Matrix
        /// <summary>
        /// Propety for the Inverse of the view matrix, can be used to get objects screen coordinates
        /// from its object coordinates.
        /// </summary>
        public Matrix InverseTransform
        {
            get { return inverseTransform; }
        }

        private Vector2 position; //Camera Position
        /// <summary>
        /// Property for this Camera's current Position.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }

        private float rotation; //Camera Rotation Value (Radians)
        private Viewport viewport; //Cameras Viewport
        private PlayScreen parent; //Reference to Parent class
        private int scrollValue; //Previous Mouse Scroll Wheel Value
        private bool panning; //Whether zooming or not
        private Vector2 panPoint; //Point to move to
        private float zoomSpeed; //loaded by file, adjusted on settings screen
        private float panSpeed; //loaded by file, adjusted on settings screen
        private const float zoomSpeedMin = 0.01f; //Minimum zoomspeed
        private const float scrollSpeedMin = 1f; //Minimum pan Speed

        /// <summary>
        /// Primary constructor for the Camera class.
        /// </summary>
        /// <param name="game">The Game1 class that is the game parent class of this object.</param>
        /// <param name="playScreen">The PlayScreen class that is the parent class of this object.</param>
        public Camera(Game1 game, PlayScreen playScreen) : base (game)
        {
            this.game = game;
            this.viewport = game.GraphicsDevice.Viewport;
            this.parent = playScreen;

            // Default camera values
            zoom = 1.2f;
            scrollValue = 0;
            rotation = 0.0f;
            position = Vector2.Zero;

            //Loading zoom and pan speed from Game Settings
            zoomSpeed = this.game.GameSettings.ZoomSpeed;
            if (zoomSpeed < zoomSpeedMin)
                zoomSpeed = zoomSpeedMin;
            panSpeed = this.game.GameSettings.PanSpeed * 10;
            if (panSpeed < scrollSpeedMin)
                panSpeed = scrollSpeedMin;
        }


        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method simply updates the camera properties and reads camera related inputs.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            //Call Camera Input
            ReadCameraInput();
            //Clamp zoom value
            zoom = MathHelper.Clamp(zoom, 0.6f, 2f);
            //Clamp rotation value
            rotation = game.ClampAngle(rotation);

            //Clamp camera to world bounds... hopefully
            position.X = MathHelper.Clamp(position.X, -parent.MapSize, parent.MapSize);
            position.Y = MathHelper.Clamp(position.Y, -parent.MapSize, parent.MapSize);
            parent.Background.UpdateBackground(position, zoom);

            //Create view matrix
            Vector2 tempPos = -position * zoom;
            tempPos.X += viewport.Width / 2;
            tempPos.Y += viewport.Height / 2;

            transform = Matrix.CreateRotationZ(rotation) *
                            Matrix.CreateScale(new Vector3(zoom, zoom, 1f)) *
                            Matrix.CreateTranslation(tempPos.X, tempPos.Y, 0);
            //Update inverse matrix
            inverseTransform = Matrix.Invert(transform);

            if (panning)
                Pan();

            base.Update(gameTime);
        }

        /// <summary>
        /// ReadCameraInput reads the input to the user to adjust the camera view.
        /// </summary>
        protected virtual void ReadCameraInput()
        {
            //Check zoom Input
            if (game.InputManager.Ms.ScrollWheelValue > scrollValue)
            {
                zoom += zoomSpeed;
                scrollValue = game.InputManager.Ms.ScrollWheelValue;
            }
            else if (game.InputManager.Ms.ScrollWheelValue < scrollValue)
            {
                zoom -= zoomSpeed;
                scrollValue = game.InputManager.Ms.ScrollWheelValue;
            }
            //Check Move Input
            if (game.InputManager.Ks.IsKeyDown(Keys.A))
            {
                panning = false;
                position.X -= panSpeed;
            }
            if (game.InputManager.Ks.IsKeyDown(Keys.D))
            {
                panning = false;
                position.X += panSpeed;
            }
            if (game.InputManager.Ks.IsKeyDown(Keys.W))
            {
                panning = false;
                position.Y -= panSpeed;
            }
            if (game.InputManager.Ks.IsKeyDown(Keys.S))
            {
                panning = false;
                position.Y += panSpeed;
            }
        }

        /// <summary>
        /// AngleToScreenPoint is a helper method that returns an angle in radians based on a Vector2 location
        /// in world coords and a Point location in Screen coords.
        /// </summary>
        /// <param name="originInWorld">A Vector2 of the position to measure the angle from in world coordinates.</param>
        /// <param name="pointOnScreen">A Point of the locaition to measure to in screen coordinates.</param>
        /// <returns></returns>
        public float AngleToScreenPoint(Vector2 originInWorld, Point pointOnScreen)
        {
            Vector2 vectorUp = new Vector2(0, 1); //(Vector pointing straight up)
            Vector2 worldCoordinatesOfPoint = Vector2.Transform(new Vector2(pointOnScreen.X, pointOnScreen.Y), inverseTransform); //world coordinates of the point
            Vector2 directionOfPointFromOrigin = worldCoordinatesOfPoint - originInWorld; //Vector2 of direction from position to point
            directionOfPointFromOrigin.Normalize();

            float angle = (float)Math.Atan2(vectorUp.Y - directionOfPointFromOrigin.Y, vectorUp.X - directionOfPointFromOrigin.X);
            return angle * 2 + (MathHelper.Pi);
        }

        /// <summary>
        /// PanToPoint is a method that sets the Camera to start panning towards a point in world coordinates 
        /// based on a point in screen coordinates.
        /// </summary>
        /// <param name="point">The location to pan towards in screen coordinates.</param>
        public void PanToPoint(Point point)
        {
            panning = true;
            Vector2 tempPos = new Vector2(point.X, point.Y);
            panPoint = ScreenToWorld(tempPos);
        }

        /// <summary>
        /// ScreenToWorld is a helper method that returns a Vector2 based in world coordinates of a specified screen 
        /// coordinate based on the current view matrix.
        /// </summary>
        /// <param name="screenPos">A Vector2 containing the screen coordinate to be turned into world coordinates.</param>
        /// <returns>A Vector2 in world coordinates based on the given screen coordinate.</returns>
        public Vector2 ScreenToWorld(Vector2 screenPos)
        {
            return Vector2.Transform(screenPos, inverseTransform);
        }

        /// <summary>
        /// ScreenToWorld is a helper method that returns a Vector2 based in screen coordinates of a specified world 
        /// coordinate based on the current view matrix.
        /// </summary>
        /// <param name="worldPos">A Vector2 containing the world coordinate to be turned into screen coordinates.</param>
        /// <returns>A Vector2 in screen coordinates based on the given world coordinate.</returns>
        public Vector2 WorldToScreen(Vector2 worldPos)
        {
            return Vector2.Transform(worldPos, transform);
        }

        /// <summary>
        /// PanToPoint is a method that sets the Camera Pan to position and sets the camera state to panning.
        /// </summary>
        /// <param name="pos">The Vector2 position to pan towards.</param>
        public void PanToPoint(Vector2 pos)
        {
            panning = true;
            panPoint = pos;
        }

        /// <summary>
        /// Pan is a method that moves the camera positon towards the pan point, and checks if it has reached it.
        /// </summary>
        private void Pan()
        {
            float xDiff = panPoint.X - position.X;
            float yDiff = panPoint.Y - position.Y;
            position.X += xDiff * 0.05f;
            position.Y += yDiff * 0.05f;
            if (Vector2.Distance(panPoint, position) < 0.5f)
                panning = false;
        }
    }
}
