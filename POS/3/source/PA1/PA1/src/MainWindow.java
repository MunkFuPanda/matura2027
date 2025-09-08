import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableModel;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

class RectComparator implements Comparator<Rect> {
    @Override
    public int compare(Rect r1, Rect r2) {
        return r1.getArea() - r2.getArea();
    }
}

class Rect {
    private int width;
    private int height;

    public int getWidth() {
        return width;
    }

    public int getHeight() {
        return height;
    }

    public Rect(int width, int height) {
        this.width = width;
        this.height = height;
    }

    public int getCircumference() {
        return 2 * width + 2 * height;
    }

    public int getArea() {
        return width * height;
    }
}

class IndexedButton extends JButton {
    public int id = Integer.MIN_VALUE;
};

public class MainWindow extends JFrame {
    private JPanel main;
    private JTextField aufgabe2_eingabe;
    private JButton aufgabe2_button;
    private JLabel aufgabe2_ausgabe;
    private JTextField aufgabe3_breite;
    private JTextField aufgabe3_hoehe;
    private JButton aufgabe3_button;
    private JTable aufgabe5;
    private JLabel aufgabe3_umfang;
    private JLabel aufgabe3_flaeche;
    private IndexedButton aufgabe6_button1;
    private IndexedButton aufgabe6_button2;
    private IndexedButton aufgabe6_button3;
    private IndexedButton aufgabe6_button4;
    private IndexedButton aufgabe6_button5;
    private IndexedButton aufgabe6_button6;
    private IndexedButton aufgabe6_button7;
    private IndexedButton aufgabe6_button8;
    private IndexedButton aufgabe6_button9;

    private IndexedButton[] buttonRefs = new IndexedButton[]{
            aufgabe6_button1,
            aufgabe6_button2,
            aufgabe6_button3,
            aufgabe6_button4,
            aufgabe6_button5,
            aufgabe6_button6,
            aufgabe6_button7,
            aufgabe6_button8,
            aufgabe6_button9
    };
    private int emptyButtonDx = 4;

    private List<Rect> rects = new ArrayList<>();
    private DefaultTableModel tableModel = new DefaultTableModel();

    public MainWindow() throws HeadlessException {
        setTitle("PA1");
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setContentPane(main);

        tableModel.addColumn("width");
        tableModel.addColumn("height");
        tableModel.addColumn("circumference");
        tableModel.addColumn("area");
        aufgabe5.setModel(tableModel);

        for (int i = 0; i < buttonRefs.length; i++) {
            buttonRefs[i].id = i;
        }

        aufgabe2_button.addActionListener(new ActionListener() {
            void insertWord() {
                String current = aufgabe2_ausgabe.getText();
                String newString = aufgabe2_eingabe.getText();
                System.out.println(current);
                if (!current.isEmpty()) {
                    // validate
                    if (newString.toLowerCase().charAt(0) != current.toLowerCase().charAt(current.length() - 1)) {
                        JOptionPane.showMessageDialog(null, "Invalid Input", "Error", JOptionPane.ERROR_MESSAGE);
                        return;
                    }
                }

                aufgabe2_ausgabe.setText(newString);
            }

            @Override
            public void actionPerformed(ActionEvent actionEvent) {
                insertWord();
            }
        });

        aufgabe3_button.addActionListener(new ActionListener() {
            public static boolean isInvalidInt(String str) {
                try {
                    Integer.parseInt(str);
                    return false;
                } catch (NumberFormatException e) {
                    return true;
                }
            }

            private void updateRectStats() {
                int totalCircumference = 0;
                int totalArea = 0;

                for (Rect rect : rects) {
                    totalCircumference += rect.getCircumference();
                    totalArea += rect.getArea();
                }

                aufgabe3_umfang.setText(String.valueOf(totalCircumference));
                aufgabe3_flaeche.setText(String.valueOf(totalArea));
                System.out.println("done");
            }

            private void updateRectDisplay() {
                while (tableModel.getRowCount() > 0) {
                    tableModel.removeRow(0);
                }

                rects.sort(new RectComparator());
                for (Rect rect : rects) {
                    tableModel.addRow(new Object[]{rect.getWidth(), rect.getHeight(), rect.getCircumference(), rect.getArea()});
                }
            }

            public void actionPerformed(ActionEvent actionEvent) {
                String width = aufgabe3_breite.getText();
                String height = aufgabe3_hoehe.getText();

                if (isInvalidInt(width) || isInvalidInt(height)) {
                    System.out.println("invalid height or width");
                    return;
                }

                int w = Integer.parseInt(width);
                int h = Integer.parseInt(height);

                if (w <= 0 || h <= 0) {
                    System.out.println("negative height or width");
                    return;
                }

                rects.add(new Rect(w, h));
                updateRectStats();
                updateRectDisplay();
            }
        });

        ActionListener buttonListener = new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                IndexedButton button = (IndexedButton) e.getSource();

                if (emptyButtonDx + 3 == button.id
                        || (emptyButtonDx + 1 == button.id
                        && button.id % 3 != 0)
                        || (emptyButtonDx - 1 == button.id
                        && button.id % 3 != 2)
                        || emptyButtonDx - 3 == button.id) {
                    Icon emptyIcon = buttonRefs[emptyButtonDx].getIcon();
                    buttonRefs[emptyButtonDx].setIcon(buttonRefs[button.id].getIcon());
                    buttonRefs[button.id].setIcon(emptyIcon);
                    emptyButtonDx = button.id;
                }
            }
        };

        for (IndexedButton button : buttonRefs) {
            button.addActionListener(buttonListener);
        }

        pack();
    }
}
