using BioSimLib.Positions;

namespace BioSimLib.Field;

public class Signals
{
    private readonly Config _p;
    private readonly byte[,,] _board;

    public Signals(Config p)
    {
        _p = p;
        _board = new byte[p.signalLayers, p.sizeX, p.sizeY];
    }

    public byte GetMagnitude(uint layerNum, Coord loc) => _board[layerNum, loc.X, loc.Y];

    public float GetSignalDensity(uint layerNum, Coord loc)
    {
        var countLocs = 0u;
        var sum = 0uL;
        var center = loc;

        void F(Coord tloc) {
            ++countLocs;
            sum += GetMagnitude(layerNum, tloc);
        };

        VisitNeighborhood(center, _p.signalSensorRadius, F);
        var maxSum = (float)countLocs * byte.MaxValue;
        var sensorVal = sum / maxSum; 

        return sensorVal;
    }

    public void VisitNeighborhood(Coord loc, float radius, Action<Coord> f)
    {
        for (var dx = -Math.Min((int)radius, loc.X); dx <= Math.Min((int)radius, _p.sizeX - loc.X - 1); ++dx)
        {
            var x = loc.X + dx;
            var extentY = (int)Math.Sqrt(radius * radius - dx * dx);
            for (int dy = -Math.Min(extentY, loc.Y); dy <= Math.Min(extentY, _p.sizeY - loc.Y - 1); ++dy)
            {
                var y = loc.Y + dy;
                f(new Coord { X = (short)x, Y = (short)y} );
            }
        }
    }

    public float GetSignalDensityAlongAxis(uint layerNum, Coord loc, Dir dir)
    {
        var sum = 0.0;
        void F(Coord tloc)
        {
            if (tloc == loc) return;
            var offset = tloc - loc;
            var anglePosCos = offset.RaySameness(dir);
            var dist = Math.Sqrt((double)offset.X * offset.X + (double)offset.Y * offset.Y);
            var contrib = (1.0 / dist) * anglePosCos * GetMagnitude(layerNum, loc);
            sum += contrib;
        };

        VisitNeighborhood(loc, _p.signalSensorRadius, F);
        var maxSumMag = 6.0 * _p.signalSensorRadius * byte.MaxValue;
        var sensorVal = sum / maxSumMag;
        sensorVal = (sensorVal + 1.0) / 2.0;

        return (float)sensorVal;
    }

    public void Increment(int i, Coord loc)
    {
    }

    public void ZeroFill()
    {
    }
}