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
using Android.Media;

namespace MuzikaClasses
{
    public class OnLoadCompleteListener : Java.Lang.Object, SoundPool.IOnLoadCompleteListener
    {
        public bool Loaded = false;

        public void OnLoadComplete(SoundPool soundPool, int sampleId, int status)
        {
            Loaded = true;
        }
    }
}