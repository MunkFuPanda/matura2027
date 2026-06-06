using Robotersteuerung.Models;

namespace Robotersteuerung.Parser
{
    // Abstract Syntax Tree (AST) Nodes
    public abstract class ASTNode
    {
        public abstract void Accept(IASTVisitor visitor);
    }

    public class Program : ASTNode
    {
        public List<Command> Commands { get; set; }

        public Program(List<Command> commands)
        {
            Commands = commands;
        }

        public override void Accept(IASTVisitor visitor) => visitor.Visit(this);
    }

    public abstract class Command : ASTNode
    {
    }

    public class MoveCommand : Command
    {
        public Direction Direction { get; set; }

        public MoveCommand(Direction direction)
        {
            Direction = direction;
        }

        public override void Accept(IASTVisitor visitor) => visitor.Visit(this);
    }

    public class CollectCommand : Command
    {
        public override void Accept(IASTVisitor visitor) => visitor.Visit(this);
    }

    public class RepeatCommand : Command
    {
        public int Count { get; set; }
        public List<Command> Commands { get; set; }

        public RepeatCommand(int count, List<Command> commands)
        {
            Count = count;
            Commands = commands;
        }

        public override void Accept(IASTVisitor visitor) => visitor.Visit(this);
    }

    public class IfCommand : Command
    {
        public Condition Condition { get; set; }
        public List<Command> Commands { get; set; }

        public IfCommand(Condition condition, List<Command> commands)
        {
            Condition = condition;
            Commands = commands;
        }

        public override void Accept(IASTVisitor visitor) => visitor.Visit(this);
    }

    public class UntilCommand : Command
    {
        public Condition Condition { get; set; }
        public List<Command> Commands { get; set; }

        public UntilCommand(Condition condition, List<Command> commands)
        {
            Condition = condition;
            Commands = commands;
        }

        public override void Accept(IASTVisitor visitor) => visitor.Visit(this);
    }

    // Condition
    public class Condition : ASTNode
    {
        public Direction Direction { get; set; }
        public ConditionType Type { get; set; }
        public char? Letter { get; set; }

        public Condition(Direction direction, ConditionType type, char? letter = null)
        {
            Direction = direction;
            Type = type;
            Letter = letter;
        }

        public override void Accept(IASTVisitor visitor) => visitor.Visit(this);
    }

    public enum ConditionType
    {
        IS_OBSTACLE,
        IS_LETTER
    }

    public interface IASTVisitor
    {
        void Visit(Program program);
        void Visit(MoveCommand command);
        void Visit(CollectCommand command);
        void Visit(RepeatCommand command);
        void Visit(IfCommand command);
        void Visit(UntilCommand command);
        void Visit(Condition condition);
    }
}
