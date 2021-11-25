namespace BioSimLib.Genes;

public class GeneBuilder
{
    public Gene.GeneType SourceType { get; set; } = Gene.GeneType.Neuron;
    public int SourceNum { get; set; }
    public Gene.GeneType SinkType { get; set; } = Gene.GeneType.Neuron;
    public int SinkNum { get; set; }
    public short Weight { get; set; }

    public float WeightAsFloat
    {
        get => Weight / 8192.0f;
        set => Weight = (short)(value * 8192.0f);
    }

    public GeneBuilder() {}

    public GeneBuilder(Gene gene)
    {
        SourceType = gene.SourceType;
        SourceNum = gene.SourceNum;
        SinkType = gene.SinkType;
        SinkNum = gene.SinkNum;
        Weight = gene.WeightAsShort;
    }
}