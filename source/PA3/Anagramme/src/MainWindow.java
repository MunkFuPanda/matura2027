import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.*;
import java.io.*;

public class MainWindow extends JFrame {
    private JTextField anagramm_letters;
    private JButton enterButton;
    private JButton nextButton;
    private JButton backButton;
    private JPanel main;
    private JTextArea resultArea;
    private JLabel anagrammPages;

    private List<String> anagramList = new ArrayList<>();
    private int currentPage = 0;
    private static final int PAGE_SIZE = 20;

    // Set von ungültigen Bigrammen
    private Set<String> invalidBigrams = new HashSet<>();

    public MainWindow() {
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setContentPane(main);
        setTitle("Anagramm-Generator");
        setSize(600, 450);
        setLocationRelativeTo(null);

        resultArea.setEditable(false);

        // Lade ungültige Bigramme aus der CSV-Datei
        loadInvalidBigrams("invalid_bigrams.csv");

        // ActionListener für den Enter-Button
        enterButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                String input = anagramm_letters.getText().trim().replaceAll("\\s+", "");
                if (!input.isEmpty()) {
                    if (input.length() > 15) {
                        JOptionPane.showMessageDialog(MainWindow.this, "Bitte maximal 15 Zeichen eingeben!");
                        return;
                    }
                    Set<String> anagrams = generateAnagrams(input);
                    anagramList = new ArrayList<>(filterValidAnagrams(anagrams));
                    currentPage = 0;
                    displayAnagrams();
                } else {
                    JOptionPane.showMessageDialog(MainWindow.this, "Bitte Text eingeben!");
                }
            }
        });

        // ActionListener für den Next-Button
        nextButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                if ((currentPage + 1) * PAGE_SIZE < anagramList.size()) {
                    currentPage++;
                    displayAnagrams();
                } else {
                    JOptionPane.showMessageDialog(MainWindow.this, "Keine weiteren Anagramme.");
                }
            }
        });

        // ActionListener für den Back-Button
        backButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                if (currentPage > 0) {
                    currentPage--;
                    displayAnagrams();
                } else {
                    JOptionPane.showMessageDialog(MainWindow.this, "Du bist schon auf der ersten Seite.");
                }
            }
        });

        setVisible(true);
    }

    // Permutationsgenerator
    private Set<String> generateAnagrams(String input) {
        Set<String> result = new HashSet<>();
        permute("", input, result);
        return result;
    }

    private void permute(String prefix, String remaining, Set<String> result) {
        int n = remaining.length();
        if (n == 0) {
            result.add(prefix);
        } else {
            for (int i = 0; i < n; i++) {
                permute(prefix + remaining.charAt(i),
                        remaining.substring(0, i) + remaining.substring(i + 1, n),
                        result);
            }
        }
    }

    // Filtern der Anagramme, die ungültige Bigramme enthalten
    private Set<String> filterValidAnagrams(Set<String> anagrams) {
        Set<String> validAnagrams = new HashSet<>();
        for (String anagram : anagrams) {
            if (!containsInvalidBigram(anagram)) {
                validAnagrams.add(anagram);
            }
        }
        return validAnagrams;
    }

    // Überprüfen, ob ein Anagramm ungültige Bigramme enthält
    private boolean containsInvalidBigram(String anagram) {
        for (int i = 0; i < anagram.length() - 1; i++) {
            String bigram = anagram.substring(i, i + 2);
            if (invalidBigrams.contains(bigram)) {
                return true;
            }
        }
        return false;
    }

    private void loadInvalidBigrams(String filePath) {
        try (BufferedReader reader = new BufferedReader(new FileReader(filePath))) {
            String line;
            while ((line = reader.readLine()) != null) {
                String[] bigrams = line.split(";");
                invalidBigrams.addAll(Arrays.asList(bigrams));
            }
        } catch (IOException e) {
            JOptionPane.showMessageDialog(MainWindow.this, "Fehler beim Laden der Bigramme.");
        }
    }

    // Anagramme + Seitenanzeige aktualisieren
    private void displayAnagrams() {
        resultArea.setText("");
        int start = currentPage * PAGE_SIZE;
        int end = Math.min(start + PAGE_SIZE, anagramList.size());

        for (int i = start; i < end; i++) {
            resultArea.append(anagramList.get(i) + "\n");
        }

        // Seitenanzeige aktualisieren
        int totalPages = (int) Math.ceil((double) anagramList.size() / PAGE_SIZE);
        anagrammPages.setText("Seite " + (currentPage + 1) + " von " + totalPages);
    }

    public static void main(String[] args) {
        new MainWindow();
    }
}
