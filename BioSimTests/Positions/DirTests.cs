//    Copyright 2022 Gregory Eakin
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

using BioSimLib.Positions;
using Xunit;

namespace BioSimTests.Positions;

public class DirTests
{
    [Fact]
    public void WestIntTest()
    {
        var dir = new Dir(Dir.Compass.W);
        Assert.Equal(3, dir.AsInt());
    }

    [Fact]
    public void WestCoordTest()
    {
        var dir = new Dir(Dir.Compass.W);
        var normalizedCoord = dir.AsNormalizedCoord();
        Assert.Equal(-1, normalizedCoord.X);
        Assert.Equal(0, normalizedCoord.Y);
    }

    [Fact]
    public void WestPolarTest()
    {
        var dir = new Dir(Dir.Compass.W);
        var normalizedPolar = dir.AsNormalizedPolar();
        Assert.Equal(1, normalizedPolar.Mag);
        Assert.Equal(new Dir(Dir.Compass.W), normalizedPolar.Dir);
    }

    [Fact]
    public void WestRotateNorthTest()
    {
        var dir = new Dir(Dir.Compass.W);
        var newDir = dir.Rotate(2);
        Assert.Equal(new Dir(Dir.Compass.N), newDir);
    }

    [Fact]
    public void WestRotateSouthTest()
    {
        var dir = new Dir(Dir.Compass.W);
        var newDir = dir.Rotate(-2);
        Assert.Equal(new Dir(Dir.Compass.S), newDir);
    }

    [Fact]
    public void CenterIntTest()
    {
        var dir = new Dir(Dir.Compass.CENTER);
        Assert.Equal(4, dir.AsInt());
    }

    [Fact]
    public void CenterCoordTest()
    {
        var dir = new Dir(Dir.Compass.CENTER);
        var normalizedCoord = dir.AsNormalizedCoord();
        Assert.Equal(0, normalizedCoord.X);
        Assert.Equal(0, normalizedCoord.Y);
    }

    [Fact]
    public void CenterPolarTest()
    {
        var dir = new Dir(Dir.Compass.CENTER);
        var normalizedPolar = dir.AsNormalizedPolar();
        Assert.Equal(1, normalizedPolar.Mag);
        Assert.Equal(new Dir(Dir.Compass.CENTER), normalizedPolar.Dir);
    }

    [Fact]
    public void CenterRotateNorthTest()
    {
        var dir = new Dir(Dir.Compass.CENTER);
        var newDir = dir.Rotate(2);
        Assert.Equal(new Dir(Dir.Compass.CENTER), newDir);
    }

    [Fact]
    public void CenterRotateSouthTest()
    {
        var dir = new Dir(Dir.Compass.CENTER);
        var newDir = dir.Rotate(-2);
        Assert.Equal(new Dir(Dir.Compass.CENTER), newDir);
    }

    [Fact]
    public void RotateCwTest()
    {
        var dir = new Dir(Dir.Compass.N);
        var expected = new[]
        {
            Dir.Compass.N,
            Dir.Compass.NE,
            Dir.Compass.E, 
            Dir.Compass.SE,
            Dir.Compass.S, 
            Dir.Compass.SW,
            Dir.Compass.W, 
            Dir.Compass.NW,
            Dir.Compass.N,
        };

        for(var i = 0; i < expected.Length; i++) 
            Assert.Equal(new Dir(expected[i]), dir.Rotate(i));
        
        var center = new Dir(Dir.Compass.CENTER);
        for (var i = 0; i < expected.Length; i++)
            Assert.Equal(new Dir(Dir.Compass.CENTER), center.Rotate(i));
    }

    [Fact]
    public void RotateCcwTest()
    {
        var dir = new Dir(Dir.Compass.N);
        var expected = new[]
        {
            Dir.Compass.N,
            Dir.Compass.NW,
            Dir.Compass.W,
            Dir.Compass.SW,
            Dir.Compass.S,
            Dir.Compass.SE,
            Dir.Compass.E,
            Dir.Compass.NE,
            Dir.Compass.N,
        };

        for (var i = 0; i < expected.Length; i++)
            Assert.Equal(new Dir(expected[i]), dir.Rotate(-i));

        var center = new Dir(Dir.Compass.CENTER);
        for (var i = 0; i < expected.Length; i++)
            Assert.Equal(new Dir(Dir.Compass.CENTER), center.Rotate(i));
    }
}