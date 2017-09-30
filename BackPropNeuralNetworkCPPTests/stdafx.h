// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

// Headers for CppUnitTest
#include "CppUnitTest.h"

#include <vector>

std::vector<double> ToVectorDouble(double * input);
std::vector<int> ToVectorInt(int * input);
bool CompareIntArrays(int* arr1, int* arr2);