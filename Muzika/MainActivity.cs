using Android.App;
using Android.Widget;
using Android.OS;
using static Android.Views.View;
using System;
using Android.Views;

namespace Muzika
{
    [Activity(Label = "Muzika", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private bool IsDrawAGridHidden = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            View v_Overlay = FindViewById<View>(Resource.Id.v_Overlay);

            v_Overlay.Touch += new EventHandler<TouchEventArgs>(OverlayTouch);

            v_Overlay.Drag += new EventHandler<DragEventArgs>(OverlayDrag);
        }

        /// <summary>
        /// capture overlay Touch event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverlayTouch(object sender, TouchEventArgs e)
        {
            if (!IsDrawAGridHidden)
            {
                TextView tv_DrawAGrid = FindViewById<TextView>(Resource.Id.tv_DrawAGrid);

                ViewMethods.Display.Hide(tv_DrawAGrid);

                IsDrawAGridHidden = true;
            }
        }

        /// <summary>
        /// capture overlay Drag event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverlayDrag(object sender, DragEventArgs e)
        {
            View v_GridDrawer = FindViewById<View>(Resource.Id.v_GridDrawer);

            v_GridDrawer.Visibility = ViewStates.Visible;
        }
    }
}

