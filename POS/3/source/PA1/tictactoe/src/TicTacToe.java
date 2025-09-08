import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.Objects;
import java.util.Random;


public class TicTacToe extends JFrame {
    private final JPanel main;
    private final JPanel info;
    private final JPanel game;

    private final ImageIcon starIcon;
    private final ImageIcon crossIcon;
    private final ImageIcon emptyIcon;

    private final JButton[][] buttons = new JButton[3][3]; //scheiß auf .form

    private final Random r = new Random();
    private boolean turnX = r.nextInt(2) == 0;

    private final JLabel currentTurnLabel;
    private final JLabel gameStatusLabel;
    JButton resetButton = new JButton("Reset");


    private String winner = "";

    public String checkWinner() {
        // Zeilen überprüfen
        for (int row = 0; row < 3; row++) {
            if (buttons[row][0] != null &&
                    buttons[row][0].getName().equals(buttons[row][1].getName()) &&
                    buttons[row][1].getName().equals(buttons[row][2].getName())) {
                return buttons[row][0].getName(); // Gibt den Gewinner zurück ("X" oder "O")
                //TODO: das selbe halt mit anderem Index für 2. & 3. Reihe
            }
        }

        // Spalten überprüfen
        for (int col = 0; col < 3; col++) {
            if (buttons[0][col] != null &&
                    buttons[0][col].getName().equals(buttons[1][col].getName()) &&
                    buttons[1][col].getName().equals(buttons[2][col].getName())) {
                return buttons[0][col].getName(); // Gibt den Gewinner zurück
            }
            //TODO: Genau so wie oben, einfach index ändern
        }

        // Diagonalen überprüfen
        if (buttons[0][0] != null && buttons[0][0].getName().equals(buttons[1][1].getName()) && buttons[1][1].getName().equals(buttons[2][2].getName())) {
            return buttons[0][0].getName(); // Gibt den Gewinner zurück
        }
        if (buttons[0][2] != null && buttons[0][2].getName().equals(buttons[1][1].getName()) && buttons[1][1].getName().equals(buttons[2][0].getName())) {
            return buttons[0][2].getName(); // Gibt den Gewinner zurück
        }
        return "---";
    }


    public TicTacToe(String title) throws HeadlessException {
        super(title);
        starIcon = new ImageIcon(Objects.requireNonNull(getClass().getResource("Star.png")));
        crossIcon = new ImageIcon(Objects.requireNonNull(getClass().getResource("Cross.png")));
        emptyIcon = new ImageIcon(Objects.requireNonNull(getClass().getResource("Empty.png")));
        main = new JPanel();
        main.setLayout(new BorderLayout());
        info = new JPanel();
        info.setLayout(new GridLayout(1, 3));
        currentTurnLabel = new JLabel("Current Turn: " + (turnX ? "Cross" : "Star"));
        gameStatusLabel = new JLabel("Game in Progres...");


        resetButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                resetGame(); // Reset the game when button is clicked
            }
        });

        info.add(currentTurnLabel);
        info.add(gameStatusLabel);
        info.add(resetButton);


        main.add(BorderLayout.NORTH, info);


        game = new JPanel();
        game.setSize(200, 200);
        main.add(BorderLayout.CENTER, game);
        game.setLayout(new GridLayout(3, 3));


        for (int row = 0; row < 3; row++) {
            for (int col = 0; col < 3; col++) {
                buttons[row][col] = new JButton(emptyIcon);
                buttons[row][col].setName("Leer");
                buttons[row][col].addActionListener(new ActionListener() {
                    @Override
                    public void actionPerformed(ActionEvent e) {
                        JButton button = (JButton) e.getSource();
                        if (button.getIcon() == emptyIcon) {
                            if (turnX) {
                                gameStatusLabel.setText("Game in Progres...");
                                currentTurnLabel.setText("Current Turn: Star");
                                button.setIcon(crossIcon);
                                button.setName("C");
                                turnX = false;
                            } else {
                                gameStatusLabel.setText("Game in Progres...");
                                currentTurnLabel.setText("Current Turn: Cross");
                                button.setIcon(starIcon);
                                button.setName("S");
                                turnX = true;
                            }
                            winner = checkWinner();

                            if (winner.equals("C") || winner.equals("S")) {
                                if (winner.equals("C")) {
                                    gameStatusLabel.setText("Cross has Won!");
                                    JOptionPane.showMessageDialog(null, "Cross Wins!", "Game Over!", JOptionPane.INFORMATION_MESSAGE);
                                } else {
                                    gameStatusLabel.setText("Star has Won!");
                                    JOptionPane.showMessageDialog(null, "Star Wins!", "Game Over!", JOptionPane.INFORMATION_MESSAGE);
                                }
                                disableButtons();
                            } else if (checkDraw()) {
                                gameStatusLabel.setText("Draw!");
                                JOptionPane.showMessageDialog(null, "Draw!", "Game Over!", JOptionPane.INFORMATION_MESSAGE);
                                disableButtons();
                            }

                            System.out.println(winner);
                        }
                    }
                });
                game.add(buttons[row][col]);
            }
        }


        setSize(800, 600);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setContentPane(main);
    }

    private void resetGame() {
        for (int row = 0; row < 3; row++) {
            for (int col = 0; col < 3; col++) {
                buttons[row][col].setIcon(emptyIcon);
                buttons[row][col].setName("Leer");
                buttons[row][col].setEnabled(true);
            }
        }
        turnX = r.nextInt(2) == 0;
        currentTurnLabel.setText("Current Turn: " + (turnX ? "Cross" : "Star"));
        gameStatusLabel.setText("Game has been Reset!");
        winner = "";
    }

    public boolean checkDraw() {
        for (int row = 0; row < 3; row++) {
            for (int col = 0; col < 3; col++) {
                if (buttons[row][col].getIcon() == emptyIcon) {
                    return false; // There's still an empty space, so it's not a draw
                }
            }
        }
        return true; // All spaces are filled, it's a draw
    }


    private void disableButtons() {
        for (int row = 0; row < 3; row++) {
            for (int col = 0; col < 3; col++) {
                buttons[row][col].setEnabled(false);
            }
        }
    }


    public static void main(String[] args) {
        TicTacToe mw = new TicTacToe("Tic Tac Toe");
        mw.setVisible(true);
    }
}