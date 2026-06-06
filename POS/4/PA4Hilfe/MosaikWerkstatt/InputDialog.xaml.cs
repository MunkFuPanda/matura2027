using System.Windows;

namespace MosaikWerkstatt
{
    // Wiederverwendbarer Eingabe-Dialog. Aufruf:
    //   string name = InputDialog.Ask(this, "Titel", "Frage:", "Vorgabe");
    //   -> liefert den eingegebenen Text, oder null bei Abbrechen.
    public partial class InputDialog : Window
    {
        public string Value { get; private set; }

        public InputDialog(string title, string prompt, string defaultValue)
        {
            InitializeComponent();
            Title = title;
            PromptText.Text = prompt;
            InputBox.Text = defaultValue ?? "";
            InputBox.SelectAll();
            InputBox.Focus();
        }

        private void OnOk_Click(object sender, RoutedEventArgs e)
        {
            Value = InputBox.Text;
            DialogResult = true; // schliesst das Fenster, ShowDialog() liefert true
        }

        public static string Ask(Window owner, string title, string prompt, string defaultValue)
        {
            var dlg = new InputDialog(title, prompt, defaultValue) { Owner = owner };
            return dlg.ShowDialog() == true ? dlg.Value : null;
        }
    }
}
