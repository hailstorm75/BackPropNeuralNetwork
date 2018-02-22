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
    TEST_METHOD(LayerTestConstructor_inputsA)
    //--------------------------------------------------
    {
      auto numberOfInputs = 0;
      auto numberOfOutputs = 0;
      bool caughtExceotion = false;
      try
      {
        Layer layer(numberOfInputs, numberOfOutputs);
      }
      catch (const std::exception&)
      {
        caughtExceotion = true;
      }

      Assert::IsTrue(caughtExceotion);
    }

    //--------------------------------------------------
    TEST_METHOD(LayerTestConstructor_inputsB)
    //--------------------------------------------------
    {
      auto numberOfInputs = -10;
      auto numberOfOutputs = -10;
      bool caughtExceotion = false;
      try
      {
        Layer layer(numberOfInputs, numberOfOutputs);
      }
      catch (const std::exception&)
      {
        caughtExceotion = true;
      }

      Assert::IsTrue(caughtExceotion);
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
      struct Layer::DoubleArray a;
      a.arr = nullptr;      

      try
      {
        layer.BackPropHidden(a, a);
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

