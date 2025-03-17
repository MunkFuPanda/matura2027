package org.example;

import javax.swing.*;
import java.util.ArrayList;

public class SudokuBlock {
    private JPanel main;
    private JButton button1;
    private JButton button2;
    private JButton button3;
    private JButton button4;
    private JButton button5;
    private JButton button6;
    private JButton button7;
    private JButton button8;
    private JButton button9;

    private ArrayList<JButton> buttons = new ArrayList<>() {
        {
            add(button1);
            add(button2);
            add(button3);
            add(button4);
            add(button5);
            add(button6);
            add(button7);
            add(button8);
            add(button9);
        }
    };


    public void setButton(int x, int y, int value) {
        int localRow = y % 3; // Y-Position innerhalb des Blocks
        int localCol = x % 3; // X-Position innerhalb des Blocks
        int localIndex = localRow * 3 + localCol; // 1D-Index für die Zelle im Block
        buttons.get(localIndex).setText(String.valueOf(value));
    }

    public int getButton(int x, int y) {
        int localRow = y % 3; // Y-Position innerhalb des Blocks
        int localCol = x % 3; // X-Position innerhalb des Blocks
        int localIndex = localRow * 3 + localCol; // 1D-Index für die Zelle im Block
        return Integer.parseInt(buttons.get(localIndex).getText());
    }


}