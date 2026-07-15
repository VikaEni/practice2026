using System;
using System.Threading;
using System.Threading.Tasks;

namespace task14
{
    public static class DefiniteIntegral
    {
        public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
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

                Thread thread = new Thread(() =>
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

                thread.Start();
            }

            barrier.SignalAndWait();

            double totalSum = 0.0;
            for (int i = 0; i < threadsNumber; i++)
            {
                totalSum += partialResults[i];
            }

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

        public static double SolveSingleThread(double a, double b, Func<double, double> function, double step)
        {
            if (step <= 0) throw new ArgumentException(nameof(step));
            if (b <= a) throw new ArgumentException(nameof(b));

            return CalculateTrapezoidal(a, b, function, step);
        }
    }
}