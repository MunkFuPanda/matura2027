import jakarta.xml.bind.*;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;

import javax.swing.*;
import java.io.File;
import java.util.HashMap;
import java.util.Map;

@XmlRootElement
public class shoppingList {
    @XmlElement(name = "item")
    public Map<String, Integer> shoppingList = new HashMap<>();

    public static void saveToXML(Map<String, Integer> shoppingList) {
        JFileChooser fileChooser = new JFileChooser();
        fileChooser.setDialogTitle("Save Shopping List as XML");

        int userSelection = fileChooser.showSaveDialog(null);

        if (userSelection == JFileChooser.APPROVE_OPTION) {
            File file = fileChooser.getSelectedFile();
            if (!file.getName().endsWith(".xml")) {
                file = new File(file.getAbsolutePath() + ".xml");
            }

            try {
                // Wrap the shopping list
                shoppingList wrapper = new shoppingList();
                wrapper.shoppingList = shoppingList;

                // Create JAXB context and marshaller
                JAXBContext context = JAXBContext.newInstance(shoppingList.class);
                Marshaller marshaller = context.createMarshaller();
                marshaller.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, true);

                // Write to XML file
                marshaller.marshal(wrapper, file);
                System.out.println("Shopping list saved to " + file.getAbsolutePath());

            } catch (JAXBException e) {
                e.printStackTrace();
            }
        }
    }

    public static HashMap<String, Integer> loadFromXML() {
        JFileChooser fileChooser = new JFileChooser();
        fileChooser.setDialogTitle("Open Shopping List XML");

        int userSelection = fileChooser.showOpenDialog(null);

        if (userSelection == JFileChooser.APPROVE_OPTION) {
            File file = fileChooser.getSelectedFile();

            try {
                JAXBContext context = JAXBContext.newInstance(shoppingList.class);
                Unmarshaller unmarshaller = context.createUnmarshaller();

                shoppingList wrapper = (shoppingList) unmarshaller.unmarshal(file);
                System.out.println("Shopping list loaded from " + file.getAbsolutePath());

                return new HashMap<>(wrapper.shoppingList);

            } catch (JAXBException e) {
                e.printStackTrace();
            }
        }
        return new HashMap<>();
    }
}
