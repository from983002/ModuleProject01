using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Ink;
using OpenCvSharp;
using OpenCvSharp.UserInterface;

namespace SystemAlign
{
    public class PositionConvert
    {
        #region Members
        private float _boxVsImageX = 0f;
        private float _boxVsImageY = 0f;
        Rectangle _rectBoxToImage = new Rectangle(0,0,0,0);
        Rectangle _rectImageToBox = new Rectangle(0, 0, 0, 0);

        CvPoint[] _pointsBoxToImage = new CvPoint[2];
        
        //원본 이미지에서의 좌표값을 박스의 좌표값으로 변경한값
        //Image To Box Start Point
        CvPoint ITB_Sta_Point = new CvPoint(0,0);
        //Image To Box End Point
        CvPoint ITB_End_Point = new CvPoint(0,0);

        //ROI 이미지에서의 좌표값을 원본 이미지의 좌표값으로 변경한값
        //ROI To Orignal Image Start Point
        CvPoint RTO_Sta_Point = new CvPoint(0, 0);
        //ROI To Orignal Image End Point
        CvPoint RTO_End_Point = new CvPoint(0, 0);
        #endregion


        #region Properties
        public CvPoint ImageToBoxStaPoint
        {
            get { return ITB_Sta_Point; }
            private set { ITB_Sta_Point = value; }
        }

        public CvPoint ImageToBoxEndPoint
        {
            get { return ITB_End_Point; }
            private set { ITB_End_Point = value; }
        }

        public CvPoint RtoStaPoint
        {
            get { return RTO_Sta_Point; }
            private set { RTO_Sta_Point = value; }
        }

        public CvPoint RtoEndPoint
        {
            get { return RTO_End_Point; }
            private set { RTO_End_Point = value; }
        }

        public float BoxVsImageX
        {
            get { return _boxVsImageX; }
            private set { _boxVsImageX = value; }
        }

        public float BoxVsImageY
        {
            get { return _boxVsImageY; }
            private set { _boxVsImageY = value; }
        }
        #endregion


        #region Constructer
        private PositionConvert()
        {
        }
        #endregion

        #region SingleTone Design Pattern
        private static PositionConvert posinConverter;
        private static object syncRoot = new object();

        public static PositionConvert InstanceConvert
        {
            get
            {
                lock (syncRoot)
                {
                    if (posinConverter == null)
                    {
                        posinConverter = new PositionConvert();
                    }
                    return posinConverter;
                }
            }
        }
        #endregion

        public List<CvPoint>  ImageToBox(IplImage fromImage, PictureBoxIpl targetBox, List<CvPoint> fromPoint)
        {
            List<CvPoint> targetPoint = new List<CvPoint>();
            double imgToBox_X = (double) targetBox.Width/(double) fromImage.Width;
            double imgToBox_Y = (double) targetBox.Height/(double) fromImage.Height;

            for (int i = 0; i < fromPoint.Count(); i++)
            {
                double tmpX = (double) fromPoint[i].X*imgToBox_X;
                 double tmpY = (double) fromPoint[i].Y*imgToBox_Y;
                CvPoint tmPoint = new CvPoint((int)tmpX, (int)tmpY);
                targetPoint.Add(tmPoint);
            }
            return targetPoint;
        }

        public CvPoint ImageToBox(IplImage fromImage, PictureBoxIpl targetBox, CvPoint fromPoint)
        {
            double imgToBox_X = (double) targetBox.Width/(double) fromImage.Width;
            double imgToBox_Y = (double) targetBox.Height/(double) fromImage.Height;


            double tmpX = (double) fromPoint.X*imgToBox_X;
            double tmpY = (double) fromPoint.Y*imgToBox_Y;
            CvPoint targetPoint = new CvPoint((int) tmpX, (int) tmpY);
            return targetPoint;
        }

        public PointF ImageToBox(IplImage fromImage, PictureBoxIpl targetBox, PointF fromPoint)
        {
            float imgToBox_X = (float)(targetBox.Width / fromImage.Width);
            float imgToBox_Y = (float)(targetBox.Height / fromImage.Height);


            float tmpX = (float)(fromPoint.X * imgToBox_X);
            float tmpY = (float)(fromPoint.Y * imgToBox_Y);
            PointF targetPoint = new PointF((float)tmpX, (float)tmpY);
            return targetPoint;
        }
        #region Functions
        public void BoxVsImage(PictureBox sourceBox, Image sourceImage)
        {
            var tempx1 = (float)sourceImage.Width;
            var tempx2 = (float)sourceBox.Width;
            this.BoxVsImageX = tempx1 / tempx2;

            var tempy1 = (float)sourceImage.Height;
            var tempy2 = (float)sourceBox.Height;
            this.BoxVsImageY = tempy1 / tempy2;
        }

        public void BoxVsImage(PictureBoxIpl sourceBox, IplImage sourceImage)
        {
            var tempx1 = (float)sourceImage.Width;
            var tempx2 = (float)sourceBox.Width;
            this.BoxVsImageX = tempx1 / tempx2;

            var tempy1 = (float)sourceImage.Height;
            var tempy2 = (float)sourceBox.Height;
            this.BoxVsImageY = tempy1 / tempy2;
        }

        public void BoxVsImage(PictureBoxIpl sourceBox, Image sourceImage, ref float[] resultData)
        {
            var tempx1 = (float)sourceImage.Width;
            var tempx2 = (float)sourceBox.Width;
            this.BoxVsImageX = tempx1 / tempx2;
            resultData[0] = tempx1 / tempx2;

            var tempy1 = (float)sourceImage.Height;
            var tempy2 = (float)sourceBox.Height;
            this.BoxVsImageY = tempy1 / tempy2;
            resultData[1] = BoxVsImageY;
        }

        public void BoxVsImage(PictureBox sourceBox, Image sourceImage, ref float[] resultData)
        {
            var tempx1 = (float)sourceImage.Width;
            var tempx2 = (float)sourceBox.Width;
            this.BoxVsImageX = tempx1 / tempx2;
            resultData[0] = tempx1 / tempx2;

            var tempy1 = (float)sourceImage.Height;
            var tempy2 = (float)sourceBox.Height;
            this.BoxVsImageY = tempy1 / tempy2;
            resultData[1] = BoxVsImageY;
        }

        public void BoxVsImage(PictureBoxIpl sourceBox, IplImage sourceImage, ref float[] resultData)
        {
            var tempx1 = (float)sourceImage.Width;
            var tempx2 = (float)sourceBox.Width;
            this.BoxVsImageX = tempx1 / tempx2;
            resultData[0] = BoxVsImageX;

            var tempy1 = (float)sourceImage.Height;
            var tempy2 = (float)sourceBox.Height;
            this.BoxVsImageY = tempy1 / tempy2;
            resultData[1] = BoxVsImageY;
        }
        
        public void BoxToImage(Rectangle sourceRect)
        {
            this._rectBoxToImage.X = Convert.ToInt32(sourceRect.X * this.BoxVsImageX);
            this._rectBoxToImage.Y = Convert.ToInt32(sourceRect.Y * this.BoxVsImageY);
            this._rectBoxToImage.Width = Convert.ToInt32(sourceRect.Width * this.BoxVsImageX);
            this._rectBoxToImage.Height = Convert.ToInt32(sourceRect.Height * this.BoxVsImageY);
        }

        public void BoxToImage(Rectangle sourceRect, ref Rectangle resultRect, float pBoxVsImageX, float pBoxVsImageY)
        {
            this._rectBoxToImage.X = Convert.ToInt32(sourceRect.X * pBoxVsImageX);
            this._rectBoxToImage.Y = Convert.ToInt32(sourceRect.Y * pBoxVsImageY);
            this._rectBoxToImage.Width = Convert.ToInt32(sourceRect.Width * pBoxVsImageX);
            this._rectBoxToImage.Height = Convert.ToInt32(sourceRect.Height * pBoxVsImageY);

            resultRect.X = Convert.ToInt32(sourceRect.X * pBoxVsImageX);
            resultRect.Y = Convert.ToInt32(sourceRect.Y * pBoxVsImageY);
            resultRect.Width = Convert.ToInt32(sourceRect.Width * pBoxVsImageX);
            resultRect.Height = Convert.ToInt32(sourceRect.Height * pBoxVsImageY);
        }

        public void BoxToImage(Rectangle sourceRect, ref CvRect resultRect, float pBoxVsImageX, float pBoxVsImageY)
        {
            this._rectBoxToImage.X = Convert.ToInt32(sourceRect.X * pBoxVsImageX);
            this._rectBoxToImage.Y = Convert.ToInt32(sourceRect.Y * pBoxVsImageY);
            this._rectBoxToImage.Width = Convert.ToInt32(sourceRect.Width * pBoxVsImageX);
            this._rectBoxToImage.Height = Convert.ToInt32(sourceRect.Height * pBoxVsImageY);

            resultRect.X = Convert.ToInt32(sourceRect.X * pBoxVsImageX);
            resultRect.Y = Convert.ToInt32(sourceRect.Y * pBoxVsImageY);
            resultRect.Width = Convert.ToInt32(sourceRect.Width * pBoxVsImageX);
            resultRect.Height = Convert.ToInt32(sourceRect.Height * pBoxVsImageY);
        }

        public void ImageToBox(Rectangle sourceRect)
        {
            this._rectImageToBox.X = Convert.ToInt32(sourceRect.X / this.BoxVsImageX);
            this._rectImageToBox.Y = Convert.ToInt32(sourceRect.Y / this.BoxVsImageY);
            this._rectImageToBox.Width = Convert.ToInt32(sourceRect.Width / this.BoxVsImageX);
            this._rectImageToBox.Height = Convert.ToInt32(sourceRect.Height / this.BoxVsImageY);
        }

        public void ImageToBox(Rectangle sourceRect, ref Rectangle resultRect, float pImageVsBoxX, float pImageVsBoxY)
        {
            resultRect.X = Convert.ToInt32(sourceRect.X / pImageVsBoxX);
            resultRect.Y = Convert.ToInt32(sourceRect.Y / pImageVsBoxY);
            resultRect.Width = Convert.ToInt32(sourceRect.Width / pImageVsBoxX);
            resultRect.Height = Convert.ToInt32(sourceRect.Height / pImageVsBoxY);
        }

        public void ImageToBox(CvRect sourceRect, ref Rectangle resultRect, float pImageVsBoxX, float pImageVsBoxY)
        {
            resultRect.X = Convert.ToInt32(sourceRect.X / pImageVsBoxX);
            resultRect.Y = Convert.ToInt32(sourceRect.Y / pImageVsBoxY);
            resultRect.Width = Convert.ToInt32(sourceRect.Width / pImageVsBoxX);
            resultRect.Height = Convert.ToInt32(sourceRect.Height / pImageVsBoxY);
        }


        public void ImageToBox(CvPoint sourcePointSta, CvPoint sourcePointEnd)
        {
            this.ITB_Sta_Point.X = Convert.ToInt32(sourcePointSta.X / this.BoxVsImageX);
            this.ITB_Sta_Point.Y = Convert.ToInt32(sourcePointSta.Y / this.BoxVsImageY);

            this.ITB_End_Point.X = Convert.ToInt32(sourcePointEnd.X / this.BoxVsImageX);
            this.ITB_End_Point.Y = Convert.ToInt32(sourcePointEnd.Y / this.BoxVsImageY);
        }

        
        //허프변화 알고리즘으로 찾은 라인 포인트값은 ROI영역에서 찾은 값이다.
        //이 ROI좌표 값을 원래의 이미지의 포인트 값으로 변환한다.
        //1.CvPoint[] ROI 영역의 직선 시작과 끝 좌표 값을 가지고 있다
        //2.Rectangle ROI 영역의 정보를 가지고 있다.
        public void RoiToOriginal(CvPoint[] lineCvPoint, Rectangle sourceRect)
        {
            this.RTO_Sta_Point = lineCvPoint[0];

            if (this.RTO_Sta_Point.X < 0) this.RTO_Sta_Point.X = sourceRect.X;
            else this.RTO_Sta_Point.X = this.RTO_Sta_Point.X + sourceRect.X;

            if (this.RTO_Sta_Point.Y < 0) this.RTO_Sta_Point.Y = sourceRect.Y;
            else this.RTO_Sta_Point.Y = this.RTO_Sta_Point.Y + sourceRect.Y;


            this.RTO_End_Point = lineCvPoint[1];
            
            if (this.RTO_End_Point.X < 0) this.RTO_End_Point.X = sourceRect.X;
            else this.RTO_End_Point.X = RTO_End_Point.X + sourceRect.X;

            if (this.RTO_End_Point.Y < 0) this.RTO_End_Point.Y = sourceRect.Y;
            else this.RTO_End_Point.Y = RTO_End_Point.Y + sourceRect.Y;
        }

        public bool GetCrossPoint1(Point AP1, Point AP2, Point BP1, Point BP2, ref Point mPoint)
        {
            double t, s;
            double under = (BP2.Y - BP1.Y)*(AP2.X - AP1.X) -(BP2.X - BP1.X)*(AP2.Y - AP1.Y);
            if (under == 0) return false;

            double _t = (BP2.X - BP1.X) * (AP1.Y - BP1.Y) - (BP2.Y - BP1.Y) * (AP1.X - BP1.X);
            double _s = (AP2.X - AP1.X) * (AP1.Y - BP1.Y) - (AP2.Y - AP1.Y) * (AP1.X - BP1.X);

            t = _t/under;
            s = _s/under;

            //if (t < 0.0 || t > 1.0 || s < 0.0 || s > 1.0) return false;

            //if (_t == 0.0 && _s == 0.0) return false;

            double tempX = AP1.X + t*(double) (AP2.X - AP1.X);
            double tempY = AP1.Y + t * (double)(AP2.Y - AP1.Y);
            mPoint.X = (int)(AP1.X + t* (double) (AP2.X - AP1.X));
            mPoint.Y = (int)(AP1.Y + t * (double)(AP2.Y - AP1.Y));
            return true;
        }
        public bool GetCrossPoint(PointF AP1, PointF AP2, PointF BP1, PointF BP2, ref PointF mPoint)
        {
            float t, s;
            float under =  (float)((BP2.Y - BP1.Y) * (AP2.X - AP1.X) - (BP2.X - BP1.X) * (AP2.Y - AP1.Y));
            if (under == 0) return false;

            float _t = (float)( (BP2.X - BP1.X) * (AP1.Y - BP1.Y) - (BP2.Y - BP1.Y) * (AP1.X - BP1.X));
            float _s = (float)( (AP2.X - AP1.X) * (AP1.Y - BP1.Y) - (AP2.Y - AP1.Y) * (AP1.X - BP1.X));
            
            t = (float)(_t / under);
            s = (float)(_s / under);

            if (_t == 0f && _s == 0f) return false;

            mPoint.X = (float)(AP1.X + t * AP2.X - AP1.X);
            mPoint.Y = (float)(AP1.Y + t * AP2.Y - AP1.Y);

            return true;
        }
        public bool GetCrossPoint(CvPoint AP1, CvPoint AP2, CvPoint BP1, CvPoint BP2, ref CvPoint mPoint)
        {
            double t, s;
            double under = (BP2.Y - BP1.Y) * (AP2.X - AP1.X) - (BP2.X - BP1.X) * (AP2.Y - AP1.Y);
            if (under == 0) return false;

            double _t = (BP2.X - BP1.X) * (AP1.Y - BP1.Y) - (BP2.Y - BP1.Y) * (AP1.X - BP1.X);
            double _s = (AP2.X - AP1.X) * (AP1.Y - BP1.Y) - (AP2.Y - AP1.Y) * (AP1.X - BP1.X);

            t = _t / under;
            s = _s / under;

            //if (t < 0.0 || t > 1.0 || s < 0.0 || s > 1.0) 
            //    return false;
            

            if (_t == 0.0 && _s == 0.0) return false;

            double tempX = AP1.X + t * (double)(AP2.X - AP1.X);
            double tempY = AP1.Y + t * (double)(AP2.Y - AP1.Y);
            mPoint.X = (int)(AP1.X + t * (double)(AP2.X - AP1.X));
            mPoint.Y = (int)(AP1.Y + t * (double)(AP2.Y - AP1.Y));

            //Trace.WriteLine("_t : " + _t.ToString("0.0000   ") + "_s : " + _s.ToString("0.0000   ") + " t : " + t.ToString("0.0000   ") + " s : " + s.ToString("0.0000   ") + 
            //    "x : " + mPoint.X.ToString("0.0000   ") + "y : " + mPoint.Y.ToString("0.0000   ") );

            return true;
        }

        public bool GetCrossPoint(CvPoint AP1, CvPoint AP2, CvPoint BP1, CvPoint BP2, ref PointF mPoint)
        {
            double t, s, t2, s2;
            double under = (BP2.Y - BP1.Y) * (AP2.X - AP1.X) - (BP2.X - BP1.X) * (AP2.Y - AP1.Y);
            double under2 = (double)((BP2.Y - BP1.Y) * (AP2.X - AP1.X) - (BP2.X - BP1.X) * (AP2.Y - AP1.Y));
            if (under == 0) return false;

            double _t = (BP2.X - BP1.X) * (AP1.Y - BP1.Y) - (BP2.Y - BP1.Y) * (AP1.X - BP1.X);
            double _s = (AP2.X - AP1.X) * (AP1.Y - BP1.Y) - (AP2.Y - AP1.Y) * (AP1.X - BP1.X);

            double _t2 = (double)((BP2.X - BP1.X) * (AP1.Y - BP1.Y) - (BP2.Y - BP1.Y) * (AP1.X - BP1.X));
            double _s2 = (double)((AP2.X - AP1.X) * (AP1.Y - BP1.Y) - (AP2.Y - AP1.Y) * (AP1.X - BP1.X));

            t = _t / under;
            s = _s / under;

            t2 =(double)(_t / under);
            s2 = (double)(_s / under);

            //if (t < 0.0 || t > 1.0 || s < 0.0 || s > 1.0) 
            //    return false;


            if (_t == 0.0 && _s == 0.0) return false;

            double tempX =(double)(AP1.X + t * (double)(AP2.X - AP1.X));
            double tempY =(double)(AP1.Y + t * (double)(AP2.Y - AP1.Y));

           
            mPoint.X = (int)(AP1.X + t * (double)(AP2.X - AP1.X));
            mPoint.Y = (int)(AP1.Y + t * (double)(AP2.Y - AP1.Y));

            //Trace.WriteLine("_t : " + _t.ToString("0.0000   ") + "_s : " + _s.ToString("0.0000   ") + " t : " + t.ToString("0.0000   ") + " s : " + s.ToString("0.0000   ") + 
            //    "x : " + mPoint.X.ToString("0.0000   ") + "y : " + mPoint.Y.ToString("0.0000   ") );

            return true;
        }
        #endregion
    }
}
