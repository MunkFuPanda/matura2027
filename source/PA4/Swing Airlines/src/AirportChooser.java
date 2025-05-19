import javax.swing.*;
import java.awt.event.*;
import java.io.IOException;
import java.sql.ResultSet;
import java.sql.SQLException;

public class AirportChooser extends JDialog {
    private JPanel contentPane;
    private JButton buttonOK;
    private JButton buttonCancel;
    private JComboBox start_comboBox;
    private JComboBox end_comboBox;

    public AirportChooser(ResultSet start_rs, ResultSet end_rs) throws SQLException {
        setContentPane(contentPane);
        setModal(true);
        getRootPane().setDefaultButton(buttonOK);

        while(start_rs.next()) {
            start_comboBox.addItem(start_rs.getString(4));
        }
        while(end_rs.next()) {
            start_comboBox.addItem(end_rs.getString(4));
        }

        buttonOK.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                try {
                    onOK();
                } catch (Exception ex) {
                    throw new RuntimeException(ex);
                }
            }
        });

        buttonCancel.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                onCancel();
            }
        });

        // call onCancel() when cross is clicked
        setDefaultCloseOperation(DO_NOTHING_ON_CLOSE);
        addWindowListener(new WindowAdapter() {
            public void windowClosing(WindowEvent e) {
                onCancel();
            }
        });

        // call onCancel() on ESCAPE
        contentPane.registerKeyboardAction(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                onCancel();
            }
        }, KeyStroke.getKeyStroke(KeyEvent.VK_ESCAPE, 0), JComponent.WHEN_ANCESTOR_OF_FOCUSED_COMPONENT);
    }

    private void onOK() throws SQLException, IOException {
        MainWindow.getInstance().setAirports(start_comboBox.getSelectedItem().toString(), end_comboBox.getSelectedItem().toString());
        dispose();
    }

    private void onCancel() {
        // add your code here if necessary
        dispose();
    }
}
