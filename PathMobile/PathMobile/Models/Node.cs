﻿using System;
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
    public class NodeData
    {
        public int index;
        public string data;
        public int Distance { get; set; }
        public bool IsEnd { get; set; }
        public List<int> Path { get; set; }
        public NodeData(string data, int index)
        {
            this.index = index;
            this.data = data;
        }
    }
}
