import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class DominoGame extends JFrame {
    private Player player1, player2, currentPlayer;
    private List<DominoTile> boardTiles;
    private JComboBox<DominoTile> tileComboBox;
    private JLabel boardLabel;

    public DominoGame() {
        player1 = new Player("Player 1");
        player2 = new Player("Player 2");
        boardTiles = new ArrayList<>();
        setUpGame();

        // Setup GUI
        setTitle("Domino Game");
        setSize(500, 300);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLocationRelativeTo(null);
        setLayout(new BorderLayout());

        boardLabel = new JLabel("Board: ");
        add(boardLabel, BorderLayout.NORTH);

        tileComboBox = new JComboBox<>();
        add(tileComboBox, BorderLayout.CENTER);

        JButton placeTileButton = new JButton("Place Tile");
        placeTileButton.addActionListener(new PlaceTileListener());
        add(placeTileButton, BorderLayout.SOUTH);

        refreshTileComboBox();
        updateBoardLabel();
    }

    private void setUpGame() {
        List<DominoTile> allTiles = generateAllTiles();
        Collections.shuffle(allTiles);
        dealTiles(allTiles);
        currentPlayer = playerWithDoubleSix();
    }

    private List<DominoTile> generateAllTiles() {
        List<DominoTile> tiles = new ArrayList<>();
        for (int i = 0; i <= 6; i++) {
            for (int j = i; j <= 6; j++) {
                tiles.add(new DominoTile(i, j));
            }
        }
        return tiles;
    }

    private void dealTiles(List<DominoTile> allTiles) {
        for (int i = 0; i < allTiles.size(); i++) {
            if (i % 2 == 0) {
                player1.addTile(allTiles.get(i));
            } else {
                player2.addTile(allTiles.get(i));
            }
        }
    }

    private Player playerWithDoubleSix() {
        return player1.getTiles().contains(new DominoTile(6, 6)) ? player1 : player2;
    }

    private void refreshTileComboBox() {
        tileComboBox.removeAllItems();
        for (DominoTile tile : currentPlayer.getTiles()) {
            tileComboBox.addItem(tile);
        }
    }

    private void updateBoardLabel() {
        StringBuilder boardText = new StringBuilder("Board: ");
        for (DominoTile tile : boardTiles) {
            boardText.append(tile.toString()).append(" ");
        }
        boardLabel.setText(boardText.toString());
    }

    private class PlaceTileListener implements ActionListener {
        @Override
        public void actionPerformed(ActionEvent e) {
            if (tileComboBox.getSelectedItem() != null) {
                DominoTile selectedTile = (DominoTile) tileComboBox.getSelectedItem();
                boardTiles.add(selectedTile);
                currentPlayer.getTiles().remove(selectedTile);
                switchPlayers();
                updateBoardLabel();
                refreshTileComboBox();
                checkGameOver();
            }
        }
    }

    private void switchPlayers() {
        currentPlayer = currentPlayer == player1 ? player2 : player1;
    }

    private void checkGameOver() {
        if (player1.isEmpty() || player2.isEmpty()) {
            JOptionPane.showMessageDialog(this, currentPlayer.getName() + " wins!");
            System.exit(0);
        }
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> new DominoGame().setVisible(true));
    }
}
