import javax.swing.*;
import javax.swing.table.DefaultTableCellRenderer;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.util.Locale;
import java.util.Random;
import java.util.ResourceBundle;

public class MainWindow extends JFrame {
    private JButton btnNewGame;
    private JButton btnSubmit;
    private JComboBox<Integer> comboLetters; // Anzahl Buchstaben (6 oder 8)
    private JTable tableCode;
    private JPanel Main;
    private JTextField textLength; // Anzahl Stellen (4 oder 5) - als Textfeld
    private JTextField textEntered; // Rateingabe
    private JLabel lblStatus;

    private String secretCode;
    private int maxAttempts = 10;
    private int attemptCount = 0;
    private ResourceBundle messages;

    public MainWindow() {
        setTitle("Mastermind");
        setSize(600, 400);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLocationRelativeTo(null);
        setContentPane(Main);
        setVisible(true);

        // Sprache setzen
        Locale locale = Locale.getDefault();
        messages = ResourceBundle.getBundle("messages", locale);
        btnNewGame.setText(messages.getString("new_game"));
        btnSubmit.setText(messages.getString("submit_guess"));
        lblStatus.setText(messages.getString("status_ready"));

        // Event-Listener setzen
        btnNewGame.addActionListener(e -> startNewGame());
        btnSubmit.addActionListener(e -> submitGuess());

        // Combobox für Buchstabenauswahl (6 oder 8)
        comboLetters.setModel(new DefaultComboBoxModel<>(new Integer[]{6, 8}));

        // Tabelle initialisieren
        DefaultTableModel model = new DefaultTableModel(maxAttempts, 10);
        tableCode.setModel(model);

        for (int i = 0; i < 10; i++) {
            tableCode.getColumnModel().getColumn(i).setCellRenderer(new CenteredRenderer());
        }

        startNewGame();
    }

    private void startNewGame() {
        int letterCount = (int) comboLetters.getSelectedItem();
        int codeLength;

        try {
            codeLength = Integer.parseInt(textLength.getText());
            if (codeLength < 4 || codeLength > 5) throw new NumberFormatException();
        } catch (NumberFormatException e) {
            lblStatus.setText(messages.getString("error_length"));
            return;
        }

        secretCode = generateSecretCode(letterCount, codeLength);
        attemptCount = 0;
        lblStatus.setText(messages.getString("status_ready"));

        // Tabelle leeren
        for (int row = 0; row < maxAttempts; row++) {
            for (int col = 0; col < 10; col++) {
                tableCode.setValueAt("", row, col);
            }
        }
    }

    private String generateSecretCode(int letterCount, int codeLength) {
        Random rand = new Random();
        String letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".substring(0, letterCount);
        StringBuilder code = new StringBuilder();

        for (int i = 0; i < codeLength; i++) {
            code.append(letters.charAt(rand.nextInt(letterCount)));
        }

        return code.toString();
    }

    private void submitGuess() {
        String guess = textEntered.getText().toUpperCase();

        if (guess.length() != secretCode.length()) {
            lblStatus.setText(messages.getString("error_length"));
            return;
        }

        if (attemptCount >= maxAttempts) {
            lblStatus.setText(messages.getString("game_over") + " " + secretCode);
            return;
        }

        for (int i = 0; i < guess.length(); i++) {
            tableCode.setValueAt(guess.charAt(i), attemptCount, i);
        }

        String feedback = checkGuess(guess);
        for (int i = 0; i < feedback.length(); i++) {
            tableCode.setValueAt(feedback.charAt(i), attemptCount, guess.length() + i);
        }

        if (feedback.equals("R".repeat(secretCode.length()))) {
            lblStatus.setText(messages.getString("win"));
        } else {
            attemptCount++;
            if (attemptCount >= maxAttempts) {
                lblStatus.setText(messages.getString("game_over") + " " + secretCode);
            }
        }
    }

    private String checkGuess(String guess) {
        StringBuilder feedback = new StringBuilder();

        for (int i = 0; i < guess.length(); i++) {
            if (guess.charAt(i) == secretCode.charAt(i)) {
                feedback.append("R");
            } else if (secretCode.contains(String.valueOf(guess.charAt(i)))) {
                feedback.append("F");
            }
        }

        return feedback.toString();
    }

    public static class CenteredRenderer extends DefaultTableCellRenderer {
        public Component getTableCellRendererComponent(JTable table, Object value, boolean isSelected,
                                                       boolean hasFocus, int row, int column) {
            super.getTableCellRendererComponent(table, value, isSelected, hasFocus, row, column);
            setHorizontalAlignment(SwingConstants.CENTER);
            return this;
        }
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(MainWindow::new);
    }
}
