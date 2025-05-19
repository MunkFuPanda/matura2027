import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.IOException;
import java.sql.*;

public class MainWindow extends JFrame {

    private static MainWindow instance;

    private JPanel main;
    private JTextField start_airport;
    private JTextField end_airport;
    private JButton search_button;
    private JButton show_button;
    private JPanel map;
    private Map map1;
    private Connection con;
    private Airport start;
    private Airport end;

    public MainWindow() throws IOException {
        setVisible(true);
        setTitle("Airlines");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(800, 600);
        setContentPane(main);

        instance = this;

        map.setLayout(new GridLayout(1, 1));

        try {
            Class.forName("org.apache.derby.jdbc.EmbeddedDriver");
            System.setProperty("derby.language.sequence.preallocator", "1");
            con = DriverManager.getConnection("jdbc:derby:C:/Users/Lenny/Documents/Lenny/Schule/Klasse 3/POS/Übungen/Swing Airlines/Airlines/Airlines");
        } catch (Exception e) {
            e.printStackTrace();
        }

        search_button.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                try {
                    setAirports(start_airport.getText(), end_airport.getText());
                } catch (Exception ex) {
                    throw new RuntimeException(ex);
                }

            }
        });

        show_button.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {

            }
        });
    }

    public void setAirports(String start_airport, String end_airport) throws SQLException, IOException {
        // TODO (Weil Tobi alles so GENAU will): Nach Airport Namen suchen und Dialog anzeigen
        try {
            PreparedStatement stps = con.prepareStatement("select * from Airport where name like ?");
            stps.setString(1, start_airport);
            ResultSet start_rs = stps.executeQuery();
            if (!start_rs.next()) {
                stps = con.prepareStatement("select * from Airport where iata like ?");
                stps.setString(1, start_airport + "%");
                start_rs = stps.executeQuery();
            }

            PreparedStatement etps = con.prepareStatement("select * from Airport where name like ?");
            etps.setString(1, end_airport);
            ResultSet end_rs = etps.executeQuery();
            if (!end_rs.next()) {
                etps = con.prepareStatement("select * from Airport where iata like ?");
                etps.setString(1, end_airport + "%");
                end_rs = etps.executeQuery();
            }

            if (start_rs.getFetchSize() == 0 || end_rs.getFetchSize() == 0) {
                System.out.println("Start und/oder End Flughafen wurde nicht eingegeben");
                return;
            }
            if ((start_rs.getFetchSize() == 1 && end_rs.getFetchSize() == 1) && (start_rs.next() && end_rs.next())) {
                start =  new Airport(start_rs.getInt(1), start_rs.getDouble(2), start_rs.getDouble(3),
                        start_rs.getString(4), start_rs.getString(5));
                end =  new Airport(end_rs.getInt(1), end_rs.getDouble(2), end_rs.getDouble(3),
                        end_rs.getString(4), end_rs.getString(5));

                //drawAirport(getX(start.getLng()), getY(start.getLat()), start.getIATA());
                //drawAirport(getX(end.getLng()), getY(end.getLat()), end.getIATA());
                //drawDistance();
                repaint();
            } else {
                Dialog dialog = new AirportChooser(start_rs, end_rs);
                dialog.pack();
                dialog.setVisible(true);
            }
        } catch (Exception ex) {
            throw new RuntimeException(ex);
        }
    }

    /*private void drawAirport(int lng, int lat, String airport) throws IOException {
        Graphics g = map1.getGraphics();

        g.setColor(Color.RED);
        g.drawString(airport, lng - 3, lat - 3);
        g.fillOval(lng - 3, lat - 3, 6, 6);

        //airports.add(new Point(lng, lat));
    }*/

    /*private void drawDistance() {
        Graphics g = map1.getGraphics();

        g.setColor(Color.RED);
        g.drawLine(getX(start.lng) - 3, getY(start.getLat()), getX(end.getLng())- 3, getY(end.getLat()));
    }*/

    /*@Override
    public void paint(Graphics g) {
        super.paint(g);
        g.setColor(Color.RED);
        if(start == null)
            return;
        g.drawString(start.getIATA(), getX(start.lng) - 3, getY(start.lat) - 3);
        g.fillOval(getX(start.lng) - 3, getY(start.lat) - 3, 6, 6);

        if(end == null)
            return;
        g.drawString(end.getIATA(), getX(end.lng) - 3, getY(end.lat) - 3);
        g.fillOval(getX(end.lng) - 3, getY(end.lat) - 3, 6, 6);
        drawDistance();
    }*/

    private void createUIComponents() throws IOException {
        map1 = new Map();
    }

    public static void main(String[] args) throws IOException {
        new MainWindow();
    }

    public static MainWindow getInstance() { return instance; }

    public Airport getStart() { return start; }

    public Airport getEnd() { return end; }

    public JPanel getPanel() { return map; }
}