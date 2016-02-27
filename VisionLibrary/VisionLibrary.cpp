// VisionLibrary.cpp : �ش� DLL�� �ʱ�ȭ ��ƾ�� �����մϴ�.
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
//TODO: �� DLL�� MFC DLL�� ���� �������� ��ũ�Ǿ� �ִ� ���
//		MFC�� ȣ��Ǵ� �� DLL���� ���������� ��� �Լ���
//		���� �κп� AFX_MANAGE_STATE ��ũ�ΰ�
//		��� �־�� �մϴ�.
//
//		��:
//
//		extern "C" BOOL PASCAL EXPORT ExportedFunction()
//		{
//			AFX_MANAGE_STATE(AfxGetStaticModuleState());
//			// �Ϲ����� �Լ� ������ ���⿡ �ɴϴ�.
//		}
//
//		�� ��ũ�δ� MFC�� ȣ���ϱ� ����
//		�� �Լ��� �ݵ�� ��� �־�� �մϴ�.
//		��, ��ũ�δ� �Լ��� ù ��° ���̾�� �ϸ� 
//		��ü ������ �����ڰ� MFC DLL��
//		ȣ���� �� �����Ƿ� ��ü ������ ����Ǳ� ����
//		���;� �մϴ�.
//
//		�ڼ��� ������
//		MFC Technical Note 33 �� 58�� �����Ͻʽÿ�.
//

// CVisionLibraryApp

BEGIN_MESSAGE_MAP(CVisionLibraryApp, CWinApp)
END_MESSAGE_MAP()


// CVisionLibraryApp ����

CVisionLibraryApp::CVisionLibraryApp()
{
	// TODO: ���⿡ ���� �ڵ带 �߰��մϴ�.
	// InitInstance�� ��� �߿��� �ʱ�ȭ �۾��� ��ġ�մϴ�.
}


// ������ CVisionLibraryApp ��ü�Դϴ�.

CVisionLibraryApp theApp;


// CVisionLibraryApp �ʱ�ȭ

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

	// ĳ�� �˰��� ����
	cv::Mat contours;
	cv::Canny(image, contours, 125, 350);

	LineFinder ld;

	// Ȯ���� ������ȯ �Ķ���� �����ϱ�
	ld.setLineLengthAndGap(100, 20);
	ld.setMinVote(80);

	std::vector<cv::Vec4i> li = ld.findLines(contours);

	// ���� ù ��°�� ���� ������ ���̴� �� ������ �����ϱ� ���� ������ ���� ����
	// ���� ���� �Ͼ� ���� �׸� �� �� ���� �����ϱ� ���� ����ϴ� �ܰ����� ĳ�� �������� �Բ� ����
	int n = 0; // 0��° �� ����
	cv::Mat oneline(image.size(), CV_8U, cv::Scalar(0)); // ���� ����
	cv::line(oneline, // �Ͼ� ��
		cv::Point(li[n][0], li[n][1]),
		cv::Point(li[n][2], li[n][3]),
		cv::Scalar(255),
		5); //Ư�� �β� 5�� ���� ���� �׸�
	cv::bitwise_and(contours, oneline, oneline);
	// �ܰ����� �Ͼ� �� ���� AND
	// ����� ������ ���� ���õ� ���� ����

	// ���� ����� �Ͼ� ���� ������Ŵ
	cv::Mat onelineInv;
	cv::threshold(oneline, onelineInv, 128, 255, cv::THRESH_BINARY_INV);
	cv::namedWindow("One line");
	cv::imshow("One line", onelineInv);

	// ���� �ݺ������� ���� ���� �ִ� ���� ��ǥ�� cv::Points�� std::vector�� ����
	std::vector<cv::Point> points;
	// ��� �� ��ġ�� ��� ��ȯ ȭ�� ��ȸ
	for (int y = 0; y < oneline.rows; y++) { // y��
		uchar* rowPtr = oneline.ptr<uchar>(y);
		for (int x = 0; x < oneline.cols; x++) { // x��
			// �ܰ����� �ִٸ�
			if (rowPtr[x]) {
				points.push_back(cv::Point(x, y));
			}
		}
	}


	// cv::fitLine�� ȣ���� ���� ������ ���� ã��
	cv::Vec4f line;
	cv::fitLine(cv::Mat(points), line,
		CV_DIST_L2, // �Ÿ� ����
		0, // L2 �Ÿ��� ������� ����
		0.01, 0.01); // ��Ȯ��
	// ���� ���� ����(cv::Vec4f�� ù �ΰ� ��), 
	// ���� ���� �� ���� ��ǥ(cv::Vec4f�� ������ �� ��) ������ �� �������� �Ķ���͸� ����
	// ������ �� �Ķ���ʹ� �� �Ķ���Ϳ� ���� �䱸 ��Ȯ���� ����
	// �Լ����� �䱸 �ϴ� std::vector ���� ���Ե� �Է� ���� cv::Mat�� ����

	// �� �������� �Ϻ� �Ӽ���꿡 ���
	// �ùٸ� ���� ����ϴ��� Ȯ���ϱ� ���� ���� ���� ���� �׸�
	// 200ȭ�� ���̿� 3ȭ�� �β��� ���� ������ ���� ���׸�Ʈ�� �׸�
	int x0 = line[2]; // ���� ���� �� ��
	int y0 = line[3];
	int x1 = x0 - 200 * line[0]; // 200 ���̸� ���� ���� �߰�
	int y1 = y0 - 200 * line[1]; // ���� ���� ���
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