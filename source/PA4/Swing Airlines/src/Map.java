import javax.imageio.ImageIO;
import javax.swing.*;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;

public class Map extends JPanel {
    private BufferedImage img = ImageIO.read(new File("lib/Earthmap.jpg"));

    public Map() throws IOException {
    }

    private int getX(double lng) {
        return (int) ((lng + 180) / 360 * MainWindow.getInstance().getPanel().getWidth());
    }

    private int getY(double lat) {
        return (int) ((90 - lat) / 180 * MainWindow.getInstance().getPanel().getHeight());
    }

    @Override
    public void paint(Graphics g) {
        super.paint(g);
        Graphics2D g2 = (Graphics2D) g;
        int w = this.getWidth();
        int h = this.getHeight();
        g2.drawImage(img, 0, 0, w, h, 0, 0, img.getWidth(), img.getHeight(), null);

        Airport start = MainWindow.getInstance().getStart();
        Airport end = MainWindow.getInstance().getEnd();

        g.setColor(Color.RED);
        if(start == null)
            return;
        g.drawString(start.getIATA(), getX(start.lng) - 3, getY(start.lat) - 3);
        g.fillOval(getX(start.lng) - 3, getY(start.lat) - 3, 6, 6);

        if(end == null)
            return;
        g.drawString(end.getIATA(), getX(end.lng) - 3, getY(end.lat) - 3);
        g.fillOval(getX(end.lng) - 3, getY(end.lat) - 3, 6, 6);

        g.setColor(Color.RED);
        g.drawLine(getX(start.lng) - 3, getY(start.lat), getX(end.lng)- 3, getY(end.lat));
    }
}
