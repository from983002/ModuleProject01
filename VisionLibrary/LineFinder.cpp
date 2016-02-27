#include "stdafx.h"
#include <iostream>
#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>

#define PI 3.1415926

class LineFinder {
private:
	cv::Mat img; // �� ����
	std::vector<cv::Vec4i> lines; // ���� �����ϱ� ���� ������ ���� ������ ����
	double deltaRho;
	double deltaTheta; // ����� �ػ� �Ķ����
	int minVote; // ���� ����ϱ� ���� �޾ƾ� �ϴ� �ּ� ��ǥ ����
	double minLength; // ���� ���� �ּ� ����
	double maxGap; // ���� ���� �ִ� ��� ����

public:
	LineFinder() : deltaRho(1), deltaTheta(PI / 180), minVote(10), minLength(0.), maxGap(0.) {}
	// �⺻ ���� �ػ󵵴� 1���� 1ȭ�� 
	// ������ ���� �ּ� ���̵� ����

	// �ش� ���� �޼ҵ��

	// �����⿡ �ػ� ����
	void setAccResolution(double dRho, double dTheta) {
		deltaRho = dRho;
		deltaTheta = dTheta;
	}

	// ��ǥ �ּ� ���� ����
	void setMinVote(int minv) {
		minVote = minv;
	}

	// �� ���̿� ���� ����
	void setLineLengthAndGap(double length, double gap) {
		minLength = length;
		maxGap = gap;
	}

	// ���� �� ���׸�Ʈ ������ �����ϴ� �޼ҵ�
	// Ȯ���� ���� ��ȯ ����
	std::vector<cv::Vec4i> findLines(cv::Mat& binary) {
		lines.clear();
		cv::HoughLinesP(binary, lines, deltaRho, deltaTheta, minVote, minLength, maxGap);
		return lines;
	} // cv::Vec4i ���͸� ��ȯ�ϰ�, ������ �� ���׸�Ʈ�� ���۰� ������ �� ��ǥ�� ����.

	// �� �޼ҵ忡�� ������ ���� ���� �޼ҵ带 ����ؼ� �׸�
	// ���󿡼� ������ ���� �׸���
	void drawDetectedLines(cv::Mat &image, cv::Scalar color = cv::Scalar(255, 255, 255)) {

		// �� �׸���
		std::vector<cv::Vec4i>::const_iterator it2 = lines.begin();

		while (it2 != lines.end()) {
			cv::Point pt1((*it2)[0], (*it2)[1]);
			cv::Point pt2((*it2)[2], (*it2)[3]);
			cv::line(image, pt1, pt2, color);
			++it2;
		}
	}
};
