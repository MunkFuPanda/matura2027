import javax.swing.*;
import java.awt.*;
import java.awt.image.BufferedImage;

public class Map extends JPanel {
    private static final Map instance = new Map();
    private BufferedImage img;

    private Airport lastDestination;

    public static Map getInstance() {
        return instance;
    }

    private Map() {
        try {
            img = javax.imageio.ImageIO.read(new java.io.File("Earthmap.jpg"));
        } catch (java.io.IOException e) {
            e.printStackTrace();
        }
    }

    private int getX(double lng) {
        return (int) ((lng + 180) / 360 * getWidth());
    }

    private int getY(double lat) {
        return (int) ((90 - lat) / 180 * getHeight());
    }

    @Override
    public void paint(Graphics g) {
        super.paint(g);
        if (img != null) {
            g.drawImage(img, 0, 0, getWidth(), getHeight(), null);
        }

        while (lastDestination != null && lastDestination.getParent() != null) {
            if (lastDestination != null) {
                g.setColor(java.awt.Color.RED);
                g.drawString(lastDestination.getIata(), getX(lastDestination.getLng()) - 3, getY(lastDestination.getLat()) - 3);
                g.fillOval(getX(lastDestination.getLng()) - 3, getY(lastDestination.getLat()) - 3, 6, 6);
            }

            if (lastDestination.getParent() != null) {
                g.setColor(java.awt.Color.RED);
                g.drawLine(getX(lastDestination.getLng()) - 3, getY(lastDestination.getLat()),
                           getX(lastDestination.getParent().getLng()) - 3, getY(lastDestination.getParent().getLat()));

                g.drawString(lastDestination.getParent().getIata(), getX(lastDestination.getParent().getLng()) - 3,
                             getY(lastDestination.getParent().getLat()) - 3);
                g.fillOval(getX(lastDestination.getParent().getLng()) - 3, getY(lastDestination.getParent().getLat()) - 3, 6, 6);
            }

            lastDestination = lastDestination.getParent();
        }
    }

    public void setDestination(Airport dest) {
        this.lastDestination = dest;
        repaint();
    }
}
