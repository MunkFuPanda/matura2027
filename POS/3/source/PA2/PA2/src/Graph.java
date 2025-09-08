import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;
import java.util.Collections;
import java.util.TreeMap;

public class Graph extends JPanel {

    private int maxValue = 100;
    private TreeMap<Character, Integer> letters = new TreeMap<>();

    public void setLetters(TreeMap<Character, Integer> letters) {
        if(letters != null) {
            this.letters = letters;
            maxValue = Collections.max(letters.values());
        }
    }

    @Override
    public void paint(Graphics g) {
        super.paint(g);
        double width = this.getWidth();
        double height = this.getHeight();
        double heightDelta = height / letters.size();
        ArrayList<Character> l = new ArrayList<>();
        l.addAll(letters.keySet());
        for(int i = 0; i < letters.size(); i++)
        {
			g.drawRect(15, (int) (heightDelta * i) + 5, (int) (width * ((double) letters.get(l.get(i)) / maxValue)) - 20, (int) heightDelta / 2);
            g.drawString("" + l.get(i), 0, (int) (heightDelta * i + 15));
        }
    }
}
