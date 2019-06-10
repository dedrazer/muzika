using System;
using System.Collections.Generic;
using Android.Media;
using Android.Content.Res;

namespace MuzikaClasses
{
    public class Synthesizer
    {
        //declare variables
        int Level = 0;
        int[] SoundIds, PercIds;
        short Keys;
        Stack<short> Playing, PlayingPercussion;
        SoundPool[] Lead, Percussion;
        OnLoadCompleteListener OnLoadCompleteListener = new OnLoadCompleteListener();

        const int Synth = 1;

        /// <summary>
        /// initiate a synthesizer with the given amount of keys
        /// </summary>
        /// <param name="keys">3 to 12 keys</param>
        public Synthesizer(AssetManager assets, short keys)
        {
            Keys = keys;
            if (keys > 2 && keys < 13)
            {
                //prepare stacks
                Playing = new Stack<short>();
                PlayingPercussion = new Stack<short>();

                //load sounds
                SoundPool.Builder soundPoolBuilder = new SoundPool.Builder();
                Lead = new SoundPool[keys];

                //prepare percussion
                PercIds = new int[3];

                //prepare kick
                SoundPool kick = soundPoolBuilder.Build();
                kick.SetOnLoadCompleteListener(OnLoadCompleteListener);
                PercIds[0] = kick.Load(assets.OpenFd("perc/kick.mp3"), 1);

                //prepare snare
                SoundPool snare = soundPoolBuilder.Build();
                snare.SetOnLoadCompleteListener(OnLoadCompleteListener);
                PercIds[1] = snare.Load(assets.OpenFd("perc/snare.mp3"), 1);

                //prepare hi-hat
                SoundPool hat = soundPoolBuilder.Build();
                snare.SetOnLoadCompleteListener(OnLoadCompleteListener);
                PercIds[2] = hat.Load(assets.OpenFd("perc/hat.mp3"), 1);

                Percussion = new SoundPool[3]
                    {
                        kick,
                        snare,
                        hat
                    };

                //prepare lead
                SoundIds = new int[keys];

                for (int l = 0; l < keys; l++)
                {

                    SoundPool lead = soundPoolBuilder.Build();
                    lead.SetOnLoadCompleteListener(OnLoadCompleteListener);
                    SoundIds[0] = lead.Load(assets.OpenFd("keys/" + Synth + "/A.mp3"), 1);
                    Lead[l] = lead;
                }
            }
            else
            {
                throw new IndexOutOfRangeException("Synthesizer size must be between 3 and 12.");
            }
        }

        /// <summary>
        /// play the key of the given index
        /// </summary>
        /// <param name="key">key index</param>
        public void Play(short aKeyDifference, int simultaneousKeys, short shift)
        {
            if (OnLoadCompleteListener.Loaded)
            {
                float volume = 1;

                //create an inverse relationship between number of simultaneous keys and volume
                if (simultaneousKeys > 1)
                    volume -= (float)simultaneousKeys / (simultaneousKeys + 1);

                short degree = (short)Numbers.AllMaj[Keys - 3][aKeyDifference];
                Console.WriteLine("Degree: " + degree);
                Console.WriteLine("Shift: " + shift);
                float frequency = (Frequency.CalculateFrequencyBySemitone((short)(degree+shift))/440f)/2;
                //Console.WriteLine("Frequency: " + frequency);

                //Console.WriteLine("simultaneous notes: {0}, volume: {1}", simultaneousKeys, volume);
                Lead[Level].Play(SoundIds[0], volume, volume, 1, 0, frequency);
                Playing.Push(Synth);

                Level++;
            }
            else
                SynthNotLoadedWarning("note");
        }

        /// <summary>
        /// stop all sounds from playing
        /// </summary>
        public void Stop()
        {
            while (Playing.Count > 0)
            {
                short index = Playing.Pop();
                Lead[Synth].Stop(SoundIds[index]);
            }

            Level = 0;

            while (PlayingPercussion.Count > 0)
            {
                short index = PlayingPercussion.Pop();
                Percussion[index].Stop(PercIds[index]);
            }
        }

        /// <summary>
        /// kick
        /// </summary>
        public void Kick()
        {
            if (OnLoadCompleteListener.Loaded)
            {
                Percussion[0].Play(PercIds[0], 1f, 1f, 1, 0, 1);
                PlayingPercussion.Push(0);
            }
            else
                SynthNotLoadedWarning("kick");
        }

        /// <summary>
        /// hi-hat
        /// </summary>
        public void Hat()
        {
            if (OnLoadCompleteListener.Loaded)
            {
                Percussion[2].Play(PercIds[2], 1f, 1f, 1, 0, 1);
                PlayingPercussion.Push(2);
            }
            else
                SynthNotLoadedWarning("hat");
        }

        /// <summary>
        /// snare
        /// </summary>
        public void Snare()
        {
            if (OnLoadCompleteListener.Loaded)
            {
                Percussion[1].Play(PercIds[1], 1f, 1f, 1, 0, 1);
                PlayingPercussion.Push(1);
            }
            else
                SynthNotLoadedWarning("snare");
        }

        /// <summary>
        /// write a console warning that an attempt was made to play a sound when the synthesizer was not yet loaded
        /// </summary>
        /// <param name="element">the sound which was attempted</param>
        public void SynthNotLoadedWarning(string element)
        {
            Console.WriteLine("WARNING: Tried to play a {0} when the synthesizer had not yet loaded.", element);
        }
    }
}