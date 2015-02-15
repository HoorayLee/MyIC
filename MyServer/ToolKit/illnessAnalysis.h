#ifndef __ILLNESS_ANALYSIS_H__
#define __ILLNESS_ANALYSIS_H__

#include <opencv/cv.h>
#include <opencv/highgui.h>
#include <opencv2/objdetect/objdetect.hpp>

struct OnMouseParam
{
	IplImage *src;
	IplImage *dst;
	IplImage *markers;
	std::vector<int> vec;
};

typedef struct _Illnesses
{
	int nbTeeth;
	int status;
	int *illnesses;
}Illnesses;

//�����ı��
const int UPPERGINGIVA = 20;
const int LOWERGINGIVA = 21;

_declspec(dllexport) Illnesses _stdcall analyze(const char *fileName);
int getSeeds(CvSeq **seeds, IplImage *img, CvMemStorage *storages[]);

const int ILLNESS_DECAY_MASK = 7;
//��������
enum
{
	//ȣ��
	ILLNESS_DECAY_LIGHT = 1,
	ILLNESS_DECAY_MEDIUM = 2,
	ILLNESS_DECAY_SEVERE = 4,
};

//��������
enum
{
	ERR_HAAR_CLASSIFIER_NOT_FOUND = -1,
};

#endif