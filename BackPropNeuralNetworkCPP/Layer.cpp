#include "stdafx.h"
#include "Layer.h"

#pragma region Constructor

Layer::Layer(int numberOfInputs, int numberOfOutputs)
{
  this->numberOfInputs = numberOfInputs;
  this->numberOfOutputs = numberOfOutputs;

  // 1D Arrays
  inputs = static_cast<double *>(calloc(this->numberOfInputs, sizeof(double)));
  outputs = static_cast<double *>(calloc(this->numberOfOutputs, sizeof(double)));
  gamma = static_cast<double *>(calloc(this->numberOfOutputs, sizeof(double)));
  error = static_cast<double *>(calloc(this->numberOfOutputs, sizeof(double)));

  // 2D Arrays
  weights = static_cast<double *>(calloc(this->numberOfOutputs * this->numberOfInputs, sizeof(double)));
  weightsDelta = static_cast<double *>(calloc(this->numberOfOutputs * this->numberOfInputs, sizeof(double)));

  InitilizeWeights();
}

#pragma endregion 

#pragma region Methods

void Layer::InitilizeWeights() const
{
  std::random_device rd;
  std::default_random_engine generator(rd());
  std::uniform_real_distribution<double> distribution(0.0001, 0.9999);

  for (auto col = 0; col < this->numberOfOutputs; col++)
    for (auto row = 0; row < this->numberOfInputs; row++)
      weights[col * this->numberOfInputs + row] = distribution(generator);
}

void Layer::FeedForward(double* inputs, double** retVal)
{
  if (inputs == nullptr)
    throw std::logic_error("Invalid argument -> Layer::FeedForward(...)");

  this->inputs = inputs;

  for (auto out = 0; out < numberOfOutputs; out++)
  {
    outputs[out] = 0;

    for (auto in = 0; in < numberOfInputs; in++)
      outputs[out] += inputs[in] * weights[out * numberOfInputs + in];

    outputs[out] = ActivateFunction(outputs[out]);
  }
  if (retVal != nullptr)
    *retVal = outputs;
}

double Layer::ActivateFunction(double value)
{
  return tanh(value);
}

double Layer::DeriveFunction(double value)
{
  return 1 - value * value;
}

void Layer::BackPropOutput(double* expected)
{
  // Checking input
  if (expected == nullptr)
    throw std::logic_error("Invalid argument -> Layer::BackPropOutput(...)");

  int out;

  for (out = 0; out < numberOfOutputs; out++)
  {
    error[out] = outputs[out] - expected[out];
    gamma[out] = error[out] * DeriveFunction(outputs[out]);
  }

  for (out = 0; out < numberOfOutputs; out++)
    for (auto in = 0; in < numberOfInputs; in++)
      weightsDelta[out * numberOfInputs + in] = gamma[out] * inputs[in];
}

void Layer::BackPropHidden(double* gammaForward, double* weightsForward)
{
  // Checking inputs
  if (gammaForward == nullptr || weightsForward == nullptr)
    throw std::logic_error("Invalid argument -> Layer::BackPropHidden(...)");

  int out, in;

  for (out = 0; out < numberOfOutputs; out++)
  {
    gamma[out] = 0;

    for (in = 0; in < _msize(gammaForward) / sizeof(double); in++)
      gamma[out] += gammaForward[in] * weightsForward[in * numberOfOutputs + out];

    gamma[out] *= DeriveFunction(outputs[out]);
  }

  for (out = 0; out < numberOfOutputs; out++)
    for (in = 0; in < numberOfInputs; in++)
      weightsDelta[out * numberOfInputs + in] = gamma[out] * inputs[in];
}

void Layer::UpdateWeights() const
{
  for (auto out = 0; out < numberOfOutputs; out++)
    for (auto in = 0; in < numberOfInputs; in++)
      weights[out * numberOfInputs + in] -= weightsDelta[out * numberOfInputs + in] * learningRate;
}

#pragma endregion
