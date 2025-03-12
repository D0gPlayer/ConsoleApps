// See https://aka.ms/new-console-template for more information
using ConsoleApps;
using ConsoleApps.MathGame;
using ConsoleApps.ProgressGame;
using ConsoleApps.Prototypes;
var analyticsService =  new MultiThreadedProcessingService();
await analyticsService.StartProcessingData();
Console.ReadKey();

