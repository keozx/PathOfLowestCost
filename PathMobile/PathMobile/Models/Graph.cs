using System;
using System.Collections.Generic;
using System.Text;

namespace PathMobile.Models
{
    public class AdjListNode
    {

        public AdjListNode(int _v, int _w) { V = _v; Weight = _w; }
        public int V { get; set; }
        public int Weight { get; set; }
    }

}
