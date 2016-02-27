using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
//using OpenCvSharp;
//using OpenCvSharp.UserInterface;

namespace SystemAlign
{
    public delegate void MyEvent1(int nowObjectCount);

    

    public partial class DrawArea : UserControl
    {
        public event MyEvent1 DrawingEnd;
        public event RecipeEvent2 RectSeclecting;

        private GraphicsList _graphicsList;

        private int _nowObjectCount = 0;

        private Point _lastPoint = new Point(0, 0);

        // Object which is currently resized:
        private DrawObject _resizedObject;
        private int _resizedObjectHandle;
        private SelectionMode _selectMode = SelectionMode.None;

        // Keep state about last and current point (used to move and resize objects)
        private Point _startPoint = new Point(0, 0);
        private Cursor _getSetCursor;

        #region Properties

        public bool MakeMode { get; set; }

        //public bool MakeMode
        //{
        //    get { return _makeMode; }
        //    set { _makeMode = value; }
        //}


        /// <summary>
        ///     Tool cursor.
        /// </summary>
        public Cursor GetSetCursor
        {
            get { return _getSetCursor; }
            set { _getSetCursor = value; }
        }

        /*
        public Cursor GetSetCursor
        {
            get { return _cursor; }
            set { _cursor = value; }
        }
        */

        /// <summary>
        ///     Reference to the owner form
        /// </summary>
        public Form Owner { get; set; }

        /// <summary>
        ///     List of graphics objects.
        /// </summary>
        public GraphicsList GetSetGraphicsList
        {
            get { return _graphicsList; }
            set { _graphicsList = value; }
        }

        #endregion

        public DrawArea()
        {
            MakeMode = false;
            GetSetCursor = Cursors.Default;
            InitializeComponent();
        }

        /// <summary>
        ///     Initialization
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="docManager"></param>
        public void Initialize(Form owner, ref GraphicsList graphicsList)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            // Keep reference to owner form
            Owner = owner;

            // create list of graphic objects
            //_graphicsList = new GraphicsList();
            _graphicsList = graphicsList;
        }
        

         public void AddNewObject(DrawObject o)
        {
            GetSetGraphicsList.UnselectAll();

            o.Selected = true;
            GetSetGraphicsList.Add(o);

            pictureBox1.Capture = true;
            pictureBox1.Refresh();
        }


        public void ResizeAreaCheck()
        {
            for (int i = 0; i < _graphicsList.Count; i++)
            {
                if (_graphicsList[i].Selected != true) continue;

                _graphicsList.Dump();

                _nowObjectCount = _graphicsList.Count;
                DrawingEnd(_nowObjectCount);
                return;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //Point_test();

            if (e.Button == MouseButtons.Left)
            {
                if (MakeMode)
                    Make_MouseDown(sender, e);
                else
                    Point_MouseDown(sender, e);
            }
        }

        private void Make_MouseDown(object sender, MouseEventArgs e)
        {
            AddNewObject(new DrawRectangle(e.X, e.Y, 1, 1));
        }


        public int Now_Select_Object_Is(Point ePoint)
        {
            if (_graphicsList.Count < 1) return -1;
            for (int i = 0; i < _graphicsList.Count; i++)
            {
                Rectangle tempRect = _graphicsList[i].GetSetRectangle();
                if (tempRect.Contains(ePoint) == true)
                {
                    _graphicsList.UnselectAll();
                    return i;
                }
            }
            return -1;
        }


        public void Now_Select_Grid_Row(int rectNo)
        {
            RectSeclecting(rectNo);
        }

        private int selectedListNo = -1;
        private void Point_MouseDown(object sender, MouseEventArgs e)
        {
            _selectMode = SelectionMode.None;
            var downPoint = new Point(e.X, e.Y);

            int selectObject = Now_Select_Object_Is(downPoint);
            if (selectObject > -1) Now_Select_Grid_Row(selectObject);

            // Test for resizing (only if control is selected, cursor is on the handle)
            foreach (DrawObject o in _graphicsList.Selection)
            {
                selectedListNo++;
                int handleNumber = o.HitTest(downPoint);
                Trace.WriteLine(handleNumber.ToString());
                if (handleNumber > 0)
                {
                    _selectMode = SelectionMode.Size;

                    // keep resized object in class member
                    _resizedObject = o;
                    _resizedObjectHandle = handleNumber;

                    // Since we want to resize only one object, unselect all other objects
                    _graphicsList.UnselectAll();
                    o.Selected = true;
                    
                    break;
                }
            }

            // Test for move (cursor is on the object)
            if (_selectMode == SelectionMode.None)
            {
                int n1 = _graphicsList.Count;
                DrawObject o = null;

                for (int i = 0; i < n1; i++)
                {
                    if (_graphicsList[i].HitTest(downPoint) == 0)
                    {
                        o = _graphicsList[i];
                        break;
                    }
                }

                if (o != null)
                {
                    _selectMode = SelectionMode.Move;

                    // Unselect all if Ctrl is not pressed and clicked object is not selected yet
                    if ((ModifierKeys & Keys.Control) == 0 && !o.Selected)
                        _graphicsList.UnselectAll();

                    // Select clicked object
                    o.Selected = true;

                    pictureBox1.Cursor = Cursors.SizeAll;
                }
            }

            // Net selection
            if (_selectMode == SelectionMode.None)
            {
                // click on background
                if ((ModifierKeys & Keys.Control) == 0)
                    _graphicsList.UnselectAll();

                _selectMode = SelectionMode.NetSelection;
            }

            _lastPoint.X = e.X;
            _lastPoint.Y = e.Y;
            _startPoint.X = e.X;
            _startPoint.Y = e.Y;

            pictureBox1.Capture = true;
            pictureBox1.Refresh();

            if (_selectMode == SelectionMode.NetSelection)
            {
                // Draw selection rectangle in initial position
                ControlPaint.DrawReversibleFrame(
                    RectangleToScreen(DrawRectangle.GetNormalizedRectangle(_startPoint, _lastPoint)), Color.Black,
                    FrameStyle.Dashed);
            }
        }

        
        public void Point_test()
        {
            var tempx1 = (float)pictureBox1.Image.Width;
            var tempx2 = (float)pictureBox1.Width;
            var multiplyX = tempx1 / tempx2;

            var tempy1 = (float)pictureBox1.Image.Height;
            var tempy2 = (float)pictureBox1.Height;
            var multiplyY = tempy1 / tempy2;

            var pointx = Convert.ToInt32(MousePosition.X * multiplyX);
            var pointy = Convert.ToInt32(MousePosition.Y * multiplyY);
        }

        public bool GetSet_ImageLoadPass { get; set; }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            if (GetSet_ImageLoadPass == true) return;
            
            if (MakeMode)
                Make_MouseMove(sender, e);
            else
                Point_MouseMove(sender, e);
        }

        private void Make_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.None)
            {
                pictureBox1.Cursor = GetSetCursor;
                if (e.Button == MouseButtons.Left)
                {
                    var point = new Point(e.X, e.Y);
                    _graphicsList[0].MoveHandleTo(point, 5);
                    pictureBox1.Refresh();
                }
            }
            else
                Cursor = Cursors.Default;
        }


        private void Point_MouseMove(object sender, MouseEventArgs e)
        {
            var point = new Point(e.X, e.Y);
            Point oldPoint = _lastPoint;
            Cursor cursor = null;
            // set cursor when mouse button is not pressed
            if (e.Button == MouseButtons.None)
            {
                //Cursor cursor = null;
                _selectMode = SelectionMode.Move;
                for (int i = 0; i < _graphicsList.Count; i++)
                {
                    int n = _graphicsList[i].HitTest(point);

                    if (n > 0)
                    {
                        cursor = _graphicsList[i].GetHandleCursor(n);
                        break;
                    }
                }

                if (cursor == null)
                    cursor = Cursors.Default;

                pictureBox1.Cursor = cursor;

                return;
            }

            if (e.Button != MouseButtons.Left)
                return;

            // Find difference between previous and current position
            int dx = e.X - _lastPoint.X;
            int dy = e.Y - _lastPoint.Y;

            _lastPoint.X = e.X;
            _lastPoint.Y = e.Y;

            // resize
            if (_selectMode == SelectionMode.Size)
            {
                if (_resizedObject != null)
                {
                    _resizedObject.MoveHandleTo(point, _resizedObjectHandle);
                    pictureBox1.Refresh();
                }
            }

            // move
            if (_selectMode == SelectionMode.Move)
            {
                
                foreach (DrawObject o in _graphicsList.Selection)
                {
                    o.Move(dx, dy);
                }

                pictureBox1.Cursor = Cursors.SizeAll;
                pictureBox1.Refresh();

            }

            if (_selectMode == SelectionMode.NetSelection)
            {
                // Remove old selection rectangle
                ControlPaint.DrawReversibleFrame(RectangleToScreen(DrawRectangle.GetNormalizedRectangle(_startPoint, oldPoint)),Color.Black,FrameStyle.Dashed);

                // Draw new selection rectangle
                ControlPaint.DrawReversibleFrame(RectangleToScreen(DrawRectangle.GetNormalizedRectangle(_startPoint, point)),Color.Black,FrameStyle.Dashed);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MakeMode)
                    Make_MouseUp(sender, e);
                else
                    Point_MouseUp(sender, e);

                pictureBox1.Cursor = Cursors.Default;
            }
        }

         private void Make_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _graphicsList.Count == 0) return;

            _graphicsList[0].Normalize();

            pictureBox1.Capture = false;
            pictureBox1.Refresh();
             
            if (_nowObjectCount != _graphicsList.Count)
            {
                _graphicsList.Dump();

                _nowObjectCount = _graphicsList.Count;
                DrawingEnd(_nowObjectCount);
            }
            else
            {
                if (_graphicsList.Count > 0)
                {
                    ResizeAreaCheck();
                }
            }
        }

        private void Point_MouseUp(object sender, MouseEventArgs e)
        {
            if (_selectMode == SelectionMode.NetSelection)
            {
                // Remove old selection rectangle
                ControlPaint.DrawReversibleFrame(
                    RectangleToScreen(DrawRectangle.GetNormalizedRectangle(_startPoint, _lastPoint)),
                    Color.Black,
                    FrameStyle.Dashed);

                // Make group selection
                _graphicsList.SelectInRectangle(DrawRectangle.GetNormalizedRectangle(_startPoint, _lastPoint));

                _selectMode = SelectionMode.None;
            }

            if (_resizedObject != null)
            {
                // after resizing
                _resizedObject.Normalize();
                _resizedObject = null;
            }

            if (_graphicsList.Count > 0)
            {
                ResizeAreaCheck();
            }

            
            pictureBox1.Capture = false;
            pictureBox1.Refresh();
        }

        private enum SelectionMode
        {
            None,
            NetSelection, // group selection is active
            Move, // object(s) are moves
            Size // object is resized
        }


        private void DrawArea_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            //const string imagePath = "D:\\align011.bmp";
            
            //Image firstImage = Image.FromFile(imagePath); 
            //Image firstImage = new Bitmap(SystemAlign.Properties.Resources.DefaultImg); 
            //pictureBox1.Image = firstImage;
            //pictureBox1.Refresh();
        }


        private void pictureBoxIpl1_Paint(object sender, PaintEventArgs e)
        {
            var brush = new SolidBrush(Color.FromArgb(255, 255, 255));

            Graphics gc = e.Graphics;

            if (_graphicsList != null)
            {
                _graphicsList.Draw(gc);
            }
            brush.Dispose();
        }

    
    }
}
