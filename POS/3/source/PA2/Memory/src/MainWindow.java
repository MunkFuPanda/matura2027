import javax.swing.*;
import javax.swing.Timer;
import java.awt.*;
import java.util.*;

public class MainWindow extends JFrame {
	private JPanel mainPanel;
	private JPanel setupPanel;
	private JPanel gamePanel;
	private JTextField playerNames;
	private JLabel setupLabel;
	private JButton startGameButton;
	private JComboBox<Integer> pairSelector;
	private JPanel gameFrame;
	private JButton restartButton;
	private JLabel currentPlayerLabel;
	private JList<String> playersList;

	private final HashMap<String, Integer> players = new HashMap<>();
	private final ArrayList<MemoryButton> buttons = new ArrayList<>();
	private MemoryButton lastButton = null;

	private final ArrayList<ImageIcon> icons = loadImages();

	public MainWindow(String title) {
		super(title);

		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setSize(500, 400);
		setContentPane(mainPanel);

		showSetupScreen();

		startGameButton.addActionListener(e -> {
			if (playerNames.getText().isEmpty()) {
				JOptionPane.showMessageDialog(null, "Please enter player names");
				return;
			}

			Arrays.stream(playerNames.getText().split(";")).filter(s -> !s.isEmpty()).forEach(s -> {
				players.put(s.trim(), 0);
			});

			int pairs = (int) pairSelector.getSelectedItem();
			showGameScreen(pairs);
		});

		restartButton.addActionListener(e -> {
			showSetupScreen();
		});
	}

	private void showSetupScreen() {
		players.clear();
		playerNames.setText("");
		pairSelector.removeAllItems();

		for (int i = 6; i <= 10; i += 2) {
			pairSelector.addItem(i);
		}

		CardLayout cardLayout = (CardLayout) mainPanel.getLayout();
		cardLayout.show(mainPanel, "setupPanel");
	}

	private void showGameScreen(int pairs) {
		buttons.clear();
		gameFrame.removeAll();
		gameFrame.setLayout(new GridLayout(4, pairs / 2));

		playersList.setListData(players.keySet().toArray(String[]::new));
		playersList.addListSelectionListener(e -> updateCurrentPlayer());
		playersList.setSelectedIndex(0);
		playersList.setEnabled(false);
		playersList.setSelectedIndex(playersList.getSelectedIndex() + 1 % players.size());

		generatePairs(pairs);

		CardLayout cardLayout = (CardLayout) mainPanel.getLayout();
		cardLayout.show(mainPanel, "gamePanel");
	}

	private void generatePairs(int pairs) {
		for (int i = 0; i < pairs; i++) {
			buttons.add(new MemoryButton(icons.get(i % icons.size())));
			buttons.add(new MemoryButton(icons.get(i % icons.size())));
		}

		Collections.shuffle(buttons);

		for (MemoryButton button : buttons) {
			button.addActionListener(e -> onMemoryButtonClicked(button));
			gameFrame.add(button);
		}
	}

	private void onMemoryButtonClicked(MemoryButton button) {
		if (button.isFound()) return;

		button.setIcon(button.getValue());

		if (lastButton == null) {
			lastButton = button;
		} else if (lastButton.getValue() == button.getValue()) {
			lastButton.setEnabled(false);
			lastButton.setFound(true);
			button.setEnabled(false);
			button.setFound(true);
			lastButton = null;

			players.put(playersList.getSelectedValue(), players.get(playersList.getSelectedValue()) + 1);
			updateCurrentPlayer();

			if (buttons.stream().filter(b -> !b.isFound()).count() == 0) {
				String winner = Objects.requireNonNull(players.keySet().stream().max((String o1, String o2) -> players.get(o1) - players.get(o2)).get());
				JOptionPane.showMessageDialog(mainPanel, "Player " + winner + " won!");
			}
		} else {
			for (MemoryButton b : buttons) {
				b.setEnabled(false);
			}

			Timer timer = new Timer(1500, e -> {
				lastButton.setIcon(null);
				button.setIcon(null);
				lastButton = null;

				playersList.setSelectedIndex((playersList.getSelectedIndex() + 1) % players.size());

				for (MemoryButton b : buttons.stream().filter(b -> !b.isFound()).toArray(MemoryButton[]::new)) {
					b.setEnabled(true);
				}
			});

			timer.setRepeats(false);
			timer.start();
		}
	}

	private void updateCurrentPlayer() {
		currentPlayerLabel.setText("Current Player: " + playersList.getSelectedValue() + " Score: " + players.get(playersList.getSelectedValue()));
	}

	private ArrayList<ImageIcon> loadImages() {
		ArrayList<ImageIcon> icons = new ArrayList<>();
		icons.add(new ImageIcon("images/dice-multiple.png"));
		icons.add(new ImageIcon("images/ghost.png"));
		icons.add(new ImageIcon("images/home-city.png"));
		icons.add(new ImageIcon("images/ice-cream.png"));
		icons.add(new ImageIcon("images/lightbulb-on.png"));
		icons.add(new ImageIcon("images/lighthouse.png"));
		icons.add(new ImageIcon("images/television-classic.png"));
		icons.add(new ImageIcon("images/train.png"));
		icons.add(new ImageIcon("images/tshirt-v.png"));
		icons.add(new ImageIcon("images/weather-lightning-rainy.png"));

		return icons;
	}

	public static void main(String[] args) {
		MainWindow mainWindow = new MainWindow("Memory");
		mainWindow.setVisible(true);
	}
}
