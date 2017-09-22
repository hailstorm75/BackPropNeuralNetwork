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
      Assert::ExpectException<int>([] { NeuralNetwork(nullptr, 0); }, nullpointerEx);
    }

    TEST_METHOD(NeuralNetworkTestConstructor_NoSize)
    {
      Assert::ExpectException<int>([] { NeuralNetwork(new int[0] { }, 0); }, nullpointerEx);
    }

    TEST_METHOD(NeuralNetworkTest_layers)
    {
      auto layer = new int[3]{ 3, 25, 1 };
      NeuralNetwork net(layer, 3);

      Assert::IsTrue(CompareIntArrays(layer, net.GetLayer()), L"Array content");
    }

    //TEST_METHOD(NeuralNetworkTest_BackPropagate)
    //{
    //  auto layer = new int[3]{ 3, 25, 1 };
    //  NeuralNetwork net(layer, 3);

    //  Assert::ExpectException<int>([net] { net.BackPropagate(nullptr); });
    //}
  };
}