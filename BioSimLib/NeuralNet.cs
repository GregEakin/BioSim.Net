//    Copyright 2021 Gregory Eakin
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System.Collections;
using System.Text;
using BioSimLib.Genes;
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