#include "stdafx.h"
#include "NeuralNetworkWrapper.h"


NeuralNetworkWrapper::NeuralNetworkWrapper(int* layer, const int size)
{
  pNN = &NeuralNetwork(layer, size);
}

void NeuralNetworkWrapper::TrainNetwork(int iterations, bool silent)
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