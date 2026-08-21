using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rubik_s_Cube
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ModelVisual3D> boxes;
        private AxisAngleRotation3D rotation;
        private RotateTransform3D transform;
        private DoubleAnimation animation;
        private Point3D center;

        private bool animationRunning = false;

        Random r = new Random();

        public MainWindow()
        {
            this.InitializeComponent();
            center = new Point3D(0, 0, 2.93);
            boxes =
            [
                Cube001,
                Cube002,
                Cube003,
                Cube004,
                Cube005,
                Cube006,
                Cube007,
                Cube008,
                Cube009,
                Cube010,
                Cube011,
                Cube012,
                Cube013,
                null, //leere Box in der Mitte
                Cube014,
                Cube015,
                Cube016,
                Cube017,
                Cube018,
                Cube019,
                Cube020,
                Cube021,
                Cube022,
                Cube023,
                Cube024,
                Cube025,
                Cube026,
            ];
            foreach (ModelVisual3D m in boxes)
            {
                if (m != null)
                {
                    m.Transform = new Transform3DGroup();
                }
            }

        }

        private void rotateY(int angle, int schicht)
        {
            animationRunning = true;
            rotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            transform = new RotateTransform3D(rotation, center);
            animation = new DoubleAnimation(angle, TimeSpan.FromMilliseconds(1000), FillBehavior.HoldEnd);
            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);
            ModelVisual3D[,] temp = new ModelVisual3D[3, 3];

            animation.Completed += (s, e) =>
            {
                animationRunning = false;
            };
            for (int i = 0; i < 3; i++) //zeile
            {
                for (int j = 0; j < 3; j++)//spalte
                {
                    temp[i, j] = boxes[schicht * 9 + i + j * 3];
                    if (boxes[schicht * 9 + i + j * 3] != null)
                    {
                        ((Transform3DGroup)(boxes[schicht * 9 + i + j * 3].Transform)).Children.Add(transform);
                    }
                }

            }
            //animation.Completed
            //90 rotation
            if (angle == -90)
            {
                for (int i = 0; i < 3; i++) //zeile
                {
                    for (int j = 0; j < 3; j++)//spalte
                    {
                        boxes[schicht * 9 + i + j * 3] = temp[2 - j, i];
                    }

                }
            }
            else
            {
                for (int i = 0; i < 3; i++) //zeile
                {
                    for (int j = 0; j < 3; j++)//spalte
                    {
                        boxes[schicht * 9 + i + j * 3] = temp[j, 2 - i];
                    }

                }
            }

            
        }

        private void rotateX(int angle, int schicht)
        {
            animationRunning = true;
            rotation = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0);
            transform = new RotateTransform3D(rotation, center);
            animation = new DoubleAnimation(angle, TimeSpan.FromMilliseconds(1000), FillBehavior.HoldEnd);
            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);
            ModelVisual3D[,] temp = new ModelVisual3D[3, 3];

            animation.Completed += (s, e) =>
            {
                animationRunning = false;
            };

            for (int i = 0; i < 3; i++) //zeile
            {
                for (int j = 0; j < 3; j++)//spalte
                {
                    temp[i, j] = boxes[schicht + i * 3 + j * 9];
                    if (boxes[schicht + i * 3 + j * 9] != null)
                    {
                        ((Transform3DGroup)(boxes[schicht + i * 3 + j * 9].Transform)).Children.Add(transform);
                    }
                }

            }
            //90 rotation
            if (angle == -90)
            {
                for (int i = 0; i < 3; i++) //zeile
                {
                    for (int j = 0; j < 3; j++)//spalte
                    {
                        boxes[schicht + i * 3 + j * 9] = temp[2 - j, i];
                    }

                }
            }
            else
            {
                for (int i = 0; i < 3; i++) //zeile
                {
                    for (int j = 0; j < 3; j++)//spalte
                    {
                        boxes[schicht + i * 3 + j * 9] = temp[j, 2 - i];
                    }

                }
            }

            
        }

        private void rotateZ(int angle, int schicht)
        {
            animationRunning = true;
            rotation = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0);
            transform = new RotateTransform3D(rotation, center);
            animation = new DoubleAnimation(angle, TimeSpan.FromMilliseconds(1000), FillBehavior.HoldEnd);
            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);
            ModelVisual3D[,] temp = new ModelVisual3D[3, 3];

            animation.Completed += (s, e) =>
            {
                animationRunning = false;
            };
            for (int i = 0; i < 3; i++) //zeile
            {
                for (int j = 0; j < 3; j++)//spalte
                {
                    temp[i, j] = boxes[schicht * 3 + i + j * 9];
                    if (boxes[schicht * 3 + i + j * 9] != null)
                    {
                        ((Transform3DGroup)(boxes[schicht * 3 + i + j * 9].Transform)).Children.Add(transform);
                    }
                }

            }
            //90 rotation
            if (angle == -90)
            {
                for (int i = 0; i < 3; i++) //zeile
                {
                    for (int j = 0; j < 3; j++)//spalte
                    {
                        boxes[schicht * 3 + i + j * 9] = temp[2 - j, i];
                    }

                }
            }
            else
            {
                for (int i = 0; i < 3; i++) //zeile
                {
                    for (int j = 0; j < 3; j++)//spalte
                    {
                        boxes[schicht * 3 + i + j * 9] = temp[j, 2 - i];
                    }

                }
            }

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < 10; i++)
            {
                switch (r.Next(3))
                {
                    case 0:
                        rotateX(r.Next(2) == 0 ? 90 : -90, r.Next(3));
                        break;
                    case 1:
                        rotateY(r.Next(2) == 0 ? 90 : -90, r.Next(3));
                        break;
                    case 2:
                        rotateZ(r.Next(2) == 0 ? 90 : -90, r.Next(3));
                        break;
                }

                // neuen Thread machen mit Dispatcher

                // while (animationRunning) { }

                //Thread.Sleep(2000);
            }
            
        }
    }
}