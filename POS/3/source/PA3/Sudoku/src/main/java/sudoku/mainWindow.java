package sudoku;

import jakarta.xml.bind.JAXBException;
import sudoku.sudoku_gen.Generator;
import sudoku.sudoku_gen.Grid;
import sudoku.sudoku_gen.Solver;

import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Random;


import static sudoku.SudokuDatenbank.verbindenMitDatenbank;

public class mainWindow extends JFrame {
    private block field1;
    private block field2;
    private block field3;
    private block field4;
    private block field5;
    private block field6;
    private block field7;
    private block field8;
    private block field9;
    private JPanel panel1;
    private JButton hilfeButton;
    private Grid.Cell[][] solved;
    private Grid grid = Grid.emptyGrid();;

    private block[] fields = {field1, field2, field3, field4, field5, field6, field7, field8, field9};
    private JButton[][] buttons = {
            {field1.button1, field1.button2, field1.button3, field2.button1, field2.button2, field2.button3, field3.button1, field3.button2, field3.button3},

            {field1.button4, field1.button5, field1.button6, field2.button4, field2.button5, field2.button6, field3.button4, field3.button5, field3.button6},

            {field1.button7, field1.button8, field1.button9, field2.button7, field2.button8, field2.button9, field3.button7, field3.button8, field3.button9},

            {field4.button1, field4.button2, field4.button3, field5.button1, field5.button2, field5.button3, field6.button1, field6.button2, field6.button3},

            {field4.button4, field4.button5, field4.button6, field5.button4, field5.button5, field5.button6, field6.button4, field6.button5, field6.button6},

            {field4.button7, field4.button8, field4.button9, field5.button7, field5.button8, field5.button9, field6.button7, field6.button8, field6.button9},

            {field7.button1, field7.button2, field7.button3, field8.button1, field8.button2, field8.button3, field9.button1, field9.button2, field9.button3},

            {field7.button4, field7.button5, field7.button6, field8.button4, field8.button5, field8.button6, field9.button4, field9.button5, field9.button6},

            {field7.button7, field7.button8, field7.button9, field8.button7, field8.button8, field8.button9, field9.button7, field9.button8, field9.button9}};


    public mainWindow() {
        super("Sudoku");
        setContentPane(panel1);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(1100, 1200);
        setVisible(true);



        JMenuBar mb = new JMenuBar();
        JMenu m = new JMenu("Datei");
        JMenuItem mi1 = new JMenuItem("Neu");
        JMenuItem mi2 = new JMenuItem("Improtieren");
        JMenuItem mi3 = new JMenuItem("Speichern");
        JMenuItem mi4 = new JMenuItem("Laden");

        m.add(mi1);
        m.add(mi2);
        m.add(mi3);
        m.add(mi4);
        mb.add(m);
        setJMenuBar(mb);




        Grid grid = Grid.emptyGrid();

        Generator generator = new Generator();

        grid = generator.generate(55);
        Grid.Cell[][] unsolved = grid.getCells().clone();
        toCells(unsolved);

        Solver solver = new Solver();

        solver.solve(grid);

        System.out.println(grid);
        solved = grid.getCells().clone();
        while (!checkgrid()) {
            grid = generator.generate(55);
            unsolved = grid.getCells().clone();
            toCells(unsolved);

            solver = new Solver();

            solver.solve(grid);

            System.out.println(grid);
            solved = grid.getCells().clone();
        }








        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                buttons[i][j].setActionCommand(String.valueOf(i) + String.valueOf(j));
            }
        }

        for (block field : fields) {
            for (JButton button : field.getButtons()) {
                button.addActionListener(e -> {
                    try {
                        int i = Integer.parseInt(JOptionPane.showInputDialog("Enter a number"));
                        check(i, button);
                    }
                    catch (NumberFormatException ex) {
                        JOptionPane.showMessageDialog(null, "Wrong input", "Error", JOptionPane.ERROR_MESSAGE);
                    }
                });
            }
        }




        mi1.addActionListener(e -> {
            clear();
            newGame();
        });

        mi2.addActionListener(e -> {
            clear();
            File f;
            try {
                JFileChooser fc = new JFileChooser();
                fc.showSaveDialog(mainWindow.this);
                f = fc.getSelectedFile();
            } catch (Exception ex) {
                return;
            }
            SudokuList productlist = new SudokuList();
            if (f == null) {
                return;
            }
                try {
                    productlist.readListe(f);
                    for (Feld feld : productlist.getListe()) {
                        System.out.println(feld.getPositionY() + " " + feld.getPositionX() + " " + feld.getZahl());
                        buttons[feld.getPositionY()][feld.getPositionX()].setText(String.valueOf(feld.getZahl()));
                    }
                    getCellsfromSudoku();
                }

                 catch (JAXBException ex) {
                    throw new RuntimeException(ex);
                }
        });

        mi3.addActionListener(e -> {
            String name = JOptionPane.showInputDialog("Enter a Name");
            if (name == null) {
                return;
            }
            try {
                SudokuDatenbank.speichern(name, toint(buttons));
            } catch (SQLException ex) {
                throw new RuntimeException(ex);
            }
        });

        mi4.addActionListener(e -> {
            String name = JOptionPane.showInputDialog("Enter a Name");
            if (name == null) {
                return;
            }
            try {
                clear();
                String game = SudokuDatenbank.laden(name);
                for (JButton[] button : buttons) {
                    for (JButton button1 : button) {
                        if (game.substring(0, 1).equals("0")) {
                            button1.setText(" ");
                        }
                        else {
                            button1.setText(game.substring(0, 1));
                        }
                        game = game.substring(1);
                    }
                }
                getCellsfromSudoku();

            } catch (SQLException ex) {
                JOptionPane.showMessageDialog(mainWindow.this, "Sudoku konnte nicht geladen werden!");
            }
        });


        hilfeButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                try {
                ArrayList<JButton> hilfe = new ArrayList<>();
                for (JButton[] button : buttons) {
                    for (JButton button1 : button) {
                        if (button1.getText().equals(" ")) {
                            hilfe.add(button1);
                        }
                    }
                }

                Random random = new Random();
                int index = random.nextInt(hilfe.size());
                JButton button1 = hilfe.get(index);
                int right = (solved[Integer.parseInt(button1.getActionCommand().substring(0, 1))][Integer.parseInt(button1.getActionCommand().substring(1))].getValue());
                button1.setText(String.valueOf(right));
                } catch (Exception ex) {
                    JOptionPane.showMessageDialog(null, "Keine Felder mehr übrig", "Error", JOptionPane.ERROR_MESSAGE);
                }
            }
        });
    }



    private void newGame() {
        grid = Grid.emptyGrid();

        Generator generator = new Generator();
        grid = generator.generate(55);
        Grid.Cell[][] unsolved = grid.getCells().clone();
        toCells(unsolved);
        System.out.println("called");
        Solver solver = new Solver();
        solver.solve(grid);
        System.out.println(grid);
        solved = grid.getCells().clone();
        while (!checkgrid()) {
            grid = generator.generate(55);
            unsolved = grid.getCells().clone();
            toCells(unsolved);
            System.out.println("called");
            solver = new Solver();
            solver.solve(grid);
            System.out.println(grid);
            solved = grid.getCells().clone();
        }
    }

    private boolean checkgrid() {
        for (Grid.Cell[] cells : solved) {
            for (Grid.Cell cell : cells) {
                if (cell.getValue() == 0) {
                    return false;
                }
            }
        }
        return true;
    }

    private void toCells(Grid.Cell[][] grid) {
        Grid.Cell[][] cells = grid;
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                buttons[i][j].setText(cells[i][j].getValue() == 0 ? " " : String.valueOf(cells[i][j].getValue()));
            }
        }
    }

    private String toint(JButton[][] buttons) {
        String s = "";
        for (JButton[] button : buttons) {
            for (JButton button1 : button) {
                if (button1.getText().equals(" ")) {
                    s += "0";
                }
                else {
                    s += button1.getText();
                }
            }
        }
        System.out.println(s);
        return s;
    }

    private void clear() {
        for (JButton[] button : buttons) {
            for (JButton button1 : button) {
                button1.setText(" ");
            }
        }
    }

    private void check(int i, JButton button) {
        if (button.getText().equals(" ") && i > 0 && i < 10) {
            System.out.println(button.getActionCommand());
            int right = (solved[Integer.parseInt(button.getActionCommand().substring(0, 1))][Integer.parseInt(button.getActionCommand().substring(1))].getValue());
            if (right == i) {
                button.setText(String.valueOf(i));
            }
            else {
                JOptionPane.showMessageDialog(null, "Falsche Zahl", "Error", JOptionPane.ERROR_MESSAGE);
            }
        }

    }

    private void getCellsfromSudoku() {
        Grid newgrid = Grid.emptyGrid();
        for (JButton[] button : buttons) {
            for (JButton button1 : button) {
                if (button1.getText().equals(" ")) {
                    newgrid = newgrid.setCell(Integer.parseInt(button1.getActionCommand().substring(0, 1)), Integer.parseInt(button1.getActionCommand().substring(1)), 0);
                    continue;
                }
                newgrid = newgrid.setCell(Integer.parseInt(button1.getActionCommand().substring(0, 1)), Integer.parseInt(button1.getActionCommand().substring(1)), Integer.parseInt(button1.getText()));


            }
        }


        Solver solver = new Solver();
        try {
            solver.solve(newgrid);
        } catch (IllegalStateException ex) {
            clear();
            JOptionPane.showMessageDialog(null, "Falsches Sudoku", "Error", JOptionPane.ERROR_MESSAGE);
            return;
        }
        solved = newgrid.getCells().clone();
        System.out.println(newgrid);
        grid = newgrid;
        System.out.println(grid);
    }

    public static void main(String[] args) {
        mainWindow mainWindow = new mainWindow();
        verbindenMitDatenbank();
    }
}


