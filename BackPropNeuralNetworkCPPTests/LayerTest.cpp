#include "stdafx.h"
#include "CppUnitTest.h"

#include "../BackPropNeuralNetworkCPP/NeuralNetwork.h"
#include "../BackPropNeuralNetworkCPP/Layer.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace BackPropNeuralNetworkCPPTests
{
  TEST_CLASS(LayerTest)
  {
  public:
    wchar_t * nullpointerEx = L"Nullpointer invalid argument not caught.";

#pragma region Constructor

    //--------------------------------------------------
    TEST_METHOD(LayerTestConstructor_inputs)
    //--------------------------------------------------
    {
      auto numberOfInputs = 10;
      auto numberOfOutputs = 10;
      Layer layer(numberOfInputs, numberOfOutputs);

      Assert::IsTrue(_msize(layer.inputs) / sizeof(layer.inputs) == numberOfInputs);
    }

    //--------------------------------------------------
    TEST_METHOD(LayerTestConstructor_outputs)
    //--------------------------------------------------
    {
      auto numberOfInputs = 10;
      auto numberOfOutputs = 10;
      Layer layer(numberOfInputs, numberOfOutputs);

      Assert::IsTrue(_msize(layer.outputs) / sizeof(layer.outputs) == numberOfOutputs);
    }

    //--------------------------------------------------
    TEST_METHOD(LayerTestConstructor_gamma)
    //--------------------------------------------------
    {
      auto numberOfInputs = 10;
      auto numberOfOutputs = 10;
      Layer layer(numberOfInputs, numberOfOutputs);

      Assert::IsTrue(_msize(layer.gamma) / sizeof(layer.gamma) == numberOfOutputs);
    }

    //--------------------------------------------------
    TEST_METHOD(LayerTestConstructor_error)
    //--------------------------------------------------
    {
      auto numberOfInputs = 10;
      auto numberOfOutputs = 10;
      Layer layer(numberOfInputs, numberOfOutputs);

      Assert::IsTrue(_msize(layer.error) / sizeof(layer.error) == numberOfOutputs);
    }

    //--------------------------------------------------
    TEST_METHOD(LayerTestConstructor_weights)
    //--------------------------------------------------
    {
      auto numberOfInputs = 10;
      auto numberOfOutputs = 10;
      Layer layer(numberOfInputs, numberOfOutputs);

      Assert::IsTrue(_msize(layer.weights) / sizeof(layer.weights) == numberOfOutputs * numberOfInputs);
    }

    //--------------------------------------------------
    TEST_METHOD(LayerTestConstructor_weightsDelta)
    //--------------------------------------------------
    {
      auto numberOfInputs = 10;
      auto numberOfOutputs = 10;
      Layer layer(numberOfInputs, numberOfOutputs);

      Assert::IsTrue(_msize(layer.weightsDelta) / sizeof(layer.weightsDelta) == numberOfOutputs * numberOfInputs);
    }

#pragma endregion

    //--------------------------------------------------
    TEST_METHOD(LayerTest_FeedForward)
    //--------------------------------------------------
    {
      Layer layer(1,1);

      bool caughtException;

      try
      {
        layer.FeedForward(nullptr);
        caughtException = false;
      }
      catch (std::logic_error)
      {
        caughtException = true;
      }

      Assert::IsTrue(caughtException, nullpointerEx);
    }

    //--------------------------------------------------
    TEST_METHOD(LayerTest_BackPropOutput)
    //--------------------------------------------------
    {
      Layer layer(1, 1);

      bool caughtException;

      try
      {
        layer.BackPropOutput(nullptr);
        caughtException = false;
      }
      catch (std::logic_error)
      {
        caughtException = true;
      }
      
      Assert::IsTrue(caughtException, nullpointerEx);      
    }

    //--------------------------------------------------
    TEST_METHOD(LayerTest_BackPropHidden)
    //--------------------------------------------------
    {
      Layer layer(1, 1);

      bool caughtException;

      try
      {
        layer.BackPropHidden(nullptr, nullptr);
        caughtException = false;
      }
      catch (std::logic_error)
      {
        caughtException = true;
      }

      Assert::IsTrue(caughtException, nullpointerEx);
    }
  };
}

