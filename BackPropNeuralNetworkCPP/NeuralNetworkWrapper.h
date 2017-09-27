#include "NeuralNetwork.h"
//#include "NeuralNetwork.cpp"

using namespace System;

namespace Wrapper
{
  public ref class NeuralNetworkWrapper
  {
  public:
    NeuralNetworkWrapper(int* layer, const int size);
    void TrainNetwork(int iterations, bool silent);
    void FeedForward(double* inputs, double** retVal);
  
    NeuralNetwork* pNN;
  };
}


