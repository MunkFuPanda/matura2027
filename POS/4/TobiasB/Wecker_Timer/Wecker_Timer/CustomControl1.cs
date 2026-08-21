using System.Globalization;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wecker_Timer
{
    public class Timer : Control
    {
        static Timer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Timer), new FrameworkPropertyMetadata(typeof(Timer)));
        }

        public static readonly DependencyProperty
            AlarmTimeProperty = DependencyProperty.Register(
                    "AlarmTime",
                  typeof(TimeSpan),
                     typeof(Timer),
            new FrameworkPropertyMetadata(
               new TimeSpan(0,0,0), null));

        public TimeSpan AlarmTime
        {
            get { return (TimeSpan)base.GetValue(AlarmTimeProperty); }
            set { base.SetValue(AlarmTimeProperty, value); }
        }

        public static readonly DependencyProperty
            AlarmSetProperty = DependencyProperty.Register(
                    "AlarmSet",
                  typeof(bool),
                     typeof(Timer),
            new FrameworkPropertyMetadata(
               false,
               FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool AlarmSet
        {
            get { return (bool)base.GetValue(AlarmSetProperty); }
            set { base.SetValue(AlarmSetProperty, value); }
        }

        public static readonly DependencyProperty
            CurrentTimeProperty = DependencyProperty.Register(
                    "CurrentTime",
                  typeof(DateTime),
                     typeof(Timer),
            new FrameworkPropertyMetadata(
               DateTime.Now, null));

        public DateTime CurrentTime
        {
            get { return (DateTime)base.GetValue(CurrentTimeProperty); }
            set { base.SetValue(CurrentTimeProperty, value); }
        }

        public static readonly RoutedEvent AlarmEvent =
           EventManager.RegisterRoutedEvent("Alarm",
             RoutingStrategy.Bubble, typeof(RoutedEventHandler),
             typeof(Timer));

        public event RoutedEventHandler Alarm
        {
            add { base.AddHandler(AlarmEvent, value); }
            remove { base.RemoveHandler(AlarmEvent, value); }
        }

        protected void FireAlarm()
        {
            base.RaiseEvent(new RoutedEventArgs(AlarmEvent));
        }

        protected void RingAlarm()
        {
            SoundPlayer sp = new SoundPlayer(@"c:\windows\media\tada.wav");
            sp.Play();
            FireAlarm();
        }
        public void OnDisplayTimerTick(object o, EventArgs args)
        {
            this.CurrentTime = DateTime.Now;

            if (this.AlarmSet == true)
            {
                if (AlarmTime.Minutes == 0 && AlarmTime.Seconds == 0 && AlarmTime.Milliseconds == 0)
                {
                    RingAlarm();
                    this.AlarmSet = false;
                }
                TimeSpan temp = new TimeSpan(0, 0, 0, 1, 0);
                AlarmTime = AlarmTime.Subtract(temp);
            }
        }


        // added variables

        TimeSpan savedAlarmTime = TimeSpan.Zero;

        void OnShowSetAlarmDlg(object sender, RoutedEventArgs ea)
        {
            DateTimeDlg dateTimeDlg = new DateTimeDlg();

            dateTimeDlg.AlarmTime = new DateTime(2000, 1, 1, 0, 0, 0);
            if (dateTimeDlg.ShowDialog() == true)
            {
                this.AlarmTime = new TimeSpan(dateTimeDlg.AlarmTime.Hour, dateTimeDlg.AlarmTime.Minute, dateTimeDlg.AlarmTime.Second);
                this.savedAlarmTime = new TimeSpan(dateTimeDlg.AlarmTime.Hour, dateTimeDlg.AlarmTime.Minute, dateTimeDlg.AlarmTime.Second);

            }

        }

        void OnStartButton(object sender, RoutedEventArgs e)
        {
            this.AlarmSet = true;
        }

        void OnPauseButton(object sender, RoutedEventArgs e)
        {
            this.AlarmSet = false;
        }

        void OnResetButton(object sender, RoutedEventArgs e)
        {
            this.AlarmTime = this.savedAlarmTime;
            this.AlarmSet = false;
        }

        System.Windows.Threading.DispatcherTimer displayTimer;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button bSetAlarmDlg =
                (Button)this.Template.FindName("PART_SETALARMBUTTON", this);

            bSetAlarmDlg.Click += OnShowSetAlarmDlg;

            Button startAlarm = (Button)this.Template.FindName("PART_STARTBUTTON", this);
            startAlarm.Click += OnStartButton;

            Button pauseAlarm = (Button)this.Template.FindName("PART_PAUSEBUTTON", this);
            pauseAlarm.Click += OnPauseButton;

            Button resetAlarm = (Button)this.Template.FindName("PART_RESETBUTTON", this);
            resetAlarm.Click += OnResetButton;

            displayTimer = new System.Windows.Threading.DispatcherTimer();
            displayTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            displayTimer.Tick += OnDisplayTimerTick;

            displayTimer.Start();

            TextBlock tbAlarmSetButtonTextPane =
                (TextBlock)this.Template.FindName("PART_SETALARMBUTTONTEXTPANE", this);
            Binding bindingSetAlarmButtonTextPane = new Binding();
            bindingSetAlarmButtonTextPane.Source = this;
            bindingSetAlarmButtonTextPane.Path = new PropertyPath("AlarmTime");
            tbAlarmSetButtonTextPane.SetBinding(TextBlock.TextProperty, bindingSetAlarmButtonTextPane);

            TextBlock tbCurrentTime =
                (TextBlock)this.Template.FindName("PART_CURRENTDATETIME", this);
            Binding bindingCurrentTime = new Binding();
            bindingCurrentTime.Source = this;
            bindingCurrentTime.Path = new PropertyPath("CurrentTime");
            tbCurrentTime.SetBinding(TextBlock.TextProperty, bindingCurrentTime);
        }
    }
}