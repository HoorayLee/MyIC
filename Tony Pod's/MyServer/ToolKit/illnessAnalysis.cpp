#include "illnessAnalysis.h"
#include <opencv/cv.h>
#include <opencv/highgui.h>
#include <opencv2/objdetect/objdetect.hpp>


_declspec(dllexport) int _stdcall analyze(const char *fileName)
{
	int i, j;

	IplImage *img0 = cvLoadImage(fileName);
	//检查文件是否存在
	if (img0 == NULL)
	{
		return -1;
	}

	//cvNamedWindow("image");
	//cvShowImage("image", img0);

	/************************************************************************/
	/* 预处理                                                               */
	/************************************************************************/

#pragma region Preprocessing

	//平滑处理
	IplConvKernel *kernel = cvCreateStructuringElementEx(5, 5, 2, 2, CV_SHAPE_ELLIPSE, NULL);
	cvMorphologyEx(img0, img0, NULL, kernel, CV_MOP_OPEN, 1);
	cvMorphologyEx(img0, img0, NULL, kernel, CV_MOP_CLOSE, 1);
	cvReleaseStructuringElement(&kernel);
	//cvShowImage("image", img0);
	//cvWaitKey();

	//直方图均衡
	/*IplImage *b, *g, *r;
	b = cvCreateImage(cvGetSize(img0), IPL_DEPTH_8U, 1);
	g = cvCreateImage(cvGetSize(img0), IPL_DEPTH_8U, 1);
	r = cvCreateImage(cvGetSize(img0), IPL_DEPTH_8U, 1);
	cvSplit(img0, b, g, r, NULL);
	cvEqualizeHist(b, b);
	cvEqualizeHist(g, g);
	cvEqualizeHist(r, r);
	cvMerge(b, g, r, NULL, img0);
	cvShowImage("image", img0);
	cvWaitKey();*/

#pragma endregion

	/************************************************************************/
	/* 使用Haar分类器作为种子点                                             */
	/************************************************************************/
	
#pragma region HaarAsSeeds

	//Haar牙齿识别
	CvHaarClassifierCascade *cascade = (CvHaarClassifierCascade *)cvLoad("cascade_tooth.xml");
	if (cascade == NULL)
	{
		printf("Haar Classifier Not Exist!");
		return -1;
	}
	CvMemStorage *storage = cvCreateMemStorage(0);
	cvClearMemStorage(storage);
	CvSeq *seq = cvHaarDetectObjects(img0, cascade, storage, 1.1, 2, CV_HAAR_DO_CANNY_PRUNING, cvSize(30, 30));
	cvReleaseHaarClassifierCascade(&cascade);

	//将识别的矩形框按x坐标选择排序
	CvRect *xminRect = NULL;
	for (i = 0; i < seq->total; i++)
	{
		CvRect *r1 = (CvRect *)cvGetSeqElem(seq, i);
		xminRect = r1;
		for (j = i + 1; j < seq->total; j++)
		{
			CvRect *r2 = (CvRect *)cvGetSeqElem(seq, j);
			xminRect = xminRect->x < r2->x ? xminRect : r2;
		}

		CvRect rTemp = CvRect(*r1);
		*r1 = *xminRect;
		*xminRect = rTemp;
	}

	//识别结果图片初始化
	IplImage *haarImg = cvCloneImage(img0);

	//判断上下牙的基准线（图片水平中轴线）
	int yBaseline = haarImg->height / 2;

	CvMat *pointsUpper = cvCreateMat(seq->total * 2, 2, CV_32SC1);
	CvMat *pointsLower = cvCreateMat(seq->total * 2, 2, CV_32SC1);
	CvRect *pUpperRect = NULL;
	CvRect *pLowerRect = NULL;
	int nbSeedUpper = 0, nbSeedLower = 0;
	int *pos = NULL;
	for (i = 0; i < seq->total; i++)
	{
		CvRect *rect = (CvRect *)cvGetSeqElem(seq, i);
		cvDrawRect(haarImg, cvPoint(rect->x, rect->y), cvPoint(rect->x + rect->width, rect->y + rect->height), cvScalar(0, 0, 255));

		if (rect->y + 0.5 * rect->height < yBaseline)
		{
			if (pUpperRect == NULL || (pUpperRect->x + pUpperRect->width - rect->x) <= (0.25 * MIN(pUpperRect->width, rect->width)))
			{
				CV_MAT_ELEM(*pointsUpper, int, nbSeedUpper, 0) = rect->x + 0.25 * rect->width;
				CV_MAT_ELEM(*pointsUpper, int, nbSeedUpper, 1) = rect->y + 0.5 * rect->height;
				nbSeedUpper++;
			}

			CV_MAT_ELEM(*pointsUpper, int, nbSeedUpper, 0) = rect->x + 0.75 * rect->width;
			CV_MAT_ELEM(*pointsUpper, int, nbSeedUpper, 1) = rect->y + 0.5 * rect->height;
			nbSeedUpper++;

			pUpperRect = rect;
		}
		else
		{
			if (pLowerRect == NULL || (pLowerRect->x + pLowerRect->width - rect->x) <= (0.25 * MIN(pLowerRect->width, rect->width)))
			{
				CV_MAT_ELEM(*pointsLower, int, nbSeedLower, 0) = rect->x + 0.25 * rect->width;
				CV_MAT_ELEM(*pointsLower, int, nbSeedLower, 1) = rect->y + 0.5 * rect->height;
				nbSeedLower++;
			}

			CV_MAT_ELEM(*pointsLower, int, nbSeedLower, 0) = rect->x + 0.75 * rect->width;
			CV_MAT_ELEM(*pointsLower, int, nbSeedLower, 1) = rect->y + 0.5 * rect->height;
			nbSeedLower++;

			pLowerRect = rect;
		}
	}

	int ymin = 0;
	int ymax = haarImg->height - 1;
	for (i = 0; i < seq->total; i++)
	{
		CvRect *rect = (CvRect *)cvGetSeqElem(seq, i);
		ymin = MIN(ymin, rect->y);
		ymax = MAX(ymax, rect->y + rect->height);
	}
	//上牙龈种子点
	CvPoint upperPoint = cvPoint(haarImg->width / 2, ymin);
	//下牙龈种子点
	CvPoint lowerPoint = cvPoint(haarImg->width / 2, ymax);

	//storage中存放Haar级联分类器获得的结果，之前不能释放
	cvReleaseMemStorage(&storage);

	//cvNamedWindow("haar");
	//cvShowImage("haar", haarImg);
	cvReleaseImage(&haarImg);
	//cvWaitKey();

#pragma endregion

	/************************************************************************/
	/* 开始分割                                                             */
	/************************************************************************/

#pragma region Segmentation
	//种子点图像初始化
	IplImage *markers = cvCreateImage(cvGetSize(img0), IPL_DEPTH_32S, 1);
	cvZero(markers);

	//上牙齿种子点
	for (int i = 0; i < nbSeedUpper; i++)
	{
		int x = CV_MAT_ELEM(*pointsUpper, int, i, 0);
		int y = CV_MAT_ELEM(*pointsUpper, int, i, 1);
		cvDrawCircle(markers, cvPoint(x, y), 10, cvScalarAll(i + 1));
	}
	cvReleaseMat(&pointsUpper);

	//下牙齿种子点
	for (int i = 0; i < nbSeedLower; i++)
	{
		int x = CV_MAT_ELEM(*pointsLower, int, i, 0);
		int y = CV_MAT_ELEM(*pointsLower, int, i, 1);
		cvDrawCircle(markers, cvPoint(x, y), 10, cvScalarAll(nbSeedUpper + i + 1));
	}
	cvReleaseMat(&pointsLower);

	//上下牙龈
	cvDrawCircle(markers, upperPoint, 10, cvScalarAll(UPPERGINGIVA));
	cvDrawCircle(markers, lowerPoint, 10, cvScalarAll(LOWERGINGIVA));
	//cvDrawCircle(markers, cvPoint(markers->width / 2, ymin / 2), ymin / 2, cvScalarAll(20));
	//cvDrawCircle(markers, cvPoint(markers->width / 2, (ymax + markers->height) / 2), (markers->height - ymax) / 2, cvScalarAll(21));


	/*IplImage *markers_bi = cvCloneImage(markers);
	markers_bi->depth = 8;
	cvThreshold(markers_bi, markers_bi, 1, 255, CV_THRESH_BINARY);
	cvNamedWindow("markers");
	cvShowImage("markers", markers_bi);
	cvWaitKey();
	cvReleaseImage(&markers_bi);*/

	//分水岭算法
	cvWatershed(img0, markers);
	////cvSave("markers.xml", markers);

	//IplImage *img = cvCloneImage(img0);

	////为何用cvGetElemType得到结果不对
	////CvSize s = cvGetSize(img0);
	////int elemType = cvGetElemType(img0);
	////IplImage *img = cvCreateImage(cvGetSize(img0), cvGetElemType(img0), 3);

	////初始化随机数种子
	//CvRNG rng = cvRNG(time(NULL));

	////初始化颜色矩阵
	//int nbTeeth = nbSeedUpper + nbSeedLower;
	//CvMat *colors = cvCreateMat(nbTeeth, 3, CV_8UC3);
	//for (i = 0; i < nbTeeth; i++)
	//{
	//	uchar *ptr = colors->data.ptr + i * 3;
	//	ptr[0] = (uchar)(cvRandInt(&rng) % 200 + 30);
	//	ptr[1] = (uchar)(cvRandInt(&rng) % 200 + 30);
	//	ptr[2] = (uchar)(cvRandInt(&rng) % 200 + 30);
	//}

	////设置颜色
	//for (i = 0; i < markers->height; i++)
	//{
	//	for (j = 0; j < markers->width; j++)
	//	{
	//		int cur = CV_IMAGE_ELEM(markers, int, i, j);
	//		uchar *ptr = &CV_IMAGE_ELEM(img, uchar, i, j * 3);
	//		if (cur == -1)
	//		{
	//			ptr[0] = ptr[1] = ptr[2] = (uchar)255;
	//		}
	//		else
	//		{
	//			uchar *pColor = colors->data.ptr + (cur - 1) * 3;
	//			ptr[0] = pColor[0];
	//			ptr[1] = pColor[1];
	//			ptr[2] = pColor[2];
	//		}
	//	}
	//}

	////cvNamedWindow("after");
	////cvShowImage("after", img);
	////cvWaitKey();
	//cvReleaseImage(&img);

#pragma endregion

	/************************************************************************/
	/* 抠出单独牙齿                                                         */
	/************************************************************************/

#pragma region Mask

	IplImage *mask = cvCloneImage(img0);
	uchar *pMask = &CV_IMAGE_ELEM(mask, uchar, 0, 0);
	int *pMarkers = &CV_IMAGE_ELEM(markers, int, 0, 0);
	for (i = 0; i < img0->width * img0->height; i++)
	{
		//提取3号牙齿
		if (pMarkers[i] == 3)
		{
			pMask[3 * i] = pMask[3 * i + 1] = pMask[3 * i + 2] = (uchar)255;
		}
		else
		{
			pMask[3 * i] = pMask[3 * i + 1] = pMask[3 * i + 2] = 0;
		}
	}

	//cvNamedWindow("mask");
	//cvShowImage("mask", mask);
	//cvWaitKey();

	IplConvKernel *elem = cvCreateStructuringElementEx(11, 11, 5, 5, CV_SHAPE_ELLIPSE);
	cvMorphologyEx(mask, mask, NULL, elem, CV_MOP_OPEN);
	cvMorphologyEx(mask, mask, NULL, elem, CV_MOP_CLOSE);
	cvReleaseStructuringElement(&elem);
	//cvShowImage("mask", mask);
	//cvWaitKey();

	//IplImage *imgTooth = cvCloneImage(img0);
	//uchar *pImgTooth = &CV_IMAGE_ELEM(imgTooth, uchar, 0, 0);

	IplImage *imgTooth = cvCloneImage(img0);
	cvZero(imgTooth);
	cvAnd(img0, mask, imgTooth);

	//cvNamedWindow("tooth");
	//cvShowImage("tooth", imgTooth);
	//cvWaitKey();

#pragma endregion

	/************************************************************************/
	/* 单独牙齿疾病分析                                                     */
	/************************************************************************/

#pragma region Analysis


	IplImage *b, *g, *r;
	b = cvCreateImage(cvGetSize(img0), IPL_DEPTH_8U, 1);
	g = cvCreateImage(cvGetSize(img0), IPL_DEPTH_8U, 1);
	r = cvCreateImage(cvGetSize(img0), IPL_DEPTH_8U, 1);
	cvSplit(imgTooth, b, g, r, NULL);

	int dim = 1;
	int sizes = 256;
	float range[] = { 0, 255 };
	float *ranges[] = { range };
	CvHistogram *histR = cvCreateHist(dim, &sizes, CV_HIST_ARRAY, ranges, 1);
	CvHistogram *histG = cvCreateHist(dim, &sizes, CV_HIST_ARRAY, ranges, 1);
	CvHistogram *histB = cvCreateHist(dim, &sizes, CV_HIST_ARRAY, ranges, 1);

	cvCalcHist(&r, histR);
	cvCalcHist(&g, histG);
	cvCalcHist(&b, histB);

	IplImage *imgHist = cvCreateImage(cvSize(256, 300), 8, 3);
	for (int i = 1; i < sizes; i++)
	{
		cvLine(imgHist,
			cvPoint(i - 1, imgHist->height - (int)(cvQueryHistValue_1D(histR, i - 1) / 10)),
			cvPoint(i, imgHist->height - (int)(cvQueryHistValue_1D(histR, i) / 10)),
			CV_RGB(255, 0, 0));
		cvLine(imgHist,
			cvPoint(i - 1, imgHist->height - (int)(cvQueryHistValue_1D(histG, i - 1) / 10)),
			cvPoint(i, imgHist->height - (int)(cvQueryHistValue_1D(histG, i) / 10)),
			CV_RGB(0, 255, 0));
		cvLine(imgHist,
			cvPoint(i - 1, imgHist->height - (int)(cvQueryHistValue_1D(histB, i - 1) / 10)),
			cvPoint(i, imgHist->height - (int)(cvQueryHistValue_1D(histB, i) / 10)),
			CV_RGB(0, 0, 255));
	}

	//cvNamedWindow("hist");
	//cvShowImage("hist", imgHist);
	//cvWaitKey();

	cvReleaseHist(&histB);
	cvReleaseHist(&histG);
	cvReleaseHist(&histR);
	cvReleaseImage(&b);
	cvReleaseImage(&g);
	cvReleaseImage(&r);

#pragma endregion

	/************************************************************************/
	/* 释放资源                                                             */
	/************************************************************************/

	cvReleaseImage(&imgTooth);
	cvReleaseImage(&img0);
	cvReleaseImage(&markers);

	//return 1 << (cvRandInt(&rng) % 32);
	CvRNG rng = cvRNG(-1);
	return (1 << (cvRandInt(&rng) % 31));
}