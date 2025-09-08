import javax.swing.*;
import javax.swing.Timer;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.awt.event.InputEvent;
import java.awt.event.KeyEvent;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.*;

public class MainWindow extends JFrame {
	private JPanel main;
	private JPanel aufgabe3Panel;
	private JButton auswertenButton;
	private JTextArea inputArea;
	private JTable frequencyTable;
	private JPanel graphPanel;
	private JLabel timeLabel;

	private JButton otherButton;
	private DefaultTableModel tableModel = new DefaultTableModel() {
		@Override
		public boolean isCellEditable(int row, int column) {
			return false;
		}
	};
	private DateTimeFormatter timeFormatter = DateTimeFormatter.ofPattern("HH:mm:ss", Locale.GERMAN);

	public MainWindow() throws HeadlessException {
		setDefaultCloseOperation(EXIT_ON_CLOSE);
		setContentPane(main);
		setTitle("2. praktische Arbeit");

		LinkedList<JButton> bl = new LinkedList<>();
		bl.add(new JButton(new ImageIcon(getClass().getResource("slice_0_0.png"))));
		bl.add(new JButton(new ImageIcon(getClass().getResource("slice_0_1.png"))));
		bl.add(new JButton(new ImageIcon(getClass().getResource("slice_0_2.png"))));
		bl.add(new JButton(new ImageIcon(getClass().getResource("slice_1_0.png"))));
		bl.add(new JButton(new ImageIcon(getClass().getResource("slice_1_1.png"))));
		bl.add(new JButton(new ImageIcon(getClass().getResource("slice_1_2.png"))));
		bl.add(new JButton(new ImageIcon(getClass().getResource("slice_2_0.png"))));
		bl.add(new JButton(new ImageIcon(getClass().getResource("slice_2_1.png"))));
		bl.add(new JButton(new ImageIcon(getClass().getResource("slice_2_2.png"))));
		Collections.shuffle(bl);
		aufgabe3Panel.setLayout(new GridLayout(3, 3));
		for (JButton b : bl) {
			aufgabe3Panel.add(b);
		}

		// aufgabe 2
		frequencyTable.setModel(tableModel);
		frequencyTable.getTableHeader().setReorderingAllowed(false);
		tableModel.addColumn("Buchstabe");
		tableModel.addColumn("Anzahl");
		tableModel.setRowCount(0);
		auswertenButton.addActionListener(e -> {
			String input = inputArea.getText().toUpperCase().replaceAll("[^A-Z]", "");
			char[] chars = input.toCharArray();
			tableModel.setRowCount(0);

			TreeMap<Character, Integer> letters = new TreeMap<>();
			for (char c : chars) {
				letters.merge(c, 1, Integer::sum);
			}

			((Graph) graphPanel).setLetters(letters);
			graphPanel.repaint();

			for (Map.Entry<Character, Integer> entry : letters.entrySet()) {
				tableModel.addRow(new Object[]{entry.getKey(), entry.getValue()});
			}
		});

		// aufgabe 3
		for (JButton b : bl) {
			b.addActionListener(e -> {
				if (otherButton == null) {
					otherButton = b;
				} else {
					Icon icon = otherButton.getIcon();
					otherButton.setIcon(b.getIcon());
					b.setIcon(icon);
					otherButton = null;
				}
			});
		}

		// aufgabe 4
		new Timer(1000, e -> timeLabel.setText(timeFormatter.format(LocalDateTime.now()))).start();

		// aufgabe 5
		JMenuBar menuBar = new JMenuBar();
		setJMenuBar(menuBar);

		JMenu fileMenu = new JMenu("Datei");
		fileMenu.setMnemonic(KeyEvent.VK_D);
		menuBar.add(fileMenu);

		JMenuItem newMenuItem = new JMenuItem("Neu");
		newMenuItem.setMnemonic(KeyEvent.VK_N);
		newMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_N, InputEvent.CTRL_DOWN_MASK));
		fileMenu.add(newMenuItem);
		newMenuItem.addActionListener(e -> {
			inputArea.setText("");
			tableModel.setRowCount(0);
			((Graph) graphPanel).setLetters(null);
		});

		// aufgabe 6
		JMenuItem exitMenuItem = new JMenuItem("Beenden");
		exitMenuItem.setMnemonic(KeyEvent.VK_B);
		exitMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_B, InputEvent.CTRL_DOWN_MASK));
		fileMenu.add(exitMenuItem);
		exitMenuItem.addActionListener(e -> {
			if (JOptionPane.showConfirmDialog(this, "Sind Sie sicher?", "Beenden", JOptionPane.YES_NO_OPTION) == JOptionPane.YES_OPTION) {
				System.exit(0);
			}
		});

		pack();
	}

	public static void main(String[] args) {
		MainWindow mw = new MainWindow();
		mw.setVisible(true);
	}

	private void createUIComponents() {
		// TODO: place custom component creation code here
		graphPanel = new Graph();
	}
}


// aufgabe 1
// ui fertig machen
// aufgabe 2
// zahlen auswerten (tabelle)
// aufgabe 3
// bilder wechseln
// aufgabe 4
// zeit formatieren
// aufgabe 5
// datei menü mit neu
// aufgabe 6
// datei eintrag "beenden" mit confirmation dialog
// aufgabe 7
// graph
