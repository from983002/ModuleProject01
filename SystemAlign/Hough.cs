using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Infragistics.Documents.Reports.Report.Tree;
using OpenCvSharp;

namespace SystemAlign
{
    public delegate void MyEvent2(float param1Floats, float param2Floats, IplImage LineImage);
    public delegate void MyEvent3(IplImage GrayImage, int no);
    class Hough : IDisposable
    {
        public int retryCount = 0;
        public event MyEvent2 LineData;
        public event MyEvent3 LineInfo;
        private int linePos = 0;

        private CvPoint[] _linePoints1 = new CvPoint[2];
        private CvPoint[] _linePoints2 = new CvPoint[2];
        public CvPoint[] LinePoints1
        {
            get { return _linePoints1; }
            set { _linePoints1 = value; }
        }

        public CvPoint[] LinePoints2
        {
            get { return _linePoints2; }
            set { _linePoints2 = value; }
        }

        public int GetSetLinepos
        {
            get { return this.linePos; }
            set { this.linePos = value; }
        }

        private int[] cannyParam = new int[] {80, 80};
        
        IplImage image_gray = new IplImage();
        public void Image_Binary_Gray(IplImage sourceImage)
        {
            this.image_gray = Cv.CreateImage(Cv.GetSize(sourceImage), BitDepth.U8, 1);

            IplImage normal_gray = Cv.CreateImage(Cv.GetSize(sourceImage), BitDepth.U8, 1);
            Cv.CvtColor(sourceImage, normal_gray, ColorConversion.BgrToGray);
            //LineInfo(normal_gray,1);

            Cv.Copy(normal_gray, this.image_gray);
            normal_gray.Dispose();
        }

//         public void Sharpen(IplImage sourceMat, IplImage resultImage)
//         {
//             CvMat myCvMat = Cv.GetMat(sourceMat);
//             CvMat resultMat = new CvMat(myCvMat.Rows, myCvMat.Cols, MatrixType.F64C1);
// 
//             int nChannels = myCvMat.ElemChannels;
// 
//             for (int j = 1; j < myCvMat.Rows - 1; ++j)
//             {
//                 int previous = myCvMat[j - 1].
//             }
// 
//         }

        public IplImage houghLine;
        public IplImage OriginImage;

        //HoughLines2()
        //확률적 허프 변환을 지정해 선분의 검출을 실시한다
        //1. CvArr* image : 변환을 할 이미지가 들어간다.
        //2. void* line_storage : 라인을 저장할 공간
        //3. int method : 허프변환에는 3개의 방법이 있다 . 이 방법을 설정하는 인자
        //4. double rho / double theta : 이 둘은 얼마나 조밀한 단위(?)를 사용할 것인가를 정하는 인자 이다. (예로 rho=1이라면 1픽셀단위로 조사를 하고 theta = PI/180 이라면 1도 단위로 조사를 하겠다는 뜻)
        //5. int threshold : 허프 공간상에 그려지는 곡선들이 중첩되는 부분을 이용해서 직선을 검출하는데 threshold 값보다 중첩되는 갯수가 많으면 직선으로 간주한다는 뜻이다. 숫자가 커지면 당연히 더 직선의 기준이 엄격하게 된다.
        //6.double param1 : probabilistic  일 경우  직선의 최소 길이를 설정할 수 있다.
        //7.double param2 : probabilistic  일 경우 직선의 최대 길이를 설정할 수 있다. 
        public IplImage HoughLines(IplImage src, IplImage boxImage, ref IplImage resultImage)
        {
            // cvHoughLines2
            // 확률적 허프 변환을 지정해 선분의 검출을 실시한다
            IplImage orImage = boxImage.Clone();
            // (1) 화상 읽기 
            using (IplImage srcImgStd = src.Clone())
            using (IplImage srcImgGray = new IplImage(src.Size, BitDepth.U8, 1))
            {
                Cv.CvtColor(srcImgStd, srcImgGray, ColorConversion.BgrToGray);

                // (2) 허프변환을 위한 캐니엣지 처리 
                Cv.Canny(srcImgGray, srcImgGray, 50, 200, ApertureSize.Size3);

                houghLine = srcImgGray.Clone();
                int lineMinLength = srcImgStd.Width/2;
                int lineMaxLength = srcImgStd.Width;

                using (CvMemStorage storage = new CvMemStorage())
                {
                    // (3) 표준적 허프 변환에 의한 선의 검출과 검출된 선 그리기
                    CvSeq lines = srcImgGray.HoughLines2(storage, HoughLinesMethod.Standard, 1, Math.PI / 180, 50, 0,0);
                    int limit = Math.Min(lines.Total, 10);
                    for (int i = 0; i < limit; i++)
                    {
                        CvLineSegmentPolar elem = lines.GetSeqElem<CvLineSegmentPolar>(i).Value;
                        float rho = elem.Rho;
                        float theta = elem.Theta;

                        double a = Math.Cos(theta);
                        double b = Math.Sin(theta);
                        double x0 = a * rho;
                        double y0 = b * rho;

                        CvPoint pt1 = new CvPoint { X = Cv.Round(x0 + src.Width * (-b)), Y = Cv.Round(y0 + src.Height * (a)) };
                        CvPoint pt2 = new CvPoint { X = Cv.Round(x0 - src.Width * (-b)), Y = Cv.Round(y0 - src.Height * (a)) };

                        if (pt1.X < 1)
                        {
                            pt1.X = 0;
                            pt2.X = src.Width;
                        }
                        
                        if (pt2.X < 1)
                        {
                            pt1.X = src.Width;
                            pt2.X = 0;
                        }

                        if (pt1.Y <1)
                        {
                            pt1.Y = 0;
                            pt2.Y = src.Height;
                        }

                        if (pt2.Y < 1)
                        {
                            pt1.Y = src.Height; 
                            pt2.Y = 0;
                        }

                        srcImgStd.Line(pt1, pt2, CvColor.Red, 1, LineType.AntiAlias, 0);
                        houghLine = srcImgStd.Clone();

                        orImage.Line(pt1, pt2, CvColor.Red, 1, LineType.AntiAlias, 0);
                        houghLine = orImage.Clone();

                        return houghLine;
                    }
                }
            }
            return houghLine;
        }

        //public List<CvPoint> LinePoints = new List<CvPoint>();

        //public List<CvPoint> GetSetFindedPoints
        //{
            //get { return LinePoints; }
            //set { LinePoints = value; }
        //}

        
        //Hough_Lines_Point LinesFinder = new Hough_Lines_Point();

        private int Now_ROI_No = 0;

        public int GetSet_NowROI
        {
            get { return Now_ROI_No; }
            set { Now_ROI_No = value; }
        }

        public struct Hough_Lines_Point
        {
            //public List<CvPoint> LinePoints;// = new List<CvPoint>();
            //public IplImage srcImgStd;
            //public IplImage srcImgGray;
            public CvMemStorage storage;
            public int limit;
            public CvLineSegmentPolar elem;
            public CvPoint pt1;
            public CvPoint pt2;
        }

        //Hough_Lines_Point LinesFinder = new Hough_Lines_Point();

        private int Threshold = 0;
        //private CvSeq houghLines;
        //public IplImage srcImgGray = new IplImage();
        //public IplImage srcImgStd = new IplImage();
        //public CvMemStorage storage = new CvMemStorage();
        public List<CvPoint> HoughLines_Point(IplImage src, int edge1, int edge2, int line1, int line2, int line3)
        {
            try
            {
                // 확률적 허프 변환을 지정해 선분의 검출을 실시한다
                List<CvPoint> LinePoints = new List<CvPoint>();
                IplImage srcImgStd = new IplImage(src.ROI.Size, BitDepth.U8, 3);
                Cv.Copy(src, srcImgStd);
                IplImage srcImgGray = new IplImage(src.ROI.Size, BitDepth.U8, 1);
                Cv.CvtColor(srcImgStd, srcImgGray, ColorConversion.BgrToGray);
                Cv.Canny(srcImgGray, srcImgGray, edge1, edge2, ApertureSize.Size3);
                CvMemStorage storage = new CvMemStorage();
                CvSeq houghLines = srcImgGray.HoughLines2(storage, HoughLinesMethod.Probabilistic, 1, Math.PI / 180, line1, line2, line3);
                //CvWindow.ShowImages(src);
                
                //Hough_Lines_Point LinesFinder = new Hough_Lines_Point();
                //LinesFinder.srcImgGray = new IplImage(src.Size, BitDepth.U8, 1);
                //Cv.CvtColor(src, LinesFinder.srcImgGray, ColorConversion.BgrToGray);
                //Cv.Canny(LinesFinder.srcImgGray, LinesFinder.srcImgGray, edge1, edge2, ApertureSize.Size3);
                //CvMemStorage storage = new CvMemStorage();
                //CvSeq houghLines = LinesFinder.srcImgGray.HoughLines2(storage, HoughLinesMethod.Probabilistic, 1, Math.PI / 180, line1, line2, line3);

                //CvWindow.ShowImages(LinesFinder.srcImgGray);
                //bool tmpFalg = true;

                //             for(int i = 1; i < 256;i++)
                //             {
                //                 if (houghLines.Total >= 10)
                //                 {
                //                     break;
                //                 }
                //                 houghLines = LinesFinder.srcImgGray.HoughLines2(LinesFinder.storage, HoughLinesMethod.Probabilistic, 1, Math.PI / 180, 2, line2, line3);
                //             }

                //LinePoints = new List<CvPoint>();
                LinePoints.Clear();
                int limit = Math.Min(houghLines.Total, 10);
                for (int i = 0; i < limit; i++)
                {
                    CvLineSegmentPolar elem = houghLines.GetSeqElem<CvLineSegmentPolar>(i).Value;
                    CvPoint pt1 = houghLines.GetSeqElem<CvLineSegmentPoint>(i).Value.P1;
                    CvPoint pt2 = houghLines.GetSeqElem<CvLineSegmentPoint>(i).Value.P2;

                    //Trace.WriteLine(pt1.X.ToString("000.00000  ") + pt1.Y.ToString("000.00000  ") + pt2.X.ToString("000.00000  ")+ pt2.Y.ToString("000.00000"));

                    //LinesFinder.srcImgStd.Line(LinesFinder.pt1, LinesFinder.pt2, CvColor.LawnGreen, 3, LineType.AntiAlias, 0);
                    //TestImage = LinesFinder.srcImgStd.Clone();

                    //src.Line(pt1, pt2, CvColor.LawnGreen, 3, LineType.AntiAlias, 0);
                    //CvWindow.ShowImages(src);
                    //TestImage = src.Clone();

                    LinePoints.Add(pt1);
                    LinePoints.Add(pt2);
                }

                srcImgStd.Dispose();
                srcImgGray.Dispose();
                houghLines.Dispose();
                storage.Dispose();

                return LinePoints;
            }
            catch (Exception e)
            {
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + " " + e.Message);
                throw;
            }
        }


        /*
        public IplImage HoughLines_Point(IplImage src, int canny1, int canny2, int thresh, int sideData)
        {
            // cvHoughLines2
            // 확률적 허프 변환을 지정해 선분의 검출을 실시한다

            // (1) 화상 읽기 
            IplImage srcImgStd = src.Clone();
            IplImage srcImgGray = new IplImage(src.Size, BitDepth.U8, 1);

            CvMemStorage storage = new CvMemStorage();
            CvSeq houghLines;
            Cv.CvtColor(srcImgStd, srcImgGray, ColorConversion.BgrToGray);
            Cv.Canny(srcImgGray, srcImgGray, canny1, canny2, ApertureSize.Size3);
            houghLines = srcImgGray.HoughLines2(storage, HoughLinesMethod.Probabilistic, 1, Math.PI/180, thresh, 5, 0);


            LinePoints.Clear();
            int limit = Math.Min(houghLines.Total, 6);
            for (int i = 0; i < limit; i++)
            {
                CvLineSegmentPolar elem = houghLines.GetSeqElem<CvLineSegmentPolar>(i).Value;
                CvPoint pt1 = houghLines.GetSeqElem<CvLineSegmentPoint>(i).Value.P1;
                CvPoint pt2 = houghLines.GetSeqElem<CvLineSegmentPoint>(i).Value.P2;

                //Trace.WriteLine(pt1.X.ToString("000.00000  ") + pt1.Y.ToString("000.00000  ") + pt2.X.ToString("000.00000  ")+ pt2.Y.ToString("000.00000"));

                srcImgStd.Line(pt1, pt2, CvColor.Red, 1, LineType.AntiAlias, 0);

                LinePoints.Add(pt1);
                LinePoints.Add(pt2);
            }
            srcImgStd.Dispose();
            srcImgGray.Dispose();
            houghLines.Dispose();
            storage.Dispose();
            return srcImgStd;
        }
        */

        public IplImage HoughLines_Point08(IplImage src, int canny1, int canny2, int thresh, int sideData)
        {
            List<CvPoint> LinePoints = new List<CvPoint>();
            int lineMinLength = 0;
            if (sideData == 0 && sideData == 2) lineMinLength = src.Width / 2;
            else lineMinLength = src.Height / 2;

            // cvHoughLines2
            // 확률적 허프 변환을 지정해 선분의 검출을 실시한다

            // (1) 화상 읽기 
            using (IplImage srcImgStd = src.Clone())
            using (IplImage srcImgGray = new IplImage(src.Size, BitDepth.U8, 1))
            {
                Cv.CvtColor(srcImgStd, srcImgGray, ColorConversion.BgrToGray);

                // (2) 허프변환을 위한 캐니엣지 처리 
                //Cv.Canny(srcImgGray, srcImgGray, 50, 200, ApertureSize.Size3);
                Cv.Canny(srcImgGray, srcImgGray, canny1, canny2, ApertureSize.Size3);

                houghLine = srcImgGray.Clone();

                using (CvMemStorage storage = new CvMemStorage())
                {
                    LinePoints.Clear();
                    // (3) 표준적 허프 변환에 의한 선의 검출과 검출된 선 그리기
                    CvSeq lines = srcImgGray.HoughLines2(storage, HoughLinesMethod.Probabilistic, 1, Math.PI / 180, thresh, 5, 0);
                    int limit = Math.Min(lines.Total, 6);
                    for (int i = 0; i < limit; i++)
                    {
                        CvLineSegmentPolar elem = lines.GetSeqElem<CvLineSegmentPolar>(i).Value;
                        CvPoint pt1 = lines.GetSeqElem<CvLineSegmentPoint>(i).Value.P1;
                        CvPoint pt2 = lines.GetSeqElem<CvLineSegmentPoint>(i).Value.P2;

                        //Trace.WriteLine(pt1.X.ToString("000.00000  ") + pt1.Y.ToString("000.00000  ") + pt2.X.ToString("000.00000  ")+ pt2.Y.ToString("000.00000"));

                        srcImgStd.Line(pt1, pt2, CvColor.Red, 1, LineType.AntiAlias, 0);

                        LinePoints.Add(pt1);
                        LinePoints.Add(pt2);

                        houghLine = srcImgStd.Clone();
                    }
                }
            }
            return houghLine;
        }
        

        public IplImage HoughLines_Line(IplImage src, int canny1, int canny2, int thresh)
        {
            // cvHoughLines2
            // 확률적 허프 변환을 지정해 선분의 검출을 실시한다
            List<CvPoint> LinePoints = new List<CvPoint>();
            // (1) 화상 읽기 
            using (IplImage srcImgStd = src.Clone())
            using (IplImage srcImgGray = new IplImage(src.Size, BitDepth.U8, 1))
            {
                Cv.CvtColor(srcImgStd, srcImgGray, ColorConversion.BgrToGray);

                // (2) 허프변환을 위한 캐니엣지 처리 
                //Cv.Canny(srcImgGray, srcImgGray, 50, 200, ApertureSize.Size3);
                Cv.Canny(srcImgGray, srcImgGray, canny1, canny2, ApertureSize.Size3);

                houghLine = srcImgGray.Clone();

                using (CvMemStorage storage = new CvMemStorage())
                {


                    LinePoints.Clear();
                    // (3) 표준적 허프 변환에 의한 선의 검출과 검출된 선 그리기
                    CvSeq lines = srcImgGray.HoughLines2(storage, HoughLinesMethod.Standard, 1, Math.PI / 180, thresh, 0, 0);
                    //int limit = Math.Min(lines.Total, 10);
                    for (int i = 0; i < Math.Min(lines.Total, 3); i++)
                    {

                        CvLineSegmentPolar elem = lines.GetSeqElem<CvLineSegmentPolar>(i).Value;


                       
                        float rho = elem.Rho;
                        float theta = elem.Theta;

                        double a = Math.Cos(theta), b = Math.Sin(theta);
                        double x0 = a * rho, y0 = b * rho;

                        CvPoint pt1, pt2;

                        //pt1.X = Cv.Round(x0 + 1000*(-b));
                        //pt1.Y = Cv.Round(y0 + 1000*(a));
                        //pt2.X = Cv.Round(x0 - 1000*(-b));
                        //pt2.Y = Cv.Round(y0 - 1000*(a));

                        pt1.X = Cv.Round(x0 + srcImgStd.Width * (-b));
                        pt1.Y = Cv.Round(y0 + srcImgStd.Height * (a));
                        pt2.X = Cv.Round(x0 - srcImgStd.Width * (-b));
                        pt2.Y = Cv.Round(y0 - srcImgStd.Height * (a));

                        if (pt1.X < 0)
                        {
                            pt1.X = 0;
                            pt2.X = src.Width;
                        }
                        else if (pt2.X < 0)
                        {
                            pt1.X = src.Width;
                            pt2.X = 0;
                        }

                        if (pt1.Y < 0)
                        {
                            pt1.Y = 0;
                            pt2.Y = src.Height;
                        }
                        else if (pt2.Y < 0)
                        {
                            pt1.Y = src.Height;
                            pt2.Y = 0;
                        }

                        //Trace.WriteLine(pt1.X.ToString("000.00000  ") + pt1.Y.ToString("000.00000  ") + pt2.X.ToString("000.00000  ") + pt2.Y.ToString("000.00000"));
                        srcImgStd.Line(pt1, pt2, CvColor.Red, 1, LineType.AntiAlias, 0);

                        LinePoints.Add(pt1);
                        LinePoints.Add(pt2);

                        houghLine = srcImgStd.Clone();
                    }
                }
            }
            return houghLine;
        }


        public double picXData = 0f;
        public double picYData = 0f;

        public void Dispose()
        {
            if (houghLine != null) Cv.ReleaseImage(houghLine);
        }
    }
}
