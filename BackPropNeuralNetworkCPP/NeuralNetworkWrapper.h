#include "NeuralNetwork.h"
//#include "NeuralNetwork.cpp"

using namespace System;

namespace Wrapper
{
  public ref class NeuralNetworkWrapper
  {
  public:
    NeuralNetworkWrapper(int* layer, const int size);
    void TrainNetwork(double* trainingData, double* expectedData, int dataSetSize, int iterations, bool silent);
    void FeedForward(double* inputs, double** retVal);
  
    NeuralNetwork* pNN;

  private:
    void ConvertToVectorDouble(double* input, int* rows, int* columns, std::vector<double*> *output);
  };
}


