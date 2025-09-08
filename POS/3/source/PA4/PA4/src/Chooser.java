import javax.swing.*;
import java.awt.event.KeyEvent;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.util.HashSet;

public class Chooser extends JDialog {
    private JPanel contentPane;
    private JButton buttonOK;
    private JComboBox selectedCityBox;

    private City chosenCity;

    public Chooser(HashSet<City> cities) {
        setContentPane(contentPane);
        setModal(true);
        getRootPane().setDefaultButton(buttonOK);
        setSize(400, 300);

        cities.forEach(selectedCityBox::addItem);

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
        if (selectedCityBox.getSelectedItem() != null) {
            chosenCity = (City) selectedCityBox.getSelectedItem();
        } else {
            JOptionPane.showMessageDialog(this, "Please select a city.", "Error", JOptionPane.ERROR_MESSAGE);
            return;
        }
        dispose();
    }

    public City getChosenCity() {
        return chosenCity;
    }
}
