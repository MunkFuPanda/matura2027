using Microsoft.Win32;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WPF_WordPad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String savedText = String.Empty;
        private String currentFile = String.Empty;
        private int dataFormat = -1;


        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            String text = new TextRange(rtb_textbox.Document.ContentStart, rtb_textbox.Document.ContentEnd).Text;

            if (text != savedText)
            {
                ApplicationCommands.Save.Execute(sender, Owner);
                return;
            }

            currentFile = "";
            dataFormat = -1;
            rtb_textbox.Document.Blocks.Clear();
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            String text = new TextRange(rtb_textbox.Document.ContentStart, rtb_textbox.Document.ContentEnd).Text;

            if (text != savedText)
            {
                ApplicationCommands.Save.Execute(sender, Owner);
            }

            rtb_textbox.Document.Blocks.Clear();

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text-Dateien|*.txt|XAML-Dateien|*.xaml|RTF-Dateien | *.rtf | Alle Dateien | *.* ";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string format = null; ;
                switch (dialog.FilterIndex)
                {
                    case 1:
                    case 4:
                        format = DataFormats.Text;
                        dataFormat = 1;
                        break;
                    case 2:
                        format = DataFormats.Xaml;
                        dataFormat = 2;
                        break;
                    case 3:
                        format = DataFormats.Rtf;
                        dataFormat = 3;
                        break;
                }
                FlowDocument document = rtb_textbox.Document;
                TextRange range = new TextRange(document.ContentStart,
                                                document.ContentEnd);
                FileStream stream = new FileStream(dialog.FileName, FileMode.Open,
                                                   FileAccess.ReadWrite);

                currentFile = dialog.FileName;


                range.Load(stream, format);
                stream.Close();

            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(currentFile))
            {
                ApplicationCommands.SaveAs.Execute(sender, Owner);
                return;
            }

            FlowDocument document = rtb_textbox.Document;
            TextRange range = new TextRange(document.ContentStart,
                                            document.ContentEnd);
            FileStream stream = new FileStream(currentFile, FileMode.Create,
                                               FileAccess.ReadWrite);

            switch (dataFormat)
            {
                case 1:
                    range.Save(stream, DataFormats.Text);
                    break;
                case 2:
                    range.Save(stream, DataFormats.Xaml);
                    break;
                case 3:
                    range.Save(stream, DataFormats.Rtf);
                    break;
            }

            
            stream.Close();

        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text-Dateien|*.txt|XAML-Dateien|*.xaml|RTF-Dateien|*.rtf";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string format = null; ;
                switch (dialog.FilterIndex)
                {
                    case 1:
                        format = DataFormats.Text;
                        dataFormat = 1;
                        break;
                    case 2:
                        format = DataFormats.Xaml;
                        dataFormat = 2;
                        break;
                    case 3:
                        format = DataFormats.Rtf;
                        dataFormat = 3;
                        break;
                }

                currentFile = dialog.FileName;

                FlowDocument document = rtb_textbox.Document;
                TextRange range = new TextRange(document.ContentStart,
                                                document.ContentEnd);
                FileStream stream = new FileStream(dialog.FileName, FileMode.Create,
                                                   FileAccess.ReadWrite);
                range.Save(stream, format);
                stream.Close();
            }

        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

            if (rtb_textbox == null)
            {
                return;
            }
            
            String text = new TextRange(rtb_textbox.Document.ContentStart, rtb_textbox.Document.ContentEnd).Text;
            if (String.IsNullOrWhiteSpace(text))
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        private void Size_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}