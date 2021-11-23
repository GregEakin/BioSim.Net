namespace BioSimLib;

public class Config
{
    public uint population; // >= 0
    public uint stepsPerGeneration; // > 0
    public uint maxGenerations; // >= 0
    public uint numThreads; // > 0
    public uint signalLayers; // >= 0
    public uint genomeMaxLength; // > 0
    public int maxNumberNeurons; // > 0
    public double pointMutationRate; // 0.0..1.0
    public double geneInsertionDeletionRate; // 0.0..1.0
    public double deletionRatio; // 0.0..1.0
    public bool killEnable;
    public bool sexualReproduction;
    public bool chooseParentsByFitness;
    public float populationSensorRadius; // > 0.0
    public uint signalSensorRadius; // > 0
    public float responsiveness; // >= 0.0
    public uint responsivenessCurveKFactor; // 1, 2, 3, or 4
    public uint longProbeDistance; // > 0
    public uint shortProbeBarrierDistance; // > 0
    public float valenceSaturationMag;
    public bool saveVideo;
    public uint videoStride; // > 0
    public uint videoSaveFirstFrames; // >= 0, overrides videoStride
    public uint displayScale;
    public uint agentSize;
    public uint genomeAnalysisStride; // > 0
    public uint displaySampleGenomes; // >= 0
    public uint genomeComparisonMethod; // 0 = Jaro-Winkler; 1 = Hamming
    public bool updateGraphLog;
    public uint updateGraphLogStride; // > 0
    public uint challenge;
    public uint barrierType; // >= 0
    public uint replaceBarrierType; // >= 0
    public uint replaceBarrierTypeGenerationNumber; // >= 0

    // These must not change after initialization
    public short sizeX; // 2..0x10000
    public short sizeY; // 2..0x10000
    public uint genomeInitialLengthMin; // > 0 and < genomeInitialLengthMax
    public uint genomeInitialLengthMax; // > 0 and < genomeInitialLengthMin
    public string logDir;
    public string imageDir;
    public string graphLogUpdateCommand;

    public Config()
    {
        population = 1;
        sizeX = 16;
        sizeY = 16;

        logDir = "";
        imageDir = "";
        graphLogUpdateCommand = "";

        maxNumberNeurons = 5;
        stepsPerGeneration = 1;
    }

    public override string ToString()
    {
        return $"Population = {population}, Size x = {sizeX}, Size y = {sizeY}";
    }
}