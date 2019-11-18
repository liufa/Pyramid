using System;
using System.Collections.Generic;
using System.Linq;

namespace Pyramid
{
    public class ProcessPyramid
    {
        public List<List<int>> Pyramid { get; }

        public ProcessPyramid(List<List<int>> pyramid)
        {
            Pyramid = pyramid;
        }

        public Tuple<IEnumerable<int>, int> GetMaxPathAndSum()
        {
            var pyramidPaths = this.GetPossiblePaths(this.Pyramid);
            var max = pyramidPaths.Max(o => o.Sum(oo => oo.Value));
            var maxList = pyramidPaths.Where(o => o.Sum(oo => oo.Value) == max).First();
            return new Tuple<IEnumerable<int>, int>(maxList.Select(o => o.Value), max);
        }

        private List<List<ValueAndPosition>> GetPossiblePaths(List<List<int>> list)
        {
            var currentLine = new List<ValueAndPosition> { new ValueAndPosition(list[0][0], 0) };
            var paths = new List<List<ValueAndPosition>> { currentLine };
            for (int i = 0; i < list.Count - 1; i++)
            {
                var newLongerPaths = new List<List<ValueAndPosition>>();

                for (int j = paths.Count - 1; j >= 0; j--)
                {
                    var currentElement = paths[j].Last();
                    var nextLine = list[i + 1];
                    var possibleStepsDown = GetPossibleStepsDown(IsEven(currentElement.Value), currentElement.Position, nextLine);

                    foreach (var stepDown in possibleStepsDown)
                    {
                        var newLongerPath = paths[j].Append(stepDown).ToList();
                        newLongerPaths.Add(newLongerPath);
                    }
                }

                paths = newLongerPaths;
            }

            return paths;
        }

        private bool IsEven(int number) { return number % 2 == 0; }

        private IEnumerable<ValueAndPosition> GetPossibleStepsDown(bool isEven, int position, List<int> list)
        {
            var shortlist = list.GetRange(Math.Max(0, position), Math.Min(list.Count - position, 2));
            var filteredShortList = isEven ? shortlist.Where(o => o % 2 != 0) : shortlist.Where(o => o % 2 == 0);
            return filteredShortList.Select(o => new ValueAndPosition(o, list.IndexOf(o)));
        }
    }
}
