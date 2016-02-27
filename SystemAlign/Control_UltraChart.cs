using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Shared.Styles;
using Infragistics.Win.UltraWinChart;
using Infragistics.Win.UltraWinDataSource;
using Infragistics.Win.UltraWinGrid;
using System.Collections;
using System.Data;

namespace SystemAlign
{
    class Control_UltraChart
    {
        public void ChartLabelMakeAxisX(Infragistics.Win.UltraWinChart.UltraChart iChart)
        {
            iChart.Axis.X.TickmarkInterval = 10;
        }

        public void ChartLabelSetAxisX(Infragistics.Win.UltraWinChart.UltraChart iChart)
        {
            // Item Labels Setting 항목 라벨 설정
            iChart.Axis.X.Labels.Visible = true;      // 라벨 활성 여부
            iChart.Axis.X.Labels.Font = new Font("Arial", 6F);      // 폰트
            iChart.Axis.X.Labels.FontColor = Color.Gray;       // 폰트색
            iChart.Axis.X.Labels.HorizontalAlign = StringAlignment.Center;        // 가로 정렬    Default : Near
            iChart.Axis.X.Labels.VerticalAlign = StringAlignment.Center;      // 세로 정렬    Default : Center
            iChart.Axis.X.Labels.ReverseText = false;     // 텍스트 반대방향 여부

            //iChart.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;      // 방향 유형    Default : VerticalLeftFacing
            iChart.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom;      // 방향 유형    Default : VerticalLeftFacing
            if (iChart.Axis.X.Labels.Orientation == Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom)
            {
                iChart.Axis.X.Labels.OrientationAngle = 90;      // 방향 각도 (기울임)  Default : 0 (거꾸로 상태)
                iChart.Axis.X.Labels.Flip = false;        // 뒤집힘 여부
            }

            //iChart.Axis.X.Labels.ItemFormat = Infragistics.UltraChart.Shared.Styles.AxisItemLabelFormat.ItemLabel;
            iChart.Axis.X.Labels.ItemFormat = Infragistics.UltraChart.Shared.Styles.AxisItemLabelFormat.Custom;

            if (iChart.Axis.X.Labels.ItemFormat == Infragistics.UltraChart.Shared.Styles.AxisItemLabelFormat.Custom)
            {
                iChart.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
                //iChart.Axis.X.Labels.ItemFormatString = "<CUSTOM>";
            }

            iChart.Axis.X.Labels.Layout.Padding = 1;      // Padding  Default : 2
            iChart.Axis.X.Labels.SeriesLabels.Layout.Padding = 1;     // Padding  Default : 2
        }
        public void CallClassMemberCall(Infragistics.Win.UltraWinChart.UltraChart iChart)
        {
             System.Collections.Hashtable labelRenderers1 = new System.Collections.Hashtable();
            DateTime date = DateTime.Now.Date;
            labelRenderers1.Add("CUSTOM", new LabelRendererDate(date));

            iChart.LabelHash = labelRenderers1;

           

            System.Collections.Hashtable labelRenderers2 = new System.Collections.Hashtable();

            labelRenderers2.Add("CUSTOM", new LabelRendererMonth());

            iChart.LabelHash = labelRenderers2;
        }
        public void InitializeChart(Infragistics.Win.UltraWinChart.UltraChart iChart)
        {
            //Chart Type Setting (챠트 유형 설정)
            iChart.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;

            #region Title Setting 타이틀(제목) 설정

            // Top Title
            iChart.TitleTop.Visible = false;       // 상단 타이틀 활성 여부
            iChart.TitleTop.Text = "Reservation";      // 보여 질 텍스트
            iChart.TitleTop.Font = new Font("Arial", 16F, FontStyle.Bold);       // 폰트
            iChart.TitleTop.FontColor = Color.DarkMagenta;        // 폰트색
            iChart.TitleTop.HorizontalAlign = StringAlignment.Center;     // 텍스트 방향을 기준으로 가로정렬
            iChart.TitleTop.VerticalAlign = StringAlignment.Near;       // 텍스트 방향을 기준으로 세로정렬
            iChart.TitleTop.Margins.Top = 0;      // 상단 여백
            iChart.TitleTop.Margins.Bottom = 0;       // 하단 여백
            iChart.TitleTop.ReverseText = false;       // 텍스트 반대방향 여부
            iChart.TitleTop.WrapText = false;
            iChart.TitleTop.Flip = false;     // 뒤집힘 여부
            iChart.TitleTop.Extent = 35;      // 영역(넓이)   Default : 33
            iChart.TitleTop.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;       // 방향 유형    Default : Horizontal
            if (iChart.TitleTop.Orientation == Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom)
            {
                iChart.TitleTop.OrientationAngle = 0;     // 방향 각도 (기울임)  Default : 0 (거꾸로 상태)
            }
            
            #endregion Title Setting 타이틀(제목) 설정

            #region Tooltips Setting (툴팁 설정)

            iChart.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
            if (iChart.Tooltips.Format == Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom)
            {
                //iChart.Tooltips.FormatString = "<ITEM_LABEL> : <DATA_VALUE:#,###>";
                iChart.Tooltips.FormatString = "<CUSTOM> : <DATA_VALUE:#,###>";
            }

            iChart.Tooltips.Display = Infragistics.UltraChart.Shared.Styles.TooltipDisplay.MouseMove;     // 툴팁 표시 유형   Default : MouseMove
            iChart.Tooltips.AlphaLevel = 150;     // 투명레벨  Default : 150
            iChart.Tooltips.BackColor = Color.FromArgb(255, 255, 255, 255);        // 배경색
            iChart.Tooltips.Font = new Font("Arial", 10F);     // 폰트 설정
            iChart.Tooltips.FontColor = Color.Black;      // 폰트색   Default : Black
            iChart.Tooltips.BorderColor = Color.Black;       // 테두리색 Default : Black
            iChart.Tooltips.BorderThickness = 0;      // 테두리 선 두께  Default : 1
            iChart.Tooltips.BorderStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Solid;      // 테두리 선 유형   Default : Solid

            #endregion Tooltips Setting (툴팁 설정)

            #region Category(Legends:범주) Setting

            iChart.Legend.Visible = true;     // 범주 활성여부
            //iChart.Legend.Location‍ = Infragistics.UltraChart.Shared.Styles.LegendLocation.Right;      // 범주 위치    Default : Right
            iChart.Legend.SpanPercentage = 10;        // 범주가 챠트에서 차지하는 퍼센트  Default : 25
            iChart.Legend.MoreIndicatorText = "표시할 수 없습니다.";     // 제한된 영역에 따라 표시할 수 없을 때 사용하는 문자열
            iChart.Legend.BackgroundColor = Color.White;      // 배경색   Default : white
            //iChart.Legend.FormatString = "";
            // Font Setting 폰트 설정
            iChart.Legend.Font = new Font("Arial", 9F);        // 폰트 설정
            iChart.Legend.FontColor = Color.Black;      // 폰트색 Default : Black
            // Border Setting 테두리 설정
            iChart.Legend.BorderThickness = 1;        // 범주 테두리 두께 Default : 1
            iChart.Legend.BorderColor = Color.Navy;        // 테두리색 Default : Navy
            iChart.Legend.BorderStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Solid;        // 범주 테두리 모양 Default : Solid
            // Margins Setting 여백 설정
            iChart.Legend.Margins.Top = 0;        // 상단
            iChart.Legend.Margins.Bottom = 0;     // 하단
            iChart.Legend.Margins.Left = 0;       // 좌측
            iChart.Legend.Margins.Right = 0;      // 우측

            #endregion Category(Legends:범주) Setting
            
            #region Axis (X,Y) Setting 축 설정

            #region Axix X Setting X축 설정

            iChart.Axis.X.Visible = true;     // 활성 여부
            iChart.Axis.X.Extent = 40;        // X축 영역(넓이)   Default : 80

            // 여백 설정
            iChart.Axis.X.Margin.Near.MarginType = Infragistics.UltraChart.Shared.Styles.LocationType.Percentage;     // 여백 유형    Default : Percentage
            iChart.Axis.X.Margin.Near.Value = 0;      // 여백 값  Default : 0.0
            iChart.Axis.X.Margin.Far.MarginType = Infragistics.UltraChart.Shared.Styles.LocationType.Percentage;      //여백 유형 Default : Percentage
            iChart.Axis.X.Margin.Far.Value = 2;       // 여백 값  Default : 0.0

            // 주(시리즈) 단위 라인 설정
            iChart.Axis.X.MajorGridLines.Visible = true;      // 활성 여부
            iChart.Axis.X.MajorGridLines.AlphaLevel = 255;        // 투명도   Default : 255
            iChart.Axis.X.MajorGridLines.Color = Color.Gainsboro;     // 선색 Default : Gainsboro
            iChart.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;     // 선모양   Default : Dot
            iChart.Axis.X.MajorGridLines.Thickness = 1;       // 선 두께      Default : 1

            // 부(항목별) 단위 라인 설정
            iChart.Axis.X.MinorGridLines.Visible = false;     // 활성 여부
            iChart.Axis.X.MinorGridLines.AlphaLevel = 255;        // 투명도   Default : 255
            iChart.Axis.X.MinorGridLines.Color = Color.LightGray;     // 선색 Default : LightGray
            iChart.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;     // 선모양   Default : Dot
            iChart.Axis.X.MinorGridLines.Thickness = 1;       // 선 두께  Default : 1
            
            // Line Setting 축 라인 설정
            iChart.Axis.X.LineThickness = 1;  // 선 두께  Default : 1
            iChart.Axis.X.LineColor = Color.Black;  // 선색 Default : Black
            iChart.Axis.X.LineDrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Solid;     // 선 모양  Default : Solid
            iChart.Axis.X.LineEndCapStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.NoAnchor;      // 선 끝 모양   Default : NoAnchor

            // Range Setting 범위 설정
            iChart.Axis.X.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Automatic;      // 범위 유형    Default : Automatic
            iChart.Axis.X.RangeMax = 12.0;        // 최대 범위값  Default : 0.0
            iChart.Axis.X.RangeMin = 0.0;     // 최소 범위값  Default : 0.0

            // Scroll Scale Setting 스크롤 설정
            iChart.Axis.X.ScrollScale.Visible = false;     // 스크롤 활성 여부
            iChart.Axis.X.ScrollScale.Scroll = 0.0;       // 스크롤 최초 위치 Default : 0.0 (0.0 ~ 1.0)
            iChart.Axis.X.ScrollScale.Scale = 1.0;        // 스크롤 크기 조정 (확대율)  Default : 1.0 (0.0 ~ 1.0)
            iChart.Axis.X.ScrollScale.Height = 15;        // 스크롤 높이  Default : 10px
            iChart.Axis.X.ScrollScale.Width = 15;      // 스크롤 넓이  Default : 15px
            
            // Item Labels Setting 항목 라벨 설정
            iChart.Axis.X.Labels.Visible = true;      // 라벨 활성 여부
            iChart.Axis.X.Labels.Font = new Font("Arial", 7F);      // 폰트
            iChart.Axis.X.Labels.FontColor = Color.Black;       // 폰트색
            iChart.Axis.X.Labels.HorizontalAlign = StringAlignment.Center;        // 가로 정렬    Default : Near
            iChart.Axis.X.Labels.VerticalAlign = StringAlignment.Center;      // 세로 정렬    Default : Center
            iChart.Axis.X.Labels.ReverseText = false;     // 텍스트 반대방향 여부

            //iChart.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;      // 방향 유형    Default : VerticalLeftFacing
            iChart.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom;      // 방향 유형    Default : VerticalLeftFacing
            if (iChart.Axis.X.Labels.Orientation == Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom)
            {
                iChart.Axis.X.Labels.OrientationAngle = 90;      // 방향 각도 (기울임)  Default : 0 (거꾸로 상태)
                iChart.Axis.X.Labels.Flip = false;        // 뒤집힘 여부
            }

            //iChart.Axis.X.Labels.ItemFormat = Infragistics.UltraChart.Shared.Styles.AxisItemLabelFormat.ItemLabel;
            iChart.Axis.X.Labels.ItemFormat = Infragistics.UltraChart.Shared.Styles.AxisItemLabelFormat.Custom;

            if (iChart.Axis.X.Labels.ItemFormat == Infragistics.UltraChart.Shared.Styles.AxisItemLabelFormat.Custom)
            {
                iChart.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
                //iChart.Axis.X.Labels.ItemFormatString = "<CUSTOM>";
            }

            iChart.Axis.X.Labels.Layout.Padding = 10;      // Padding  Default : 2

            // Series Labels Setting 시리즈 라벨(묶음) 설정
            iChart.Axis.X.Labels.SeriesLabels.Visible = true;      // 라벨 활성 여부
            iChart.Axis.X.Labels.SeriesLabels.Font = new Font("Arial", 9F);     // 폰트
            iChart.Axis.X.Labels.SeriesLabels.FontColor = Color.Black;      // 폰트색
            iChart.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Center;        // 가로 정렬    Default : Center
            iChart.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;      // 세로 정렬    Default : Center
            iChart.Axis.X.Labels.SeriesLabels.ReverseText = false;        // 텍스트 반대방향 여부
            iChart.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;     // 방향 유형    Default : Horizontal

            if (iChart.Axis.X.Labels.SeriesLabels.Orientation == Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom)
            {
                iChart.Axis.X.Labels.SeriesLabels.OrientationAngle = 60;     // 방향 각도 (기울임)  Default : 0 (거꾸로 상태)
                iChart.Axis.X.Labels.SeriesLabels.Flip = false;       // 뒤집힘 여부
            }

            iChart.Axis.X.Labels.SeriesLabels.Format = Infragistics.UltraChart.Shared.Styles.AxisSeriesLabelFormat.SeriesLabel;
            if (iChart.Axis.X.Labels.SeriesLabels.Format == Infragistics.UltraChart.Shared.Styles.AxisSeriesLabelFormat.Custom)
            {
                iChart.Axis.X.Labels.SeriesLabels.FormatString = "<SERIES_LABEL>";
            }

            iChart.Axis.X.Labels.SeriesLabels.Layout.Padding = 2;     // Padding  Default : 2

            #endregion Axix X Setting X축 설정

            #region Axix Y Setting Y축 설정
            
            iChart.Axis.Y.Visible = true;     // 활성 여부
            iChart.Axis.Y.Extent = 60;        // X축 영역(넓이)   Default : 80

            // 여백 설정
            iChart.Axis.Y.Margin.Near.MarginType = Infragistics.UltraChart.Shared.Styles.LocationType.Percentage;     // 여백 유형    Default : Percentage
            iChart.Axis.Y.Margin.Near.Value = 0;      // 여백 값  Default : 0.0
            iChart.Axis.Y.Margin.Far.MarginType = Infragistics.UltraChart.Shared.Styles.LocationType.Percentage;      //여백 유형 Default : Percentage
            iChart.Axis.Y.Margin.Far.Value = 0;       // 여백 값  Default : 0.0

            // 주(시리즈) 단위 라인 설정
            iChart.Axis.Y.MajorGridLines.Visible = true;      // 활성 여부
            iChart.Axis.Y.MajorGridLines.AlphaLevel = 255;        // 투명도   Default : 255
            iChart.Axis.Y.MajorGridLines.Color = Color.Gainsboro;     // 선색 Default : Gainsboro
            iChart.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;     // 선모양   Default : Dot
            iChart.Axis.Y.MajorGridLines.Thickness = 1;       // 선 두께      Default : 1

            // 부(항목별) 단위 라인 설정
            iChart.Axis.Y.MinorGridLines.Visible = false;     // 활성 여부
            iChart.Axis.Y.MinorGridLines.AlphaLevel = 255;        // 투명도   Default : 255
            iChart.Axis.Y.MinorGridLines.Color = Color.LightGray;     // 선색 Default : LightGray
            iChart.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;     // 선모양   Default : Dot
            iChart.Axis.Y.MinorGridLines.Thickness = 1;       // 선 두께  Default : 1

            // Line Setting 축 라인 설정
            iChart.Axis.Y.LineThickness = 1;  // 선 두께  Default : 1
            iChart.Axis.Y.LineColor = Color.Black;  // 선색 Default : Black
            iChart.Axis.Y.LineDrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Solid;     // 선 모양  Default : Solid
            iChart.Axis.Y.LineEndCapStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.NoAnchor;      // 선 끝 모양   Default : NoAnchor

            // Range Setting 범위 설정
            iChart.Axis.Y.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Automatic;      // 범위 유형    Default : Automatic
            iChart.Axis.Y.RangeMax = 0.0;        // 최대 범위값  Default : 0.0
            iChart.Axis.Y.RangeMin = 0.0;     // 최소 범위값  Default : 0.0
            
            // Scroll Scale Setting 스크롤 설정
            iChart.Axis.Y.ScrollScale.Visible = false;     // 스크롤 활성 여부
            iChart.Axis.Y.ScrollScale.Scroll = 0.0;       // 스크롤 최초 위치 Default : 0.0 (0.0 ~ 1.0)
            iChart.Axis.Y.ScrollScale.Scale = 1.0;        // 스크롤 크기 조정 (확대율)  Default : 1.0 (0.0 ~ 1.0)
            iChart.Axis.Y.ScrollScale.Height = 15;        // 스크롤 높이  Default : 10px
            iChart.Axis.Y.ScrollScale.Width = 15;      // 스크롤 넓이  Default : 15px
            
            // Labels Setting 라벨 설정
            iChart.Axis.Y.Labels.Visible = true;      // 라벨 활성 여부
            iChart.Axis.Y.Labels.Font = new Font("Arial", 9F);      // 폰트
            iChart.Axis.Y.Labels.FontColor = Color.Black;       // 폰트색
            iChart.Axis.Y.Labels.HorizontalAlign = StringAlignment.Center;        // 가로 정렬    Default : Near
            iChart.Axis.Y.Labels.VerticalAlign = StringAlignment.Center;      // 세로 정렬    Default : Center
            iChart.Axis.Y.Labels.ReverseText = false;     // 텍스트 반대방향 여부
            iChart.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;      // 방향 유형    Default : VerticalLeftFacing

            if (iChart.Axis.Y.Labels.Orientation == Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom)
            {
                iChart.Axis.Y.Labels.OrientationAngle = 270;      // 방향 각도 (기울임)  Default : 0 (거꾸로 상태)
                iChart.Axis.Y.Labels.Flip = false;        // 뒤집힘 여부
            }

            iChart.Axis.Y.Labels.ItemFormat = Infragistics.UltraChart.Shared.Styles.AxisItemLabelFormat.DataValue;

            if (iChart.Axis.Y.Labels.ItemFormat == Infragistics.UltraChart.Shared.Styles.AxisItemLabelFormat.Custom)
            {
                iChart.Axis.Y.Labels.ItemFormatString = "<ITEM_LABEL>";
            }

            iChart.Axis.Y.Labels.Layout.Padding = 2;      // Padding  Default : 2
            
            // Series Labels Setting 시리즈 라벨(라벨 묶음) 설정
            iChart.Axis.Y.Labels.SeriesLabels.Visible = true;      // 라벨 활성 여부
            iChart.Axis.Y.Labels.SeriesLabels.Font = new Font("Arial", 9F);     // 폰트
            iChart.Axis.Y.Labels.SeriesLabels.FontColor = Color.Black;      // 폰트색
            iChart.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Center;        // 가로 정렬    Default : Center
            iChart.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;      // 세로 정렬    Default : Center
            iChart.Axis.Y.Labels.SeriesLabels.ReverseText = false;        // 텍스트 반대방향 여부
            iChart.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;     // 방향 유형    Default : Horizontal

            if (iChart.Axis.Y.Labels.SeriesLabels.Orientation == Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom)
            {
                iChart.Axis.Y.Labels.SeriesLabels.OrientationAngle = 270;     // 방향 각도 (기울임)  Default : 0 (거꾸로 상태)
                iChart.Axis.Y.Labels.SeriesLabels.Flip = false;       // 뒤집힘 여부
            }

            iChart.Axis.Y.Labels.SeriesLabels.Format = Infragistics.UltraChart.Shared.Styles.AxisSeriesLabelFormat.SeriesLabel;
            if (iChart.Axis.Y.Labels.SeriesLabels.Format == Infragistics.UltraChart.Shared.Styles.AxisSeriesLabelFormat.Custom)
            {
                iChart.Axis.Y.Labels.SeriesLabels.FormatString = "<SERIES_LABEL>";
            }

            iChart.Axis.Y.Labels.SeriesLabels.Layout.Padding = 2;     // Padding  Default : 2

            iChart.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:#,###>";

            #endregion Axix Y Setting Y축 설정
            
            #endregion Axis (X,Y,Z) Setting 축 설정

            
            // 행과 열 상호 교환(Pivot) 여부
            iChart.Data.SwapRowsAndColumns = false;

            // Value Column을 0값 부터 표현할지 여부
            iChart.Data.ZeroAligned = false;
            
            // 데이터가 바인딩 되지 않거나 데이터가 없어서 표시 할 챠트가 없을 경우 표시 할 텍스트
            iChart.EmptyChartText = "";

            #region Charts Setting

            #region LineChart

            iChart.LineChart.Thickness = 4;       // 선 두께  Default : 3;
            iChart.LineChart.NullHandling = Infragistics.UltraChart.Shared.Styles.NullHandling.Zero;      // 데이터가 공백일 경우 처리
            iChart.LineChart.MidPointAnchors = true;
            iChart.LineChart.HighLightLines = true;
            iChart.LineChart.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Solid;       // 선 모양  Default : Solid
            iChart.LineChart.StartStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.DiamondAnchor;       // 시작 모양    Default : DiamondAnchor
            iChart.LineChart.EndStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.DiamondAnchor;     // 끝 모양  Default : DiamondAnchor

            #endregion LineChart

            #endregion Charts Setting
        }

        /// <summary>
        ///     Ultra Chart 의 데이터 값의 표시 구성항목을 설정한다.
        /// </summary>
        private void Chart_Initialize(Infragistics.Win.UltraWinChart.UltraChart ultraChart1,Infragistics.Win.UltraWinChart.UltraChart ultraChart2)
        {
            ultraChartSetup(ultraChart1);
            ultraChartSetup(ultraChart2);

            ultraChart1.DataSource = initializeDataTable(measureTable1);
            ultraChart2.DataSource = initializeDataTable(measureTable2);
        }

        private DataTable measureTable1, measureTable2;
        /// <summary>
        /// Ultra Chart 에 사용되어지는 DataTable을 초기화 한다.
        /// </summary>
        private DataTable initializeDataTable(DataTable measureTable)
        {
            measureTable.Columns.Clear();
            measureTable.Rows.Clear();

            for (int i = 0; i < 21; i++)
            {
                //measureTable.Columns.Add(new DataColumn(i.ToString(), Type.GetType("System.Decimal")));
                //measureTable.Columns[i].AllowDBNull = false;

                var measureColumn = new DataColumn();
                string colName = i.ToString();
                measureColumn.DataType = Type.GetType("System.Decimal");
                measureColumn.AllowDBNull = false;
                measureColumn.Caption = colName;
                measureColumn.ColumnName = colName;
                measureColumn.DefaultValue = 0;
                measureTable.Columns.Add(measureColumn);
            }

            measureRow = measureTable.NewRow();
            measureTable.Rows.Add(measureRow);
            return measureTable;
        }
        private void ultraChartSetup(Infragistics.Win.UltraWinChart.UltraChart iChart)
        {
            iChart.Legend.Visible = false;

            iChart.Axis.Y.RangeMax = 10;
            iChart.Axis.X.LineThickness = 2;
            iChart.Axis.Y.LineThickness = 2;
            iChart.ColorModel.CustomPalette = new[] { Color.Gray, Color.DimGray };


            iChart.Axis.X.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval;
            iChart.Axis.X.TickmarkInterval = (double)4;
            iChart.Axis.X.Labels.ItemFormat = AxisItemLabelFormat.DataValue;
            iChart.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";

            iChart.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom;
            if (iChart.Tooltips.Format == Infragistics.UltraChart.Shared.Styles.TooltipStyle.Custom)
            {
                iChart.Tooltips.FormatString = "<ITEM_LABEL> : <DATA_VALUE:#,###>";
            }
        }


        private int m_nChartMaxYAsixValue = 0;
        private int m_nChartAxisYCount = 0;
        private void ChartTableWriteData(DataTable UseTable)
        {
            m_nChartMaxYAsixValue = 0;
            int nAxisCount = this.ChartAxisXCountCompute();

            if (nAxisCount == 0)
            {
                NowNull(UseTable.Columns.Count, UseTable.Columns.Count, UseTable);
                m_nChartAxisYCount = UseTable.Columns.Count;
                return;
            }

            m_nChartAxisYCount = nAxisCount;
            int nNowChartAxis = UseTable.Columns.Count;

            if (nAxisCount < nNowChartAxis) NowSmall(nAxisCount, nNowChartAxis, UseTable);
            if (nAxisCount == nNowChartAxis) NowSame(nAxisCount, nNowChartAxis, UseTable);
            if (nAxisCount > nNowChartAxis) NowBig(nAxisCount, nNowChartAxis, UseTable);
        }

        private string sLongTime = "0.25";//?
        private int ChartAxisXCountCompute()
        {
            int Acount = 0;
            try
            {
                float fLTime = float.Parse(sLongTime) * 1000;
                double AxisCount = fLTime / 5;
                Acount = (int)AxisCount;
                return Acount;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        private DataRow measureRow;
        private string[] m_sReadGraphDataArray;
        private void NowNull(int i_nNowAxis, int i_nChartAxis, DataTable UseTable)
        {
            measureRow = UseTable.NewRow();
            for (int i = 0; i < i_nNowAxis; i++)
            {
                if (m_sReadGraphDataArray[i] == null || int.Parse(m_sReadGraphDataArray[i]) < 0)
                    m_sReadGraphDataArray[i] = "0";
                if (m_nChartMaxYAsixValue < int.Parse(m_sReadGraphDataArray[i]))
                    m_nChartMaxYAsixValue = int.Parse(m_sReadGraphDataArray[i]);

                int tmpInt = int.Parse(m_sReadGraphDataArray[i]);
                string colNames = i.ToString();
                measureRow[colNames] = int.Parse(m_sReadGraphDataArray[i]);
            }

            UseTable.Rows.Add(measureRow);
        }


        private void NowSmall(int i_nNowAxis, int i_nChartAxis, DataTable UseTable)
        {
            measureRow = UseTable.NewRow();
            for (int i = 0; i < i_nNowAxis + 2; i++)
            {
                if (m_sReadGraphDataArray[i] == null) m_sReadGraphDataArray[i] = "0";
                if (m_nChartMaxYAsixValue < int.Parse(m_sReadGraphDataArray[i]))
                    m_nChartMaxYAsixValue = int.Parse(m_sReadGraphDataArray[i]);

                int tmpInt = int.Parse(m_sReadGraphDataArray[i]);
                string colNames = i.ToString();
                measureRow[colNames] = int.Parse(m_sReadGraphDataArray[i]);
            }

            UseTable.Rows.Add(measureRow);
        }

        private void NowSame(int i_nNowAxis, int i_nChartAxis, DataTable UseTable)
        {
            measureRow = UseTable.NewRow();
            for (int i = 0; i < i_nNowAxis + 2; i++)
            {
                if (m_sReadGraphDataArray[i] == null) m_sReadGraphDataArray[i] = "0";
                if (m_nChartMaxYAsixValue < int.Parse(m_sReadGraphDataArray[i]))
                    m_nChartMaxYAsixValue = int.Parse(m_sReadGraphDataArray[i]);

                int tmpInt = int.Parse(m_sReadGraphDataArray[i]);
                string colNames = i.ToString();
                measureRow[colNames] = int.Parse(m_sReadGraphDataArray[i]);
            }

            UseTable.Rows.Add(measureRow);
        }

        private void NowBig(int i_nNowAxis, int i_nChartAxis, DataTable UseTable)
        {
            try
            {
                int nAddCount = i_nNowAxis - i_nChartAxis;

                for (int i = i_nChartAxis; i < i_nNowAxis + 2; i++)
                {
                    var measureColumn = new DataColumn();
                    string colName = i.ToString();
                    measureColumn.DataType = Type.GetType("System.Decimal");
                    measureColumn.AllowDBNull = false;
                    measureColumn.Caption = colName;
                    measureColumn.ColumnName = colName;
                    measureColumn.DefaultValue = 0;
                    UseTable.Columns.Add(measureColumn);
                }

                measureRow = UseTable.NewRow();
                string colNames = "";
                //for (int i = 0; i < m_iGraphDataCount; i++)
                for (int i = 0; i < i_nNowAxis + 2; i++)
                {
                    if (m_sReadGraphDataArray[i] == null) m_sReadGraphDataArray[i] = "0";
                    if (m_nChartMaxYAsixValue < int.Parse(m_sReadGraphDataArray[i]))
                        m_nChartMaxYAsixValue = int.Parse(m_sReadGraphDataArray[i]);

                    int tmpInt = int.Parse(m_sReadGraphDataArray[i]);
                    colNames = i.ToString();
                    if (tmpInt > 0)
                        measureRow[colNames] = int.Parse(m_sReadGraphDataArray[i]);
                }
                UseTable.Rows.Add(measureRow);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }


    public class LabelRendererDate : Infragistics.UltraChart.Resources.IRenderLabel
    {
        public LabelRendererDate(){}
        public LabelRendererDate(DateTime date) { StartDate = date; }
        public DateTime StartDate = DateTime.Now;

        /// <summary>
        /// ITEM_LABEL 기준으로 다른명으로 변경 혹은 긴면 줄임 표현.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>

        public string ToString(System.Collections.Hashtable Context)
        {
            string label = (string)(Context["ITEM_LABEL"]);

            switch (label)
            {
                case "DAYSUM":
                    label = "Sum";
                    break;
                    
                case "DAY1":
                    label = StartDate.AddDays(-13).ToString("MM/dd").Replace("-", "/");
                    break;

                case "DAY2":
                    label = StartDate.AddDays(-12).ToString("MM/dd").Replace("-", "/");
                    break;

                case "DAY3":
                    label = StartDate.AddDays(-11).ToString("MM/dd").Replace("-", "/");
                    break;
                    
                case "DAY4":
                    label = StartDate.AddDays(-10).ToString("MM/dd").Replace("-", "/");
                    break;
                    
                case "DAY5":
                    label = StartDate.AddDays(-9).ToString("MM/dd").Replace("-", "/");
                    break;
                    
                case "DAY6":
                    label = StartDate.AddDays(-8).ToString("MM/dd").Replace("-", "/");
                    break;

                case "DAY7":
                    label = StartDate.AddDays(-7).ToString("MM/dd").Replace("-", "/");
                    break;

                case "DAY8":
                    label = StartDate.AddDays(-6).ToString("MM/dd").Replace("-", "/");
                    break;

                case "DAY9":
                    label = StartDate.AddDays(-5).ToString("MM/dd").Replace("-", "/");
                    break;
                    
                case "DAY10":
                    label = StartDate.AddDays(-4).ToString("MM/dd").Replace("-", "/");
                    break;

                case "DAY11":
                    label = StartDate.AddDays(-3).ToString("MM/dd").Replace("-", "/");
                    break;

                case "DAY12":
                    label = StartDate.AddDays(-2).ToString("MM/dd").Replace("-", "/");
                    break;

                case "DAY13":
                    label = StartDate.AddDays(-1).ToString("MM/dd").Replace("-", "/");
                    break;

                case "DAY14":
                    label = StartDate.ToString("MM/dd").Replace("-", "/");
                    break;
            }
            
            return label;
        }

 

        /// <summary>

        /// 요일

        /// </summary>

        /// <param name="dateTime"></param>

        /// <returns></returns>

        private string GetDayOfWeek(DateTime dateTime)
        {
            string ret = string.Empty;

            switch (dateTime.DayOfWeek)
            {
                    case DayOfWeek.Friday:
                    ret = "Fri";
                    break;

                case DayOfWeek.Monday:
                    ret = "Mon";
                    break;

                case DayOfWeek.Saturday:
                    ret = "Sat";
                    break;

                case DayOfWeek.Sunday:
                    ret = "Sun";
                    break;

                case DayOfWeek.Thursday:
                    ret = "Thu";
                    break;

                case DayOfWeek.Tuesday:
                    ret = "Tue";
                    break;

                case DayOfWeek.Wednesday:
                    ret = "Wed";
                    break;
            }
            return ret;
        }
    }

 

    public class LabelRendererMonth : Infragistics.UltraChart.Resources.IRenderLabel
    {
        /// <summary>
        /// ITEM_LABEL 기준으로 다른명으로 변경 혹은 긴면 줄임 표현.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public string ToString(System.Collections.Hashtable Context)
        {
            string label = (string)(Context["ITEM_LABEL"]);

            switch (label)
            {
                case "MM_SUM":
                    label = "Sum";
                    break;

                case "MM_01":
                    label = "January";
                    break;

                case "MM_02":
                    label = "February";
                    break;

                case "MM_03":
                    label = "March";
                    break;

                case "MM_04":
                    label = "April";
                    break;

                case "MM_05":
                    label = "May";
                    break;

                case "MM_06":
                    label = "June";
                    break;
                    
                case "MM_07":
                    label = "July";
                    break;

                case "MM_08":
                    label = "August";
                    break;
                    
                case "MM_09":
                    label = "September";
                    break;

                case "MM_10":
                    label = "October";
                    break;
                    
                case "MM_11":
                    label = "November";
                    break;
                    
                case "MM_12":
                    label = "December";
                    break;
            }
            return label;
        }

    }
}
