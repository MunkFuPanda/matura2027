package org.example;

import javax.swing.*;

public class SudokuGame extends JFrame {
    private JPanel main;
    private static final SudokuGame instance = new SudokuGame();

    public static SudokuGame getInstance() {
        return instance;
    }

    private SudokuGame() {
        super("Sudoku Game");

        System.out.println("SudokuGame constructor");
        this.setContentPane(main);
        this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        this.pack();
        this.setVisible(true);
    }

    public static void main(String[] args) {
        SudokuGame.getInstance();
    }
}
