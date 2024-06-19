using System;
using System.Diagnostics;

namespace Bullet_Hell_Game
{
    public class Iterator
    {
        private float Iteration;
        private float IteratorStep;
        private float IteratorStart;
        private float IteratorEnd;
        private bool IteratorReset;

        public Iterator(float iteratorStart, float iteratorStep, float iteratorEnd, bool iteratorReset)
        {
            IteratorStep = iteratorStep;
            IteratorStart = iteratorStart;
            IteratorEnd = iteratorEnd;
            IteratorReset = iteratorReset;
            Iteration = IteratorStart;
        }

        public Iterator(float iteratorStep, float iteratorStart) :this(iteratorStep, iteratorStart, 0, false) { }

        public float Iterate()
        {
            if (IteratorReset && (MathF.Abs(Iteration) >= MathF.Abs(IteratorEnd)))
            {
                Iteration = IteratorStart;
            } else
            {
                Iteration += IteratorStep;
            }

            return Iteration;
        }


    }
}
