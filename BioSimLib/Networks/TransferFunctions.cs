//    Copyright 2023 Gregory Eakin
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

namespace BioSimLib.Networks;

public class TransferFunctions
{
    public float Linear(float x, bool derivative = false)
    {
        return derivative 
            ? 1.0f 
            : x;
    }

    public float Sigmoid(float x, bool derivative = false)
    {
        var sigmoid = 1.0f / (1.0f + float.Exp(-x));
        return derivative 
            ? sigmoid * (1.0f - sigmoid) 
            : sigmoid;
    }

    public float HyperbolicTangent(float x, bool derivative = false)
    {
        var tanh = (float)Math.Tanh(x);
        return derivative 
            ? 1.0f - tanh * tanh 
            : tanh;
    }

    public float Gaussian(float x, bool derivative = false)
    {
        var exp = float.Exp(-x * -x);
        return derivative 
            ? -2.0f * x * exp 
            : exp;
    }

    public float Step(float x, float t, bool derivative = false)
    {
        return derivative 
            ? 0.0f 
            : x <= t ? 0.0f : 1.0f;
    }

    public float Ramp(float x, float t1, float t2, bool derivative = false)
    {
        if (derivative) return x < t1 || x > t2 ? 0.0f : 1.0f;
        if (x < t1) return 0;
        if (x > t2) return 1.0f;
        return (x - t1) / (t2 - t1);
    }

    public float Relu(float x, bool derivative = false)
    {
        return derivative 
            ? x < 0 ? 0.0f : 1.0f 
            : (float)Math.Max(0.0, x);
    }
}