package Mastermind;

import javax.swing.*;
import javax.swing.table.DefaultTableCellRenderer;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.*;

public class mainWindow extends JFrame {
    private JPanel panel1;
    private JTable table1;
    private JTextField textField1;
    private JButton button1;
    private JLabel label;
    private JLabel label_c;
    private JLabel label_p;
    private JLabel label_er;
    private int color = 6;
    private int place = 4;
    private String[] allowed_chars;
    private ResourceBundle bundle = ResourceBundle.getBundle("Mastermind/language");
    private ArrayList<String> chars = new ArrayList<>();
    private ArrayList<String> tip = new ArrayList<>();
    private int trys = 0;



    public mainWindow() {
        super("Mastermind");
        setContentPane(panel1);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setSize(800, 600);
        setVisible(true);
        Locale.setDefault( new Locale("en") );
        label.setText(bundle.getString("erlaubt_sind"));
        button1.setText(bundle.getString("button"));

        JMenuBar mb = new JMenuBar();
        JMenu m = new JMenu(bundle.getString("Datei"));
        JMenuItem mi1 = new JMenuItem(bundle.getString("Neu"));
        JMenuItem mi2 = new JMenuItem(bundle.getString("Farbe"));
        JMenuItem mi3 = new JMenuItem(bundle.getString("Stellen"));

        m.add(mi1);
        m.add(mi2);
        m.add(mi3);
        mb.add(m);
        setJMenuBar(mb);

        mi1.addActionListener(e -> {
            startgame();
        });

        mi2.addActionListener(e -> {
            //choose if color is 4 or 6
            color = JOptionPane.showOptionDialog(
                    null,
                    bundle.getString("Farbe"),
                    bundle.getString("Farbe"),
                    JOptionPane.DEFAULT_OPTION,
                    JOptionPane.INFORMATION_MESSAGE,
                    null,
                    new String[]{"6", "8"},
                    null
            );
            if (color == 0) {
                color = 6;
            }
            else if (color == 1) {
                color = 8;
            }
        });


        mi3.addActionListener(e -> {
            place = JOptionPane.showOptionDialog(
                    null,
                    bundle.getString("Stellen"),
                    bundle.getString("Stellen"),
                    JOptionPane.DEFAULT_OPTION,
                    JOptionPane.INFORMATION_MESSAGE,
                    null,
                    new String[]{"4", "5"},
                    null
            );
            if (place == 0) {
                place = 4;
            }
            else if (place == 1) {
                place = 5;
            }
        });


        startgame();


        button1.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                tip.clear();
                if (trys == 8) {
                    JOptionPane.showMessageDialog(null, bundle.getString("verloren"));
                    return;
                }
                ArrayList<String> temp = new ArrayList<>();
                HashSet<String> set = new HashSet<>();
                temp.addAll(Arrays.asList(textField1.getText().split("")));
                for (String s : temp) {
                    set.add(s);
                }
                if (set.size() != place) {
                    JOptionPane.showMessageDialog(null, bundle.getString("doppelt"));
                    return;
                }

                ArrayList<String> temp2 = new ArrayList<>();
                temp2.addAll(Arrays.stream(allowed_chars).toList());
                for (String s : temp) {
                    if (!temp2.contains(s)) {
                        JOptionPane.showMessageDialog(null, bundle.getString("nicht_erlaubt"));
                        return;
                    }
                }

                if (textField1.getText().length() == place) {
                    String guess = textField1.getText();
                    textField1.setText("");
                    String[] guess_split = guess.split("");
                    ArrayList<String> guess_list = new ArrayList<>();
                    for (int i = 0; i < guess_split.length; i++) {
                        guess_list.add(guess_split[i]);
                    }
                    System.out.println(guess_list);
                    for (String s : guess_list) {
                        if (chars.contains(s)) {
                            if (chars.indexOf(s) == guess_list.indexOf(s)) {
                                tip.add("R");
                            }
                            else {
                                tip.add("r");
                            }
                        }
                    }
                    tip.sort(String::compareTo);
                    System.out.println(tip);
                    for (int i = 0; i < guess_list.size(); i++) {
                        table1.setValueAt(guess_list.get(i), 7 - trys, i);
                    }
                    table1.setValueAt(tip.toString().replace("[", "").replace("]", "").replace(",", ""), 7 - trys, place);
                    trys++;

                    if (tip.toString().replace("[", "").replace("]", "").replace(",", "").replace(" ", "").equals("RRRR") && place == 4) {
                        JOptionPane.showMessageDialog(null, bundle.getString("gewonnen"));
                        startgame();
                    }
                    if (tip.toString().replace("[", "").replace("]", "").replace(",", "").replace(" ", "").equals("RRRRR") && place == 5) {
                        JOptionPane.showMessageDialog(null, bundle.getString("gewonnen"));
                        startgame();
                    }

                }
                else {
                    JOptionPane.showMessageDialog(null, bundle.getString("falsch"));
                }
            }
        });
    }


    private void startgame() {
        trys = 0;
        chars.clear();
        tip.clear();
        label.setText(bundle.getString("erlaubt_sind"));
        label_c.setText(bundle.getString("Farbe") + " " + String.valueOf(color));
        label_p.setText(bundle.getString("Stellen") + " " + String.valueOf(place));
        DefaultTableModel model = new DefaultTableModel();
        table1.setModel(model);
        table1.setBorder(BorderFactory.createLineBorder(Color.black));
        for (int i = 0; i < place + 1; i++) {
            model.addColumn("");
        }
        for (int i = 0; i < place + 1; i++) {
            table1.getColumnModel().getColumn(i).setCellRenderer(new CenteredRenderer());
        }

        for (int i = 0; i < 8; i++) {
            if (place == 4) {
                model.addRow(new Object[]{"", "", "", ""});
            }
            else {
                model.addRow(new Object[]{"", "", "", "", ""});
            }
        }
        if (color == 6) {
            allowed_chars = new String[]{"a", "b", "c", "d", "e", "f"};
        }
        else if (color == 8) {
            allowed_chars = new String[]{"a", "b", "c", "d", "e", "f", "g", "h"};
        }
        label.setText(label.getText() + " ");
        for (int i = 0; i < color; i++) {
            label.setText(label.getText() + allowed_chars[i] + " ");
        }


        //game
        ArrayList<String> allowed_chars_temp = new ArrayList<>();
        for (int i = 0; i < allowed_chars.length; i++) {
            allowed_chars_temp.add(allowed_chars[i]);
        }

        Random r = new Random();
        int j;
        //random element from allowed_chars
        for (int i = 0; i < place; i++) {
            j = r.nextInt(allowed_chars_temp.size());
            chars.add(allowed_chars_temp.get(j));
            allowed_chars_temp.remove(j);
        }
        System.out.println(chars);
    }


    public class CenteredRenderer extends DefaultTableCellRenderer
    {

        public Component getTableCellRendererComponent(JTable table, Object value, boolean isSelected,
                                                       boolean hasFocus, int row, int column)
        {
            super.getTableCellRendererComponent(table, value, isSelected,
                    hasFocus, row, column);
            setHorizontalAlignment(SwingConstants.CENTER);
            return this;
        }
    }


    public static void main(String[] args) {
        mainWindow mainWindow = new mainWindow();
    }
}
