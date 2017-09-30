// stdafx.cpp : source file that includes just the standard includes
// BackPropNeuralNetworkCPPTests.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"

bool CompareIntArrays(int* arr1, int* arr2)
{
  if (_msize(arr1) == _msize(arr2))
  {
    for (auto i = 0; i < _msize(arr1) / sizeof(int); i++)
      if (arr1[i] != arr2[i])
        return false;
  }
  else
    return false;

  return true;
}
