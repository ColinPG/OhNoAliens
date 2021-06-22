/* GameSettings.cs
 * Description: GameSettings is a class that contains all game settings as specified by the user.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.06: Created
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// GameSettings: A class that contains all game settings as specified by the user.
    /// </summary>
    public class GameSettings
    {
        private Game1 game;

        /// <summary>
        /// The file location to create or save the game settings to.
        /// </summary>
        private const string filepath = "Content/settings.txt";

        /// <summary>
        /// Whether the player wanted the camera to focus on turrets after switching.
        /// </summary>
        public bool TurretFocus;

        /// <summary>
        /// The pan speed of the camera in game.
        /// </summary>
        public float PanSpeed;

        /// <summary>
        /// The zoom speed of the camera in game.
        /// </summary>
        public float ZoomSpeed;

        /// <summary>
        /// Whether or not the game will display in FullScreen mode.
        /// </summary>
        public bool FullScreen;

        /// <summary>
        /// The current % Sound Effects are played at. Max 100% and Min 0%.
        /// </summary>
        public float SoundVolume;

        /// <summary>
        /// The current % Music is played at. Max 100% and Min 0%.
        /// </summary>
        public float MusicVolume;

        /// <summary>
        /// Primary constructor for the GameSettings class.
        /// </summary>
        /// <param name="game">A reference to the Game1 class that is the parent of this class.</param>
        public GameSettings(Game1 game)
        {
            this.game = game;
            ReadSettings();
        }

        /// <summary>
        /// ReadSettings is a method that attempts to read from an existing game settings file.
        /// If the file does not exist, it will create one.
        /// </summary>
        private void ReadSettings()
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(filepath);
                reader.ReadLine();
                PanSpeed = (float)Convert.ToInt32(reader.ReadLine()) / 100;
                reader.ReadLine();
                ZoomSpeed = (float)Convert.ToInt32(reader.ReadLine()) / 100;
                reader.ReadLine();
                SoundVolume = (float)Convert.ToInt32(reader.ReadLine()) / 100;
                reader.ReadLine();
                MusicVolume = (float)Convert.ToInt32(reader.ReadLine()) / 100; 
                reader.ReadLine();
                FullScreen = Convert.ToBoolean(reader.ReadLine());
                reader.ReadLine();
                TurretFocus = Convert.ToBoolean(reader.ReadLine());
                reader.Close();
            }
            catch
            {
                if (reader != null)
                    reader.Close();
                Console.WriteLine("Valid Settings Score file not found!");
                ResetToDefault();
                Console.WriteLine("Creating new settings file...");
                SaveSettingsToTxt();
                Console.WriteLine("Created.");
            }
        }

        /// <summary>
        /// SaveSettingsToTxt is a method that saves the current game settings to a text file, overwriting 
        /// and existing settings file.
        /// </summary>
        private void SaveSettingsToTxt()
        {
            StreamWriter writer = new StreamWriter(filepath);
            writer.WriteLine("Pan Speed - ");
            writer.WriteLine((PanSpeed * 100).ToString());
            writer.WriteLine("Zoom Speed - ");
            writer.WriteLine((ZoomSpeed * 100).ToString());
            writer.WriteLine("Sound Volume - ");
            writer.WriteLine((SoundVolume * 100).ToString());
            writer.WriteLine("Music Volume - ");
            writer.WriteLine((MusicVolume * 100).ToString());
            writer.WriteLine("FullScreen - ");
            writer.WriteLine(FullScreen.ToString());
            writer.WriteLine("Turret Focus - ");
            writer.WriteLine(TurretFocus.ToString());
            writer.Close();
        }

        /// <summary>
        /// ResetToDefault is a method that resets all game settings to their defaults.
        /// </summary>
        public void ResetToDefault()
        {
            TurretFocus = true;
            PanSpeed = 0.6f; //x10
            ZoomSpeed = 0.1f;
            FullScreen = true;
            SoundVolume = 1f;
            MusicVolume = 1f;
        }

        /// <summary>
        /// ApplySettings is a method that applies the current game settings to the running instance of the game 
        /// and calls the SaveSettingsToTxt to preserve them for later.
        /// </summary>
        public void ApplySettings()
        {
            game.Graphics.IsFullScreen = FullScreen;
            game.Graphics.ApplyChanges();

            game.AudioManager.ChangeVolume(SoundVolume, MusicVolume);

            SaveSettingsToTxt();
        }
    }
}