//SPDX-License-Identifier: MIT

using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Tiger.Math
{
    public static class Damping
    {
        #region Double Precision
        public static double3 SmoothDamp(double3 current, double3 target, ref double3 currentVelocity, double smoothTime,
            double3 maxSpeed, double deltaTime)
        {
            if (smoothTime == 0) return target;
            
            var omega = 2d / smoothTime;

            var x = omega * deltaTime;
            var exp = 1d / (1d + x + 0.48 * x * x + 0.235 * x * x * x);

            var change = current - target;
            var originalTo = target;

            // Clamp maxSpeed
            var maxChange = maxSpeed * smoothTime;
            change = math.clamp(change, -maxChange, maxChange);
            target = current - change;

            var temp = (currentVelocity + omega * change) * deltaTime;
            currentVelocity = (currentVelocity - omega * temp) * exp;
            var output = target + (change + temp) * exp;

            // Prevent overshooting - FIXME - probably needs to treat all components separately.
            if (math.any(originalTo > current == output > originalTo))
            {
                output = originalTo;
                currentVelocity = (output - originalTo) / deltaTime;
            }

            return output;
        }
        
        //--------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Damper(double x, double g, double lambda, double dt)
        {
            return math.lerp(x, g, 1.0 - FastNegExp((0.69314718056 * dt) / (lambda + math.EPSILON_DBL)));
        }

        //--------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double FastNegExp(double x)
        {
            return 1.0 / (1.0 + x + 0.48f * x * x + 0.235f * x * x * x);
        }

        //--------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double Square(double x) { return x * x; }

        //--------------------------------------------------------------------
        /// <summary>
        /// Damper spring
        /// </summary>
        /// <param name="frequency">how many oscillations are done</param>
        /// <param name="lambda">how quick the goal is reached</param>
        public static void DamperSpring(ref double x, ref double v, double x_goal, double v_goal, double frequency, double lambda, double dt)
        {
            var g = x_goal;
            var q = v_goal;
            var s = frequency_to_stiffness(frequency);
            var d = lambda_to_damping(lambda);
            var c = g + (d * q) / (s + math.EPSILON_DBL);
            var y = d / 2.0;

            if (math.abs(s - (d * d) / 4.0) < math.EPSILON_DBL) // Critically Damped
            {
                var j0 = x - c;
                var j1 = v + j0 * y;

                var eydt = FastNegExp(y * dt);

                x = j0 * eydt + dt * j1 * eydt + c;
                v = -y * j0 * eydt - y * dt * j1 * eydt + j1 * eydt;
            }
            else if (s - (d * d) / 4.0 > 0.0) // Under Damped
            {
                double w = math.sqrt(s - (d * d) / 4.0);
                double j = math.sqrt(Square(v + y * (x - c)) / (w * w + math.EPSILON_DBL) + Square(x - c));
                double p = math.atan((v + (x - c) * y) / (-(x - c) * w + math.EPSILON_DBL));

                j = (x - c) > 0.0 ? j : -j;

                var eydt = FastNegExp(y * dt);

                x = j * eydt * math.cos(w * dt + p) + c;
                v = -y * j * eydt * math.cos(w * dt + p) - w * j * eydt * math.sin(w * dt + p);
            }
            else if (s - (d * d) / 4.0 < 0.0) // Over Damped
            {
                var y0 = (d + math.sqrt(d * d - 4 * s)) / 2.0;
                var y1 = (d - math.sqrt(d * d - 4 * s)) / 2.0;
                var j1 = (c * y0 - x * y0 - v) / (y1 - y0);
                var j0 = x - j1 - c;

                var ey0dt = FastNegExp(y0 * dt);
                var ey1dt = FastNegExp(y1 * dt);

                x = j0 * ey0dt + j1 * ey1dt + c;
                v = -y0 * j0 * ey0dt - y1 * j1 * ey1dt;
            }
        }

        //--------------------------------------------------------------------
        /// <summary>
        /// Damper spring set to critical (w = 0) spring goes to goal as fast as possible without oscillations, frequency is set by lambda
        /// </summary>
        public static void Critical(ref double3 x, ref double3 v, double3 x_goal, double3 v_goal, double lambda, double dt)
        {
            var g = x_goal;
            var q = v_goal;
            var d = lambda_to_damping(lambda);
            var c = g + (d * q) / ((d * d) / 4.0);
            var y = d / 2.0;
            var j0 = x - c;
            var j1 = v + j0 * y;
            var ey_dt = FastNegExp(y * dt);

            x = ey_dt * (j0 + j1 * dt) + c;
            v = ey_dt * (v - j1 * y * dt);
        }

        //--------------------------------------------------------------------
        /// <summary>
        /// Damper spring set to critical AND we assume the goal has no speed, very smooth! smoothness similar to unity smoothdamping
        /// </summary>
        public static void CriticalNoGoalSpeed( ref double3 x, ref double3 v, double3 x_goal, double lambda, double dt)
        {
            var y = lambda_to_damping(lambda) / 2.0;
            var j0 = x - x_goal;
            var j1 = v + j0 * y;
            var ey_dt = FastNegExp(y * dt);

            x = ey_dt * (j0 + j1 * dt) + x_goal;
            v = ey_dt * (v - j1 * y * dt);
        }

        //--------------------------------------------------------------------
        public static double critical_lambda(double frequency)
        {
            return damping_to_lambda(math.sqrt(frequency_to_stiffness(frequency) * 4.0));
        }

        //--------------------------------------------------------------------
        public static double critical_frequency(double lambda)
        {
            return stiffness_to_frequency(Square(lambda_to_damping(lambda)) / 4.0);
        }

        //--------------------------------------------------------------------
        private static double frequency_to_stiffness(double frequency)
        {
            return Square(2.0 * math.PI_DBL * frequency);
        }

        //--------------------------------------------------------------------
        private static double stiffness_to_frequency(double stiffness)
        {
            return math.sqrt(stiffness) / (2.0 * math.PI_DBL);
        }

        //--------------------------------------------------------------------
        private static double lambda_to_damping(double lambda)
        {
            return (4.0 * 0.69314718056) / (lambda + math.EPSILON_DBL);
        }

        //--------------------------------------------------------------------
        private static double damping_to_lambda(double damping)
        {
            return (4.0 * 0.69314718056) / (damping + math.EPSILON_DBL);
        }

        //--------------------------------------------------------------------
        #endregion
    }
}

//--------------------------------------------------------------------
/*
MIT License

Copyright (c) 2023 tiger.blue
Modified and ported to Unity Mathematics and double precision.


MIT License

Copyright (c) 2021 Meta

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/