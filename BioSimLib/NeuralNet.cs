using System.Collections;
using System.Text;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

namespace BioSimLib;

public class NeuralNet : IEnumerable<Neuron>
{
    private readonly Genome _connections;
    private readonly Neuron[] _neurons;

    public NeuralNet(Genome genome)
    {
        _connections = genome;
        _neurons = new Neuron[genome.NeuronsNeeded];
        for (var i = 0; i < _neurons.Length; i++)
            _neurons[i] = new Neuron();
    }

    public IEnumerator<Neuron> GetEnumerator() => _neurons.Cast<Neuron>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _neurons.GetEnumerator();

    public int Length => _neurons.Length;

    public Neuron this[int index] => _neurons[index];

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(_connections.ToDna());
        // foreach (var neuron in _neurons)
        //     sb.Append(neuron);
        return sb.ToString();
    }

    public (IEnumerable<int> sensors, IEnumerable<int> actions) ActionReferenceCounts()
    {
        var sensors = new int[Enum.GetNames<Sensor>().Length];
        var actions = new int[Enum.GetNames<Action>().Length];

        foreach (var gene in _connections)
        {
            if (gene.SourceType == Gene.GeneType.Sensor)
                ++sensors[(byte)gene.SourceSensor];

            if (gene.SinkType == Gene.GeneType.Action)
                ++actions[(byte)gene.SinkNeuron];
        }

        return (sensors, actions);
    }
}