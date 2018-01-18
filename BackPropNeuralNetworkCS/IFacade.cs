namespace NeuralNetworkFacadeCS
{
  public interface IFacade
  {
    void ExportToCSV();
    void TrainNetwork();
    void OutputToConsoleTest(ref double totalError, ref bool stability);
  }
}