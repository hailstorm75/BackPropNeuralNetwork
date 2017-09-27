#include "stdafx.h"
#include "NeuralNetworkWrapper.h"


Wrapper::NeuralNetworkWrapper::NeuralNetworkWrapper(int* layer, const int size)
{
  NeuralNetwork* net = new NeuralNetwork(layer, size);
  pNN = net;
}

void Wrapper::NeuralNetworkWrapper::TrainNetwork(int iterations, bool silent)
{
#pragma region Training data

  std::vector<double *> fedData = {
    new double[3]{ 0, 0, 0 },
    new double[3]{ 0, 0, 1 },
    new double[3]{ 0, 1, 0 },
    new double[3]{ 1, 0, 0 },
    new double[3]{ 0, 1, 1 },
    new double[3]{ 1, 0, 1 },
    new double[3]{ 1, 1, 0 },
  };

  std::vector<double *> expectedData = { new double[7]{ 0, 1, 1, 0, 1, 0, 0 } };

#pragma endregion

  pNN->TrainNetwork(fedData, expectedData, iterations, silent);
}

void Wrapper::NeuralNetworkWrapper::FeedForward(double* inputs, double** retVal)
{
  pNN->FeedForward(inputs, retVal);
}
