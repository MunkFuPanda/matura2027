package org.example;

import javax.swing.*;
import java.awt.*;

public class SudokuGame extends JFrame {
    private static final int GRID_SIZE = 3;
    private SudokuBlock[][] blocks = new SudokuBlock[GRID_SIZE][GRID_SIZE];
    private JPanel panel1;
    private JButton a1Button;
    private JButton a2Button;
    private JButton a3Button;
    private JButton a4Button;
    private JButton a5Button;
    private JButton a6Button;
    private JButton a7Button;
    private JButton a8Button;
    private JButton a9Button;

    public SudokuGame() {
        setTitle("Sudoku");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(600, 600);
        setLayout(new BorderLayout());

        JMenuBar menuBar = new JMenuBar();
        JMenu menu = new JMenu("Spiel");
        JMenuItem newGame = new JMenuItem("Neu");
        JMenuItem importGame = new JMenuItem("Importieren");
        JMenuItem saveGame = new JMenuItem("Speichern");
        JMenuItem loadGame = new JMenuItem("Laden");

        menu.add(newGame);
        menu.add(importGame);
        menu.add(saveGame);
        menu.add(loadGame);
        menuBar.add(menu);
        setJMenuBar(menuBar);

        JPanel mainPanel = new JPanel(new GridLayout(GRID_SIZE, GRID_SIZE));
        add(mainPanel, BorderLayout.CENTER);

        for (int i = 0; i < GRID_SIZE; i++) {
            for (int j = 0; j < GRID_SIZE; j++) {
                blocks[i][j] = new SudokuBlock();
                mainPanel.add(blocks[i][j]);
            }
        }

        setVisible(true);
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(SudokuGame::new);
    }
}
