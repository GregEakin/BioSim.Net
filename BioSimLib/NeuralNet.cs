using System.Collections;
using System.Text;
using BioSimLib.Sensors;
using Action = BioSimLib.Actions.Action;

namespace BioSimLib;

public class NeuralNet : IEnumerable<Neuron>
{
    private readonly Params _p;
    private readonly Indiv _individual;
    private readonly Genome _connections;
    private readonly Neuron[] _neurons;

    public NeuralNet(Params p, Indiv individual, Genome genome)
    {
        _p = p;
        _individual = individual;
        _connections = genome;
        _neurons = new Neuron[genome.NeuronsNeeded];
        for (var i = 0; i < _neurons.Length; i++)
            _neurons[i] = new Neuron();
    }

    public IEnumerator<Neuron> GetEnumerator()
    {
        return _neurons.Cast<Neuron>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _neurons.GetEnumerator();
    }

    public int Length => _neurons.Length;

    public Neuron this[int index] => _neurons[index];

    public float[] FeedForward(uint simStep)
    {
        var sensors = new SensorFactory();

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        var neuronAccumulators = new float[_neurons.Length];
        var neuronOutputsComputed = false;
        foreach (var conn in _connections)
        {
            if (conn.SinkType == Gene.GeneType.Action && !neuronOutputsComputed)
            {
                for (var neuronIndex = 0; neuronIndex < _neurons.Length; ++neuronIndex)
                {
                    if (_neurons[neuronIndex].Driven)
                        _neurons[neuronIndex].Output = (float)Math.Tanh(neuronAccumulators[neuronIndex]);
                }

                neuronOutputsComputed = true;
            }

            var inputVal = conn.SourceType == Gene.GeneType.Sensor
                ? sensors[conn.SourceSensor].Calc(_p, _individual, simStep)
                : _neurons[conn.SourceNum].Output;

            if (conn.SinkType == Gene.GeneType.Action)
                actionLevels[conn.SinkNum] += inputVal * conn.WeightAsFloat;
            else
                neuronAccumulators[conn.SinkNum] += inputVal * conn.WeightAsFloat;
        }

        return actionLevels;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(_connections.ToDna());
        foreach (var neuron in _neurons)
            sb.Append(neuron);
        return sb.ToString();
    }

    public (int[] sensors, int[] actions) ActionReferenceCounts()
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