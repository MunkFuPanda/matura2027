import javax.swing.*;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.util.HashSet;

public class Map extends JPanel {
    private static final Map instance = new Map();
    private BufferedImage img;
    private HashSet<City> cachedCities = new HashSet<>();
    private double distance = 0;

    public static Map getInstance() {
        return instance;
    }

    private Map() {
        try {
            img = javax.imageio.ImageIO.read(new java.io.File("DACH.png"));
        } catch (java.io.IOException e) {
            e.printStackTrace();
        }
    }

    public void setCitiesAndDistance(HashSet<City> cities, double distance) {
        this.cachedCities = cities;
        this.distance = distance;
        repaint();
    }

    private int getX(double lng) {
        return (int) ((lng - 5.5f) / (17.2f - 5.5f) * getWidth());
    }

    private int getY(double lat) {
        return (int) ((55.1f - lat) / (55.1f - 45.7f) * getHeight());
    }

    @Override
    public void paint(Graphics g) {
        super.paint(g);
        if (img != null) {
            g.drawImage(img, 0, 0, getWidth(), getHeight(), null);
        }

        for (City city : cachedCities) {
            g.setColor(Color.BLACK);
            g.drawString(city.getName(), getX(city.getLongitude()) - 3, getY(city.getLatitude()) - 3);
            g.setColor(Color.RED);
            g.drawOval(getX(city.getLongitude()) - 3, getY(city.getLatitude()) - 3, 6, 6);

            for (City connectedCity : cachedCities) {
                if (Distance.measure(city.getLongitude(), city.getLatitude(), connectedCity.getLongitude(), connectedCity.getLatitude()) < distance) {
                    g.setColor(Color.BLUE);
                    g.drawLine(getX(city.getLongitude()), getY(city.getLatitude()), getX(connectedCity.getLongitude()), getY(connectedCity.getLatitude()));
                }
            }
        }
    }
}
