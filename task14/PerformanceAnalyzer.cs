using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ScottPlot;

namespace task14
{
    public static class PerformanceAnalyzer
    {
        private const double A = -100;
        private const double B = 100;
        private const int WarmupIterations = 10;
        private const int MeasureIterations = 100;
        private const double RequiredAccuracy = 1e-4;
        private readonly static double ExactValue = -Math.Cos(B) + Math.Cos(A);

        public static void RunAnalysis()
        {
            double optimalStep = FindOptimalStep();
            Console.WriteLine($"Optimal step: {optimalStep:E}");

            var results = FindOptimalThreads(optimalStep);
            var optimalThreads = results.OrderBy(x => x.Value).First();
            Console.WriteLine($"Optimal threads: {optimalThreads.Key}");
            Console.WriteLine($"Execution time: {optimalThreads.Value:F2} ms");

            CreatePlot(results);
            CompareWithSingleThread(results, optimalStep);
        }

        private static double FindOptimalStep()
        {
            var steps = new double[] { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
            var sin = (double x) => Math.Sin(x);
            double selectedStep = steps.Last();

            Console.WriteLine("Step\t\tError");

            foreach (var step in steps)
            {
                double result = DefiniteIntegral.Solve(A, B, sin, step, 4);
                double error = Math.Abs(result - ExactValue);
                Console.WriteLine($"{step:E}\t{error:E}");

                if (error < RequiredAccuracy)
                {
                    selectedStep = step;
                    break;
                }
            }

            if (selectedStep > 1e-4)
            {
                selectedStep = 1e-4;
                Console.WriteLine($"Using step 1e-4 for performance measurements (better accuracy)");
            }

            return selectedStep;
        }

        private static Dictionary<int, double> FindOptimalThreads(double step)
        {
            var sin = (double x) => Math.Sin(x);
            var results = new Dictionary<int, double>();
            var threadCounts = new int[] { 1, 2, 4, 6, 8, 10, 12, 16, 20, 24, 32 };

            foreach (int threads in threadCounts)
            {
                for (int i = 0; i < WarmupIterations; i++)
                    DefiniteIntegral.Solve(A, B, sin, step, threads);

                var sw = Stopwatch.StartNew();
                for (int i = 0; i < MeasureIterations; i++)
                    DefiniteIntegral.Solve(A, B, sin, step, threads);
                sw.Stop();

                double avgTime = sw.Elapsed.TotalMilliseconds / MeasureIterations;
                results[threads] = avgTime;
                Console.WriteLine($"Threads: {threads,2}, Time: {avgTime,8:F2} ms");
            }

            return results;
        }

        private static void CreatePlot(Dictionary<int, double> results)
        {
            var sortedData = results.OrderBy(x => x.Key).ToList();
            var xs = sortedData.Select(x => (double)x.Key).ToArray();
            var ys = sortedData.Select(x => x.Value).ToArray();

            var plt = new Plot();
            var scatter = plt.Add.Scatter(xs, ys);
            scatter.LegendText = "Execution Time";
            scatter.LineWidth = 2;
            scatter.MarkerSize = 8;

            var optimal = sortedData.OrderBy(x => x.Value).First();
            double optimalX = optimal.Key;
            double optimalY = optimal.Value;

            var marker = plt.Add.Marker(optimalX, optimalY);
            marker.Shape = ScottPlot.MarkerShape.FilledDiamond;
            marker.Size = 12;
            marker.Color = new ScottPlot.Color(255, 0, 0);
            marker.LegendText = $"Optimal: {optimalX} threads ({optimalY:F2} ms)";

            plt.Title($"Performance: Execution Time vs Thread Count");
            plt.XLabel("Number of Threads");
            plt.YLabel("Average Execution Time (ms)");
            plt.ShowLegend(ScottPlot.Alignment.UpperRight);

            string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "performance_graph.png");
            plt.SavePng(outputPath, 800, 600);
            Console.WriteLine($"Graph saved to: {outputPath}");
        }

        private static void CompareWithSingleThread(Dictionary<int, double> multiThreadResults, double step)
        {
            var sin = (double x) => Math.Sin(x);
            int optimalThreads = multiThreadResults.OrderBy(x => x.Value).First().Key;
            double multiAvg = multiThreadResults[optimalThreads];

            for (int i = 0; i < WarmupIterations; i++)
                DefiniteIntegral.SolveSingleThread(A, B, sin, step);

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < MeasureIterations; i++)
                DefiniteIntegral.SolveSingleThread(A, B, sin, step);
            sw.Stop();

            double singleAvg = sw.Elapsed.TotalMilliseconds / MeasureIterations;
            double speedup = singleAvg / multiAvg;
            double improvement = (singleAvg - multiAvg) / singleAvg * 100;

            Console.WriteLine($"Single thread: {singleAvg:F2} ms");
            Console.WriteLine($"Multi thread ({optimalThreads} threads): {multiAvg:F2} ms");
            Console.WriteLine($"Speedup: {speedup:F2}x");
            Console.WriteLine($"Improvement: {improvement:F2}%");

            if (improvement >= 15)
            {
                Console.WriteLine("✓ Multi-thread is faster by more than 15%");
                SaveSummary(step, optimalThreads, singleAvg, multiAvg, speedup, improvement);
            }
            else
            {
                Console.WriteLine("Optimizing...");
                var optimized = OptimizeAndCompare(singleAvg, step);
                SaveSummary(step, optimized.Threads, singleAvg, optimized.Time,
                    singleAvg / optimized.Time, optimized.Improvement);
            }
        }

        private static (int Threads, double Time, double Improvement) OptimizeAndCompare(double singleAvg, double step)
        {
            var sin = (double x) => Math.Sin(x);
            var threadCounts = new int[] { 2, 4, 6, 8, 10, 12, 16 };

            double bestTime = double.MaxValue;
            int bestThreads = 1;

            foreach (int threads in threadCounts)
            {
                for (int i = 0; i < WarmupIterations; i++)
                    OptimizedSolve(A, B, sin, step, threads);

                var sw = Stopwatch.StartNew();
                for (int i = 0; i < MeasureIterations; i++)
                    OptimizedSolve(A, B, sin, step, threads);
                sw.Stop();

                double avgTime = sw.Elapsed.TotalMilliseconds / MeasureIterations;
                double improvement = (singleAvg - avgTime) / singleAvg * 100;
                Console.WriteLine($"Threads: {threads,2}, Time: {avgTime,8:F2} ms, Imp: {improvement,6:F2}%");

                if (avgTime < bestTime)
                {
                    bestTime = avgTime;
                    bestThreads = threads;
                }
            }

            double bestImprovement = (singleAvg - bestTime) / singleAvg * 100;
            Console.WriteLine($"Optimized: {bestTime:F2} ms, Improvement: {bestImprovement:F2}%");

            return (bestThreads, bestTime, bestImprovement);
        }

        private static double OptimizedSolve(double a, double b, Func<double, double> function, double step, int threadsNumber)
        {
            if (threadsNumber <= 0) throw new ArgumentException(nameof(threadsNumber));
            if (step <= 0) throw new ArgumentException(nameof(step));
            if (b <= a) throw new ArgumentException(nameof(b));

            double totalLength = b - a;
            double segmentLength = totalLength / threadsNumber;
            double[] partialResults = new double[threadsNumber];

            using Barrier barrier = new Barrier(threadsNumber + 1);

            for (int i = 0; i < threadsNumber; i++)
            {
                int index = i;
                double segmentA = a + index * segmentLength;
                double segmentB = (index == threadsNumber - 1) ? b : a + (index + 1) * segmentLength;

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        partialResults[index] = CalculateTrapezoidal(segmentA, segmentB, function, step);
                    }
                    finally
                    {
                        barrier.SignalAndWait();
                    }
                });
            }

            barrier.SignalAndWait();

            double totalSum = 0.0;
            for (int i = 0; i < threadsNumber; i++)
                totalSum += partialResults[i];

            return totalSum;
        }

        private static double CalculateTrapezoidal(double a, double b, Func<double, double> function, double step)
        {
            int stepsCount = (int)Math.Ceiling((b - a) / step);
            double actualStep = (b - a) / stepsCount;
            double sum = 0.0;

            for (int i = 0; i <= stepsCount; i++)
            {
                double x = a + i * actualStep;
                double y = function(x);

                if (i == 0 || i == stepsCount)
                    sum += y / 2.0;
                else
                    sum += y;
            }

            return sum * actualStep;
        }

        private static void SaveSummary(double step, int threads, double singleTime, double multiTime, double speedup, double improvement)
        {
            using var writer = new StreamWriter("results_summary.txt");
            writer.WriteLine($"Step: {step:E}");
            writer.WriteLine($"Optimal threads: {threads}");
            writer.WriteLine($"Single thread time: {singleTime:F2} ms");
            writer.WriteLine($"Multi thread time: {multiTime:F2} ms");
            writer.WriteLine($"Speedup: {speedup:F2}x");
            writer.WriteLine($"Improvement: {improvement:F2}%");
        }
    }
}