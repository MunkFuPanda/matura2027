using Robotersteuerung.Models;
using Robotersteuerung.Parser;
using AbcRobotCore;

namespace Robotersteuerung.Interpreter
{
    public class RobotInterpreter : IASTVisitor
    {
        private RobotFieldWrapper _fieldWrapper;
        public bool ShouldStop { get; set; }
        public List<string> ExecutionHistory { get; private set; }
        private List<string> _collectedLetters;

        public RobotInterpreter(RobotFieldWrapper fieldWrapper)
        {
            _fieldWrapper = fieldWrapper;
            ExecutionHistory = new List<string>();
            _collectedLetters = new List<string>();
            ShouldStop = false;
        }

        public void Execute(Program program)
        {
            ExecutionHistory.Clear();
            _collectedLetters.Clear();
            
            foreach (var command in program.Commands)
            {
                if (ShouldStop)
                    break;
                command.Accept(this);
            }
        }

        public string GetCollectedLetters()
        {
            if (_collectedLetters.Count == 0)
                return "(noch keine)";
            return string.Join(", ", _collectedLetters);
        }

        public void Visit(Program program)
        {
            foreach (var command in program.Commands)
            {
                if (ShouldStop)
                    break;
                command.Accept(this);
            }
        }

        public void Visit(MoveCommand command)
        {
            var abcDirection = DirectionConverter.ToAbcDirection(command.Direction);
            _fieldWrapper.Move(abcDirection);
            ExecutionHistory.Add($"→ MOVE {command.Direction}");
        }

        public void Visit(CollectCommand command)
        {
            _fieldWrapper.Collect();
            ExecutionHistory.Add($"→ COLLECT");
        }

        public void Visit(RepeatCommand command)
        {
            for (int i = 0; i < command.Count; i++)
            {
                if (ShouldStop)
                    break;
                foreach (var cmd in command.Commands)
                {
                    if (ShouldStop)
                        break;
                    cmd.Accept(this);
                }
            }
        }

        public void Visit(IfCommand command)
        {
            if (EvaluateCondition(command.Condition))
            {
                foreach (var cmd in command.Commands)
                {
                    if (ShouldStop)
                        break;
                    cmd.Accept(this);
                }
            }
        }

        public void Visit(UntilCommand command)
        {
            while (!EvaluateCondition(command.Condition))
            {
                if (ShouldStop)
                    break;
                foreach (var cmd in command.Commands)
                {
                    if (ShouldStop)
                        break;
                    cmd.Accept(this);
                }
            }
        }

        public void Visit(Condition condition)
        {
        }

        private bool EvaluateCondition(Condition condition)
        {
            var abcDirection = DirectionConverter.ToAbcDirection(condition.Direction);

            if (condition.Type == ConditionType.IS_OBSTACLE)
            {
                return _fieldWrapper.IsObstacle(abcDirection);
            }
            else if (condition.Type == ConditionType.IS_LETTER)
            {
                return _fieldWrapper.IsLetter(condition.Letter?.ToString() ?? "", abcDirection);
            }

            return false;
        }
    }
}
