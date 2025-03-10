package org.example;

import javax.swing.*;
import java.awt.*;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;

public class SudokuBlock extends JPanel {
    private static final int BLOCK_SIZE = 3;
    private JTextField[][] fields = new JTextField[BLOCK_SIZE][BLOCK_SIZE];

    public SudokuBlock() {
        setLayout(new GridLayout(BLOCK_SIZE, BLOCK_SIZE));
        setBorder(BorderFactory.createLineBorder(Color.BLACK));

        for (int i = 0; i < BLOCK_SIZE; i++) {
            for (int j = 0; j < BLOCK_SIZE; j++) {
                fields[i][j] = new JTextField();
                fields[i][j].setHorizontalAlignment(JTextField.CENTER);
                fields[i][j].setFont(new Font("Arial", Font.BOLD, 20));

                // Klick-Event zur Zahleneingabe
                fields[i][j].addMouseListener(new MouseAdapter() {
                    @Override
                    public void mouseClicked(MouseEvent e) {
                        String input = JOptionPane.showInputDialog("Gib eine Zahl (1-9) ein:");
                        if (input != null && input.matches("[1-9]")) {
                            ((JTextField) e.getSource()).setText(input);
                        } else {
                            JOptionPane.showMessageDialog(null, "Ungültige Eingabe!");
                        }
                    }
                });

                add(fields[i][j]);
            }
        }
    }
}

