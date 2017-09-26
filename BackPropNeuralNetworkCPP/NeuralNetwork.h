#pragma once
#include "Layer.h"
#include <vector>
#include <windows.h>

class NeuralNetwork
{
public:
#pragma region Constructor

  /**
  * \brief Default constructor
  * \param layer Number of neurons in each layer
  * \param size size of the Neural network
  */
  NeuralNetwork(int* layer, const int size);

#pragma endregion

#pragma region Methods

  /**
  * \brief Trains the Neural network
  * \param inputData Training dataset
  * \param outputData Expected results
  * \param iterations Times to repeat training
  */
  void TrainNetwork(std::vector<double*> inputData, std::vector<double*> outputData, int iterations, bool silent);

  /**
  * \brief Feeds data through Neural network
  * \param inputs
  * \return Neural network predictions
  */
  void FeedForward(double* inputs, double** retVal = nullptr);

  /**
  * \brief Backpropagates expected result in Neural network
  * \param expected
  */
  void BackPropagate(double* expected);

#pragma endregion

#pragma region Unit Testing

  int* GetLayer() const;

#pragma endregion

private:
#pragma region Fields

  int* layer;
  int layersLength;

  std::vector<Layer> layers;

  HANDLE outHandle = GetStdHandle(STD_OUTPUT_HANDLE);
  CONSOLE_CURSOR_INFO cursorInfo;

#pragma endregion
};
