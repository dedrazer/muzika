using System;

namespace MuzikaClasses
{
    public class Numbers
    {
        #region numbers
        public const short Zero = 0;
        public const short One = 1;
        public const short Two = 2;
        public const short Three = 3;
        public const short Four = 4;
        public const short Five = 5;
        public const short Six = 6;
        public const short Seven = 7;
        public const short Eight = 8;
        public const short Nine = 9;
        public const short Ten = 10;
        public const short Eleven = 11;
        public const short Twelve = 12;
        public const short Fifteen = 15;
        public const short Sixteen = 16;
        public const short Seventeen = 17;
        public const short Nineteen = 19;
        public const short TwentyFour = 24;
        public static readonly double TwelveRootTwo = Math.Pow(Two, (double)One / Twelve);
        #endregion

        #region keys
        public const string Eb6 = "Eb6";
        public const string D6 = "D6";
        public const string Db6 = "Db6";
        public const string C6 = "C6";
        public const string B5 = "B5";
        public const string Bb5 = "Bb5";
        public const string A5 = "A5";
        public const string Ab5 = "Ab5";
        public const string G5 = "G5";
        public const string Gb5 = "Gb5";
        public const string F5 = "F5";
        public const string E5 = "E5";
        public const string Eb5 = "Eb5";
        public const string D5 = "D5";
        public const string Db5 = "Db5";
        public const string C5 = "C5";
        public const string B4 = "B4";
        public const string Bb4 = "Bb4";
        public const string A4 = "A";
        public const string Ab4 = "Ab";
        public const string G4 = "G";
        public const string Gb4 = "Gb";
        public const string F4 = "F";
        public const string E4 = "E";
        public const string Eb4 = "Eb";
        public const string D4 = "D";
        public const string Db4 = "Db";
        public const string C4 = "C";
        public const string B3 = "B";
        public const string Bb3 = "Bb";
        public const string A3 = "A3";
        public const string Ab3 = "Ab3";
        public const string G3 = "G3";
        public const string Gb3 = "Gb3";
        public const string F3 = "F3";
        public const string E3 = "E3";
        public const string Eb3 = "Eb3";
        #endregion

        #region tones
        public static readonly string[] AllTonesC = { C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5, A5, Bb5, B5, C6 };
        public static readonly string[] AllTonesG = { G3, Ab3, A3, Bb3, B3, C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5, Gb5, G5 };
        public static readonly string[] AllTonesD = { D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5, A5, Bb5, B5, C6, Db6, D6 };
        public static readonly string[] AllTonesA = { A3, Bb3, B3, C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5, A5 };
        public static readonly string[] AllTonesE = { E3, F3, Gb3, G3, Ab3, A3, Bb3, B3, C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5 };
        public static readonly string[] AllTonesB = { B3, C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5, A5, Bb5, B5 };
        public static readonly string[] AllTonesGb = { Gb3, G3, Ab3, A3, Bb3, B3, C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5, Gb5 };
        public static readonly string[] AllTonesDb = { Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5, A5, Bb5, B5, C6, Db6 };
        public static readonly string[] AllTonesAb = { Ab3, A3, Bb3, B3, C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5 };
        public static readonly string[] AllTonesEb = { Eb3, E3, F3, Gb3, G3, Ab3, A3, Bb3, B3, C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5 };
        public static readonly string[] AllTonesBb = { Bb3, B3, C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5, A5, Bb5 };
        public static readonly string[] AllTonesF = { F3, Gb3, G3, Ab3, A3, Bb3, B3, C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5 };

        public static readonly CircularArray2D<string> AllTones = new CircularArray2D<string>(12, 24)
        {
            Content = new string[12, 25]
            {
                 { C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4,
                    C5, Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5, A5, Bb5, B5, C6 },
                 { G3, Ab3, A3, Bb3, B3, C4, Db4, D4, Eb4, E4, F4, Gb4,
                    G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5, Gb5, G5 },
                 { D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5,
                    D5, Eb5, E5, F5, Gb5, G5, Ab5, A5, Bb5, B5, C6, Db6, D6 },
                 { A3, Bb3, B3, C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4,
                    A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5, A5 },
                 { E3, F3, Gb3, G3, Ab3, A3, Bb3, B3, C4, Db4, D4, Eb4,
                    E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5 },
                 { B3, C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4,
                    B4, C5, Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5, A5, Bb5, B5 },
                 { Gb3, G3, Ab3, A3, Bb3, B3, C4, Db4, D4, Eb4, E4, F4,
                    Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5, Gb5 },
                 { Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5,
                    Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5, A5, Bb5, B5, C6, Db6 },
                 { Ab3, A3, Bb3, B3, C4, Db4, D4, Eb4, E4, F4, Gb4, G4,
                    Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5 },
                 { Eb3, E3, F3, Gb3, G3, Ab3, A3, Bb3, B3, C4, Db4, D4,
                    Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5 },
                 { Bb3, B3, C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4,
                    Bb4, B4, C5, Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5, A5, Bb5 },
                 { F3, Gb3, G3, Ab3, A3, Bb3, B3, C4, Db4, D4, Eb4, E4,
                    F4, Gb4, G4, Ab4, A4, Bb4, B4, C5, Db5, D5, Eb5, E5, F5 },
            }
        };
        #endregion

        #region scales
        public static readonly int[] ThreeTonesMaj = { Zero, Four, Seven };
        public static readonly int[] FourTonesMaj = { Zero, Four, Seven, Twelve };
        public static readonly int[] FiveTonesMaj = { Zero, Four, Seven, Eleven, Twelve };
        public static readonly int[] SixTonesMaj = { Zero, Two, Four, Seven, Eleven, Twelve };
        public static readonly int[] SevenTonesMaj = { Zero, Two, Four, Five, Seven, Eleven, Twelve };
        public static readonly int[] EightTonesMaj = { Zero, Two, Four, Five, Seven, Nine, Eleven, Twelve };
        public static readonly int[] NineTonesMaj = { Zero, Two, Four, Five, Seven, Nine, Eleven, Twelve, Nineteen };
        public static readonly int[] TenTonesMaj = { Zero, Two, Four, Five, Seven, Nine, Eleven, Twelve, Sixteen, Nineteen };
        public static readonly int[] ElevenTonesMaj = { Zero, Two, Four, Five, Seven, Nine, Eleven, Twelve, Sixteen, Nineteen, TwentyFour };
        public static readonly int[] TwelveTonesMaj = { Zero, Two, Four, Five, Seven, Nine, Eleven, Twelve, Sixteen, Seventeen, Nineteen, TwentyFour };

        public static readonly int[][] AllMaj = { ThreeTonesMaj, FourTonesMaj, FiveTonesMaj, SixTonesMaj, SevenTonesMaj, EightTonesMaj, NineTonesMaj, TenTonesMaj, ElevenTonesMaj, TwelveTonesMaj };
        #endregion

        private Numbers() { }
    }
}