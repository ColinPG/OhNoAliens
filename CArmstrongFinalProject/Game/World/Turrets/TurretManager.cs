/* TurretManager.cs
 * Description: TurretManager.cs is a class file that holds the TurretManager class.
 * The TurretManager class inherits from the GameComponent class,
 * it contains all Turret objects and related logic for controlling the Turret objects.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.04: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// TurretManager: Inherits from the GameComponent class, containing and controlling all Turret objects for the game.
    /// </summary>
    internal class TurretManager : GameComponent
    {
        private Game1 game;
        private List<Turret> turrets;
        private PlayScreen parent;
        private int currentTurret;
        private Turret activeTurret;
        private Texture2D activeTex;
        private Texture2D inactiveTex;
        private const int numberOfTurrets = 6; //MUST BE AT LEAST 1 OR it will cause an error

        /// <summary>
        /// Primary constructor of the TurretManager class.
        /// </summary>
        /// <param name="game">The Game1 class that is the game parent class of this object.</param>
        /// <param name="parent">The PlayScreen class that is the parent class of this object.</param>
        public TurretManager(Game1 game, PlayScreen parent) : base (game)
        {
            this.game = game;
            this.parent = parent;
            activeTex = game.Content.Load<Texture2D>("Images/Turrets/turretactive0");
            inactiveTex = game.Content.Load<Texture2D>("Images/Turrets/turretinactive0");
            turrets = new List<Turret>();
            for (int i = 0; i < numberOfTurrets; i++)
            {
                Turret newTurret = new Turret(game,
                    inactiveTex,
                    activeTex,
                    Vector2.Zero,
                    parent.Mothership,
                    i * (float)(Math.PI * 2 / numberOfTurrets),
                    350)
                {
                    AttackValue = 3
                };
                activeTurret = newTurret;
                turrets.Add(newTurret);
            }

            currentTurret = -1;
            ChangeTurret(true);
            parent.Cam.PanToPoint(activeTurret.Position);
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method checks for player input related to controlling the active turret or switching 
        /// to a different turret.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            activeTurret.UpdateFireRate(gameTime);
            if (game.InputManager.LeftClick())
            {
                if (!CheckforTurretSwitch())
                {
                    if (activeTurret.canFire)
                        parent.BulletManager.FireBullet(activeTurret, true);
                }
            }
            if (game.InputManager.SingleKeyPress(Keys.E))
            {
                ChangeTurret(false);
            }
            if (game.InputManager.SingleKeyPress(Keys.Q))
            {
                ChangeTurret(true);
            }

            //Panning the camera to click, for debugging
            //if (game.InputManager.SingleLeftClick())
            //    cam.PanToPoint(parent.InputManager.Ms.Position);

            //Rotate Player's turret to face cursor.
            activeTurret.Rotation = parent.Cam.AngleToScreenPoint(activeTurret.Position, game.InputManager.Ms.Position);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// CheckforTurretSwitch that checks for input on a turret switch, and if so, switch the active turret to the 
        /// one desired by the user.
        /// </summary>
        /// <returns>Returns a boolean whether a turret change occured or not.</returns>
        private bool CheckforTurretSwitch()
        {
            if (!game.InputManager.SingleLeftClick())
                return false;
            for (int i = 0; i < turrets.Count; i++)
            {
                if (turrets[i].GetBound().Contains(Vector2.Transform(
                    new Vector2(game.InputManager.Ms.Position.X,
                    game.InputManager.Ms.Position.Y), parent.Cam.InverseTransform)))
                {
                    if (game.GameSettings.TurretFocus)
                        parent.Cam.PanToPoint(turrets[i].Position);
                    activeTurret.DeActivate();
                    currentTurret = i;
                    activeTurret = turrets[i];
                    activeTurret.Fired();
                    activeTurret.Activate();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ChangeTurret is a turret that changes the active turret to the next or previous one in the list.
        /// </summary>
        /// <param name="previous">A boolean deciding if the user wants to move to the next or previous Turret.</param>
        private void ChangeTurret(bool previous)
        {
            if (previous)
                currentTurret++;
            else
                currentTurret--;
            if (currentTurret >= numberOfTurrets)
                currentTurret = 0; //loop back to first
            if (currentTurret < 0)
                currentTurret = numberOfTurrets - 1;

            activeTurret.DeActivate();
            activeTurret = turrets[currentTurret];
            activeTurret.Activate();
            if (game.GameSettings.TurretFocus)
                parent.Cam.PanToPoint(activeTurret.Position);
        }

        /// <summary>
        /// Draw is a method that is called during the Draw loop, and simply calls the draw of all the 
        /// Turret objects contained in this class.
        /// </summary>
        internal void Draw()
        {
            foreach (Turret turret in turrets)
                turret.Draw();
        }
    }
}