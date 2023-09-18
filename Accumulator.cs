using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marekkia
{
    public class Accumulator
    {

        List<CellModel> _instructions = new List<CellModel>();


        public Accumulator()
        {
            _instructions = new List<CellModel>();
        }

        public List<CellModel> Instructions
        {
            get { return _instructions; }
            set { _instructions = value; }
        }

        public void SetModel(CellModel model)
        {
            _instructions.Add(model);
        }
    }
}
