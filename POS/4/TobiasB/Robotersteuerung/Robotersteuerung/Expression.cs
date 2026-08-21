using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robotersteuerung
{
    internal abstract class Expression
    {
        internal static List<String> Errors = new List<String>();
        internal abstract void Parse(List<Token> tokens);

        internal virtual void Execute(RobotField roboter)
        {
            
        }

    }
}
