﻿using System;

namespace ProjectOlog.Code.Networking.Libs.MirrorInterpolation
{
    public struct ExponentialMovingAverage
    {
        readonly double alpha;
        bool initialized;

        public double Value;
        public double Variance;
        public double StandardDeviation; // absolute value, see test

        public ExponentialMovingAverage(int n)
        {
            // standard N-day EMA alpha calculation
            alpha = 2.0 / (n + 1);
            initialized = false;
            Value = 0;
            Variance = 0;
            StandardDeviation = 0;
        }

        public void Add(double newValue)
        {
            // simple algorithm for EMA described here:
            // https://en.wikipedia.org/wiki/Moving_average#Exponentially_weighted_moving_variance_and_standard_deviation
            if (initialized)
            {
                double delta = newValue - Value;
                Value += alpha * delta;
                Variance = (1 - alpha) * (Variance + alpha * delta * delta);
                StandardDeviation = Math.Sqrt(Variance);
            }
            else
            {
                Value = newValue;
                initialized = true;
            }
        }

        public void Reset()
        {
            initialized = false;
            Value = 0;
            Variance = 0;
            StandardDeviation = 0;
        }
    }
}