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