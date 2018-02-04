#pragma once
#include <cstdlib>
#include <iostream>
#include <math.h>
#include <random>

class Layer
{
public:
#pragma region Fields

  double* outputs;
  double* inputs;
  double* weights;
  double* weightsDelta;
  double* gamma;
  double* error;

#pragma endregion

#pragma region Constructor

  /**
  * \brief Default constructor
  * \param numberOfInputs
  * \param numberOfOutputs
  */
  Layer(int numberOfInputs, int numberOfOutputs);

  /**
  * \brief Layer destructor
  */
  void Clear() const;

#pragma endregion

#pragma region Methods

  /**
  * \brief Fills weights with random values
  */
  void InitilizeWeights() const;

  /**
  * \brief Fill weights with new values
  */
  void UpdateWeights() const;

  /**
  * \brief Activates given value
  * \param value Value to activate
  * \return Calculation
  */
  static double Layer::ActivateFunction(double value);

  /**
  * \brief Calculates the derivative of given value
  * \param value Value of which the derivative is to be calculated
  * \return Calculation
  */
  static double DeriveFunction(double value);

  /**
  * \brief Feeds value to next layer
  * \param inputs Input data
  * \return Neural network prediction
  */
  void FeedForward(double* inputs, double** retVal = nullptr);

  /**
  * \brief Back propagation algorythm from output to penultimate layer
  * \param expected Expected result
  */
  void BackPropOutput(double* expected) const;

  /**
  * \brief Back propagation algorythm from penultimate to input layer
  * \param gammaForward
  * \param weightsForward
  */
  void BackPropHidden(double* gammaForward, double* weightsForward) const;

#pragma endregion

private:
#pragma region Fields

  int numberOfInputs;
  int numberOfOutputs;

#pragma endregion

#pragma region Constants

  /**
  * \brief Neural network learning rate
  */
  const double learningRate = 0.02;

#pragma endregion
};
