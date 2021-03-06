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

namespace BioSimLib.Positions;

public readonly struct Dir
{
    public enum Compass : byte
    {
        SW, S, SE, W, CENTER, E, NW, N, NE
    }

    private static readonly Random Random = new();
    public static Dir Random8() => new Dir(Compass.N).Rotate(Random.Next(0, 7));

    public Dir(Compass dir = Compass.CENTER) => _dir9 = dir;

    // public static implicit operator Dir(Compass d) => new Dir(d);

    private readonly Compass _dir9;

    public byte AsInt() => (byte)_dir9;

    public Coord AsNormalizedCoord()
    {
        var d = AsInt();
        return new Coord { X = (short)(d % 3 - 1), Y = (short)(d / 3 - 1) };
    }

    public Polar AsNormalizedPolar()
    {
        return new Polar(1, _dir9);
    }

    public Dir Rotate(int n = 0)
    {
        var rotateRight = new byte[] { 3, 0, 1, 6, 4, 2, 7, 8, 5 };
        var rotateLeft = new byte[] { 1, 2, 5, 0, 4, 8, 3, 6, 7 };
        var n9 = AsInt();
        switch (n)
        {
            case < 0:
                {
                    while (n++ < 0)
                    {
                        n9 = rotateLeft[n9];
                    }

                    break;
                }
            case > 0:
                {
                    while (n-- > 0)
                    {
                        n9 = rotateRight[n9];
                    }

                    break;
                }
        }

        return new Dir((Compass)n9);
    }

    public Dir Rotate90DegCw() => Rotate(2);
    public Dir Rotate90DegCcw() => Rotate(-2);
    public Dir Rotate180Deg() => Rotate(4);

    public bool Equals(Dir other) => _dir9 == other._dir9;
    public override bool Equals(object? obj) => obj is Dir other && Equals(other);
    public override int GetHashCode() => (int)_dir9;

    public static bool operator ==(Dir dir, Compass compass) => dir.AsInt() == (byte)compass;
    public static bool operator !=(Dir dir, Compass compass) => dir.AsInt() != (byte)compass;
    public static bool operator ==(Dir dir1, Dir dir2) => dir1.AsInt() == dir2.AsInt();
    public static bool operator !=(Dir dir1, Dir dir2) => dir1.AsInt() != dir2.AsInt();
}