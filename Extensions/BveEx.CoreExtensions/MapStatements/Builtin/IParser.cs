﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Extensions.MapStatements.Builtin
{
    internal interface IParser
    {
        bool CanParse(Statement statement);
        void Parse(Statement statement);
    }
}
