import javax.swing.*;
import java.awt.event.*;
import java.io.*;
import java.util.HashMap;

public class MainWindow extends JDialog {
	private JPanel contentPane;
	private JButton buttonOK;
	private JButton buttonCancel;
	private JTextField textPane1;

	private final HashMap<String, String> hashMap = new HashMap<>();

	public MainWindow() {
		setContentPane(contentPane);
		setModal(true);
		getRootPane().setDefaultButton(buttonOK);

		buttonOK.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				onOK();
			}
		});

		buttonCancel.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				onCancel();
			}
		});

		// call onCancel() when cross is clicked
		setDefaultCloseOperation(DISPOSE_ON_CLOSE);
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

	private void onOK() {
		if (textPane1.getText().trim().isEmpty()) return;
		String option = hashMap.get(textPane1.getText().trim().toUpperCase());
		JOptionPane.showMessageDialog(contentPane, option == null ? "Abkürzung wurde nicht gefunden!" : option);
	}

	private void onCancel() {
		// add your code here if necessary
		dispose();
	}

	private void readList() throws IOException {
		File file = new File("./KennzeichenListe.txt");
		BufferedReader br = new BufferedReader(new FileReader(file));

		String line;
		while ((line = br.readLine()) != null) {
			String[] seperated = line.split(";");
			hashMap.put(seperated[0], seperated[1]);
		}
	}

	public static void main(String[] args) throws IOException {
		MainWindow dialog = new MainWindow();

		dialog.readList();

		dialog.pack();
		dialog.setVisible(true);
	}
}
