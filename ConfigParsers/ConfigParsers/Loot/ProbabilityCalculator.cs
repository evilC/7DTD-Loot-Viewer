using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigParsers.Loot
{
    public class ProbabilityCalculator
    {
        private ItemContainer _containerResult;

        public ProbabilityCalculator(ItemContainer containerResult)
        {
            _containerResult = containerResult;
        }

        public void CalculateProbability(int lootLevel)
        {
            for (int nl = 0; nl < _containerResult.Paths.Count; nl++)
            {
                var nodeList = _containerResult.Paths[nl].Nodes;
                Debug.WriteLine($"PATH FOR RESULT #{nl}:");
                foreach (var node in nodeList)
                {
                    //var str = $"{node.Render()} / Prob = {node.GetProb(lootLevel)}";
                    //Debug.WriteLine($"{str}");
                    Debug.WriteLine($"{node.Group.Render()} - Prob: {node.Group}");
                }
                Debug.WriteLine($"");
            }

        }
    }
}
