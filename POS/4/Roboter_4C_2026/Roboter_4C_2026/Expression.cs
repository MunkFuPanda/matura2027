using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter_4C_2026
{
    internal abstract class Expression
    {
        internal static List<String> Errors = new List<string>();
        internal abstract void Parse(List<Token> tokens);
        internal virtual void Execute(RobotField roboter) 
        {
            // Defualt implementation does nothing, can be overridden by subclasses if needed
        }
    }
}
