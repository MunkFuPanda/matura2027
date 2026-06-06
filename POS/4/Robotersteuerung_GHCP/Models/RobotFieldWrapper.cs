using AbcRobotCore;

namespace Robotersteuerung.Models
{
    /// <summary>
    /// Wraps the AbcRobotCore.RobotField custom control
    /// </summary>
    public class RobotFieldWrapper
    {
        private RobotField _robotField;

        public RobotFieldWrapper(RobotField robotField)
        {
            _robotField = robotField;
        }

        public void LoadField(string xmlPath)
        {
            _robotField.LoadField(xmlPath);
        }

        public void Move(AbcRobotCore.RobotField.Direction direction)
        {
            _robotField.Move(direction);
        }

        public string Collect()
        {
            _robotField.Collect();
            return "";
        }

        public bool IsLetter(string letter, AbcRobotCore.RobotField.Direction direction)
        {
            return _robotField.IsLetter(letter, direction);
        }

        public bool IsObstacle(AbcRobotCore.RobotField.Direction direction)
        {
            return _robotField.IsObstacle(direction);
        }
    }
}
