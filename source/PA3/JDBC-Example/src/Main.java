import java.sql.*;

public class Main {
    public static void main(String[] args) {

        Connection con = null;
        Statement stmt = null;

        ResultSet rs = null;

        try {

            /** Schritt 1: JDBC-Treiber registrieren */
            Class.forName("org.apache.derby.jdbc.EmbeddedDriver");
            /** Schritt 2: Connection zum Datenbanksystem herstellen */
            con = DriverManager.getConnection("jdbc:derby:db/derby;create=true");
            /** Schritt 3: Statement erzeugen */
            stmt = con.createStatement();
            /** Schritt 4: Statement direkt ausfuehren */
            rs = stmt.executeQuery("select author, title from Books");
            /** Schritt 5: Ergebnis der Anfrage verwenden */
            while (rs.next()) {
                System.out.println(rs.getString("author") + " "
                        + rs.getString("title"));
            }
        } catch (Exception e) {
            System.out.println(e.toString());
        } finally {
            /** Schritt 6: Ressourcen freigeben */
            try {
                if (rs != null) rs.close();
                if (stmt != null) stmt.close();
                if (con != null) con.close();
            } catch (Exception e) {
                System.out.println(e.toString());
            }
        }
    }
}