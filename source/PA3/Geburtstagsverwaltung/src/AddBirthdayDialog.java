import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.event.ActionEvent;

public class AddBirthdayDialog extends JDialog {
    private JPanel mainDialogPane;
    private JTextField nameField;
    private JTextField birthdayField;
    private JButton addButton;
    private JButton cancelButton;

    public AddBirthdayDialog(JFrame parent, DefaultTableModel tableModel) {
        super(parent, "Geburtstag hinzufügen", true);
        setContentPane(mainDialogPane);
        setSize(300, 200);
        setLocationRelativeTo(parent);

        addButton.addActionListener((ActionEvent e) -> {
            String name = nameField.getText().trim();
            String birthday = birthdayField.getText().trim();

            if (!name.isEmpty() && !birthday.isEmpty()) {
                tableModel.addRow(new Object[]{name, birthday});
                dispose();
            } else {
                JOptionPane.showMessageDialog(this, "Alle Felder müssen ausgefüllt werden.");
            }
        });

        cancelButton.addActionListener(e -> dispose());
    }
}
