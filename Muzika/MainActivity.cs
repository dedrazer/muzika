using Android.App;
using Android.Widget;
using Android.OS;
using static Android.Views.View;
using System;
using Android.Views;
using Android.Graphics;
using static Android.Views.ViewGroup;
using Android.Util;

namespace Muzika
{
    [Activity(Label = "Muzika", MainLauncher = true)]
    public class MainActivity : Activity
    {
        //declare variables
        private bool IsDrawAGridHidden = false;
        int drawingCellMagnitude;
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
        GridLayout gl_Grid;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            gl_GridDrawer = FindViewById<GridLayout>(Resource.Id.gl_GridDrawer);

            checkResolution();

            //set grid cell magnitude
            //to the smallest resolution attribute / 13
            if (resolution.X > resolution.Y)
            {
                drawingCellMagnitude = (int)Math.Floor((decimal)resolution.Y / 13);
            }
            else
            {
                drawingCellMagnitude = (int)Math.Floor((decimal)resolution.X / 13);
            }
            
            //initialize variables
            View v_Overlay = FindViewById<View>(Resource.Id.v_Overlay);
            tv_x = FindViewById<TextView>(Resource.Id.tv_x);
            tv_y = FindViewById<TextView>(Resource.Id.tv_y);
            tv_startX = FindViewById<TextView>(Resource.Id.tv_startX);
            tv_startY = FindViewById<TextView>(Resource.Id.tv_startY);
            tv_endX = FindViewById<TextView>(Resource.Id.tv_endX);
            tv_endY = FindViewById<TextView>(Resource.Id.tv_endY);
            gl_Grid = FindViewById<GridLayout>(Resource.Id.gl_Grid);

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

                int width = Convert.ToInt32(Math.Floor(x - startX));
                int height = Convert.ToInt32(Math.Floor(y - startY));

                if (width > drawingCellMagnitude * 12)
                {
                    width = drawingCellMagnitude * 12;
                }
                if (height > drawingCellMagnitude * 12)
                {
                    height = drawingCellMagnitude * 12;
                }

                int gridWidth = (int)Math.Floor((decimal)width / drawingCellMagnitude);
                int gridHeight = (int)Math.Floor((decimal)height / drawingCellMagnitude);

                int pixelWidth = gridWidth * drawingCellMagnitude;
                int pixelHeight = gridHeight * drawingCellMagnitude;

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

                    if (gridWidth > 0 && gridHeight > 0)
                    //if grid was drawn, prepare it
                    {
                        checkResolution();

                        //check logistics
                        float screenAspect = (float)resolution.X / resolution.Y;
                        float gridAspect = (float)gridWidth / gridHeight;

                        int cellMagnitude;
                        if (gridAspect > screenAspect)
                        //grid is wider than screen
                        {
                            cellMagnitude = (int)Math.Ceiling((decimal)resolution.X / gridWidth);
                            int gridPixelHeight = cellMagnitude * gridHeight;

                            gl_Grid.SetX(0);
                            gl_Grid.SetY((resolution.Y - gridPixelHeight) / 2);
                            LayoutParams layoutParams = gl_Grid.LayoutParameters;
                            layoutParams.Width = -1;
                            layoutParams.Height = gridPixelHeight;
                            gl_Grid.LayoutParameters = layoutParams;
                        }
                        else
                        //grid is thinner than screen
                        {
                            cellMagnitude = (int)Math.Ceiling((decimal)resolution.Y / gridHeight);
                            int gridPixelWidth = cellMagnitude * gridWidth;

                            gl_Grid.SetY(0);
                            gl_Grid.SetX((resolution.X - gridPixelWidth) / 2);
                            LayoutParams layoutParams = gl_Grid.LayoutParameters;
                            layoutParams.Width = gridPixelWidth;
                            layoutParams.Height = -1;
                            gl_Grid.LayoutParameters = layoutParams;
                        }

                        gl_Grid.RemoveAllViews();

                        LayoutParams gridCellLayoutParams = new LayoutParams(cellMagnitude, cellMagnitude);
                        Bitmap gridCellBitmap = BitmapFactory.DecodeStream(Assets.Open("cell.png"));

                        //draw grid
                        for (int yCell = 0; yCell < gridHeight; yCell++)
                        {
                            for (int xCell = 0; xCell < gridWidth; xCell++)
                            {
                                //define cell
                                ImageView cell = new ImageView(this);
                                cell.SetImageBitmap(gridCellBitmap);
                                cell.LayoutParameters = gridCellLayoutParams;
                                gl_Grid.AddView(cell);
                            }
                        }

                        //define column count
                        gl_Grid.ColumnCount = gridWidth;

                        gl_Grid.Visibility = ViewStates.Visible;
                    }
                    return;
                }

                //set grid drawer size
                LayoutParams gridDrawerLayoutParams = gl_GridDrawer.LayoutParameters;

                gridDrawerLayoutParams.Width = pixelWidth;
                gridDrawerLayoutParams.Height = pixelHeight;

                gl_GridDrawer.LayoutParameters = gridDrawerLayoutParams;
                gl_GridDrawer.RemoveAllViews();

                LayoutParams cellLayoutParams = new ViewGroup.LayoutParams(drawingCellMagnitude, drawingCellMagnitude);
                Bitmap cellBitmap = BitmapFactory.DecodeStream(Assets.Open("cell.png"));
                
                //draw grid
                for (int yCell = 0; yCell < gridHeight; yCell++)
                {
                    for (int xCell = 0; xCell < gridWidth; xCell++)
                    {
                        //define cell
                        ImageView cell = new ImageView(this);
                        cell.SetBackgroundColor(Color.AliceBlue);
                        cell.SetImageBitmap(cellBitmap);
                        cell.LayoutParameters = cellLayoutParams;
                        gl_GridDrawer.AddView(cell);
                    }
                }

                //define column count
                gl_GridDrawer.ColumnCount = gridWidth;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void checkResolution()
        {
            int resourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
            int statusBarHeight = Resources.GetDimensionPixelSize(resourceId);

            resolution = new Point(Application.Resources.DisplayMetrics.WidthPixels, Application.Resources.DisplayMetrics.HeightPixels - statusBarHeight);
        }
    }
}

