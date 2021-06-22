/* AudioManager.cs
 * Description: AudioManager is a class that inherits from the GameComponent class. 
 * It contains and controls all sounds and music used in the program.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.05: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// AudioManager: This class contains and controls all sounds and music used in the program.
    /// </summary>
    public class AudioManager : GameComponent
    {
        private Song menuMusic;
        private Song gameMusic;

        private SoundEffect explosion1;
        private SoundEffect explosion2;
        private SoundEffect explosion3;
        private SoundEffect playerShoot;
        private SoundEffect enemyShoot;

        /// <summary>
        /// Primary constructor for the AudioManager class.
        /// </summary>
        /// <param name="game">A reference to the Game class that is the parent of this class.</param>
        public AudioManager(Game game): base (game)
        {
            MediaPlayer.IsRepeating = true;
            menuMusic = game.Content.Load<Song>("Sound/menuMusic");
            gameMusic = game.Content.Load<Song>("Sound/gameMusic");
            explosion1 = game.Content.Load<SoundEffect>("Sound/boom1");
            explosion2 = game.Content.Load<SoundEffect>("Sound/boom2");
            explosion3 = game.Content.Load<SoundEffect>("Sound/boom3");
            playerShoot = game.Content.Load<SoundEffect>("Sound/cannon");
            enemyShoot = game.Content.Load<SoundEffect>("Sound/laser");
        }

        /// <summary>
        /// ChangeVolume is a method that changes the master volume levels for music and sound effects.
        /// </summary>
        /// <param name="soundVolume">The new volume level of sounds effects. 0.0 - 1.0f is 0-100%.</param>
        /// <param name="musicVolume">The new volume level of music. 0.0 - 1.0f is 0-100%.</param>
        internal void ChangeVolume(float soundVolume, float musicVolume)
        {
            MediaPlayer.Volume = musicVolume;
            SoundEffect.MasterVolume = soundVolume;
        }

        /// <summary>
        /// PlaySoundEffect is a method that plays a specific sound effect.
        /// </summary>
        /// <param name="title">The name of the sound effect to be played</param>
        /// <param name="volume">An optional parameter, scales the sound effect volume to a percentage.</param>
        public void PlaySoundEffect(string title, float volume = 1f)
        {
            //volume can not be more or less than 0 or 100%.
            volume = MathHelper.Clamp(volume, 0f, 1.0f);

            switch(title)
            {
                case "explosion1":
                    explosion1.Play(volume, 0, 0);
                    break;
                case "explosion2":
                    explosion2.Play(volume * 0.6f, 0, 0);
                    break;
                case "explosion3":
                    explosion3.Play(volume * 0.5f, 0, 0);
                    break;
                case "playerShoot":
                    playerShoot.Play(volume * 0.4f, 0, 0);
                    break;
                case "enemyShoot":
                    enemyShoot.Play(volume * 0.5f, 0, 0);
                    break;
                default:
                    Console.WriteLine("Sound effect " + title + " was not found.");
                    break;
            }
        }

        /// <summary>
        /// PlayMenuMusic is a method that starts to play the Menu music.
        /// </summary>
        public void PlayMenuMusic()
        {
            PlaySong(menuMusic);
        }

        /// <summary>
        /// PlayGameMusic is a method that starts to play the game music.
        /// </summary>
        public void PlayGameMusic()
        {
            PlaySong(gameMusic);
        }

        /// <summary>
        /// PlaySong is a method that starts to play a specified song.
        /// </summary>
        /// <param name="song">The song to be played.</param>
        private void PlaySong(Song song)
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(song);
        }
    }
}