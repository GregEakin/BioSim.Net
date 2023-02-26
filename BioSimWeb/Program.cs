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

using BioSimLib.Challenges;
using BioSimLib;
using BioSimLib.BarrierFactory;
using BioSimLib.Field;
using BioSimWeb;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BioSimLib.Actions;
using BioSimLib.Sensors;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// var config = new Config
// {
//     sizeX = 128,
//     sizeY = 128,
//     population = 1000,
//     stepsPerGeneration = 300,
//     genomeMaxLength = 24,
//     maxNumberNeurons = 12,
//     populationSensorRadius = 10,
//     signalSensorRadius = 10,
//     shortProbeBarrierDistance = 4,
//     longProbeDistance = 10,
//     signalLayers = 1,
//     challenge = Challenge.CornerWeighted,
// };
//
// builder.Services.AddSingleton(config);
// builder.Services.AddSingleton<Board>();
// builder.Services.AddSingleton<BarrierFactory>();
// builder.Services.AddSingleton<ChallengeFactory>();
// builder.Services.AddSingleton<SensorFactory>();
// builder.Services.AddSingleton<ActionFactory>();
// _cells = new Cell[_config.population];
// _neuronAccumulators = new float[_config.maxNumberNeurons];


await builder.Build().RunAsync();
