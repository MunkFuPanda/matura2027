package org.example;

import javax.swing.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class MainWindow extends JFrame {
    private JTextArea textArea;
    private JPanel panel1;
    private JButton auswertenButton;

    public MainWindow() {
        setTitle("Regex Matcher");
        setContentPane(panel1);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setSize(800, 600);
        setLocationRelativeTo(null);
        setVisible(true);

        final String regex = "(?<zahl>(?<vor>[\\d]+)(?:(?<komma>[,.])(?<nach>[\\d]+))?)";
        final Pattern pattern = Pattern.compile(regex, Pattern.MULTILINE);

        auswertenButton.addActionListener(e -> {
            final Matcher matcher = pattern.matcher(textArea.getText());

            StringBuilder result = new StringBuilder();
            while (matcher.find()) {
                result.append("Full match: ").append(matcher.group(0)).append("\n");

                for (int i = 1; i <= matcher.groupCount(); i++) {
                    result.append("Group ").append(i).append(": ").append(matcher.group(i)).append("\n");
                }
                result.append("\n");
            }
            JOptionPane.showMessageDialog(this, result.toString(), "Matches", JOptionPane.INFORMATION_MESSAGE);
        });
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(MainWindow::new);
    }
}
