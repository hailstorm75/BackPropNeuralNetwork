namespace NeuralNetworkFacadeCS
{
  public interface IFacade
  {
    void ExportToCSV();
    void TrainNetwork();
    void OutputTestResult(ref double totalError, ref bool stability);
  }
}