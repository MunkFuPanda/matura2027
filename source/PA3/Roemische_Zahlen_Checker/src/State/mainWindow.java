package State;

import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import static State.Numerals.E;
import static State.States.*;

public class mainWindow extends JFrame{
    private JTextField textField1;
    private JPanel panel1;
    private JButton button1;
    private JLabel label1;

    public mainWindow() {
        super("Römische Zahlen  Checker");
        setSize(500, 400);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setVisible(true);
        setContentPane(panel1);


        button1.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                if (!textField1.getText().isEmpty()) {
                    int i;
                    i = checkRoman(textField1.getText());
                    if (i == -1) {
                        JOptionPane.showMessageDialog(panel1, "Falsche Zahl");
                        textField1.setText("");
                        label1.setText("");
                    }
                    else {
                        label1.setText(String.valueOf(i));
                    }
                }
                else {
                    JOptionPane.showMessageDialog(panel1, "Kein Input!");
                    label1.setText("");
                }
            }
        });
    }



    public static int checkRoman(String input) {
        States current = S;
        int result = 0;
        for (char c : input.toCharArray()) {
            Numerals n = Numerals.which(c);
            result += val[current.ordinal()][n.ordinal()];
            current = tab[current.ordinal()][n.ordinal()];
        }
        if (current == S || current == X) {
            return -1;
        }

        return result;
    }


    private static States[][] tab = {
            // I V X L C D M E
            {A, C, F, I, K, N, P, X}, //*S*
            {B, Z, Z, X, X, X, X, X},//A
            {Z, X, X, X, X, X, X, X}, //B
            {D, X, X, X, X, X, X, X},//C
            {B, X, X, X, X, X, X, X}, //D
            {A, C, G, H, H, X, X, X},//F
            {A, C, H, X, X, X, X, X}, //G
            {A, C, X, X, X, X, X, X},//H
            {A, C, J, X, X, X, X, X}, //I
            {A, C, G, X, X, X, X, X},//J
            {A, C, F, I, L, M, M, X}, //K
            {A, C, F, I, M, X, X, X},//L
            {A, C, F, I, X, X, X, X}, //M
            {A, C, F, I, O, X, X, X},//N
            {A, C, F, I, L, X, X, X}, //O
            {A, C, F, I, K, N, P, X},//P
            {X, X, X, X, X, X, X, X}, //Z
            {X, X, X, X, X, X, X, X}//*X*


    };


    private static int[][] val = {
            // I V X L C D M E
            {1, 5, 10, 50, 100, 500, 1000, 0}, //*S*
            {1, 3, 8, 0, 0, 0, 0, 0},//A
            {1, 0, 0, 0, 0, 0, 0, 0}, //B
            {1, 0, 0, 0, 0, 0, 0, 0},//C
            {1, 0, 0, 0, 0, 0, 0, 0}, //D
            {1, 5, 10, 30, 80, 0, 0, 0},//F
            {1, 5, 10, 0, 0, 0, 0, 0}, //G
            {1, 5, 0, 0, 0, 0, 0, 0},//H
            {1, 5, 10, 0, 0, 0, 0, 0}, //I
            {1, 5, 10, 0, 0, 0, 0, 0},//J
            {1, 5, 10, 50, 100, 300, 800, 0}, //K
            {1, 5, 10, 50, 100, 0, 0, 0},//L
            {1, 5, 10, 50, 0, 0, 0, 0}, //M
            {1, 5, 10, 50, 100, 0, 0, 0},//N
            {1, 5, 10, 50, 100, 0, 0, 0}, //O
            {1, 5, 19, 50, 100, 500, 1000, 0},//P
            {0, 0, 0, 0, 0, 0, 0, 0}, //Z
            {0, 0, 0, 0, 0, 0, 0, 0}//*X*


    };



    public static void main(String[] args) {
        mainWindow mainWindow = new mainWindow();
        System.out.println(checkRoman("MCMLXXXVI"));
        System.out.println(checkRoman("X"));
    }
}