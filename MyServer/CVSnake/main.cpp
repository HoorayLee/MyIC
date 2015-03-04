#include <opencv\cv.h>
#include <opencv\highgui.h>
#include <opencv2\imgproc\imgproc.hpp>

int main(void)
{
	const char *imgWinStr = "original";

	IplImage *oriImg = cvLoadImage("C:\\Users\\Tony\\Desktop\\4.bmp");

	cvNamedWindow(imgWinStr, CV_WINDOW_AUTOSIZE);

	cvShowImage(imgWinStr, oriImg);
	cvWaitKey();

	return 0;
}