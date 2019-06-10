using Android.App;
using Android.Widget;
using Android.OS;
using static Android.Views.View;
using System;
using Android.Views;
using Android.Graphics;
using static Android.Views.ViewGroup;
using MuzikaClasses;
using System.Timers;
using MuzikaClasses.Rules;
using System.Collections.Generic;
using Math = System.Math;
using Exception = System.Exception;

namespace Muzika
{
    [Activity(Label = "Muzika", MainLauncher = true)]
    public class MainActivity : Activity
    {
        #region Variables
        Bitmap InactiveCellBitmap, ActiveCellBitmap, PlayingCellBitmap
            , PauseBitmap, PlayBitmap
            , ParallelBitmap, NumericBitmap
            , ProgressiveBitmap, UnprogressiveBitmap;
        bool Playing = false, Numeric = false, GridWiderThanScreen, Kick = true, progressive = true;
        CellularAutomata<Rule30> chordProgressionGenerator;
        ImageButton B_Play, B_Synth, B_Add, B_Remove, B_Progressive;
        delegate void ToggleImageBitmap(Bitmap bitmap, bool value);
        int DrawingCellMagnitude, CellMagnitude, GridPixelWidth, GridPixelHeight;
        int[] ChordProgression;
        float StartX, StartY, GridX, GridY;
        GridLayout GL_GridDrawer;
        List<bool[,]> CellValues;
        List<CellularAutomata<LangtonsAnt>> _CellularAutomata;
        List<GridLayout> GridLayouts;
        Random _Random = new Random();
        Point Resolution = new Point();
        short Width, Height, CurrentColumn, NumberOfGrids = 0;
        Synthesizer Synthesizer;
        TextView TV_DrawAGrid, TV_X, TV_Y, TV_StartX, TV_StartY, TV_EndX, TV_EndY;
        Timer Metronome;
        View V_Overlay;
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            GL_GridDrawer = FindViewById<GridLayout>(Resource.Id.gl_GridDrawer);

            CheckResolution();

            //define grid cell magnitude
            //as the smallest resolution attribute / 13
            if (Resolution.X > Resolution.Y)
                DrawingCellMagnitude = (int)Math.Floor((decimal)Resolution.Y / 13);
            else
                DrawingCellMagnitude = (int)Math.Floor((decimal)Resolution.X / 13);

            //initialize variables
            V_Overlay = FindViewById<View>(Resource.Id.v_Overlay);
            TV_DrawAGrid = FindViewById<TextView>(Resource.Id.tv_DrawAGrid);
            TV_X = FindViewById<TextView>(Resource.Id.tv_x);
            TV_Y = FindViewById<TextView>(Resource.Id.tv_y);
            TV_StartX = FindViewById<TextView>(Resource.Id.tv_startX);
            TV_StartY = FindViewById<TextView>(Resource.Id.tv_startY);
            TV_EndX = FindViewById<TextView>(Resource.Id.tv_endX);
            TV_EndY = FindViewById<TextView>(Resource.Id.tv_endY);
            B_Play = FindViewById<ImageButton>(Resource.Id.b_Play);
            B_Synth = FindViewById<ImageButton>(Resource.Id.b_Synth);
            B_Add = FindViewById<ImageButton>(Resource.Id.b_Add);
            B_Remove = FindViewById<ImageButton>(Resource.Id.b_Remove);
            B_Progressive = FindViewById<ImageButton>(Resource.Id.b_Progressive);

            GridLayouts = new List<GridLayout>
            {
                FindViewById<GridLayout>(Resource.Id.gl_Grid0),
                FindViewById<GridLayout>(Resource.Id.gl_Grid1)
            };

            ActiveCellBitmap = BitmapFactory.DecodeStream(Assets.Open("cell-active.png"));
            InactiveCellBitmap = BitmapFactory.DecodeStream(Assets.Open("cell.png"));
            PlayingCellBitmap = BitmapFactory.DecodeStream(Assets.Open("cell-playing.png"));
            PauseBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.pause);
            PlayBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.play);
            ParallelBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.parallel);
            NumericBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.numeric);
            ProgressiveBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.progressive);
            UnprogressiveBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.unprogressive);

            B_Play.Touch += new EventHandler<TouchEventArgs>(ButtonPlayTouch);
            B_Synth.Touch += new EventHandler<TouchEventArgs>(ButtonSynthTouch);
            B_Add.Touch += new EventHandler<TouchEventArgs>(ButtonAddTouch);
            B_Remove.Touch += new EventHandler<TouchEventArgs>(ButtonRemoveTouch);
            B_Progressive.Touch += new EventHandler<TouchEventArgs>(ButtonProgressiveTouch);

            //bind overlay touch event
            V_Overlay.Touch += new EventHandler<TouchEventArgs>(OverlayTouch);
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
                //record x and y
                float x = e.Event.GetX();
                float y = e.Event.GetY();

                TV_X.Text = "x: " + x;
                TV_Y.Text = "y: " + y;

                //calculate width
                int width = Convert.ToInt32(Math.Floor(x - StartX));
                int height = Convert.ToInt32(Math.Floor(y - StartY));

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
                if (width > DrawingCellMagnitude * 12)
                {
                    width = DrawingCellMagnitude * 12;
                }
                if (height > DrawingCellMagnitude * 12)
                {
                    height = DrawingCellMagnitude * 12;
                }

                //calculate number of cells
                short gridWidth = (short)Math.Floor((decimal)width / DrawingCellMagnitude);
                short gridHeight = (short)Math.Floor((decimal)height / DrawingCellMagnitude);

                //calculate grid size
                int pixelWidth = gridWidth * DrawingCellMagnitude;
                int pixelHeight = gridHeight * DrawingCellMagnitude;

                //dynamic starting point for negative co-ordinates
                if (negativeWidth)
                {
                    GL_GridDrawer.SetX(StartX - pixelWidth);
                }
                if (negativeHeight)
                {
                    GL_GridDrawer.SetY(StartY - pixelHeight);
                }

                if (e.Event.Action == MotionEventActions.Down)
                //touch
                {
                    GL_GridDrawer.SetX(x);
                    GL_GridDrawer.SetY(y);
                    GL_GridDrawer.Visibility = ViewStates.Visible;

                    TV_StartX.Text = "start x: " + x;
                    TV_StartY.Text = "start y: " + y;

                    StartX = x;
                    StartY = y;
                }
                else if (e.Event.Action == MotionEventActions.Up)
                //release
                {
                    GL_GridDrawer.Visibility = ViewStates.Invisible;

                    TV_EndX.Text = "end x: " + x;
                    TV_EndY.Text = "end y: " + y;

                    if (gridWidth > 1 && gridHeight > 2)
                    //if grid was drawn, prepare it
                    //minimum size 3x2
                    {
                        NumberOfGrids++;

                        CheckResolution();

                        //check logistics
                        float screenAspect = (float)Resolution.X / Resolution.Y;
                        float gridAspect = (float)gridWidth / gridHeight;

                        GridPixelWidth = (int)Math.Ceiling((decimal)Resolution.Y / gridHeight) * gridWidth;
                        GridPixelHeight = (int)Math.Ceiling((decimal)Resolution.X / gridWidth) * gridHeight;

                        if (gridAspect > screenAspect)
                        //grid is wider than screen
                        {
                            GridWiderThanScreen = true;

                            GridX = 0;
                            GridY = (Resolution.Y - GridPixelHeight) / 2;

                            GridLayouts[0].SetX(GridX);
                            GridLayouts[0].SetY(GridY);
                            LayoutParams layoutParams = GridLayouts[0].LayoutParameters;
                            layoutParams.Width = -1;
                            layoutParams.Height = GridPixelHeight;
                            GridLayouts[0].LayoutParameters = layoutParams;

                            CellMagnitude = (int)Math.Ceiling((decimal)Resolution.X / gridWidth);
                        }
                        else
                        //grid is thinner than screen
                        {
                            GridWiderThanScreen = false;

                            GridX = (Resolution.X - GridPixelWidth) / 2;
                            GridY = 0;

                            GridLayouts[0].SetX(GridX);
                            GridLayouts[0].SetY(GridY);
                            LayoutParams layoutParams = GridLayouts[0].LayoutParameters;
                            layoutParams.Width = GridPixelWidth;
                            layoutParams.Height = -1;
                            GridLayouts[0].LayoutParameters = layoutParams;

                            CellMagnitude = (int)Math.Ceiling((decimal)Resolution.Y / gridHeight);
                        }

                        GridLayouts[0].RemoveAllViews();

                        LayoutParams gridCellLayoutParams = new LayoutParams(CellMagnitude, CellMagnitude);

                        CellValues = new List<bool[,]>
                        {
                            new bool[gridWidth, gridHeight]
                        };
                        //draw grid
                        for (int yCell = 0; yCell < gridHeight; yCell++)
                        {
                            for (int xCell = 0; xCell < gridWidth; xCell++)
                            {
                                CellValues[0][xCell, yCell] = false;
                                //define cell
                                ImageView cell = new ImageView(this);
                                cell.SetTag(Resource.String.cellOn, null);
                                cell.SetTag(Resource.String.cellX, xCell);
                                cell.SetTag(Resource.String.cellY, yCell);
                                cell.Touch += new EventHandler<TouchEventArgs>(ToggleCell);
                                cell.SetImageBitmap(InactiveCellBitmap);
                                cell.LayoutParameters = gridCellLayoutParams;
                                GridLayouts[0].AddView(cell);
                            }
                        }

                        //define column count
                        GridLayouts[0].ColumnCount = gridWidth;

                        //instantiate synthesizer
                        Synthesizer = new Synthesizer(Assets, gridHeight);

                        Width = gridWidth;
                        Height = gridHeight;

                        GridLayouts[0].Visibility = ViewStates.Visible;
                        B_Play.Visibility = ViewStates.Visible;
                        B_Synth.Visibility = ViewStates.Visible;
                        B_Add.Visibility = ViewStates.Visible;
                        B_Remove.Visibility = ViewStates.Visible;
                        //B_Progressive.Visibility = ViewStates.Visible;
                        V_Overlay.Visibility = ViewStates.Gone;
                        TV_DrawAGrid.Visibility = ViewStates.Gone;
                    }

                    return;
                }

                //set grid drawer size
                LayoutParams gridDrawerLayoutParams = GL_GridDrawer.LayoutParameters;

                gridDrawerLayoutParams.Width = pixelWidth;
                gridDrawerLayoutParams.Height = pixelHeight;

                GL_GridDrawer.LayoutParameters = gridDrawerLayoutParams;
                GL_GridDrawer.RemoveAllViews();

                LayoutParams cellLayoutParams = new LayoutParams(DrawingCellMagnitude, DrawingCellMagnitude);
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
                        cell.SetTag(Resource.String.cellZ, 0);
                        GL_GridDrawer.AddView(cell);
                    }
                }

                //define column count
                GL_GridDrawer.ColumnCount = gridWidth;
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
                int x = (int)senderImage.GetTag(Resource.String.cellX);
                int y = (int)senderImage.GetTag(Resource.String.cellY);
                int z = (int)senderImage.GetTag(Resource.String.cellZ);

                if (_CellularAutomata == null)
                    InitiateCellularAutomata();

                if (senderImage.GetTag(Resource.String.cellOn) == null)
                //activate cell
                {
                    ActivateCell(senderImage);
                    CellValues[z][x, y] = true;
                    _CellularAutomata[z].SetCell(x, y, true);
                }
                else
                //deactivate cell
                {
                    DeactivateCell(senderImage);
                    CellValues[z][x, y] = false;
                    _CellularAutomata[z].SetCell(x, y, false);
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
            SetImage(senderImage, ActiveCellBitmap);
            //Console.WriteLine("cell ({0}, {1}) is now on", Convert.ToInt32(senderImage.GetTag(Resource.String.cellX)), Convert.ToInt32(senderImage.GetTag(Resource.String.cellY)));
        }

        /// <summary>
        ///  graphically deactivate grid cell
        /// </summary>
        /// <param name="senderImage"></param>
        private void DeactivateCell(ImageView senderImage)
        {
            senderImage.SetTag(Resource.String.cellOn, null);
            SetImage(senderImage, InactiveCellBitmap);
            //Console.WriteLine("cell ({0}, {1}) is now off", Convert.ToInt32(senderImage.GetTag(Resource.String.cellX)), Convert.ToInt32(senderImage.GetTag(Resource.String.cellY)));
        }

        /// <summary>
        /// safely set the image of a cell
        /// </summary>
        /// <param name="senderImage">ImageView to update</param>
        /// <param name="value">new image</param>
        private void SetImage(ImageView senderImage, Bitmap value)
        {
            void action()
            {
                senderImage.SetImageBitmap(value);
            }
            senderImage.Post(action);
        }

        /// <summary>
        /// capture play button touch event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPlayTouch(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            //only consider press
            {
                //b_Play.Visibility = ViewStates.Invisible;
                if (_CellularAutomata == null)
                    InitiateCellularAutomata();

                //Thread t = new Thread(CalculateChordProgression);
                //t.Start();
                CalculateChordProgression();

                if (Metronome == null)
                {
                    Metronome = new Timer
                    {
                        AutoReset = true,
                        Interval = 500
                    };

                    Metronome.Elapsed += Tick;
                }

                if (Playing)
                {
                    Playing = false;
                    Metronome.Stop();
                    B_Play.SetImageBitmap(PlayBitmap);
                }
                else
                {
                    Playing = true;
                    Metronome.Start();
                    B_Play.SetImageBitmap(PauseBitmap);
                }

            }
        }

        /// <summary>
        /// initialize the Cellular Automata
        /// </summary>
        private void InitiateCellularAutomata()
        {
            _CellularAutomata = new List<CellularAutomata<LangtonsAnt>>();

            for (short z = 0; z < NumberOfGrids; z++)
            {
                CellularAutomata<LangtonsAnt> _CA
                    = new CellularAutomata<LangtonsAnt>(new CircularArray2D<bool>(CellValues[z]));
                _CellularAutomata.Add(_CA);
            }

            CurrentColumn = 0;
        }

        /// <summary>
        /// capture synth button touch event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSynthTouch(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            //only consider press
            {
                if (Numeric)
                {
                    Numeric = false;
                    B_Synth.SetImageBitmap(ParallelBitmap);
                }
                else
                {
                    Numeric = true;
                    B_Synth.SetImageBitmap(NumericBitmap);
                }
            }
        }

        /// <summary>
        /// capture synth button touch event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRemoveTouch(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            //only consider press
            {
                RemoveGrid();
            }
        }

        /// <summary>
        /// capture synth button touch event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddTouch(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            //only consider press
            {
                AddGrid();
            }
        }

        /// <summary>
        /// capture progressive button touch event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonProgressiveTouch(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            //only consider press
            {
                progressive = !progressive;

                if (progressive)
                    B_Progressive.SetImageBitmap(UnprogressiveBitmap);
                else
                    B_Progressive.SetImageBitmap(ProgressiveBitmap);
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

            Resolution = new Point(Application.Resources.DisplayMetrics.WidthPixels, Application.Resources.DisplayMetrics.HeightPixels - statusBarHeight);
        }

        /// <summary>
        /// update GridLayout
        /// </summary>
        /// <param name="cellValues"></param>
        private void SetGrids(List<bool[,]> newCellValues)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int z = 0; z < NumberOfGrids; z++)
                    {
                        //get cell
                        ImageView cell = (ImageView)GridLayouts[z].GetChildAt((y * Width) + x);

                        //if cell is off
                        if (cell.GetTag(Resource.String.cellOn) == null)
                        {
                            //and cell value is to become on
                            if (newCellValues[z][x, y])
                            {
                                //activate the cell
                                ActivateCell(cell);
                            }
                        }
                        //if cell value should be off
                        else if (!newCellValues[z][x, y])
                        {
                            //make sure it is off
                            ///OPTIMIZATION: do not update if already off
                            DeactivateCell(cell);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// play one column of the grids
        /// </summary>
        private void PlayColumn(short xCell)
        {
            if ((Width * NumberOfGrids) % 2 == 0)
            {
                //play kick on root note
                if (xCell == 0)
                    Synthesizer.Kick();

                if (xCell == (float)(Width * NumberOfGrids) / 2)
                    Synthesizer.Snare();
                else
                    Synthesizer.Hat();
            }
            else
            {
                //play kick on root note
                if (xCell == 0 && Kick)
                {
                    Synthesizer.Kick();
                    Kick = !Kick;
                    Synthesizer.Hat();
                }
                else if (xCell == 0)
                {
                    Synthesizer.Snare();
                    Kick = !Kick;
                }
                else
                    Synthesizer.Hat();

            }

            //divide grids
            int z = 0;

            //determine grid
            while (xCell >= Width)
            {
                z++;
                xCell -= Width;
            }

            //play column
            if (Numeric)
            {
                short key = Numbers.Zero;

                for (short yCell = 0; yCell < Height; yCell++)
                    if (CellValues[z][xCell, yCell])
                        key++;

                if (progressive)
                    Synthesizer.Play(key, 1, (short)ChordProgression[_CellularAutomata[0].Iterations % _CellularAutomata[0].Repeat]);
                else
                    Synthesizer.Play(key, 1, 0);
            }
            else
            {
                int i = Numbers.Zero;

                for (short yCell = 0; yCell < Height; yCell++)
                    if (CellValues[z][xCell, yCell])
                        i++;

                for (short yCell = 0; yCell < Height; yCell++)
                    if (CellValues[z][xCell, yCell])
                        if (progressive)
                            Synthesizer.Play((short)(Height - 1 - yCell), i,
                            (short)ChordProgression[_CellularAutomata[0].Iterations % _CellularAutomata[0].Repeat]);
                        else
                            Synthesizer.Play((short)(Height - 1 - yCell), i, 0);
            }

            //if ((xCell % Width == 0) || (xCell % ((double)Width / 2) == 0))
            //s += xCell + ": Kick";

            //Console.WriteLine(s);
        }

        /// <summary>
        /// remove a grid
        /// </summary>
        private void RemoveGrid()
        {
            NumberOfGrids--;
            if (NumberOfGrids == 0)
            {
                if (Playing)
                {
                    Metronome.Stop();
                    Playing = false;
                    B_Play.SetImageBitmap(PlayBitmap);
                }

                CurrentColumn = 0;

                B_Play.Visibility = ViewStates.Gone;
                B_Synth.Visibility = ViewStates.Gone;
                B_Add.Visibility = ViewStates.Gone;
                B_Remove.Visibility = ViewStates.Gone;
                B_Progressive.Visibility = ViewStates.Gone;
                GridLayouts[0].Visibility = ViewStates.Gone;

                TV_DrawAGrid.Visibility = ViewStates.Visible;
                V_Overlay.Visibility = ViewStates.Visible;

                _CellularAutomata = null;
                CellValues = null;
            }
            else
            {
                CurrentColumn %= Width;

                _CellularAutomata.RemoveAt(NumberOfGrids);
                CellValues.RemoveAt(NumberOfGrids);
                
                if (GridWiderThanScreen)
                //grid is wider than screen
                {
                    GridPixelHeight *= 2;

                    GridX = 0;
                    GridY = (Resolution.Y - GridPixelHeight) / 2;

                    GridLayouts[0].SetX(GridX);
                    GridLayouts[0].SetY(GridY);
                    LayoutParams layoutParams = GridLayouts[0].LayoutParameters;
                    layoutParams.Width = -1;
                    layoutParams.Height = GridPixelHeight;
                    GridLayouts[0].LayoutParameters = layoutParams;

                    CellMagnitude = (int)Math.Ceiling((decimal)Resolution.X / Width);
                }
                else
                //grid is thinner than screen
                {
                    GridPixelWidth *= 2;

                    GridX = (Resolution.X - GridPixelWidth) / 2;
                    GridY = 0;

                    GridLayouts[0].SetX(GridX);
                    GridLayouts[0].SetY(GridY);
                    LayoutParams layoutParams = GridLayouts[0].LayoutParameters;
                    layoutParams.Width = GridPixelWidth;
                    layoutParams.Height = -1;
                    GridLayouts[0].LayoutParameters = layoutParams;

                    CellMagnitude = (int)Math.Ceiling((decimal)Resolution.Y / Height);
                }

                GridLayouts[0].RemoveAllViews();

                LayoutParams gridCellLayoutParams = new LayoutParams(CellMagnitude, CellMagnitude);
                
                //draw grid
                for (int yCell = 0; yCell < Height; yCell++)
                {
                    for (int xCell = 0; xCell < Width; xCell++)
                    {
                        //define cell
                        ImageView cell = new ImageView(this);
                        cell.SetTag(Resource.String.cellOn, null);
                        cell.SetTag(Resource.String.cellX, xCell);
                        cell.SetTag(Resource.String.cellY, yCell);
                        cell.Touch += new EventHandler<TouchEventArgs>(ToggleCell);
                        if (CellValues[0][xCell, yCell])
                            cell.SetImageBitmap(ActiveCellBitmap);
                        else
                            cell.SetImageBitmap(InactiveCellBitmap);
                        cell.LayoutParameters = gridCellLayoutParams;
                        GridLayouts[0].AddView(cell);
                    }
                }

                //define column count
                GridLayouts[0].ColumnCount = Width;

                GridLayouts[1].Visibility = ViewStates.Gone;

                B_Add.Visibility = ViewStates.Visible;
            }
        }

        /// <summary>
        /// add a grid
        /// </summary>
        private void AddGrid()
        {
            if (NumberOfGrids == 1)
            {
                B_Add.Visibility = ViewStates.Gone;
            }

            //get first or second grid accordingly
            int index = (int)Math.Ceiling((decimal)NumberOfGrids / 2) - 1;

            if (_CellularAutomata == null)
                InitiateCellularAutomata();

            bool[,] newGrid = (bool[,])CellValues[0].Clone();
            CellValues.Add(newGrid);
            _CellularAutomata.Add(new CellularAutomata<LangtonsAnt>(
                new CircularArray2D<bool>((bool[,])newGrid.Clone()))
            {
                Iterations = _CellularAutomata[0].Iterations
            });

            LayoutParams layoutParams = GridLayouts[0].LayoutParameters;

            //shrink cells
            CellMagnitude /= 2;

            if (GridWiderThanScreen)
            //grid is wider than screen
            //stack grids atop eachother
            {
                GridPixelHeight /= 2;

                layoutParams.Width = Resolution.X / 2;
                layoutParams.Height = GridPixelHeight;

                GridY = (Resolution.Y - GridPixelHeight) / 2;

                GridLayouts[0].SetY(GridY);
                GridLayouts[0].LayoutParameters = layoutParams;

                layoutParams = GridLayouts[NumberOfGrids].LayoutParameters;
                GridLayouts[NumberOfGrids].SetX(Resolution.X / 2);
                GridLayouts[NumberOfGrids].SetY(GridY);
                layoutParams.Width = Resolution.X / 2;
                layoutParams.Height = GridPixelHeight;
                GridLayouts[NumberOfGrids].LayoutParameters = layoutParams;
            }
            else
            //grid is thinner than screen
            //stack grids beside eachother
            {
                GridPixelWidth /= 2;

                layoutParams.Width = GridPixelWidth;
                layoutParams.Height = Resolution.Y / 2;

                GridX = (Resolution.X - GridPixelWidth) / 2;

                GridLayouts[0].SetX(GridX * 2);
                GridLayouts[0].LayoutParameters = layoutParams;

                layoutParams = GridLayouts[NumberOfGrids].LayoutParameters;
                GridLayouts[NumberOfGrids].SetX(GridX);
                GridLayouts[NumberOfGrids].SetY(Resolution.Y / 2);
                layoutParams.Width = GridPixelWidth;
                layoutParams.Height = Resolution.Y / 2;
                GridLayouts[NumberOfGrids].LayoutParameters = layoutParams;
            }


            NumberOfGrids++;


            //draw grid
            for (int z = 0; z < NumberOfGrids; z++)
            {
                GridLayouts[z].RemoveAllViews();
                LayoutParams gridCellLayoutParams = new LayoutParams(CellMagnitude, CellMagnitude);
                Console.WriteLine("{0}: X: {1}, width: {2}. Y: {3}, height: {4}", z, GridLayouts[z].GetX(), layoutParams.Width, GridLayouts[z].GetY(), layoutParams.Height);
                for (int yCell = 0; yCell < Height; yCell++)
                {
                    for (int xCell = 0; xCell < Width; xCell++)
                    {
                        //define cell
                        ImageView cell = new ImageView(this);
                        if (CellValues[z][xCell, yCell])
                        {
                            cell.SetTag(Resource.String.cellOn, true);
                            cell.SetImageBitmap(ActiveCellBitmap);
                        }
                        else
                        {
                            cell.SetTag(Resource.String.cellOn, null);
                            cell.SetImageBitmap(InactiveCellBitmap);
                        }
                        cell.SetTag(Resource.String.cellX, xCell);
                        cell.SetTag(Resource.String.cellY, yCell);
                        cell.SetTag(Resource.String.cellZ, z);
                        cell.Touch += new EventHandler<TouchEventArgs>(ToggleCell);
                        cell.LayoutParameters = gridCellLayoutParams;
                        GridLayouts[z].AddView(cell);
                    }
                }

                GridLayouts[z].Visibility = ViewStates.Visible;

                //define column count
                GridLayouts[z].ColumnCount = Width;
            }
        }

        /// <summary>
        /// highlight the note being played
        /// </summary>
        /// <param name="currentColumn">the column to highlight</param>
        private void Highlight(short currentColumn)
        {
            for (int y = 0; y < Height; y++)
            {
                //un-highlight previous column

                int x = currentColumn - 1;
                if (x == -1)
                {
                    x = (Width * NumberOfGrids) - 1;
                }

                int z = 0;

                //determine grid
                while (x >= Width)
                {
                    z++;
                    x -= Width;
                }

                ImageView cell = (ImageView)GridLayouts[z].GetChildAt((y * Width) + x);

                if (CellValues[z][x, y])
                    SetImage(cell, ActiveCellBitmap);

                //highlight current column

                x = currentColumn;

                z = 0;

                //determine grid
                while (x >= Width)
                {
                    z++;
                    x -= Width;
                }

                cell = (ImageView)GridLayouts[z].GetChildAt((y * Width) + x);

                if (CellValues[z][x, y])
                    SetImage(cell, PlayingCellBitmap);
            }
        }

        #region async
        /// <summary>
        /// metronome tick event
        /// play a column of the grid
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event arguments</param>
        public void Tick(object sender, ElapsedEventArgs e)
        {
            //string s = "-- TICK REPORT --";
            //s += "\nCurrent Column: " + CurrentColumn;
            if (CurrentColumn == Width * NumberOfGrids)
            {
                //s += "\n\nITERATING\n\nBEFORE";
                //for (int z = 0; z < NumberOfGrids; z++)
                //{
                //    s += "\nGrid " + z;
                //    for (int y = 0; y<Height; y++)
                //    {
                //        s += "\n";
                //        for (int x = 0; x<Width; x++)
                //        {
                //            s += CellValues[z][x, y]?"1":"0";
                //        }
                //    }

                //    s += "CA Iterations:" + _CellularAutomata[z].Iterations;
                //}

                CurrentColumn = Numbers.Zero;
                for (int z = 0; z < NumberOfGrids; z++)
                    CellValues[z] = _CellularAutomata[z].Iterate();
                SetGrids(CellValues);

                //s += "\n\nAFTER";
                //for (int z = 0; z < NumberOfGrids; z++)
                //{
                //    s += "\nGrid " + z;
                //    for (int y = 0; y < Height; y++)
                //    {
                //        s += "\n";
                //        for (int x = 0; x < Width; x++)
                //        {
                //            s += CellValues[z][x, y] ? "1" : "0";
                //        }
                //    }
                //}
            }

            //Console.WriteLine(s);

            Synthesizer.Stop();

            Highlight(CurrentColumn);
            PlayColumn(CurrentColumn);

            CurrentColumn++;

            if (_CellularAutomata[Numbers.Zero].Iterations % Numbers.Fifteen == 0)
                CalculateChordProgression();
        }

        /// <summary>
        /// calculate chord progression
        /// </summary>
        public void CalculateChordProgression()
        {
            //prepare chord progression
            if (chordProgressionGenerator == null)
            {
                chordProgressionGenerator = new CellularAutomata<Rule30>(new CircularArray2D<bool>(CellValues[Numbers.Zero]));
                chordProgressionGenerator.Repeat = Numbers.One;
            }

            string s = "Chord Progression: ";

            ChordProgression = new int[_CellularAutomata[Numbers.Zero].Repeat];

            for (int z = 0; z < _CellularAutomata[Numbers.Zero].Repeat; z++)
            {
                for (int x = 0; x < _CellularAutomata[Numbers.Zero].Repeat; x++)
                    for (int y = 0; y < Numbers.Twelve; y++)
                        if (chordProgressionGenerator[x, y])
                            ChordProgression[z]= ChordProgression[z] + y + (x*10);
                chordProgressionGenerator.Iterate();

                ChordProgression[z] %= Numbers.TwentyFour;

                ChordProgression[z] -= Numbers.Twelve;

                s += ChordProgression[z] + " ";
            }

            Console.WriteLine(s);
        }
        #endregion
    }
}