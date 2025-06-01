import javax.swing.*;
import java.awt.event.*;
import java.util.HashSet;

public class Chooser extends JDialog {
    private JPanel contentPane;
    private JButton buttonOK;
    private JComboBox selectedAirportBox;

    private Airport selectedAirport;

    public Chooser(HashSet<Airport> airports) {
        setContentPane(contentPane);
        setModal(true);
        getRootPane().setDefaultButton(buttonOK);
        setSize(400, 300);

        airports.forEach(selectedAirportBox::addItem);

        buttonOK.addActionListener(e -> onOK());

        setDefaultCloseOperation(DO_NOTHING_ON_CLOSE);
        addWindowListener(new WindowAdapter() {
            public void windowClosing(WindowEvent e) {
                onOK();
            }
        });

        contentPane.registerKeyboardAction(e -> onOK(), KeyStroke.getKeyStroke(KeyEvent.VK_ESCAPE, 0), JComponent.WHEN_ANCESTOR_OF_FOCUSED_COMPONENT);
        setVisible(true);
    }

    private void onOK() {
        selectedAirport = (Airport) selectedAirportBox.getSelectedItem();
        dispose();
    }

    public Airport getSelectedAirport() {
        return selectedAirport;
    }
}
