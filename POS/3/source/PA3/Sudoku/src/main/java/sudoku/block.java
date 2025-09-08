package sudoku;

import javax.swing.*;

public class block extends JPanel {
    public JButton button1;
    public JPanel panel1;
    public JButton button2;
    public JButton button3;
    public JButton button4;
    public JButton button8;
    public JButton button5;
    public JButton button6;
    public JButton button9;
    public JButton button7;
    private JButton[] buttons = {button1, button2, button3, button4, button5, button6, button7, button8, button9};

    public block() {
        for (int i = 1; i < 10; i++) {
            buttons[i - 1].setActionCommand(String.valueOf(i));
        }
    }

    public JButton[] getButtons() {
        return buttons;
    }


}
