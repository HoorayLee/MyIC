#include "illnessAnalysis.h"
#include "SingleTooth.h"

std::vector<int> analyze(const char *fileName)
{
	std::vector<int> vec;
	int i, j;
	IplImage *img0 = cvLoadImage(fileName);

	cvNamedWindow("image");
	cvShowImage("image", img0);

	/************************************************************************/
	/* Ԥ����                                                               */
	/************************************************************************/

#pragma region Preprocessing

	//ƽ������
	IplConvKernel *kernel = cvCreateStructuringElementEx(5, 5, 2, 2, CV_SHAPE_ELLIPSE, NULL);
	cvMorphologyEx(img0, img0, NULL, kernel, CV_MOP_OPEN, 1);
	cvMorphologyEx(img0, img0, NULL, kernel, CV_MOP_CLOSE, 1);
	cvReleaseStructuringElement(&kernel);
	cvShowImage("image", img0);
	cvWaitKey();

	//ֱ��ͼ����
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
	/* ʹ��Haar��������Ϊ���ӵ�                                             */
	/************************************************************************/

#pragma region HaarAsSeeds

	//Haar����ʶ��
	CvHaarClassifierCascade *cascade = (CvHaarClassifierCascade *)cvLoad("cascade_tooth.xml");
	if (cascade == NULL)
	{
		printf("Haar Classifier Not Exist!");
		vec.push_back(ERR_HAAR_CLASSIFIER_NOT_FOUND);
	}
	CvMemStorage *storage = cvCreateMemStorage(0);
	cvClearMemStorage(storage);
	CvSeq *seq = cvHaarDetectObjects(img0, cascade, storage, 1.1, 2, CV_HAAR_DO_CANNY_PRUNING, cvSize(30, 30));
	cvReleaseHaarClassifierCascade(&cascade);

	//��ʶ��ľ��ο�x����ѡ������
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

	//ʶ����ͼƬ��ʼ��
	IplImage *haarImg = cvCloneImage(img0);

	//�ж��������Ļ�׼�ߣ�ͼƬˮƽ�����ߣ�
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
				CV_MAT_ELEM(*pointsUpper, int, nbSeedUpper, 0) = (int)(rect->x + 0.25f * rect->width);
				CV_MAT_ELEM(*pointsUpper, int, nbSeedUpper, 1) = (int)(rect->y + 0.5f * rect->height);
				nbSeedUpper++;
			}

			CV_MAT_ELEM(*pointsUpper, int, nbSeedUpper, 0) = (int)(rect->x + 0.75f * rect->width);
			CV_MAT_ELEM(*pointsUpper, int, nbSeedUpper, 1) = (int)(rect->y + 0.5f * rect->height);
			nbSeedUpper++;

			pUpperRect = rect;
		}
		else
		{
			if (pLowerRect == NULL || (pLowerRect->x + pLowerRect->width - rect->x) <= (0.25 * MIN(pLowerRect->width, rect->width)))
			{
				CV_MAT_ELEM(*pointsLower, int, nbSeedLower, 0) = (int)(rect->x + 0.25f * rect->width);
				CV_MAT_ELEM(*pointsLower, int, nbSeedLower, 1) = (int)(rect->y + 0.5f * rect->height);
				nbSeedLower++;
			}

			CV_MAT_ELEM(*pointsLower, int, nbSeedLower, 0) = (int)(rect->x + 0.75f * rect->width);
			CV_MAT_ELEM(*pointsLower, int, nbSeedLower, 1) = (int)(rect->y + 0.5f * rect->height);
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
	//���������ӵ�
	CvPoint upperPoint = cvPoint(haarImg->width / 2, ymin);
	//���������ӵ�
	CvPoint lowerPoint = cvPoint(haarImg->width / 2, ymax);

	//storage�д��Haar������������õĽ����֮ǰ�����ͷ�

	cvNamedWindow("haar");
	cvShowImage("haar", haarImg);
	cvReleaseImage(&haarImg);
	cvWaitKey();

#pragma endregion

	/************************************************************************/
	/* ��ʼ�ָ�                                                             */
	/************************************************************************/

#pragma region Segmentation
	//һ��������״̬��0
	vec.push_back(0);

	//���ӵ�ͼ���ʼ��
	IplImage *markers = cvCreateImage(cvGetSize(img0), IPL_DEPTH_32S, 1);
	cvZero(markers);

	//���������ӵ�
	for (int i = 0; i < nbSeedUpper; i++)
	{
		int x = CV_MAT_ELEM(*pointsUpper, int, i, 0);
		int y = CV_MAT_ELEM(*pointsUpper, int, i, 1);
		cvDrawCircle(markers, cvPoint(x, y), 10, cvScalarAll(i + 1));
	}

	//���������ӵ�
	for (int i = 0; i < nbSeedLower; i++)
	{
		int x = CV_MAT_ELEM(*pointsLower, int, i, 0);
		int y = CV_MAT_ELEM(*pointsLower, int, i, 1);
		cvDrawCircle(markers, cvPoint(x, y), 10, cvScalarAll(nbSeedUpper + i + 1));
	}

	//��������
	cvDrawCircle(markers, upperPoint, 10, cvScalarAll(UPPERGINGIVA));
	//cvDrawCircle(markers, lowerPoint, 10, cvScalarAll(LOWERGINGIVA));
	//cvDrawCircle(markers, cvPoint(markers->width / 2, ymin / 2), ymin / 2, cvScalarAll(20));
	//cvDrawCircle(markers, cvPoint(markers->width / 2, (ymax + markers->height) / 2), (markers->height - ymax) / 2, cvScalarAll(21));


	/*IplImage *markers_bi = cvCloneImage(markers);
	markers_bi->depth = 8;
	cvThreshold(markers_bi, markers_bi, 1, 255, CV_THRESH_BINARY);
	cvNamedWindow("markers");
	cvShowImage("markers", markers_bi);
	cvWaitKey();
	cvReleaseImage(&markers_bi);*/

	//��ˮ���㷨
	cvWatershed(img0, markers);


#pragma region Depart
	//�з�ճ�ϵ�����
	CvMemStorage *tempStorage = cvCreateMemStorage();
	CvSeq *contours = NULL;
	int nbComp = cvFindContours(cvCloneImage(markers), tempStorage, &contours, 88, CV_RETR_CCOMP, CV_CHAIN_APPROX_SIMPLE);
	assert(nbComp != 0);

	//Ѱ��ĳ������ˮƽ�������Ϻ�������n�������м�һ�����������
	const int gap = 55;	//����������
	int nbTeeth = nbSeedLower + nbSeedUpper;

	for (CvSeq *contour = contours; contour != NULL; contour = contour->h_next)
	{
		//!��Ϊ������Χ�ɵ��������100000��Ϊ������Ȼ������
		if (cvContourArea(contour) > 100000)
		{
			continue;
		}

		CvRect rect = cvBoundingRect(contour);

		int medianLine = (int)(rect.y + rect.height * 0.5f);

		for (int i = 0; i < contour->total - gap + 1; i++)
		{
			//���е�n����Ķ���������������
			int j;
			for (j = 0; j < gap; j++)
			{
				if (CV_GET_SEQ_ELEM(CvPoint, contour, i + j)->y > medianLine) 
				{
					break;
				}
			}
			if (j != gap)
			{
				continue;
			}
			

			//�õ��м�ĵ������
			CvPoint *midPoint = CV_GET_SEQ_ELEM(CvPoint, contour, i + gap / 2);

			//�����ߵĵ��������жԱ�
			for (j = 0; j < gap; j++)
			{
				CvPoint *curPoint = CV_GET_SEQ_ELEM(CvPoint, contour, i + j);
				if (curPoint->y > midPoint->y)
				{
					break;
				}
			}
			if (j != gap)
			{
				continue;
			}

			//�м�ĵ�ȷʵ��Ӧ�÷ָ����ʼ��
			int x0 = midPoint->x;
			int y0 = midPoint->y;
			int y1, y2;
			while (CV_IMAGE_ELEM(markers, int, y0, x0) == -1)
			{
				y0++;
			}
			y1 = y0;
		
			while (CV_IMAGE_ELEM(markers, int, y0, x0) != -1)
			{
				CV_IMAGE_ELEM(markers, int, y0, x0) = -1;
			}
			y2 = y0;

			//����ߵ�������������Ϊ++nbTeeth
			int newVal = ++nbTeeth;
			int oldVal = CV_IMAGE_ELEM(markers, int, (y1 + y2) / 2, x0 + 1);

			for (int m = 0; m < markers->height; m++)
			{
				for (int n = 0; n < x0; n++)
				{
					if (CV_IMAGE_ELEM(markers, int, m, n) == oldVal)
					{
						CV_IMAGE_ELEM(markers, int, m, n) = newVal;
					}
				}
			}

			i += gap;
			continue;
		}
	}

	cvSave("markers.xml", markers);
	cvReleaseMemStorage(&tempStorage);
#pragma endregion

#ifdef DEBUG
	cvClearMemStorage(storage);
	CvSeq *contours;

	cvFindContours(cvCloneImage(markers), storage, &contours, 88, CV_RETR_CCOMP, CV_CHAIN_APPROX_SIMPLE);

	IplImage *tempImg = cvCreateImage(cvGetSize(img0), IPL_DEPTH_8U, 3);
	cvZero(tempImg);

	for (; contours != NULL; contours = contours->h_next)
	{
		cvDrawContours(tempImg, contours, cvScalarAll(255), cvScalarAll(0), 3);
	}

	cvNamedWindow("temp");
	cvShowImage("temp", tempImg);
	cvWaitKey();
#endif // DEBUG

	IplImage *img = cvCloneImage(img0);

	//Ϊ����cvGetElemType�õ��������
	//CvSize s = cvGetSize(img0);
	//int elemType = cvGetElemType(img0);
	//IplImage *img = cvCreateImage(cvGetSize(img0), cvGetElemType(img0), 3);

	//��ʼ�����������
	CvRNG rng = cvRNG(time(NULL));

	//��ʼ����ɫ����
	CvMat *colors = cvCreateMat(1, nbTeeth, CV_8UC3);
	for (i = 0; i < nbTeeth; i++)
	{
		uchar *ptr = colors->data.ptr + i * 3;
		ptr[0] = (uchar)(cvRandInt(&rng) % 200 + 30);
		ptr[1] = (uchar)(cvRandInt(&rng) % 200 + 30);
		ptr[2] = (uchar)(cvRandInt(&rng) % 200 + 30);
	}

	//������ɫ
	for (i = 0; i < markers->height; i++)
	{
		for (j = 0; j < markers->width; j++)
		{
			int cur = CV_IMAGE_ELEM(markers, int, i, j);
			uchar *ptr = &CV_IMAGE_ELEM(img, uchar, i, j * 3);

			if (cur == -1)	//�߽�
			{
				ptr[0] = ptr[1] = ptr[2] = (uchar)255;
			}
			else
			{
				uchar *pColor = colors->data.ptr + (cur - 1) * 3;
				
				ptr[0] = pColor[0];
				ptr[1] = pColor[1];
				ptr[2] = pColor[2];
			}
		}
	}

	cvNamedWindow("after");
	cvShowImage("after", img);
	cvWaitKey();

#pragma endregion

	/************************************************************************/
	/* ÿ�����ݵ����ٳ����з���                                             */
	/************************************************************************/

#pragma region SingleTooth

	for (int iTooth = 1; iTooth <= nbTeeth; iTooth++)
	{
		int result = detectIllness(img0, markers, iTooth);
		vec.push_back(result);
		//IplImage *mask = cvCloneImage(img0);
		//uchar *pMask = &CV_IMAGE_ELEM(mask, uchar, 0, 0);
		//int *pMarkers = &CV_IMAGE_ELEM(markers, int, 0, 0);
		//for (i = 0; i < img0->width * img0->height; i++)
		//{
		//	if (pMarkers[i] == iTooth)
		//	{
		//		pMask[3 * i] = pMask[3 * i + 1] = pMask[3 * i + 2] = (uchar)255;
		//	}
		//	else
		//	{
		//		pMask[3 * i] = pMask[3 * i + 1] = pMask[3 * i + 2] = 0;
		//	}
		//}

		//cvNamedWindow("mask");
		//cvShowImage("mask", mask);
		//cvWaitKey();

		////��Եƽ��
		//IplConvKernel *elem = cvCreateStructuringElementEx(11, 11, 5, 5, CV_SHAPE_ELLIPSE);
		//cvMorphologyEx(mask, mask, NULL, elem, CV_MOP_OPEN);
		//cvMorphologyEx(mask, mask, NULL, elem, CV_MOP_CLOSE);
		//cvReleaseStructuringElement(&elem);
		//cvShowImage("mask", mask);
		//cvWaitKey();

		//IplImage *imgTooth = cvCloneImage(img0);
		//cvAnd(img0, mask, imgTooth);

		//cvNamedWindow("tooth");
		//cvShowImage("tooth", imgTooth);
		//cvWaitKey();
	}

#pragma endregion

	/************************************************************************/
	/* ���ӻ�����ʾ������                                                   */
	/************************************************************************/
	IplImage *showIllnessInPic(IplImage *img0, IplImage *img, std::vector<int> vec);
	IplImage *illnessImg = showIllnessInPic(img0, img, vec);	
	cvNamedWindow("Illness", CV_WINDOW_AUTOSIZE);
	cvShowImage("Illness", illnessImg);

	OnMouseParam param;
	param.src = illnessImg;
	param.dst = cvCloneImage(illnessImg);
	param.markers = markers;
	param.vec = vec;

	void on_mouse(int event, int x, int y, int flags, void *param);
	cvSetMouseCallback("Illness", on_mouse, &param);
	cvWaitKey();

	cvReleaseImage(&param.dst);
	cvReleaseImage(&illnessImg);

	/************************************************************************/
	/* �ͷ���Դ                                                             */
	/************************************************************************/

	//���ӵ�����
	cvReleaseMat(&pointsUpper);
	cvReleaseMat(&pointsLower);

	cvReleaseImage(&img);
	cvReleaseImage(&img0);
	cvReleaseImage(&markers);
	cvReleaseMemStorage(&storage);
	return vec;
}


//ͨ����̬ѧUltimate ErosionѰ��һ��ͼ���е����ӵ�
int getSeeds(CvSeq **seeds, IplImage *img, CvMemStorage *storages[])
{
	IplImage *src = cvCloneImage(img);
	int oldNbContour, nbContour;
	CvSeq *oldContours, *contours;
	CvSeq *contour, *oldContour;
	storages[0] = cvCreateMemStorage();
	storages[1] = cvCreateMemStorage();
	storages[2] = cvCreateMemStorage();
	IplConvKernel *kernel = cvCreateStructuringElementEx(5, 5, 2, 2, CV_SHAPE_ELLIPSE);
	int iteration;
	CvRNG rng = cvRNG();

	//Ultimate ErosionѰ�����ӵ�
	//˼·����ǰһ�ε�Contour�������ٵ�ʱ�򣬼��ǰһ�ε�Contour��ʵ��ֻ��Ҫһ����͹��ˣ��Ƿ��ڰ���ĳ����һ�ε�Contour
	//������������ǰһ�ε�Contour�����ļ�Ϊһ�����ӵ�
	cvFindContours(cvCloneImage(src), storages[0], &contours, 88, CV_RETR_CCOMP, CV_CHAIN_APPROX_SIMPLE);

#ifdef DEBUG
	cvCvtColor(src, contourImg, CV_GRAY2BGR);
	for (nbContour = 0, contour = contours; contour != NULL; contour = contour->h_next, nbContour++)
		cvDrawContours(contourImg, contour, cvScalar(cvRandInt(&rng) % 256, cvRandInt(&rng) % 256, cvRandInt(&rng) % 256),
		cvScalarAll(0), 3, 3);
	cvShowImage("contour", contourImg);
#else
	for (nbContour = 0, contour = contours; contour != NULL; contour = contour->h_next, nbContour++);
#endif // DEBUG

	*seeds = cvCreateSeq(CV_SEQ_ELTYPE_POINT, sizeof(CvSeq), sizeof(CvPoint), storages[2]);

	iteration = 1;
	while (nbContour != 0)
	{
		oldNbContour = nbContour;
		oldContours = contours;

		cvErode(src, src, kernel, 1);

		cvFindContours(cvCloneImage(src), storages[iteration % 2], &contours, 88, CV_RETR_CCOMP, CV_CHAIN_APPROX_SIMPLE);

#ifdef DEBUG
		cvCvtColor(src, contourImg, CV_GRAY2BGR);
		for (nbContour = 0, contour = contours; contour != NULL; contour = contour->h_next, nbContour++)
			cvDrawContours(contourImg, contour, cvScalar(cvRandInt(&rng) % 256, cvRandInt(&rng) % 256, cvRandInt(&rng) % 256),
			cvScalarAll(0), 3, 3);
		cvShowImage("contour", contourImg);
#else
		for (nbContour = 0, contour = contours; contour != NULL; contour = contour->h_next, nbContour++);
#endif // DEBUG

		//���ĳ���򱻸�ʴ����ʧ
		if (nbContour < oldNbContour)
		{
			bool found;
			//����
			for (oldContour = oldContours; oldContour != NULL; oldContour = oldContour->h_next)
			{
				found = false;
				CvMat *polygon = cvCreateMat(1, oldContour->total, CV_32FC2);
				float *pData = (float *)(polygon->data.ptr);
				for (int i = 0; i < oldContour->total; i++)
				{
					CvPoint *point = CV_SEQ_ELEM(oldContour, CvPoint, i);
					*pData++ = (float)point->x;
					*pData++ = (float)point->y;
				}

				for (contour = contours; contour != NULL; contour = contour->h_next)
				{
					CvPoint *point = CV_GET_SEQ_ELEM(CvPoint, contour, 0);
					if (cvPointPolygonTest(polygon, cvPoint2D32f(point->x, point->y), 1) > 0)
					{
						found = true;
						break;
					}
				}

				//ȷʵΪ��ʧ�����ӵ�ΪoldContour������
				if (!found)
				{
					CvRect rect = cvBoundingRect(polygon, 1);
					CvPoint center = cvPoint((int)(rect.x + 0.5f * rect.width), (int)(rect.y + 0.5f * rect.height));
					cvSeqPush(*seeds, &center);
				}
				cvReleaseMat(&polygon);
			}
		}

#ifdef DEBUG
		cvShowImage("image", src);
		printf("Number of Components: %d\n", nbContour);
		cvWaitKey();
#endif // DEBUG

		oldNbContour = nbContour;
		iteration++;
	}

	//��ʮ�ֿ��������ӵ�ֻ����һ��
	for (int i = (*seeds)->total - 1; i >= 0; i--)
	{
		CvPoint *point1 = CV_GET_SEQ_ELEM(CvPoint, *seeds, i);
		for (int j = i - 1; j >= 0; j--)
		{
			CvPoint *point2 = CV_GET_SEQ_ELEM(CvPoint, *seeds, j);
			float x1 = (float)point1->x; float y1 = (float)point1->y;
			float x2 = (float)point2->x; float y2 = (float)point2->y;
			if (((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)) < 5 * 5)
			{
				cvSeqRemove(*seeds, i);
			}
		}
	}

	cvReleaseStructuringElement(&kernel);


	return (*seeds)->total;
}


//��ͼƬ����ʽ��ʾ������������
IplImage *showIllnessInPic(IplImage *img0, IplImage *img, std::vector<int> vec)
{
	IplImage *illnessImg = cvCreateImage(cvGetSize(img), IPL_DEPTH_8U, 3);
	cvAddWeighted(img0, 0.5, img, 0.5, 0, illnessImg);
	return illnessImg;
}

void on_mouse(int event, int x, int y, int flags, void* param)
{
	OnMouseParam *mouseParam = (OnMouseParam *)param;
	IplImage *dst = mouseParam->dst;
	int toothIndex;
	int result;
	switch (event)
	{
	case CV_EVENT_MOUSEMOVE:
		cvCopyImage(mouseParam->src, dst);

		toothIndex = CV_IMAGE_ELEM(mouseParam->markers, int, y, x);
		if (toothIndex > mouseParam->vec.size())
		{
			return;
		}

		result = mouseParam->vec[toothIndex];

		char posStr[20], toothNoStr[20], illnessStr[40];
		sprintf(posStr, "x:%d y:%d", x, y);
		sprintf(toothNoStr, "Tooth No: %d", toothIndex);
		if ((result & ILLNESS_DECAY_MASK) == 0)
		{
			strcpy(illnessStr, "No Decayed Tooth");
		}
		if ((result & ILLNESS_DECAY_LIGHT) != 0)
		{
			strcpy(illnessStr, "Light");
		}
		else if ((result & ILLNESS_DECAY_MEDIUM) != 0)
		{
			strcpy(illnessStr, "Medium");
		}
		else if ((result & ILLNESS_DECAY_SEVERE) != 0)
		{
			strcpy(illnessStr, "Severe");
		}

		cvPutText(dst, posStr, cvPoint(10, 20), &cvFont(1, 1), cvScalarAll(255));
		cvPutText(dst, toothNoStr, cvPoint(10, 40), &cvFont(1, 1), cvScalarAll(255));
		cvPutText(dst, illnessStr, cvPoint(10, 60), &cvFont(1, 1), cvScalarAll(255));

		cvShowImage("Illness", dst);
		break;
	default:
		break;
	}
}
