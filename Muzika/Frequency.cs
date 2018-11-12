using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Muzika
{
    public static class Frequency
    {
        //number used to calculate key frequency
        private const double twelveRootTwo = 1.0594630943592952646;

        //converts the number of tones below A4 into a frequency
        public static float CalculateFrequency(short tonesBelowA4)
        {
            short n = TonesToSemiTomesBelowA4(tonesBelowA4);

            return 440 * (float)Math.Pow(twelveRootTwo, -n);
        }

        //converts tones into semi tones in relation to and less than A4
        public static short TonesToSemiTomesBelowA4(short tonesFromA4)
        {
            short n = 0;
            for (short i = 0; i < tonesFromA4; i++)
            {
                if (i % 12 == 2 || i % 12 == 5)
                {
                    n++;
                }
                else
                {
                    n += 2;
                }
            }

            return n;
        }
    }
}