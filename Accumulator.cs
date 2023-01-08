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

        public Accumulator()
        {
            _instructions = new List<int>();
        }

        public List<int> Instructions
        {
            get { return _instructions; }
            set { _instructions = value; }
        }
    }
}
