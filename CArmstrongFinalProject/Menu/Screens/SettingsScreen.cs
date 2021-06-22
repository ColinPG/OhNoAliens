/* SettingsScreen.cs
 * Description: SettingsScreen is a class that inherits from the SubMenuScreen class. 
 * It displays a list of Game Settings to be viewed and editted by the user. 
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.07: Created
 */
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// SettingsScreen: A class that inherits from the SubMenuScreen class. 
    /// It displays a list of Game Settings to be viewed and editted by the user.
    /// </summary>
    internal class SettingsScreen : SubMenuScreen
    {
        private GameSettings gameSettings;

        private MenuBar soundAudioBar;
        private MenuBar musicAudioBar;
        private MenuBar panSpeedBar;
        private MenuBar zoomSpeedBar;

        private MenuItemList descMenuList;
        private MenuItemList detailMenuList;
        private List<object> descList = new List<object>
            {
                "FullScreen:",
                "Center On Turret Switch:",
                "Reset to Default",
                "Apply"
            };
        private List<object> detailList;
        private List<Rectangle> barRects;

        private Texture2D barTextureFull;
        private Texture2D barTextureEmpty;

        //Space between each Menu Bar item
        private int menuBarOffset = 50;
        //Space between each Menu Text item
        private int menuitemOffset = 10;

        private int selectedIndex = 0;
        private bool inDetail = false;

        private int maxIndex = 7;

        /// <summary>
        /// The Primary constructor for the SettingsScreen class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the parent of this class.</param>
        /// <param name="screenManager">A reference to the ScreenManager class that is the controller of this class.</param>
        public SettingsScreen(Game game, ScreenManager screenManager) : base(game, screenManager)
        {
            menuBarOffset = parent.Graphics.PreferredBackBufferHeight / 10;
            menuitemOffset = parent.Graphics.PreferredBackBufferHeight / 30;
            gameSettings = parent.GameSettings;
            detailList = new List<object>
            {
                gameSettings.FullScreen,
                gameSettings.TurretFocus
            };

            barTextureEmpty = parent.Content.Load<Texture2D>("Images/Menu/menuBarEmpty");
            barTextureFull = parent.Content.Load<Texture2D>("Images/Menu/menuBarFilled");
            barRects = new List<Rectangle>();
            Vector2 menuStartPos = parent.PositionOnScreen(0.1f, 0.1f);
            float detailOffset = screenManager.MenuFont.MeasureString(descList[1].ToString()).X + 10;
            panSpeedBar = BuildBar(ref menuStartPos, detailOffset, gameSettings.PanSpeed, "Camera Pan Speed:");
            this.Components.Add(panSpeedBar);
            zoomSpeedBar = BuildBar(ref menuStartPos, detailOffset, gameSettings.ZoomSpeed, "Camera Zoom Speed:");
            this.Components.Add(zoomSpeedBar);
            soundAudioBar = BuildBar(ref menuStartPos, detailOffset, gameSettings.SoundVolume, "Sound Volume:");
            this.Components.Add(soundAudioBar);
            musicAudioBar = BuildBar(ref menuStartPos, detailOffset, gameSettings.MusicVolume, "Music Volume:");
            this.Components.Add(musicAudioBar);

            descMenuList = new MenuItemList(game, descList, menuStartPos, screenManager.MenuFont, screenManager.MenuFont, menuBarOffset, 0, 0);
            this.Components.Add(descMenuList);
            menuStartPos.X += detailOffset;
            detailMenuList = new MenuItemList(game, detailList, menuStartPos, screenManager.MenuFont, screenManager.MenuFont, menuBarOffset, 0, 0);
            this.Components.Add(detailMenuList);
        }

        /// <summary>
        /// BuildBar is a method that creates a new MenuBar object based on the parameters and returns it.
        /// </summary>
        /// <param name="pos">The Vector2 top-left position of the Menubar.</param>
        /// <param name="detailOffset">The offset of the details from the bar.</param>
        /// <param name="barValue">The offset in pixels between the detail and the bar.</param>
        /// <param name="descriptionLabel">The new text for the description label.</param>
        /// <returns>A new instance of a MenuBar based on the parameters.</returns>
        private MenuBar BuildBar(ref Vector2 pos, float detailOffset, float barValue, string descriptionLabel)
        {
            MenuBar newBar = new MenuBar(parent, screenManager, barValue, pos, detailOffset, barTextureFull, barTextureEmpty);
            newBar.SetLabels(descriptionLabel);
            Vector2 labelSize = screenManager.MenuFont.MeasureString(descriptionLabel);
            Rectangle newbarRect = new Rectangle(
                (int)pos.X, 
                (int)pos.Y,
                (int)labelSize.X * 3,
                (int)labelSize.Y);
            barRects.Add(newbarRect);
            pos.Y += menuBarOffset;
            return newBar;
        }

        /// <summary>
        /// Update is an overriden method that all GameComponent classes have, allowing for game logic to be processed 
        /// every frame. 
        /// This Update method reads navigation input and updates the menu objects.
        /// </summary>
        /// <param name="gameTime">A snapshot of how much time has passed.</param>
        public override void Update(GameTime gameTime)
        {
            if (!inDetail)
            {
                if (parent.InputManager.SingleKeyPress(Keys.Down))
                {
                    selectedIndex++;
                }
                if (parent.InputManager.SingleKeyPress(Keys.Up))
                {
                    selectedIndex--;
                }
                if (selectedIndex < 0)
                {
                    selectedIndex = maxIndex;
                }
                if (selectedIndex > maxIndex)
                {
                    selectedIndex = 0;
                }
                MouseInput();
                UpdateSelectedItem();
            }
            else
            {
                switch (selectedIndex)
                {
                    case 0: //pan
                        panSpeedBar.CheckForBarInput();
                        break;
                    case 1: //zoom
                        zoomSpeedBar.CheckForBarInput();
                        break;
                    case 2: //sound vol
                        soundAudioBar.CheckForBarInput();
                        break;
                    case 3: //music vol
                        musicAudioBar.CheckForBarInput();
                        break;
                }
            }
            if (parent.InputManager.SingleKeyPress(Keys.Enter))
                EnterOrExitDetail();
            base.Update(gameTime);
        }

        /// <summary>
        /// EnterOrExitDetail is a method that switching between inDetail and not in Details states.
        /// </summary>
        private void EnterOrExitDetail()
        {
            inDetail = !inDetail;
            if (selectedIndex > 3)
                inDetail = false;
            switch (selectedIndex)
            {
                case 0: //pan
                    panSpeedBar.inDetail = inDetail;
                    break;
                case 1: //zoom
                    zoomSpeedBar.inDetail = inDetail;
                    break;
                case 2: //sound vol
                    soundAudioBar.inDetail = inDetail;
                    break;
                case 3: //music vol
                    musicAudioBar.inDetail = inDetail;
                    break;
                case 4: //fullscreen
                    SwapDetailBoolItemValue(ref gameSettings.FullScreen, 0);
                    break;
                case 5: //turret swap
                    SwapDetailBoolItemValue(ref gameSettings.TurretFocus, 1);
                    break;
                case 6: //default
                    gameSettings.ResetToDefault();
                    RebuildSettingsDetails();
                    break;
                case 7: //Apply
                    gameSettings.PanSpeed = panSpeedBar.BarValue;
                    gameSettings.ZoomSpeed = zoomSpeedBar.BarValue;
                    gameSettings.MusicVolume = musicAudioBar.BarValue;
                    gameSettings.SoundVolume = soundAudioBar.BarValue;
                    gameSettings.ApplySettings();
                    break;
            }
        }

        /// <summary>
        /// RebuildSettingsDetails is a method that reads the bar values and updates the game settings values.
        /// </summary>
        private void RebuildSettingsDetails()
        {
            panSpeedBar.BarValue = gameSettings.PanSpeed;
            zoomSpeedBar.BarValue = gameSettings.ZoomSpeed;
            musicAudioBar.BarValue = gameSettings.MusicVolume;
            soundAudioBar.BarValue = gameSettings.SoundVolume;
            detailMenuList.UpdateMenuItem(0, gameSettings.FullScreen);
            detailMenuList.UpdateMenuItem(1, gameSettings.TurretFocus);
        }

        /// <summary>
        /// MouseInput is a method that checks mouse input for interacting with the menu items.
        /// </summary>
        private void MouseInput()
        {
            for (int i = 0; i < barRects.Count; i++)
            {
                if (barRects[i].Contains(parent.InputManager.Ms.Position))
                {
                    selectedIndex = i;
                    if (parent.InputManager.SingleLeftClick())
                        EnterOrExitDetail();
                }
                if (descMenuList.CheckForMenuItemClickOrPress(i))
                    EnterOrExitDetail();
            }
            int hoverDescItem = descMenuList.CheckHoverMenuItem();
            int hoverDetailItem = detailMenuList.CheckHoverMenuItem(); 
            if (hoverDescItem != -1 || hoverDescItem != -1)
            {
                UpdateSelectedItem();
            }
            if (hoverDetailItem == 0 || hoverDescItem == 0) // If fullscreen is clicked
            {
                selectedIndex = 4;
            }
            else if (hoverDetailItem == 1 || hoverDescItem == 1) // If turret focus is clicked
            {
                selectedIndex = 5;
            }
            else if (hoverDescItem == 2) // if default is clicked
            {
                selectedIndex = 6;
            }
            else if (hoverDescItem == 3) //if apply is clicked
            {
                selectedIndex = 7;
            }
        }

        /// <summary>
        /// SwapDetailBoolItemValue is a helper method that switches a boolean value to be opposite,
        /// and updates the details menu item list value at a specified index.
        /// </summary>
        /// <param name="item">The boolean value to swap.</param>
        /// <param name="index">The menu item index to update.</param>
        private void SwapDetailBoolItemValue(ref bool item, int index)
        {
            item = !item;
            detailMenuList.UpdateMenuItem(index, item);
        }

        /// <summary>
        /// UpdateSelectedItem is a method that checks the selected index and sets 
        /// the focus on the newly selected menu item.
        /// </summary>
        private void UpdateSelectedItem()
        {
            panSpeedBar.isSelected = false;
            zoomSpeedBar.isSelected = false;
            soundAudioBar.isSelected = false;
            musicAudioBar.isSelected = false;
            descMenuList.UpdateSelectedIndex(-1);
            switch (selectedIndex)
            {
                case 0: //pan
                    panSpeedBar.isSelected = true;
                    break;
                case 1: //zoom
                    zoomSpeedBar.isSelected = true;
                    break;
                case 2: //sound vol
                    soundAudioBar.isSelected = true;
                    break;
                case 3: //music vol
                    musicAudioBar.isSelected = true;
                    break;
                case 4: //fullscreen
                    descMenuList.UpdateSelectedIndex(0);
                    break;
                case 5: //turret swap
                    descMenuList.UpdateSelectedIndex(1);
                    break;
                case 6: //default
                    descMenuList.UpdateSelectedIndex(2);
                    break;
                case 7: //Apply
                    descMenuList.UpdateSelectedIndex(3);
                    break;
            }
        }

        /// <summary>
        /// Hide is a overriden method that resets the values on objects for this Screen.
        /// </summary>
        public override void Hide()
        {
            selectedIndex = 0;
            try
            {
                descMenuList.UpdateSelectedIndex(-1);
                panSpeedBar.isSelected = false;
                zoomSpeedBar.isSelected = false;
                soundAudioBar.isSelected = false;
                musicAudioBar.isSelected = false;
            }
            catch { }
            inDetail = false;
            base.Hide();
        }
    }
}