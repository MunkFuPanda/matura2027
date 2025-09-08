import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;
import java.util.Vector;

public class MainWindow extends JFrame {
	private JPanel mainPanel;
	private JButton evaluateButton;
	private JTextArea inputArea;
	private JList<Integer> top10List;
	private JTable numbersTable;

	private final HashMap<Integer, Integer> frequencies = new HashMap<>(); // <Zahl, Frequenz>
	private final DefaultTableModel model = new DefaultTableModel(new String[]{"Zahl", "Frequenz"}, 0) {
		@Override
		public boolean isCellEditable(int row, int column) {
			return false;
		}
	};

	public MainWindow() {
		setTitle("Zahlen Auswerten");
		setContentPane(mainPanel);
		setSize(500, 400);
		setDefaultCloseOperation(EXIT_ON_CLOSE);

		numbersTable.setModel(model);
		numbersTable.setAutoCreateRowSorter(true);

		inputArea.setLineWrap(true);
		inputArea.setWrapStyleWord(true);


		evaluateButton.addActionListener(e -> {
			clearOutputs();

			String input = inputArea.getText();
			if (input.isEmpty()) {
				return;
			}

			String[] numbers = input.replaceAll("([^\\d.]+|\\s{2,}|\\s\\.)", " ").replaceAll("( \\.|\\s{2,}|\\. )", " ").split(" ");

			for (String number : Arrays.stream(numbers).filter(s -> !s.isEmpty()).toArray(String[]::new)) {
				int numberInt = Integer.parseInt(number);
				frequencies.put(numberInt, frequencies.getOrDefault(numberInt, 0) + 1);
			}

			for (int number : frequencies.keySet()) {
				model.addRow(new Object[]{number, frequencies.get(number)});
			}

			top10List.setListData(new Vector<>(frequencies.entrySet().stream().sorted((e1, e2) -> {
				return e2.getValue() - e1.getValue();
			}).limit(10).map(Map.Entry::getKey).toList()));
		});
	}

	private void clearOutputs() {
		frequencies.clear();
		model.setRowCount(0);
		top10List.setListData(new Vector<Integer>());
	}

	public static void main(String[] args) {
		MainWindow main = new MainWindow();
		main.setVisible(true);
	}
}
