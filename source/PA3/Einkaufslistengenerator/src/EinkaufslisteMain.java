import javax.swing.*;
import java.awt.*;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.*;

public class EinkaufslisteMain extends JFrame{
    private JComboBox languageBox;
    private JComboBox typeBox;
    private JComboBox foodBox;
    private JPanel main;
    private JSlider quantity;
    private JTable shoppingTable;
    private JButton addToListButton;
    private JTextArea foodText;

    public EinkaufslisteMain() throws HeadlessException, IOException {
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setContentPane(main);
        setTitle("Einkaufslistengenerator");
        setSize(800, 600);

//        // Create a ResourceBundle for the resources folder
//        ResourceBundle resourceBundle = ResourceBundle.getBundle("resources");
//
//        // Read the content of Products.csv and Produkte.csv
//        Set<String> uniqueWords = readFiles(resourceBundle, "Products.csv", "Produkte.csv");
//
//        // Print the unique words
//        System.out.println("Unique words: " + uniqueWords);


        try {
            UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    private static Set<String> readFiles(ResourceBundle resourceBundle, String... fileNames) throws IOException {
        // Create a set to store the unique words
        Set<String> uniqueWords = new HashSet<>();

        // Iterate over the file names
        for (String fileName : fileNames) {
            // Get the input stream for the file
            InputStream inputStream = resourceBundle.getBaseBundleName().getClass().getClassLoader().getResourceAsStream(fileName);

            // Read the content of the file
            BufferedReader reader = new BufferedReader(new InputStreamReader(inputStream));
            String line;
            while ((line = reader.readLine()) != null) {
                // Extract the words before the ";" delimiter
                String[] words = line.split("\\s+")[0].split(";")[0].split("\\s+");
                uniqueWords.addAll(Arrays.asList(words));
            }
            reader.close();
        }

        return uniqueWords;



    }

    public static void main(String[] args) throws IOException {
        EinkaufslisteMain eklm = new EinkaufslisteMain();
        eklm.setVisible(true);

    }
}