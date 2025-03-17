package org.example;

import javax.swing.*;
import java.awt.*;

public class SudokuGame extends JFrame {

    private JPanel main;

    public SudokuGame() {
        super("Sudoku Game");
        this.setContentPane(main);
        this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        this.pack();
        this.setVisible(true);
    }


}
