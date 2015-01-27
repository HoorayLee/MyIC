#include <opencv\cv.h>
#include <opencv\highgui.h>
#include <opencv2\objdetect\objdetect.hpp>
#include "illnessAnalysis.h"

int main(int argc, char **argv)
{
	char *fileName = argc > 1 ? argv[1] : (char *)"2.bmp";
	
	std::vector<int> vec = analyze(fileName);

	return 0;
}