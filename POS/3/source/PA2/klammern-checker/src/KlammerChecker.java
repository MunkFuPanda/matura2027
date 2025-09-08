import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.util.*;
import java.util.List;

public class KlammerChecker extends JFrame {
    public record Pair<K, V>(K key, V value) {
    }

    private JPanel panel;
    private JLabel label;
    private JTextArea input;
    private JTable output;
    private final DefaultTableModel model = new DefaultTableModel();

    InputListener iolistener;

    public KlammerChecker(String title) throws HeadlessException {
        super(title);

        setSize(1200, 900);
        setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);
        setContentPane(panel);

        // editing callbacks via document listener
        iolistener = new InputListener(this);
        input.getDocument().addDocumentListener(iolistener);

        model.addColumn("");
        output.setModel(model);
    }

    private void displayResult(HashMap<Pair<Integer, Integer>, Character> map) {
        while (model.getRowCount() > 0) {
            model.removeRow(0);
        }

        // list of errors sorted by row and col
        List<Pair<Integer, Integer>> items = new ArrayList<>(map.keySet());
        items.sort((pair1, pair2) -> {
            if (Objects.equals(pair2.key, pair1.key)) {
                return pair1.value.compareTo(pair2.value);
            } else {
                return pair1.key.compareTo(pair2.key);
            }
        });

        for (Pair<Integer, Integer> pair : items) {
            char c = map.get(pair);
            model.addRow(new Object[]{"bracket '" + c + "' at " + pair.key + ":" + pair.value
                    + (("([{".contains(c + "")) ? " never gets closed" : " never got opened")});
        }

        String[] lines = input.getText().split(System.lineSeparator());
        for (Pair<Integer, Integer> pair : items.reversed()) {
            StringBuilder l = new StringBuilder(lines[pair.key]);
            // red, bold
            l.replace(pair.value, pair.value + 1, "<b style=\"color:rgb(255,0,0)\">" + map.get(pair) + "</b>");
            lines[pair.key] = l.toString();
        }

        StringBuilder updatedText = new StringBuilder();
        updatedText.append("<html>");
        for (int i = 0; i < lines.length; i++) {
            updatedText.append(lines[i]).append("<br>");
        }

        SwingUtilities.invokeLater(() -> {
            // avoids recursion
            iolistener.supress_events = true;
            label.setText(updatedText.toString().replace(" ", "&nbsp;"));
            iolistener.supress_events = false;
        });
    }

    public void check() {
        String text = input.getText();

        // line number + line index and opening bracket
        Stack<Pair<Pair<Integer, Integer>, Character>> stack = new Stack<>();
        // line number + line index as key, faulty bracket value
        HashMap<Pair<Integer, Integer>, Character> map = new HashMap<>();

        // iterate line by line
        Scanner sc = new Scanner(text);
        for (int linedx = 0; sc.hasNextLine(); linedx++) {
            String line = sc.nextLine();

            // iterate char by char
            for (int chardx = 0; chardx < line.length(); chardx++) {
                char c = line.charAt(chardx);

                if ("([{".contains(c + "")) {
                    // push open brackets
                    stack.push(new Pair<>(new Pair<>(linedx, chardx), c));
                } else if (")]}".contains(c + "")) {
                    if (stack.isEmpty()) {
                        // stack empty -> no fitting opening bracket
                        map.put(new Pair<>(linedx, chardx), c);
                    } else {
                        // opening bracket left
                        char top = stack.peek().value;
                        if ((top == '(' && c == ')')
                                || (top == '{' && c == '}')
                                || (top == '[' && c == ']')) {
                            // bracket fits, pop it off
                            stack.pop();
                        } else {
                            map.put(new Pair<>(linedx, chardx), c);
                        }
                    }
                }
            }
        }

        while (!stack.isEmpty()) {
            // pop left over opening brackets
            Pair<Pair<Integer, Integer>, Character> pair = stack.pop();
            map.put(pair.key, pair.value);
        }

        displayResult(map);
    }
}
