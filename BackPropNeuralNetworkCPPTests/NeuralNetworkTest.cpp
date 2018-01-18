#include "stdafx.h"
#include "CppUnitTest.h"

#include "../BackPropNeuralNetworkCPP/NeuralNetwork.h"
#include "../BackPropNeuralNetworkCPP/NeuralNetwork.cpp"
#include "../BackPropNeuralNetworkCPP/Layer.h"
#include "../BackPropNeuralNetworkCPP/Layer.cpp"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace BackPropNeuralNetworkCPPTests
{
  TEST_CLASS(NeuralNetworkTest)
  {
  public:
    wchar_t * nullpointerEx = L"Nullpointer invalid argument not caught.";

    TEST_METHOD(NeuralNetworkTestConstructor_Null)
    {
      bool caughtException;

      try
      {
        NeuralNetwork(nullptr, 0);
        caughtException = false;
      }
      catch (std::logic_error)
      {
        caughtException = true;
      }

      Assert::IsTrue(caughtException, nullpointerEx);
    }

    TEST_METHOD(NeuralNetworkTestConstructor_NoSize)
    {
      bool caughtException;

      try
      {
        NeuralNetwork(new int[0]{}, 0);
        caughtException = false;
      }
      catch (std::logic_error)
      {
        caughtException = true;
      }

      Assert::IsTrue(caughtException, nullpointerEx);
    }

    TEST_METHOD(NeuralNetworkTest_layers)
    {
      auto layer = new int[3]{ 3, 25, 1 };
      NeuralNetwork net(layer, 3);

      Assert::IsTrue(CompareIntArrays(layer, net.GetLayer()), L"Array content");
    }

    TEST_METHOD(NeuralNetworkTest_TrainNetwork)
    {
      auto layer = new int[3]{ 3, 25, 1 };
      NeuralNetwork net(layer, 3);

      bool caughtException;

      try
      {
        net.TrainNetwork(std::vector<double *>(0), std::vector<double *>(0), NULL);
        caughtException = false;
      }
      catch (std::logic_error)
      {
        caughtException = true;
      }
      
      Assert::IsTrue(caughtException, nullpointerEx);
    }

    TEST_METHOD(NeuralNetworkTest_FeedForward)
    {
      auto layer = new int[3]{ 3, 25, 1 };
      NeuralNetwork net(layer, 3);

      bool caughtException;

      try
      {
        net.FeedForward(nullptr);
        caughtException = false;
      }
      catch (std::logic_error)
      {
        caughtException = true;
      }

      Assert::IsTrue(caughtException, nullpointerEx);
    }

    TEST_METHOD(NeuralNetworkTest_BackPropagate)
    {
      auto layer = new int[3]{ 3, 25, 1 };
      NeuralNetwork net(layer, 3);

      bool caughtException;

      try
      {
        net.BackPropagate(nullptr);
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