#include "stdafx.h"
#include "NeuralNetworkWrapper.h"


#pragma region Constructor

Wrapper::NeuralNetworkWrapper::NeuralNetworkWrapper(int* layer, const int size)
{
  if (layer == nullptr || size == 0)
    throw std::logic_error("Invalid argument -> NeuralNetworkWrapper::NeuralNetworkWrapper(...)");

  _layer = layer;
  pNN = new NeuralNetwork(layer, size);
}

void Wrapper::NeuralNetworkWrapper::Clear()
{
  pNN->Clear();
}

#pragma endregion

#pragma region Methods

void Wrapper::NeuralNetworkWrapper::TrainNetwork(double* trainingData, double* expectedData, int dataSetSize, int iterations)
{
  if (trainingData == nullptr || expectedData == nullptr || dataSetSize == 0 || iterations <= 0)
    throw std::logic_error("Invalid argument -> NeuralNetworkWrapper::TrainNetwork(...)");

  int* layer = pNN->GetLayer();

  std::vector<double *> _trainingData;
    std::vector<double *> _expectedData;

  // Converting data
  _trainingData = ConvertToVectorDouble(trainingData, dataSetSize, layer[0], _trainingData);
  _expectedData = ConvertToVectorDouble(expectedData, dataSetSize, layer[pNN->layersLength], _expectedData);

  // Training network
  pNN->TrainNetwork(_trainingData, _expectedData, iterations);

  // Freeing memmory
  for (auto i = 0; i < _trainingData.size(); ++i)
    free(_trainingData[i]);
  for (auto i = 0; i < _expectedData.size(); ++i)
    free(_expectedData[i]);
}

void Wrapper::NeuralNetworkWrapper::FeedForward(double* inputs, double** retVal)
{
  if (inputs == nullptr || retVal == nullptr)
    throw std::logic_error("Invalid argument -> NeuralNetworkWrapper::FeedForward(...)");

  pNN->FeedForward(inputs, retVal);
}

std::vector<double*> Wrapper::NeuralNetworkWrapper::ConvertToVectorDouble(double* input, int rows, int columns, std::vector<double*> output)
{
  // TODO Avoid copying data, resolve with pointers
  for (auto i = 0; i < rows; i++)
  {
    output.push_back(static_cast<double*>(malloc(columns * sizeof(double))));

    for (auto j = 0; j < columns; j++)
      output[output.size() - 1][j] = input[i * columns + j];
  }

  return output;
}

#pragma endregion

#pragma region Export

void Wrapper::NeuralNetworkWrapper::ExportNeuralNetwork(ExportType type)
{
  switch (type)
  {
  case Wrapper::NeuralNetworkWrapper::ExportType::CSV:
    ExportCSV();
    break;
  }
}

void Wrapper::NeuralNetworkWrapper::ExportCSV()
{
  std::stringstream ss;
  std::ofstream exportFile;
  exportFile.open("NeuralNetworkExport.csv");
  exportFile << "layer\n";

  for (int i = 0; i < pNN->layersLength + 1; i++)
  {
    ss << _layer[i];

    if (i = pNN->layersLength)
      ss << ";";
  }

  exportFile << ss.str();
  ss.clear();

  for (int i = 0; i < pNN->layersLength; i++)
  {
    exportFile << "layer " << i << " - inputs";
    pNN->layers[i].inputs;

    for (int j = 0; j < _layer[i]; j++)
    {
      ss << pNN->layers[i].inputs[j];

      if (j + 1 == _layer[i])
        ss << ";";
    }

    ss.clear();
    exportFile << "layer " << i << " - outputs";

    for (int j = 0; j < _layer[i + 1]; j++)
    {
      ss << pNN->layers[i].outputs[j];

      if (j + 1 == _layer[i])
        ss << ";";
    }

    ss.clear();
    exportFile << "layer " << i << " - outputs";

    for (int j = 0; j < _layer[i + 1]; j++)
    {
      ss << pNN->layers[i].outputs[j];

      if (j + 1 == _layer[i])
        ss << ";";
    }

    ss.clear();
    exportFile << "layer " << i << " - gamma";

    for (int j = 0; j < _layer[i + 1]; j++)
    {
      ss << pNN->layers[i].gamma[j];

      if (j + 1 == _layer[i])
        ss << ";";
    }

    ss.clear();
    exportFile << "layer " << i << " - error";

    for (int j = 0; j < _layer[i + 1]; j++)
    {
      ss << pNN->layers[i].error[j];

      if (j + 1 == _layer[i])
        ss << ";";
    }
  }

  exportFile.close();
}

#pragma endregion

