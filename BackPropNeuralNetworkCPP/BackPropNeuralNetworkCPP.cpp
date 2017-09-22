// BackPropNeuralNetworkCPP.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "NeuralNetwork.h"
#include <vector>

//int main()
//{
//#pragma region Training data
//
//  std::vector<double *> fedData = {
//    new double[3] {0, 0, 0},
//    new double[3] {0, 0, 1},
//    new double[3] {0, 1, 0},
//    new double[3] {1, 0, 0},
//    new double[3] {0, 1, 1},
//    new double[3] {1, 0, 1},
//    new double[3] {1, 1, 0},
//  };
//
//  std::vector<double *> expectedData = { new double[7] {0, 1, 1, 0, 1, 0, 0} };
//
//#pragma endregion
//
//#pragma region Preparing network
//
//  const auto netSize = 3;
//  NeuralNetwork net(new int[netSize] { 3, 20, 1 }, netSize);
//
//  system("PAUSE");
//  system("CLS");
//
//  // Training network
//  net.TrainNetwork(fedData, expectedData, 5000, false);
//
//#pragma endregion
//
//#pragma region Output
//
//  for (auto j = 0; j < 7; j++)
//    printf("Expected: %g\tReceived: %f\n", (expectedData[0])[j], net.FeedForward(fedData[j])[0]);
//
//  printf("Expected: %d\tReceived: %f\n", 1, net.FeedForward(new double[3]{ 1, 1, 1 })[0]);
//
//#pragma endregion
//
//  system("PAUSE");
//
//  return 0;
//}
