import javax.swing.*;
import java.awt.*;
import java.util.MissingResourceException;
import java.util.ResourceBundle;

public class EinkaufslisteMain extends JFrame{
    private JComboBox languageBox;
    private JComboBox typeBox;
    private JComboBox foodBox;
    private JPanel main;
    private JSlider quantity;
    private JTable shoppingTable;
    private JButton addToListButton;
    private JTextArea foodText;

    public EinkaufslisteMain() throws HeadlessException {
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setContentPane(main);
        setTitle("2. praktische Arbeit");
        setSize(1200, 800);

    }

    public static void main(String[] args) {
        EinkaufslisteMain eklm = new EinkaufslisteMain();
        eklm.setVisible(true);

        String baseName = "resources.Produkte";

        try
        {
            ResourceBundle bundle = ResourceBundle.getBundle( baseName );
            System.out.println( bundle.getString("Hello") );
        }
        catch ( MissingResourceException e ) {
            System.err.println( e );
        }

    }
}