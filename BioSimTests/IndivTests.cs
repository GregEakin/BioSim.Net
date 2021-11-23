using BioSimLib;
using BioSimLib.Sensors;
using Xunit;

namespace BioSimTests;

public class IndivTests
{
    public class SensorMock : ISensor
    {
        public Sensor Type => _type;
        public string ShortName => "Mock";

        public float Output(Params p, Indiv indiv, uint simStep)
        {
            return _output;
        }

        private readonly Sensor _type;
        private readonly string _name;
        private readonly float _output;

        public SensorMock(Sensor sensor, string name, float output)
        {
            _type = sensor;
            _name = name;
            _output = output;
        }
    }

    [Fact]
    public void Test1()
    {
        var sensors = new SensorFactory(
            new ISensor[]
            {
                new SensorMock(Sensor.LOC_X, "Lx", 0.8f),
                null, null, null, null, null, null, null, null, 
                null, null, null, null, null, null, null, null,
                new SensorMock(Sensor.RANDOM, "Rnd", 0.1f),
                null, null, null,
            }
        );

        var p = new Params() { maxNumberNeurons = 2 };
        var grid = new Grid(p);
        var dna = new[]
        {
            0x01012000u,
            0x00012000u,
            0x80002000u,
            0x01002000u,
            0x018A2000u,
            0x91842000u,
            0x01842000u,
        };

        var genome = new Genome(p, dna);
        var loc = new Coord { X = 4, Y = 4 };
        var individual = new Indiv(p, grid, 0, loc, genome);
        individual._nnet[0].Driven = true;
        individual._nnet[0].Output = 0.6f;
        individual._nnet[1].Driven = true;
        individual._nnet[1].Output = 0.4f;

        var actionLevels = individual.FeedForward(sensors, 0);
        Assert.Equal(0.7615942f, individual._nnet[0].Output);
        Assert.Equal(0.88535166f, individual._nnet[1].Output);

        Assert.Equal(new[]
        {
            0.0f, 0.0f, 0.0f, 0.0f, 0.8615942f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.7615942f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f
        }, actionLevels);
    }

    [Fact]
    public void Test2()
    {
        var sensors = new SensorFactory(
            new ISensor[]
            {
                new SensorMock(Sensor.LOC_X, "Lx", 0.2f),
                null, null, null, null, null, null, null, null, 
                null, null, null, null, null, null, null, null,
                new SensorMock(Sensor.RANDOM, "Rnd", 0.1f),
                null, null, null,
            }
        );

        var p = new Params() { maxNumberNeurons = 2 };
        var grid = new Grid(p);
        var dna = new[]
        {
            0x018A2000u,
            0x91842000u,
            0x01842000u,
            0x01012000u,
            0x00012000u,
            0x80002000u,
            0x01002000u,
        };

        var genome = new Genome(p, dna);
        var loc = new Coord { X = 4, Y = 4 };
        var individual = new Indiv(p, grid, 0, loc, genome);
        individual._nnet[0].Driven = true;
        individual._nnet[0].Output = 0.4f;
        individual._nnet[1].Driven = true;
        individual._nnet[1].Output = 0.6f;

        var actionLevels = individual.FeedForward2(sensors, 0);
        Assert.Equal(0.7615942f, individual._nnet[0].Output);
        Assert.Equal(0.5370496f, individual._nnet[1].Output);

        Assert.Equal(new[]
        {
            0.0f, 0.0f, 0.0f, 0.0f, 0.462117165f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.379948974f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f
        }, actionLevels);
    }

    [Fact]
    public void Test3()
    {
        var sensors = new SensorFactory(
            new ISensor[]
            {
                new SensorMock(Sensor.LOC_X, "Lx", 0.2f),
                null, null, null, null, null, null, null, null,
                null, null, null, null, null, null, null, null,
                new SensorMock(Sensor.RANDOM, "Rnd", 0.1f),
                null, null, null,
            }
        );

        var p = new Params() { maxNumberNeurons = 2 };
        var grid = new Grid(p);
        var dna = new[]
        {
            0x00012000u,
            0x01002000u,
            0x018A2000u,
            0x01842000u,
            0x01012000u,
            0x80002000u,
            0x91842000u,
        };

        var genome = new Genome(p, dna);
        var loc = new Coord { X = 4, Y = 4 };
        var individual = new Indiv(p, grid, 0, loc, genome);
        individual._nnet[0].Driven = true;
        individual._nnet[0].Output = 0.6f;
        individual._nnet[1].Driven = true;
        individual._nnet[1].Output = 0.4f;

        var actionLevels = individual.FeedForward3(sensors, 0);
        Assert.Equal(0.5773243f, individual._nnet[0].Output);
        Assert.Equal(0.916998565f, individual._nnet[1].Output);

        Assert.Equal(new[]
        {
            0.0f, 0.0f, 0.0f, 0.0f, 0.47961697f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.379948974f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f
        }, actionLevels);
    }
}