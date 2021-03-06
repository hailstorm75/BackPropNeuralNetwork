#include "NeuralNetwork.h"
#include <fstream>
#include <sstream>

using namespace System;

namespace Wrapper
{
  public ref class NeuralNetworkWrapper
  {
  public:
    enum class ExportType
    {
      CSV = 1,
    };

    NeuralNetworkWrapper(int* layer, const int size);
    void Clear();
    void TrainNetwork(double* trainingData, double* expectedData, int dataSetSize, int iterations);
    void FeedForward(double* inputs, double** retVal);
    void ExportNeuralNetwork(ExportType type);
  
    NeuralNetwork* pNN;

  private:
    std::vector<double*> ConvertToVectorDouble(double* input, int rows, int columns, std::vector<double*> output);
    void ExportCSV();

    int* _layer;
  };
}
