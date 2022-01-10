using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Extensions
{
    public static class DistanceExtensions
    {
        private const float KILOMETERS = 1609.34F;

        public static float ToKilometers(this byte miles)
        {
            return KILOMETERS * miles;
        }
    }
}
