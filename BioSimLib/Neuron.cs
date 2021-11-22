namespace BioSimLib;

public class Neuron
{
    public static float InitialNeuronOutput()
    {
        return 0.5f;
    }

    public float Output { get; set; }
    public bool Driven { get; set; }
}