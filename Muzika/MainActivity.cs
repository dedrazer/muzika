using Android.App;
using Android.Widget;
using Android.OS;
using static Android.Views.View;
using System;
using Android.Views;
using Android.Graphics;
using static Android.Views.ViewGroup;
using Android.Util;
using System.Collections.Generic;
using Android.Media;
using MuzikaClasses;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Timers;

namespace Muzika
{
    [Activity(Label = "Muzika", MainLauncher = true)]
    public class MainActivity : Activity
    {
        //declare variables
        Bitmap deactivatedCellBitmap, activeCellBitmap;
        private bool isDrawAGridHidden = false;
        bool[,] cellValues;
        Button b_Play;
        int drawingCellMagnitude;
        float startX, startY;
        GridLayout gl_Grid, gl_GridDrawer;
        Point resolution = new Point();
        short width, height, currentColumn;
        Synthesizer synthesizer;
        TextView tv_x, tv_y, tv_startX, tv_startY, tv_endX, tv_endY;
        View v_Overlay;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            gl_GridDrawer = FindViewById<GridLayout>(Resource.Id.gl_GridDrawer);

            CheckResolution();

            //define grid cell magnitude
            //as the smallest resolution attribute / 13
            if (resolution.X > resolution.Y)
            {
                drawingCellMagnitude = (int)Math.Floor((decimal)resolution.Y / 13);
            }
            else
            {
                drawingCellMagnitude = (int)Math.Floor((decimal)resolution.X / 13);
            }

            //initialize variables
            v_Overlay = FindViewById<View>(Resource.Id.v_Overlay);
            tv_x = FindViewById<TextView>(Resource.Id.tv_x);
            tv_y = FindViewById<TextView>(Resource.Id.tv_y);
            tv_startX = FindViewById<TextView>(Resource.Id.tv_startX);
            tv_startY = FindViewById<TextView>(Resource.Id.tv_startY);
            tv_endX = FindViewById<TextView>(Resource.Id.tv_endX);
            tv_endY = FindViewById<TextView>(Resource.Id.tv_endY);
            gl_Grid = FindViewById<GridLayout>(Resource.Id.gl_Grid);
            b_Play = FindViewById<Button>(Resource.Id.b_Play);

            activeCellBitmap = BitmapFactory.DecodeStream(Assets.Open("cell-active.png"));
            deactivatedCellBitmap = BitmapFactory.DecodeStream(Assets.Open("cell.png"));

            b_Play.Touch += new EventHandler<TouchEventArgs>(ButtonTouch);

            //bind overlay touch event
            v_Overlay.Touch += new EventHandler<TouchEventArgs>(OverlayTouch);
        }

        #region Events
        /// <summary>
        /// capture overlay touch event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverlayTouch(object sender, TouchEventArgs e)
        {
            try
            {
                if (!isDrawAGridHidden)
                //hide prompt
                {
                    TextView tv_DrawAGrid = FindViewById<TextView>(Resource.Id.tv_DrawAGrid);

                    MuzikaClasses.Display.Hide(tv_DrawAGrid);

                    isDrawAGridHidden = true;
                }

                //record x and y
                float x = e.Event.GetX();
                float y = e.Event.GetY();

                tv_x.Text = "x: " + x;
                tv_y.Text = "y: " + y;

                //calculate width
                int width = Convert.ToInt32(Math.Floor(x - startX));
                int height = Convert.ToInt32(Math.Floor(y - startY));

                bool negativeWidth = false;
                bool negativeHeight = false;
                //convert negatives to positives
                if (width < 0)
                {
                    width = -1 * width;
                    negativeWidth = true;
                }
                if (height < 0)
                {
                    height = -1 * height;
                    negativeHeight = true;
                }

                //cap width
                if (width > drawingCellMagnitude * 12)
                {
                    width = drawingCellMagnitude * 12;
                }
                if (height > drawingCellMagnitude * 12)
                {
                    height = drawingCellMagnitude * 12;
                }

                //calculate number of cells
                short gridWidth = (short)Math.Floor((decimal)width / drawingCellMagnitude);
                short gridHeight = (short)Math.Floor((decimal)height / drawingCellMagnitude);

                //calculate grid size
                int pixelWidth = gridWidth * drawingCellMagnitude;
                int pixelHeight = gridHeight * drawingCellMagnitude;

                //dynamic starting point for negative co-ordinates
                if (negativeWidth)
                {
                    gl_GridDrawer.SetX(startX - pixelWidth);
                }
                if (negativeHeight)
                {
                    gl_GridDrawer.SetY(startY - pixelHeight);
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

                    if (gridWidth > 1 && gridHeight > 2)
                    //if grid was drawn, prepare it
                    //minimum size 3x2
                    {
                        CheckResolution();

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

                        cellValues = new bool[gridWidth, gridHeight];
                        //draw grid
                        for (int yCell = 0; yCell < gridHeight; yCell++)
                        {
                            for (int xCell = 0; xCell < gridWidth; xCell++)
                            {
                                cellValues[xCell, yCell] = false;
                                //define cell
                                ImageView cell = new ImageView(this);
                                cell.SetTag(Resource.String.cellOn, null);
                                cell.SetTag(Resource.String.cellX, xCell);
                                cell.SetTag(Resource.String.cellY, yCell);
                                cell.Touch += new EventHandler<TouchEventArgs>(ToggleCell);
                                cell.SetImageBitmap(gridCellBitmap);
                                cell.LayoutParameters = gridCellLayoutParams;
                                gl_Grid.AddView(cell);
                            }
                        }

                        //define column count
                        gl_Grid.ColumnCount = gridWidth;

                        //instantiate synthesizer
                        synthesizer = new Synthesizer(Assets, gridHeight);

                        this.width = gridWidth;
                        this.height = gridHeight;

                        gl_Grid.Visibility = ViewStates.Visible;
                        b_Play.Visibility = ViewStates.Visible;
                        v_Overlay.Visibility = ViewStates.Gone;
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

        /// <summary>
        /// capture cell touch event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleCell(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            //only consider press
            {
                ImageView senderImage = (ImageView)sender;

                if (senderImage.GetTag(Resource.String.cellOn) == null)
                //activate cell
                {
                    ActivateCell(senderImage);
                    cellValues[(int)senderImage.GetTag(Resource.String.cellX), (int)senderImage.GetTag(Resource.String.cellY)] = true;
                }
                else
                //deactivate cell
                {
                    DeactivateCell(senderImage);
                    cellValues[(int)senderImage.GetTag(Resource.String.cellX), (int)senderImage.GetTag(Resource.String.cellY)] = false;
                }
            }
        }

        /// <summary>
        /// graphically activate grid cell
        /// </summary>
        /// <param name="senderImage"></param>
        private void ActivateCell(ImageView senderImage)
        {
            senderImage.SetTag(Resource.String.cellOn, true);
            senderImage.SetImageBitmap(activeCellBitmap);
            cellValues[Convert.ToInt32(senderImage.GetTag(Resource.String.cellX)), Convert.ToInt32(senderImage.GetTag(Resource.String.cellY))] = true;
            //Console.WriteLine("cell ({0}, {1}) is now on", Convert.ToInt32(senderImage.GetTag(Resource.String.cellX)), Convert.ToInt32(senderImage.GetTag(Resource.String.cellY)));
        }

        /// <summary>
        ///  graphically deactivate grid cell
        /// </summary>
        /// <param name="senderImage"></param>
        private void DeactivateCell(ImageView senderImage)
        {
            senderImage.SetTag(Resource.String.cellOn, null);
            senderImage.SetImageBitmap(deactivatedCellBitmap);
            cellValues[Convert.ToInt32(senderImage.GetTag(Resource.String.cellX)), Convert.ToInt32(senderImage.GetTag(Resource.String.cellY))] = false;
            //Console.WriteLine("cell ({0}, {1}) is now off", Convert.ToInt32(senderImage.GetTag(Resource.String.cellX)), Convert.ToInt32(senderImage.GetTag(Resource.String.cellY)));
        }

        /// <summary>
        /// capture play button touch event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTouch(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            //only consider press
            {
                //b_Play.Visibility = ViewStates.Invisible;

                currentColumn = 0;

                Timer metronome = new Timer();

                metronome.AutoReset = true;

                metronome.Interval = 1000;

                metronome.Elapsed += Tick;

                metronome.Start();
            }
        }
        #endregion

        /// <summary>
        /// assigns the screen resolution excluding the status bar to the local resolution variable
        /// </summary>
        private void CheckResolution()
        {
            int resourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
            int statusBarHeight = Resources.GetDimensionPixelSize(resourceId);

            resolution = new Point(Application.Resources.DisplayMetrics.WidthPixels, Application.Resources.DisplayMetrics.HeightPixels - statusBarHeight);
        }

        /// <summary>
        /// update GridLayout
        /// </summary>
        /// <param name="cellValues"></param>
        private void SetGrid(bool[,] newCellValues)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //get cell
                    ImageView cell = (ImageView)gl_Grid.GetChildAt((y * width) + x);

                    //if cell is off
                    if (cell.GetTag(Resource.String.cellOn) == null)
                    {
                        //and cell value is to become on
                        if (newCellValues[x, y])
                        {
                            //activate the cell
                            ActivateCell(cell);
                        }
                    }
                    //if cell value should be off
                    else if (!newCellValues[x, y])
                    {
                        //make sure it is off
                        ///OPTIMIZATION: do not update if already off
                        DeactivateCell(cell);
                    }
                }
            }
        }

        /// <summary>
        /// update one GridLayout column
        /// </summary>
        /// <param name="cellValues"></param>
        private void SetGridColumn(short column, bool[] newCellValues)
        {
            for (int y = 0; y < height; y++)
            {
                //get cell
                ImageView cell = (ImageView)gl_Grid.GetChildAt((y * width) + column);

                //if cell is off
                if (cell.GetTag(Resource.String.cellOn) == null)
                {
                    //and cell value is to become on
                    if (newCellValues[y])
                    {
                        //activate the cell
                        ActivateCell(cell);
                    }
                }
                //if cell value should be off
                else if (!newCellValues[y])
                {
                    //make sure it is off
                    ///OPTIMIZATION: do not update if already off
                    DeactivateCell(cell);
                }
            }
        }

        /// <summary>
        /// play the current grid
        /// </summary>
        private void Play()
        {
            //play grid
            for (short xCell = 0; xCell < width; xCell++)
            {
                for (short yCell = 0; yCell < height; yCell++)
                {
                    if (cellValues[xCell, yCell])
                    {
                        float frequency = Frequency.CalculateFrequency(yCell);

                        synthesizer.Play(yCell);
                    }
                }

                new System.Threading.ManualResetEvent(false).WaitOne(1000);
                //System.Threading.Thread.Sleep(1000);//where x is the time in seconds for which you want app to wait
                synthesizer.Stop();
            }
        }

        /// <summary>
        /// play the current grid
        /// </summary>
        private void Play(bool[,] iteratedGrid)
        {
            //play grid
            for (short xCell = 0; xCell < width; xCell++)
            {
                for (short yCell = 0; yCell < height; yCell++)
                {
                    //get cell
                    ImageView cell = (ImageView)gl_Grid.GetChildAt((yCell * width) + xCell);

                    if (cellValues[xCell, yCell])
                    {
                        float frequency = Frequency.CalculateFrequency(yCell);

                        synthesizer.Play(yCell);

                        ActivateCell(cell);
                    }
                    else
                    {
                        DeactivateCell(cell);
                    }
                }

                new System.Threading.ManualResetEvent(false).WaitOne(1000);
                //System.Threading.Thread.Sleep(1000);//where x is the time in seconds for which you want app to wait
                synthesizer.Stop();
            }
        }

        /// <summary>
        /// play one column of the grid
        /// </summary>
        private void PlayColumn(short xCell)
        {
            //play column
            for (short yCell = 0; yCell < height; yCell++)
            {
                if (cellValues[xCell, yCell])
                {
                    synthesizer.Play(yCell);
                }
            }
        }

        /// <summary>
        /// iterate cell values and update grid
        /// </summary>
        private void IterateGrid()
        {
            //iterate cells
            cellValues = CellularAutomata.Iterate(cellValues);

            //update grid
            SetGrid(cellValues);
        }

        /// <summary>
        /// explicitly iterate cell values and update grid
        /// </summary>
        private void IterateGrid(bool[,] cellValues)
        {
            //update grid
            SetGrid(cellValues);
        }

        #region async
        public void Tick(object sender, ElapsedEventArgs e)
        {
            synthesizer.Stop();

            PlayColumn(currentColumn);
            
            currentColumn++;

            if (currentColumn == width)
            {
                currentColumn = 0;
                cellValues = CellularAutomata.Iterate(cellValues);
                SetGrid(cellValues);
            }
        }

        public async Task PlayGrid()
        {
            //play grid
            for (short xCell = 0; xCell < width; xCell++)
            {
                for (short yCell = 0; yCell < height; yCell++)
                {
                    if (cellValues[xCell, yCell])
                    {
                        float frequency = Frequency.CalculateFrequency(yCell);

                        synthesizer.Play(yCell);
                    }
                }

                new System.Threading.ManualResetEvent(false).WaitOne(1000);
                //System.Threading.Thread.Sleep(1000);//where x is the time in seconds for which you want app to wait
                synthesizer.Stop();
            }
        }
        #endregion
    }
}

