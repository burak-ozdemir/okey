using Okey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burak_Okey
{
    public class BillardCue
    {
        public int BillardCueId { get; set; }
        public List<RummyTile> TilesOnBoard { get; set; }
    }
}
