using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algebra.Equivalence
{
    public class EquivalenceClass
    {
        public static readonly List<EquivalencePath> DEFAULT_PATHS = new List<EquivalencePath> {
            EquivalencePaths.IDENTITY,
            EquivalencePaths.EXPAND_BRACES
        };

        private readonly Expression anchorEquation; // The equation used to define the equivalence class
        private readonly List<EquivalencePath> paths;

        public EquivalenceClass(Expression anchorEquation, List<EquivalencePath> paths = null)
        {
            this.paths = paths ?? DEFAULT_PATHS;
            this.anchorEquation = anchorEquation;
        }
    }
}
