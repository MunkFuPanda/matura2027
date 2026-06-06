
using AbcRobotCore;

namespace Roboter_4C_2026
{
    internal class CollectExpression : Expression
    {
        internal override void Parse(List<Token> tokens) { }

        internal override void Execute(RobotField roboter)
        {
            String result = roboter.Collect();
            if (result == null || result == "")
            {
                Errors.Add("Failed to collect item. Possible no item at position");
            }
        }
    }
}