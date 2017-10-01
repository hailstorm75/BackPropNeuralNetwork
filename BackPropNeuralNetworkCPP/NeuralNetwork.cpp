#include "stdafx.h"
#include "NeuralNetwork.h"


#pragma region Constructor

NeuralNetwork::NeuralNetwork(int* layer, const int size)
{
  // Checking inputs
  if (layer == nullptr || size == NULL)
    throw std::logic_error("Invalid argument -> NeuralNetwork::NeuralNetwork(...)");

  this->layer = layer; // Copying array
  this->layersLength = size - 1; // Defining "layers" array length

  for (auto l = 0; l < layersLength; l++)
    layers.push_back(Layer(this->layer[l], this->layer[l + 1]));
}

#pragma endregion

#pragma region Methods

void NeuralNetwork::TrainNetwork(std::vector<double*> inputData, std::vector<double*> outputData, int iterations, bool silent)
{
  // Checking inputs
  if (inputData.size() <= 0 || outputData.size() <= 0)
    throw std::logic_error("Invalid argument -> NeuralNetwork::TrainNetwork(...)");

  // Checking vectors
  if (inputData[0] == nullptr || outputData[0] == nullptr)
    throw std::logic_error("Invalid argument -> NeuralNetwork::TrainNetwork(...)");

  auto division = iterations / 100;

  if (!silent)
  {
    system("CLS");
    GetConsoleCursorInfo(outHandle, &cursorInfo);
    cursorInfo.bVisible = false;
    SetConsoleCursorInfo(outHandle, &cursorInfo);
  }

  for (auto iteration = 0; iteration < iterations; iteration++)
  {
    if (!silent) std::cout << "Training: " << static_cast<int>(iteration / division) << "%";

    for (auto in = 0; in < inputData.size(); in++)
    {
      FeedForward(inputData[in]);
      BackPropagate(outputData[in]);
    }

    if (!silent)
    {
      COORD cur = { 0, 0 };
      SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), cur);
    }
  }

  if (!silent)
  {
    GetConsoleCursorInfo(outHandle, &cursorInfo);
    cursorInfo.bVisible = true;
    SetConsoleCursorInfo(outHandle, &cursorInfo);

    std::cout << "Training: " << 100 << "%" << std::endl;
  }
}

void NeuralNetwork::FeedForward(double* inputs, double** retVal)
{
  // Checking input
  if (inputs == nullptr)
    throw std::logic_error("Invalid argument -> NeuralNetwork::FeedForward(...)");

  layers[0].FeedForward(inputs);

  for (auto l = 1; l < layersLength; l++)
    layers[l].FeedForward(layers[l - 1].outputs);

  if (retVal != nullptr)
    *retVal = layers[layersLength - 1].outputs;
}

void NeuralNetwork::BackPropagate(double* expected)
{
  // Checking input
  if (expected == nullptr)
    throw std::logic_error("Invalid argument -> NeuralNetwork::BackPropagate(...)");

  int l;

  for (l = layersLength - 1; l >= 0; l--)
  {
    if (l == layersLength - 1)
      layers[l].BackPropOutput(expected);
    else
      layers[l].BackPropHidden(layers[l + 1].gamma, layers[l + 1].weights);
  }

  for (l = 0; l < layersLength; l++)
    layers[l].UpdateWeights();
}

#pragma endregion

#pragma region Unit Testing

int* NeuralNetwork::GetLayer() const
{
  return layer;
}

#pragma endregion
