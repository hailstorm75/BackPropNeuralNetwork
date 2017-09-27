#include "stdafx.h"
#include "NeuralNetworkWrapper.h"


Wrapper::NeuralNetworkWrapper::NeuralNetworkWrapper(int* layer, const int size)
{
  NeuralNetwork* net = new NeuralNetwork(layer, size);
  pNN = net;
}

void Wrapper::NeuralNetworkWrapper::TrainNetwork(double* trainingData, double* expectedData, int dataSetSize, int iterations, bool silent)
{
  std::vector<double*> _trainingData(0);
  std::vector<double *> _expectedData(0);

  int* layer = pNN->GetLayer();

  ConvertToVectorDouble(trainingData, &dataSetSize, &layer[0], &_trainingData);
  ConvertToVectorDouble(expectedData, &dataSetSize, &layer[pNN->layersLength], &_expectedData);

  pNN->TrainNetwork(_trainingData, _expectedData, iterations, silent);
}

void Wrapper::NeuralNetworkWrapper::FeedForward(double* inputs, double** retVal)
{
  pNN->FeedForward(inputs, retVal);
}

void Wrapper::NeuralNetworkWrapper::ConvertToVectorDouble(double* input, int* rows, int* columns, std::vector<double*> *output)
{
  for (auto i = 0; i < *rows; i++)
    for (auto j = 0; j < *columns; j++)
      output->push_back(&input[i * *rows + j]);
}