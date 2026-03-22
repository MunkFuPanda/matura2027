using Microsoft.Win32;
using System;
using System.Diagnostics;
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

namespace WPF_Wordpad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RichTextBox_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.CanExecute = Clipboard.ContainsText();
                e.Handled = true;
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "XAML Dokument (*.xaml)|*.xaml",
                DefaultExt = ".xaml"
            };

            if (dialog.ShowDialog() == true)
            {
                using FileStream fs = new FileStream(dialog.FileName, FileMode.Create);
                string path = dialog.FileName;
                TextRange range = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
                range.Save(fs, DataFormats.Xaml);
                Debug.WriteLine("Datei gespeichert unter: " + path);
            }
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Dokument öffnen",
                Filter = "XAML Dokument (*.xaml)|*.xaml",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                using FileStream stream =
                new FileStream(openFileDialog.FileName, FileMode.Open);
                TextRange textRange = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
                textRange.Load(stream, DataFormats.Xaml);
            }
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            richTextBox1.Document.Blocks.Clear();
        }

        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                IDocumentPaginatorSource doc = richTextBox1.Document;

                printDialog.PrintDocument(
                    doc.DocumentPaginator,
                    "WPF Wordpad Document"
                );
            }
        }

        private void DocumentNotEmpty(object sender, CanExecuteRoutedEventArgs e)
        {
            TextRange range = new TextRange(
            richTextBox1.Document.ContentStart,
            richTextBox1.Document.ContentEnd);

            string text = range.Text.Trim();

            if (!string.IsNullOrEmpty(text)) { 
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }

        }
    }
}