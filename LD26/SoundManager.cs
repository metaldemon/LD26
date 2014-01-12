using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD26
{
    class SoundManager
    {
        public static Audio.Sound Woosh;
        public static Audio.Sound Boom;
        public static Audio.Sound Hurt;
        public static Audio.Sound Lazer;
        public static Audio.Sound Powerup;
        public static Audio.Sound Notice;
        public static Audio.Sound Bash;

        public static Audio.Sound Music;



        public static void Initialize()
        {
            Music = new Audio.Sound("Content/Audio/music1.wav");
            Music.Looping = true;
            Woosh = new Audio.Sound("Content/Audio/woosh.wav");
            Boom = new Audio.Sound("Content/Audio/Boom.wav");
            Hurt = new Audio.Sound("Content/Audio/Hurt.wav");
            Lazer = new Audio.Sound("Content/Audio/lazer.wav");
            Powerup = new Audio.Sound("Content/Audio/pwerup.wav");
            Notice = new Audio.Sound("Content/Audio/Notice.wav");
            Bash = new Audio.Sound("Content/Audio/Bash.wav");
        }

        public static void Dispose()
        {
            Woosh.Dispose();
            Boom.Dispose();
            Hurt.Dispose();
            Lazer.Dispose();
            Powerup.Dispose();
        }
    }
}
