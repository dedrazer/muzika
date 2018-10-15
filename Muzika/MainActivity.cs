using Android.App;
using Android.Widget;
using Android.OS;
using static Android.Views.View;
using System;
using Android.Views;
using Android.Graphics;

namespace Muzika
{
    [Activity(Label = "Muzika", MainLauncher = true)]
    public class MainActivity : Activity
    {
        //declare variables
        private bool IsDrawAGridHidden = false;
        int cellMagnitude;
        float startX;
        float startY;
        Point resolution = new Point();
        TextView tv_x;
        TextView tv_y;
        TextView tv_startX;
        TextView tv_startY;
        TextView tv_endX;
        TextView tv_endY;
        GridLayout gl_GridDrawer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // get screen resolution
            gl_GridDrawer = FindViewById<GridLayout>(Resource.Id.gv_GridDrawer);
            var disp = WindowManager.DefaultDisplay;
            disp.GetSize(resolution);

            //set grid cell magnitude
            //to the smallest resolution attribute /12
            if (resolution.X > resolution.Y)
            {
                cellMagnitude = (int)Math.Round((decimal)resolution.Y / 12);
            }
            else
            {
                cellMagnitude = (int)Math.Round((decimal)resolution.X / 12);
            }
            
            //initialize variables
            View v_Overlay = FindViewById<View>(Resource.Id.v_Overlay);
            tv_x = FindViewById<TextView>(Resource.Id.tv_x);
            tv_y = FindViewById<TextView>(Resource.Id.tv_y);
            tv_startX = FindViewById<TextView>(Resource.Id.tv_startX);
            tv_startY = FindViewById<TextView>(Resource.Id.tv_startY);
            tv_endX = FindViewById<TextView>(Resource.Id.tv_endX);
            tv_endY = FindViewById<TextView>(Resource.Id.tv_endY);

            //bind overlay touch event
            v_Overlay.Touch += new EventHandler<TouchEventArgs>(OverlayTouch);
        }

        /// <summary>
        /// capture overlay touch event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverlayTouch(object sender, TouchEventArgs e)
        {
            try
            {
                if (!IsDrawAGridHidden)
                //hide prompt
                {
                    TextView tv_DrawAGrid = FindViewById<TextView>(Resource.Id.tv_DrawAGrid);

                    MuzikaClasses.Display.Hide(tv_DrawAGrid);

                    IsDrawAGridHidden = true;
                }

                float x = e.Event.GetX();
                float y = e.Event.GetY();

                tv_x.Text = "x: " + x;
                tv_y.Text = "y: " + y;

                if (e.Event.Action == MotionEventActions.Down)
                //touch
                {
                    gl_GridDrawer.SetX(x);
                    gl_GridDrawer.SetY(y);
                    gl_GridDrawer.Visibility = ViewStates.Visible;

                    tv_startX.Text = "start x: " + x;
                    tv_startY.Text = "start y: " + y;

                    startX = x;
                    startY = y;
                }
                else if (e.Event.Action == MotionEventActions.Up)
                //release
                {
                    gl_GridDrawer.Visibility = ViewStates.Invisible;

                    tv_endX.Text = "end x: " + x;
                    tv_endY.Text = "end y: " + y;
                }

                int width = Convert.ToInt32(Math.Floor(x - startX));
                int height = Convert.ToInt32(Math.Floor(y - startY));
                
                int gridWidth = (int)Math.Floor((decimal)width / cellMagnitude);
                int gridHeight = (int)Math.Floor((decimal)height / cellMagnitude);

                int pixelWidth = gridWidth * cellMagnitude;
                int pixelHeight = gridHeight * cellMagnitude;

                if (width < 0)
                {
                    width = -1 * width;
                    gridWidth = -1 * gridWidth;
                    pixelWidth = -1 * pixelWidth;
                    gl_GridDrawer.SetX(startX - pixelWidth);
                }
                else
                {
                    gl_GridDrawer.SetX(startX);
                }
                if (height < 0)
                {
                    height = -1 * height;
                    gridHeight = -1 * gridHeight;
                    pixelHeight = -1 * pixelHeight;
                    gl_GridDrawer.SetY(startY - pixelHeight);
                }
                else
                {
                    gl_GridDrawer.SetY(startY);
                }

                //set grid drawer size
                ViewGroup.LayoutParams gridDrawerLayoutParams = gl_GridDrawer.LayoutParameters;

                gridDrawerLayoutParams.Width = pixelWidth;
                gridDrawerLayoutParams.Height = pixelHeight;

                gl_GridDrawer.LayoutParameters = gridDrawerLayoutParams;
                gl_GridDrawer.RemoveAllViews();

                ViewGroup.LayoutParams cellLayoutParams = new ViewGroup.LayoutParams(cellMagnitude, cellMagnitude);

                //draw grid
                for (int yCell = 0; yCell < gridHeight; yCell++)
                {
                    for (int xCell = 0; xCell < gridWidth; xCell++)
                    {
                        //define cell
                        View cell = new View(this);
                        cell.SetBackgroundColor(Color.AliceBlue);
                        cell.LayoutParameters = cellLayoutParams;
                        gl_GridDrawer.AddView(cell);
                    }
                }

                gl_GridDrawer.ColumnCount = gridWidth;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

