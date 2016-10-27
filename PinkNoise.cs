using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Limestone.Utility;
using Limestone.Buffs;

using Superbest_random;
//Converted from java. Source: http://sampo.kapsi.fi/PinkNoise/PinkNoise.java
namespace Limestone
{
    public class PinkNoise
    {
        private int poles;
        private double[] multipliers;

        private double[] values;
        private Random rnd;

        /**
         * Generate pink noise from a specific randomness source
         * specifying alpha and the number of poles.  The larger the
         * number of poles, the lower are the lowest frequency components
         * that are amplified.
         * 
         * @param alpha   the exponent of the pink noise, 1/f^alpha.
         * @param poles   the number of poles to use.
         * @param random  the randomness source.
         */
        public PinkNoise(double alpha, int poles, Random random)
        {
            if (alpha < 0 || alpha > 2)
            {
                Logger.Log("Tried to use an alpha value too high or too low! Must be >= 0 and <= 2", true);
            }

            this.rnd = random;
            this.poles = poles;
            this.multipliers = new double[poles];
            this.values = new double[poles];

            double a = 1;
            for (int i = 0; i < poles; i++)
            {
                a = (i - alpha / 2) * a / (i + 1);
                multipliers[i] = a;
            }

            // Fill the history with random values
            for (int i = 0; i < 5 * poles; i++)
                this.nextValue();
        }


        /**
         * Return the next pink noise sample.
         *
         * @return  the next pink noise sample.
         */
        public double nextValue()
        {
            /*
             * The following may be changed to  rnd.nextDouble()-0.5
             * if strict Gaussian distribution of resulting values is not
             * required.
             */
            double x = rnd.NextGaussian();

            for (int i = 0; i < poles; i++)
            {
                x -= multipliers[i] * values[i];
            }
            Array.Copy(values, 0, values, 1, values.Length - 1);
            values[0] = x;

            return x;
        }
    }
}
