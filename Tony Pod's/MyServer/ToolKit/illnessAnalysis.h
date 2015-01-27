#ifndef __ILLNESS_ANALYSIS_H__
#define __ILLNESS_ANALYSIS_H__

const int UPPERGINGIVA = 20;
const int LOWERGINGIVA = 21;

_declspec(dllexport) int _stdcall analyze(const char *fileName);

_declspec(dllexport) int Double(int num);

enum
{
	ILLNESS_DECAY = 1,
};

#endif