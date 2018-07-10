using PathMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PathMobile.Services
{
    public interface IPathResolver
    {
        NodeData FindShortest();
    }
}
