﻿using MoreLinq;
using NLES.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace NLES.Correction.Methods
{
    internal class RestoringMethod : IDisplacementSelector
    {
        public LoadIncrementalState SelectDisplacement(
            StructureInfo info
            , LoadState state
            , LoadIncrementalState prediction
            , IEnumerable<LoadIncrementalState> candidates)
        {
            double Function(LoadIncrementalState candidate)
            {
                Vector displacement = state.Displacement + candidate.IncrementDisplacement;
                Vector reaction = info.Reaction(displacement);
                double lambda = state.Lambda + candidate.IncrementLambda;
                Vector equilibriumVector = info.InitialLoad + lambda * info.ReferenceLoad - reaction;
                return equilibriumVector.Norm(2);
            }

            return candidates.MinBy(Function).First();
        }
    }
}
