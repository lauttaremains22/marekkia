using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marekkia
{
    public class Accumulator
    {

        private List<int> _instructions;

        private Dictionary<int, string> translator = new Dictionary<int, string>()
        {
            {0, "NORTH_ARROW"},
            {1, "SOUTH_ARROW"},
            {2, "EAST_ARROW"},
            {3, "WEST_ARROW"},
            {4, "NORTH_EAST_ARROW"},
            {5, "NORTH_WEST_ARROW"},
            {6, "SOUTH_EAST_ARROW"},
            {7, "SOUTH_WEST_ARROW"},
            {8, "DEFAULT"}
        };

        private List<string> translatedInstr = new List<string>();

        public Accumulator()
        {
            _instructions = new List<int>();
        }

        public List<int> Instructions
        {
            get { return _instructions; }
            set { _instructions = value; }
        }

        public void SetInstruction(int instruction)
        {
            _instructions.Add(instruction);

            string translated = translator.GetValueOrDefault(instruction);
            translatedInstr.Add(translated);
        }
    }
}
