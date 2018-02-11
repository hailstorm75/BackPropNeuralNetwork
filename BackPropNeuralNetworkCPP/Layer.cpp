#include "stdafx.h"
#include "Layer.h"

#pragma region Constructor

//--------------------------------------------------
Layer::Layer(int numberOfInputs, int numberOfOutputs)
//--------------------------------------------------
{
  if (numberOfInputs <= 0 || numberOfOutputs <= 0)
    throw std::logic_error("Invalid argument -> Layer::Layer(...)");

  this->numberOfInputs = numberOfInputs;
  this->numberOfOutputs = numberOfOutputs;

  // 1D Arrays
  outputs.arr = static_cast<double *>(malloc(this->numberOfOutputs * sizeof(double)));
  outputs.size = this->numberOfOutputs;
  gamma.arr = static_cast<double *>(malloc(this->numberOfOutputs * sizeof(double)));
  gamma.size = this->numberOfOutputs;
  error.arr = static_cast<double *>(malloc(this->numberOfOutputs * sizeof(double)));
  error.size = this->numberOfOutputs;

  // 2D Arrays
  weights.arr = static_cast<double *>(malloc(this->numberOfOutputs * this->numberOfInputs * sizeof(double)));
  weights.size = this->numberOfInputs;
  weightsDelta.arr = static_cast<double *>(malloc(this->numberOfOutputs * this->numberOfInputs * sizeof(double)));
  weightsDelta.size = this->numberOfInputs;

  InitilizeWeights();
}

//--------------------------------------------------
void Layer::Clear() const
//--------------------------------------------------
{
  free(outputs.arr);
  free(gamma.arr);
  free(error.arr);
  free(weights.arr);
  free(weightsDelta.arr);
}

#pragma endregion 

#pragma region Methods

//-------------------------------------------------- 
void Layer::InitilizeWeights() const
//-------------------------------------------------- 
{
  std::random_device rd;
  std::default_random_engine generator(rd());
  std::uniform_real_distribution<double> distribution(0.0001, 0.9999);

  for (auto col = 0; col < this->numberOfOutputs; ++col)
    for (auto row = 0; row < this->numberOfInputs; ++row)
      weights.arr[col * this->numberOfInputs + row] = distribution(generator);
}

//--------------------------------------------------
void Layer::FeedForward(double* inputs, double** retVal)
//--------------------------------------------------
{
  if (inputs == nullptr)
    throw std::logic_error("Invalid argument -> Layer::FeedForward(...)");

  this->inputs.arr = inputs;

  for (auto output = 0; output < numberOfOutputs; ++output)
  {
    outputs.arr[output] = 0;

    for (auto input = 0; input < numberOfInputs; ++input)
      outputs.arr[output] += inputs[input] * weights.arr[output * numberOfInputs + input];

    outputs.arr[output] = ActivateFunction(outputs.arr[output]);
  }

  if (retVal != nullptr) *retVal = outputs.arr;
}

//--------------------------------------------------
double Layer::ActivateFunction(double value)
//--------------------------------------------------
{
  return tanh(value);
}

//--------------------------------------------------
double Layer::DeriveFunction(double value)
//--------------------------------------------------
{
  return 1 - value * value;
}

//--------------------------------------------------
void Layer::BackPropOutput(double* expected) const
//--------------------------------------------------
{
  // Checking input
  if (expected == nullptr)
    throw std::logic_error("Invalid argument -> Layer::BackPropOutput(...)");

  int output;

  for (output = 0; output < numberOfOutputs; ++output)
  {
    error.arr[output] = outputs.arr[output] - expected[output];
    gamma.arr[output] = error.arr[output] * DeriveFunction(outputs.arr[output]);
  }

  for (output = 0; output < numberOfOutputs; ++output)
    for (auto input = 0; input < numberOfInputs; ++input)
      weightsDelta.arr[output * numberOfInputs + input] = gamma.arr[output] * inputs.arr[input];
}

//--------------------------------------------------
void Layer::BackPropHidden(struct DoubleArray gammaForward, struct DoubleArray weightsForward) const
//--------------------------------------------------
{
  // Checking inputs
  if (gammaForward.arr == nullptr || weightsForward.arr == nullptr)
    throw std::logic_error("Invalid argument -> Layer::BackPropHidden(...)");

  int output, input;

  for (output = 0; output < numberOfOutputs; ++output)
  {
    gamma.arr[output] = 0;

    for (input = 0; input < gammaForward.size; ++input)
      gamma.arr[output] += gammaForward.arr[input] * weightsForward.arr[input * numberOfOutputs + output];

    gamma.arr[output] *= DeriveFunction(outputs.arr[output]);
  }

  for (output = 0; output < numberOfOutputs; ++output)
    for (input = 0; input < numberOfInputs; ++input)
      weightsDelta.arr[output * numberOfInputs + input] = gamma.arr[output] * inputs.arr[input];
}

//--------------------------------------------------
void Layer::UpdateWeights() const
//--------------------------------------------------
{
  for (auto output = 0; output < numberOfOutputs; ++output)
    for (auto input = 0; input < numberOfInputs; ++input)
      weights.arr[output * numberOfInputs + input] -= weightsDelta.arr[output * numberOfInputs + input] * learningRate;
}

#pragma endregion

