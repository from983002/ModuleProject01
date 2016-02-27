// VisionLibrary.cpp : 해당 DLL의 초기화 루틴을 정의합니다.
//
#include "stdafx.h"
#include "VisionLibrary.h"
#include "LineFinder.cpp"
#include <opencv/cv.h>
#include <opencv/highgui.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//
//TODO: 이 DLL이 MFC DLL에 대해 동적으로 링크되어 있는 경우
//		MFC로 호출되는 이 DLL에서 내보내지는 모든 함수의
//		시작 부분에 AFX_MANAGE_STATE 매크로가
//		들어 있어야 합니다.
//
//		예:
//
//		extern "C" BOOL PASCAL EXPORT ExportedFunction()
//		{
//			AFX_MANAGE_STATE(AfxGetStaticModuleState());
//			// 일반적인 함수 본문은 여기에 옵니다.
//		}
//
//		이 매크로는 MFC로 호출하기 전에
//		각 함수에 반드시 들어 있어야 합니다.
//		즉, 매크로는 함수의 첫 번째 문이어야 하며 
//		개체 변수의 생성자가 MFC DLL로
//		호출할 수 있으므로 개체 변수가 선언되기 전에
//		나와야 합니다.
//
//		자세한 내용은
//		MFC Technical Note 33 및 58을 참조하십시오.
//

// CVisionLibraryApp

BEGIN_MESSAGE_MAP(CVisionLibraryApp, CWinApp)
END_MESSAGE_MAP()


// CVisionLibraryApp 생성

CVisionLibraryApp::CVisionLibraryApp()
{
	// TODO: 여기에 생성 코드를 추가합니다.
	// InitInstance에 모든 중요한 초기화 작업을 배치합니다.
}


// 유일한 CVisionLibraryApp 개체입니다.

CVisionLibraryApp theApp;


// CVisionLibraryApp 초기화

BOOL CVisionLibraryApp::InitInstance()
{
	CWinApp::InitInstance();

	return TRUE;
}

extern "C" __declspec(dllexport) void Canny_Edge(cv::Mat* imgPath)
{	
// 	cv::Mat img = * imgPath;
// 	IplImage srcimg = cv::Mat
// 	cvNamedWindow("ParamImage", CV_WINDOW_AUTOSIZE);
// 	cvShowImage("ParamImage", srcImage);
// 	cvWaitKey(0);
}

extern "C" __declspec(dllexport) void Searching_Line(char* imgPath)
{	
	cv::Mat image = cv::imread(imgPath);

	// 캐니 알고리즘 적용
	cv::Mat contours;
	cv::Canny(image, contours, 125, 350);

	LineFinder ld;

	// 확률적 허프변환 파라미터 설정하기
	ld.setLineLengthAndGap(100, 20);
	ld.setMinVote(80);

	std::vector<cv::Vec4i> li = ld.findLines(contours);

	// 선의 첫 번째에 속한 것으로 보이는 점 집합을 추출하기 위해 다음과 같이 진행
	// 검은 영상에 하얀 선을 그린 후 그 선을 감지하기 위해 사용하는 외곽선의 캐니 영상으로 함께 교차
	int n = 0; // 0번째 선 선택
	cv::Mat oneline(image.size(), CV_8U, cv::Scalar(0)); // 검은 영상
	cv::line(oneline, // 하얀 선
		cv::Point(li[n][0], li[n][1]),
		cv::Point(li[n][2], li[n][3]),
		cv::Scalar(255),
		5); //특정 두께 5를 갖는 선을 그림
	cv::bitwise_and(contours, oneline, oneline);
	// 외곽선과 하얀 선 간의 AND
	// 결과는 지정한 선과 관련된 점만 포함

	// 검은 영상과 하얀 선을 반전시킴
	cv::Mat onelineInv;
	cv::threshold(oneline, onelineInv, 128, 255, cv::THRESH_BINARY_INV);
	cv::namedWindow("One line");
	cv::imshow("One line", onelineInv);

	// 이중 반복문으로 집합 내에 있는 점의 좌표를 cv::Points의 std::vector에 삽입
	std::vector<cv::Point> points;
	// 모든 점 위치를 얻기 위환 화소 조회
	for (int y = 0; y < oneline.rows; y++) { // y행
		uchar* rowPtr = oneline.ptr<uchar>(y);
		for (int x = 0; x < oneline.cols; x++) { // x열
			// 외곽선에 있다면
			if (rowPtr[x]) {
				points.push_back(cv::Point(x, y));
			}
		}
	}


	// cv::fitLine을 호출해 가장 적합한 선을 찾음
	cv::Vec4f line;
	cv::fitLine(cv::Mat(points), line,
		CV_DIST_L2, // 거리 유형
		0, // L2 거리를 사용하지 않음
		0.01, 0.01); // 정확도
	// 단위 방향 벡터(cv::Vec4f의 첫 두개 값), 
	// 선에 놓인 한 점의 좌표(cv::Vec4f의 마지막 두 값) 형태인 선 방정식의 파라미터를 제공
	// 마지막 두 파라미터는 선 파라미터에 대한 요구 정확도를 지정
	// 함수에서 요구 하는 std::vector 내에 포함된 입력 점은 cv::Mat로 전달

	// 선 방정식은 일부 속성계산에 사용
	// 올바른 선을 계산하는지 확인하기 위해 영상에 예상 선을 그림
	// 200화소 길이와 3화소 두께를 갖는 임의의 검은 세그먼트를 그림
	int x0 = line[2]; // 선에 놓은 한 점
	int y0 = line[3];
	int x1 = x0 - 200 * line[0]; // 200 길이를 갖는 벡터 추가
	int y1 = y0 - 200 * line[1]; // 단위 벡터 사용
	image = cv::imread(imgPath);
	cv::line(image, cv::Point(x0, y0), cv::Point(x1, y1), cv::Scalar(0), 3);
	cv::namedWindow("Estimated line");
	cv::imshow("Estimated line", image);
	return;
	

	//
	


	//IplImage * image = cvLoadImage(imgPath);
	//cvShowImage("Test Image", image);
	//cvWaitKey(0);
	//cvReleaseImage(&image);
	//return;

}