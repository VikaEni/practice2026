using System;
using System.Threading;

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
            double totalSum = 0.0;

            using Barrier barrier = new Barrier(threadsNumber + 1);

            for (int i = 0; i < threadsNumber; i++)
            {
                double segmentA = a + i * segmentLength;
                double segmentB = (i == threadsNumber - 1) ? b : a + (i + 1) * segmentLength;

                double localA = segmentA;
                double localB = segmentB;

                Thread thread = new Thread(() =>
                {
                    try
                    {
                        double segmentResult = CalculateTrapezoidal(localA, localB, function, step);

                        double originalValue;
                        double newValue;
                        do
                        {
                            originalValue = totalSum;
                            newValue = originalValue + segmentResult;
                        }
                        while (Interlocked.CompareExchange(ref totalSum, newValue, originalValue) != originalValue);
                    }
                    finally
                    {
                        barrier.SignalAndWait();
                    }
                });

                thread.Start();
            }

            barrier.SignalAndWait();
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
    }
}