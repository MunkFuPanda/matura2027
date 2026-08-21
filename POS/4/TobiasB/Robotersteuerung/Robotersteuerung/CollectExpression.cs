
using AbcRobotCore;

namespace Robotersteuerung
{
    internal class CollectExpression : Expression
    {
        internal override void Parse(List<Token> tokens)
        {
            throw new NotImplementedException();
        }

        internal override void Execute(RobotField roboter)
        {
            string result = roboter.Collect();
            if (result == null || result == "")
            {
                Errors.Add("RUNING: Failed to collect");
            }
                
        }
    }
}