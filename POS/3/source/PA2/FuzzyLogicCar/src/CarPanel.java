import javax.imageio.ImageIO;
import javax.swing.*;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.Logger;

public class CarPanel extends JPanel {
    private BufferedImage img;
    private double dist = 100;

    public double getMaxDistance() {
        return maxDistance;
    }

    public void setMaxDistance(double maxDistance) {
        this.maxDistance = maxDistance;
        repaint();
    }

    private double maxDistance = 100;

    public void setDistance(double distance)
    {
        dist = distance;
        repaint();
    }

    /**
     * Creates new form CarPanel
     */
    public CarPanel() {
        try {
            this.img = ImageIO.read(new File("data/car.png"));
        } catch (IOException ex) {
            Logger.getLogger(CarPanel.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    @Override
    public void paint(Graphics g) {
        super.paint(g);
        int w = this.getWidth();
        int h = this.getHeight();
        g.drawLine((int)(w * 0.2), (int)(h * 0.8), w - 1, (int)(h * 0.8));
        g.drawLine(w - 1, (int)(h * 0.8) - 5, w - 1, (int)(h * 0.8) + 5);
        g.drawLine((int)(w * 0.2), (int)(h * 0.8) - 5, (int)(w * 0.2), (int)(h * 0.8) + 5);
        g.drawString("" + maxDistance, (int)(w * 0.2), (int)(h * 0.8) + 13);
        g.drawString("0", w - 8, (int)(h * 0.8) + 13);
        g.drawImage(img, (int)((maxDistance - dist) * w * 0.8 / maxDistance), (int)(h * 0.8) - (int)(w * 0.2) / 2, (int)(w * 0.2), (int)(w * 0.2) / 2, null);
    }
}
