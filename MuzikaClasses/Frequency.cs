using System;

namespace MuzikaClasses
{
    public static class Frequency
    {
        //number used to calculate key frequency
		private const int A4Hz = 440;

        //converts the number of tones below A4 into a frequency
        public static float CalculateFrequencyByTone(short tonesBelowA4)
        {
            short n = TonesToSemiTomesBelowA4(tonesBelowA4);

            return A4Hz * (float)Math.Pow(Numbers.TwelveRootTwo, -n);
        }

        //converts the number of semitones from A4 into a frequency
        public static float CalculateFrequencyBySemitone(short ASemitoneDifference)
        {
            return A4Hz * (float)Math.Pow(Numbers.TwelveRootTwo, ASemitoneDifference);
        }

        //converts tones into semi tones in relation to and less than A4
        public static short TonesToSemiTomesBelowA4(short tonesFromA4)
        {
            short n = 0;
            for (short i = 0; i < tonesFromA4; i++)
            {
                if (i % Numbers.Twelve == Numbers.Two || i % Numbers.Twelve == Numbers.Five)
                {
                    n++;
                }
                else
                {
                    n += Numbers.Two;
                }
            }

            return n;
        }
    }
}