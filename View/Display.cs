using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuzikaClasses
{
    public static class Display
    {
        /// <summary>
        /// hides given view
        /// </summary>
        /// <param name="view"></param>
        public static void Hide(Android.Views.View view)
        {
            view.Visibility = Android.Views.ViewStates.Invisible;
        }
    }
}
