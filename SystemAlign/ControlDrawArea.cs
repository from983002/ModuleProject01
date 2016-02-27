using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SystemAlign.Properties;

namespace SystemAlign
{
    using DrawList = List<DrawObject>;
    
    public class ControlDrawArea
    {
        #region DrawArea 제어 : 멤버

        public event RecipeEvent3 SeclectingRect;

        public DrawArea _drawArea1;
        readonly List<string> _fnlist = new List<string>();
        private DrawList _rectArray;
        private readonly System.Drawing.Drawing2D.AdjustableArrowCap _cusCap;
        readonly Pen _myArrowPen = new Pen(Color.LawnGreen, 1);
        private GraphicsList _nowGraphicsList;
        private Rectangle _nowRectangle;
        private Rectangle _cutRectangle;
        private Image _cutImage;
        private Graphics _cutImageG;
        private float _multiplyX = 0;
        private float _multiplyY = 0;
        private float _tempx1 = 0f, _tempx2 = 0f, _tempy1 = 0f, _tempy2 = 0f;
        readonly PictureBox[] _areapPictureBoxs = new PictureBox[100];
        readonly Rectangle[] _realRectangles = new Rectangle[100];
        PositionConvert _posConverter = PositionConvert.InstanceConvert;
        const string RegistryPath = "Software\\Shinjinmtec\\VisionInspection";
        public bool ModeCheckFlag = false;
        //Infragistics.Win.UltraWinGrid.ColumnStyle _listImage = new Infragistics.Win.UltraWinGrid.ColumnStyle();
        #endregion

        #region DrawArea 제어 : 프로퍼티

        public DrawList GetSetSelectZone
        {
            get{return _nowGraphicsList.DrawedList;}
            set { _nowGraphicsList.DrawedList = value; }
        }

        public int GetSetGraphicListCount
        {
            get { return _nowGraphicsList.Count; }
        }
        #endregion

        #region DrawArea 제어 : 생성자, 초기화
        public ControlDrawArea(DrawArea drawArea, FormDlgMain formDlg)
        {
            _drawArea1 = drawArea;
            FormDlgMain formDlgMain = formDlg;

            _drawArea1.DrawingEnd += new MyEvent1(DisplayArea);
            _drawArea1.RectSeclecting += new RecipeEvent2(Rect_Selected);

            _cusCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 5, false);

            _drawArea1.Owner = formDlgMain;
            _nowGraphicsList = new GraphicsList();

            //_drawArea1.Initialize(formDlgMain);
            _drawArea1.Initialize(formDlgMain, ref _nowGraphicsList);

            _rectArray = new DrawList();
        }


        public void Initialize_Control_Uper()
        {
            _tempx1 = (float)_drawArea1.pictureBox1.Image.Width;
            _tempx2 = (float)_drawArea1.pictureBox1.Width;
            _multiplyX = _tempx1 / _tempx2;

            _tempy1 = (float)_drawArea1.pictureBox1.Image.Height;
            _tempy2 = (float)_drawArea1.pictureBox1.Height;
            _multiplyY = _tempy1 / _tempy2;


            _myArrowPen.StartCap = System.Drawing.Drawing2D.LineCap.Custom;
            _myArrowPen.EndCap = System.Drawing.Drawing2D.LineCap.Custom;
            _myArrowPen.CustomStartCap = _cusCap;

            //Test_Image_Load("path");
        }

        public void Initialize_Control_Down()
        {
            _tempx1 = (float)_drawArea1.pictureBox1.Image.Width;
            //_tempx1 = (float)_drawArea1.pictureBox1.ImageIpl.Width;
            _tempx2 = (float)_drawArea1.pictureBox1.Width;
            _multiplyX = _tempx1 / _tempx2;

            _tempy1 = (float)_drawArea1.pictureBox1.Image.Height;
            //_tempy1 = (float)_drawArea1.pictureBox1.ImageIpl.Height;
            _tempy2 = (float)_drawArea1.pictureBox1.Height;
            _multiplyY = _tempy1 / _tempy2;


            _myArrowPen.StartCap = System.Drawing.Drawing2D.LineCap.Custom;
            _myArrowPen.EndCap = System.Drawing.Drawing2D.LineCap.Custom;
            _myArrowPen.CustomStartCap = _cusCap;

            //Test_Image_Load("path");
        }

        #endregion

        #region DrawArea 제어 : 이벤트 핸들러
        [DllImport("user32.dll")]
        static extern IntPtr LoadCursorFromFile(string lpFileName);
        #endregion

        #region DrawArea 제어 : 기타 메소드

        private void Test_Image_Load(string imagePath)
        {

            var di = new DirectoryInfo(@"D:\AlignImage");
            foreach (System.IO.FileInfo f in di.GetFiles())
            {
                _fnlist.Add(f.FullName);
            }
        }


        public void DisplayArea(int objectCount)
        {
            var pBoxNo = objectCount - 1;
            for (var i = 0; i < objectCount; i++)
            {
                var tempImage = new PictureBox {Image = DisplayArray(i)};
                _areapPictureBoxs[pBoxNo] = tempImage;
                pBoxNo--;
            }
        }

        public void Rect_Selected(int selectRectNo)
        {
            SeclectingRect(selectRectNo);
        }

        public Image DisplayArray(int objectNo)
        {
            //아래의 라인을 막으면 메인에서 NowRectangle함수가 0으로 나타난다.
            //Recipe_Config_Viewer_To_List_Inspect 메인의 함수에서 NowRectangle 이를 사용할수 없다.
            _nowGraphicsList = _drawArea1.GetSetGraphicsList;
            _nowRectangle = _nowGraphicsList[objectNo].NowRectangle();

            var cg = _drawArea1.pictureBox1.CreateGraphics();

            var pointx = Convert.ToInt32(_nowRectangle.X * _multiplyX);
            var pointy = Convert.ToInt32(_nowRectangle.Y * _multiplyY);
            var lengthx = Convert.ToInt32(_nowRectangle.Width * _multiplyX);
            var lengthy = Convert.ToInt32(_nowRectangle.Height * _multiplyY);

            _realRectangles[objectNo].X = pointx;
            _realRectangles[objectNo].Y = pointy;
            _realRectangles[objectNo].Width = lengthx;
            _realRectangles[objectNo].Height = lengthy;

            _cutImage = new Bitmap(lengthx, lengthy);
            _cutImageG = Graphics.FromImage(_cutImage);

            _cutRectangle = new Rectangle(0, 0, lengthx, lengthy);

            _cutImageG.DrawImage(_drawArea1.pictureBox1.Image, _cutRectangle, pointx, pointy, lengthx, lengthy, GraphicsUnit.Pixel);
            //textBox1.Text += "Jpg : " + pointx.ToString("0000")+ "    " + pointy.ToString("0000")+ "    " + lengthx.ToString("0000") + "    " + lengthy.ToString("0000");
            return _cutImage;
        }

        public void ModeChecking()
        {
            if (ModeCheckFlag == true)
            {
                _drawArea1.MakeMode = true;
                var colorCursorHandle = LoadCursorFromFile("Rectangle.cur");
                var myCursor = new Cursor(colorCursorHandle);
                _drawArea1.GetSetCursor = myCursor;
                _drawArea1.pictureBox1.Cursor = myCursor;
            }
            else
            {
                _drawArea1.MakeMode = false;
                _drawArea1.GetSetCursor = Cursors.Default;
                _drawArea1.pictureBox1.Cursor = Cursors.Default;
            }
        }

        public void AddListObject(Rectangle drawObject)
        {
            _drawArea1.AddNewObject(new DrawRectangle(drawObject.X, drawObject.Y, drawObject.Width, drawObject.Height));
        }

        public void ClearListObject()
        {
            _drawArea1.GetSetGraphicsList.Clear();
        }
        #endregion
    }
}
