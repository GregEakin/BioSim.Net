using BioSimLib;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Positions;
using BioSimLib.Sensors;
using Xunit;
using Action = BioSimLib.Actions.Action;

namespace BioSimTests.Genes;

public class GenomeBuilderTests
{
    [Fact]
    public void ColorTest()
    {
        var config = new Config { maxNumberNeurons = 1, sizeX = 8, sizeY = 8 };
        var gene1 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = Sensor.TRUE,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.MOVE_FORWARD,
            WeightAsFloat = 1.0f
        };
        var gene2 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = Sensor.AGE,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.MOVE_REVERSE,
            WeightAsFloat = 0.6f
        };
        var gene3 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = Sensor.POPULATION,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.MOVE_NORTH,
            WeightAsFloat = 1.5f
        };

        var builder = new GenomeBuilder(config.maxNumberNeurons, new[] { gene1.ToUint(), gene2.ToUint(), gene3.ToUint() });
        builder.SetupGenome();
        var (red, green, blue) = builder.Color;
        Assert.Equal(0x64, red);
        Assert.Equal(0xb6, green);
        Assert.Equal(0xb2, blue);
    }

    [Fact]
    public void LengthTest()
    {
        var config = new Config { maxNumberNeurons = 1, sizeX = 8, sizeY = 8 };
        var gene1 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = Sensor.TRUE,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.MOVE_FORWARD,
            WeightAsFloat = 1.0f
        };
        var gene2 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = Sensor.AGE,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.MOVE_REVERSE,
            WeightAsFloat = 0.6f
        };
        var gene3 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = Sensor.POPULATION,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.MOVE_NORTH,
            WeightAsFloat = 1.5f
        };

        var builder = new GenomeBuilder(config.maxNumberNeurons, new[] { gene1.ToUint(), gene2.ToUint(), gene3.ToUint() });
        Assert.Equal(3, builder.Length);
    }

    [Fact]
    public void SetupGenomeTest()
    {
        var config = new Config { maxNumberNeurons = 2, sizeX = 8, sizeY = 8 };
        var gene1 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = Sensor.TRUE,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.MOVE_NORTH,
            WeightAsFloat = 1.0f
        };

        var builder = new GenomeBuilder(config.maxNumberNeurons, new[] { gene1.ToUint() });
        builder.SetupGenome();
        Assert.Equal(1, builder.Length);
    }

    [Fact]
    public void MakeRenumberedConnectionListTest()
    {
        var config = new Config { maxNumberNeurons = 2, sizeX = 8, sizeY = 8 };
        var gene1 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Neuron,
            SourceNum = 0xFE,
            SinkType = Gene.GeneType.Action,
            SinkNum = 0xFF,
            WeightAsFloat = 1.0f
        };

        var builder = new GenomeBuilder(config.maxNumberNeurons, new[] { gene1.ToUint() });
        builder.SetupGenome();
        builder.MakeRenumberedConnectionList();
        Assert.Equal(1, builder.Neurons);
        var gene = builder[0];
        Assert.Equal(0, gene.SourceNum);
        Assert.Equal(7, gene.SinkNum);
    }

    [Fact]
    public void MakeNodeList_SensorToAction_Test()
    {
        var config = new Config { maxNumberNeurons = 4, sizeX = 8, sizeY = 8 };
        var gene1 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = Sensor.TRUE,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.MOVE_FORWARD,
            WeightAsFloat = 1.0f
        };

        var builder = new GenomeBuilder(config.maxNumberNeurons, new[] { gene1.ToUint() });
        builder.SetupGenome();
        var nodeList = builder.MakeNodeList();
        Assert.Empty(nodeList);
    }

    [Fact]
    public void MakeNodeList_NeuronToActionTest()
    {
        var config = new Config { maxNumberNeurons = 4, sizeX = 8, sizeY = 8 };
        var gene1 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Neuron,
            SourceNum = 0,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.MOVE_NORTH,
            WeightAsFloat = 1.0f
        };

        var builder = new GenomeBuilder(config.maxNumberNeurons, new[] { gene1.ToUint() });
        builder.SetupGenome();
        var nodeList = builder.MakeNodeList();
        Assert.Equal(1, nodeList[0].NumOutputs);
        Assert.Equal(0, nodeList[0].NumSelfInputs);
        Assert.Equal(0, nodeList[0].NumInputsFromSensorsOrOtherNeurons);
    }

    [Fact]
    public void MakeNodeList_NeuronToSameNeuronTest()
    {
        var config = new Config { maxNumberNeurons = 4, sizeX = 8, sizeY = 8 };
        var gene1 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Neuron,
            SourceNum = 0,
            SinkType = Gene.GeneType.Neuron,
            SinkNum = 0,
            WeightAsFloat = 1.0f
        };

        var builder = new GenomeBuilder(config.maxNumberNeurons, new[] { gene1.ToUint() });
        builder.SetupGenome();
        var nodeList = builder.MakeNodeList();
        Assert.Equal(1, nodeList[0].NumOutputs);
        Assert.Equal(1, nodeList[0].NumSelfInputs);
        Assert.Equal(0, nodeList[0].NumInputsFromSensorsOrOtherNeurons);
    }

    [Fact]
    public void MakeNodeList_SensorToNeuronTest()
    {
        var config = new Config { maxNumberNeurons = 4, sizeX = 8, sizeY = 8 };
        var gene1 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = Sensor.TRUE,
            SinkType = Gene.GeneType.Neuron,
            SinkNum = 0,
            WeightAsFloat = 1.0f
        };

        var builder = new GenomeBuilder(config.maxNumberNeurons, new[] { gene1.ToUint() });
        builder.SetupGenome();
        var nodeList = builder.MakeNodeList();
        Assert.Equal(0, nodeList[0].NumOutputs);
        Assert.Equal(0, nodeList[0].NumSelfInputs);
        Assert.Equal(1, nodeList[0].NumInputsFromSensorsOrOtherNeurons);
    }

    [Fact]
    public void RemoveUnusedNeurons_SensorToActionTest()
    {
        var config = new Config { maxNumberNeurons = 4, sizeX = 8, sizeY = 8 };
        var gene1 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = Sensor.TRUE,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.MOVE_NORTH,
            WeightAsFloat = 1.0f
        };

        var builder = new GenomeBuilder(config.maxNumberNeurons, new[] { gene1.ToUint() });
        builder.SetupGenome();
        builder.RemoveUnusedNeurons();
        Assert.Equal(0, builder.Neurons);
        Assert.Equal(1, builder.Length);
    }

    [Fact]
    public void RemoveUnusedNeurons_NeuronToActionTest()
    {
        var config = new Config { maxNumberNeurons = 4, sizeX = 8, sizeY = 8 };
        var gene1 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Neuron,
            SourceNum = 0,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.MOVE_FORWARD,
            WeightAsFloat = 1.0f
        };

        var builder = new GenomeBuilder(config.maxNumberNeurons, new[] { gene1.ToUint() });
        builder.SetupGenome();
        builder.RemoveUnusedNeurons();
        Assert.Equal(1, builder.Neurons);
        Assert.NotEmpty(builder);
    }

    [Fact]
    public void RemoveUnusedNeurons_NeuronToSameNeuronTest()
    {
        var config = new Config { maxNumberNeurons = 4, sizeX = 8, sizeY = 8 };
        var gene1 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Neuron,
            SourceNum = 0,
            SinkType = Gene.GeneType.Neuron,
            SinkNum = 0,
            WeightAsFloat = 1.0f
        };

        var builder = new GenomeBuilder(config.maxNumberNeurons, new[] { gene1.ToUint() });
        builder.SetupGenome();
        builder.RemoveUnusedNeurons();
        Assert.Equal(0, builder.Neurons);
        Assert.Empty(builder);
    }

    [Fact]
    public void RemoveUnusedNeurons_SensorToNeuronTest()
    {
        var config = new Config { maxNumberNeurons = 4, sizeX = 8, sizeY = 8 };
        var gene1 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = Sensor.TRUE,
            SinkType = Gene.GeneType.Neuron,
            SinkNum = 0,
            WeightAsFloat = 1.0f
        };

        var builder = new GenomeBuilder(config.maxNumberNeurons, new[] { gene1.ToUint() });
        builder.SetupGenome();
        builder.RemoveUnusedNeurons();
        Assert.Equal(0, builder.Neurons);
        Assert.Empty(builder);
    }


    ///////////////////////////

    [Fact]
    public void SingleNeuronToSingleAction_ReplaceWithTrueAction_Test()
    {
        var sensorFactory = new SensorFactory(
            new ISensor[]
            {
                new SensorMock(Sensor.TRUE, "T", 1.0f),
            }
        );

        var config = new Config { maxNumberNeurons = 4, sizeX = 8, sizeY = 8 };
        var board = new Board(config);
        var gene1 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Neuron,
            SourceNum = 0,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.MOVE_FORWARD,
            WeightAsFloat = 1.0f
        };

        var builder = new GenomeBuilder(config.maxNumberNeurons, new[] { gene1.ToUint() });
        var critter = board.NewCritter(builder.ToGenome(), new Coord(3, 3));

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        var neuronAccumulators = new float[config.maxNumberNeurons];
        critter.FeedForward(sensorFactory, actionLevels, neuronAccumulators, 0);

        var newGene0 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = Sensor.TRUE,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.MOVE_FORWARD,
            WeightAsFloat = 0.5f
        };
    }

    [Fact]
    public void CombineTwoNeurons_Test()
    {
        var sensorFactory = new SensorFactory(
            new ISensor[]
            {
                new SensorMock(Sensor.TRUE, "T", 1.0f),
            }
        );

        var config = new Config { maxNumberNeurons = 4, sizeX = 8, sizeY = 8 };
        var board = new Board(config);
        var dna = new[]
        {
            new GeneBuilder
            {
                SourceType = Gene.GeneType.Sensor,
                SourceSensor = Sensor.TRUE,
                SinkType = Gene.GeneType.Neuron,
                SinkNum = 0,
                WeightAsFloat = 1.0f
            }.ToUint(),
            new GeneBuilder
            {
                SourceType = Gene.GeneType.Sensor,
                SourceSensor = Sensor.TRUE,
                SinkType = Gene.GeneType.Neuron,
                SinkNum = 0,
                WeightAsFloat = 1.0f
            }.ToUint(),
            new GeneBuilder
            {
                SourceType = Gene.GeneType.Neuron,
                SourceNum = 0,
                SinkType = Gene.GeneType.Action,
                SinkAction = Action.MOVE_FORWARD,
                WeightAsFloat = 1.0f
            }.ToUint()
        };
        var builder = new GenomeBuilder(config.maxNumberNeurons, dna);
        var critter = board.NewCritter(builder.ToGenome(), new Coord(3, 3));

        var actionLevels = new float[Enum.GetNames<Action>().Length];
        var neuronAccumulators = new float[config.maxNumberNeurons];
        critter.FeedForward(sensorFactory, actionLevels, neuronAccumulators, 0);

        var newGene0 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Sensor,
            SourceSensor = Sensor.TRUE,
            SinkType = Gene.GeneType.Neuron,
            SinkNum = 0,
            WeightAsFloat = 2.0f
        };

        var newGene1 = new GeneBuilder
        {
            SourceType = Gene.GeneType.Neuron,
            SourceNum = 0,
            SinkType = Gene.GeneType.Action,
            SinkAction = Action.MOVE_FORWARD,
            WeightAsFloat = 1.0f
        };
    }


    [Fact]
    public void Test1()
    {
        var config = new Config
        {
            genomeMaxLength = 24,
            maxNumberNeurons = 12,
        };
        var dna = new uint[]
        {
            0x2649045B,
            0x514CA58A,
            0x7ACBe677,
            0x6A2212E0,
            0x3399EEB5,
            0x1B453CDE,
            0x5BCFBE75,
            0x4766fAED,
            0x9018129F,
            0x8BA265EB,
            0x677380A7,
            0xBE70A342,
            0x0C745DD6,
            0x72BD0662,
            0x6AB20437,
            0x8AFD008C,
            0x72FE8B81,
            0x13EF5141,
            0x29F58628,
            0x5E23fA09,
            0x7E4495DD,
            0x77C3AfA5,
            0x4E292DDC,
            0x87F32F07,
        };

        var builder = new GenomeBuilder(config.maxNumberNeurons, dna);
        var genome = builder.ToGenome();
        // var connectionList = genome.MakeRenumberedConnectionList();
    }

    [Fact]
    public void Test2()
    {
        var config = new Config
        {
            maxNumberNeurons = 1
        };
        var dna = new[]
        {
            0xc8257db1u,
            0x267b4645u,
            0x32470165u,
            0x61596307u,
        };

        var genome = new GenomeBuilder(config.maxNumberNeurons, dna);
        // ar connectionList = genome.MakeRenumberedConnectionList();
    }

    [Fact]
    public void Test3()
    {
        var config = new Config
        {
            maxNumberNeurons = 1
        };
        var dna = new[]
        {
            0x28e90f86u,
            0x44bf3514u,
            0x32430165u,
            0xb19a8864u,
        };

        var genome = new GenomeBuilder(config.maxNumberNeurons, dna);
        // var connectionList = genome.MakeRenumberedConnectionList();
    }

    [Fact]
    public void MakeNodeTest()
    {
        var config = new Config
        {
            maxNumberNeurons = 1
        };
        var dna = new[]
        {
            0x00850f86u,
            0x00833514u,
            0x00000165u,
            0x83868864u,
        };

        var genome = new GenomeBuilder(config.maxNumberNeurons, dna);
        // var data = genome.MakeNodeList();
        // Assert.Single(data);
    }

    [Fact]
    public void GeneticColorTest()
    {
        var config = new Config
        {
            maxNumberNeurons = 1
        };
        var dna = new[]
        {
            0x28e90f86u,
            0x44bf3514u,
            0x32430165u,
            0xb19a8864u,
        };

        var genome = new GenomeBuilder(config.maxNumberNeurons, dna);
        var color = genome.Color;
        Assert.Equal(0xFF, color.red);
        Assert.Equal(0xFF, color.green);
        Assert.Equal(0xFF, color.blue);
    }
}