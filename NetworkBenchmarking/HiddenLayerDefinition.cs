using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using PropertyChanged;

namespace NetworkBenchmarking
{
  [AddINotifyPropertyChangedInterface]
  public class HiddenLayerDefinition : INotifyPropertyChanged
  {
    // https://www.codeproject.com/Articles/159939/Command-Binding-inside-DataTemplate-in-Parent-Chil
    public event PropertyChangedEventHandler PropertyChanged;

    #region Properties

    public int LayerCount { get; set; }

    public int NeuronCount
    {
      get
      {
        int total = 0;
        foreach (var layer in LayerDefinitions) total += layer.Neurons;

        return total;
      }
    }

    public List<LayerDefinition> LayerDefinitions { get; set; } 

    #endregion

    #region Commands

    public ICommand AddLayerCommand { get; set; }
    public ICommand DeleteLayerCommand { get; set; } 

    #endregion

    #region Constructor

    //--------------------------------------------------
    public HiddenLayerDefinition()
    //--------------------------------------------------
    {
      AddLayerCommand = new RelayCommand<string>(AddLayer);
      DeleteLayerCommand = new RelayCommand<LayerDefinition>(DeleteLayer);
    }

    #endregion

    #region Methods

    //--------------------------------------------------
    private void AddLayer(string input)
    //--------------------------------------------------
    {
      if (int.TryParse(input, out int value))
      {
        LayerDefinitions.Add(new LayerDefinition { Neurons = value });
        ++LayerCount;
      }
    }

    //--------------------------------------------------
    private void DeleteLayer(LayerDefinition definition)
    //--------------------------------------------------
    {
      if (LayerDefinitions.Remove(definition)) --LayerCount;
    } 

    #endregion
  }

  [AddINotifyPropertyChangedInterface]
  public class LayerDefinition : INotifyPropertyChanged
  {
    public int Neurons { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
