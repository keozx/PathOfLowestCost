using System;
using System.Collections.Generic;
using System.Text;

namespace PathMobile.Models
{
    public class Output
    {
        public Output()
        {
            Path = new List<int>();
        }
        public bool Solved { get; set; } = true;

        public int Cost { get; set; }

        public IList<int> Path { get; set; }
    }
}
