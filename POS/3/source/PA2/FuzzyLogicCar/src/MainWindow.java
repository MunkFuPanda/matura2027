import net.sourceforge.jFuzzyLogic.FIS;
import net.sourceforge.jFuzzyLogic.plot.JFuzzyChart;

import javax.swing.*;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class MainWindow extends JFrame {
    private JPanel mainPanel;
    private JButton showChartButton;
    private JButton runCarButton;
    private JButton resetCarButton;
    private JSlider delay;
    private JPanel panel;
    private CarPanel carPanel;

    private FIS fis;
    private Timer timer = null;
    private double distance = 100;
    private double maxDistance = 100;
    private double speed = 0;
    private double acceleration = 0;
    private double power = 0;
    private double updateTime = 1000;

    ActionListener timerListener = new ActionListener() {
        @Override
        public void actionPerformed(ActionEvent e) {
            System.out.println((updateTime / 100) / 10.0 + " Sekunden sind vergangen");
            distance -= speed * updateTime / 1000;
            speed += acceleration * updateTime / 1000;
            speed = Math.max(speed, 0);
            System.out.printf("Beschleunigung: %.2f Geschwindigkeit: %.2f Distanz: %.2f%n", acceleration, speed, distance);
            //power mit Fuzzy Logic ermitteln
            // Set inputs
            fis.setVariable("speed", speed);
            fis.setVariable("acceleration", acceleration);
            fis.setVariable("distance", distance);

            // Evaluate
            fis.evaluate();

            power += fis.getVariable("power").defuzzify() * updateTime / 1000;
            power = Math.max(Math.min(power, 10), -5);
            System.out.printf("Power: %.2f%n", power);
            acceleration = Math.min(Math.max((power - speed)* 0.5, -2), 2);
            carPanel.setDistance(distance);
        }
    };

    public MainWindow() throws HeadlessException {
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setTitle("Fuzzy Logic Car");
        setSize(800, 600);
        setContentPane(mainPanel);

        carPanel = new CarPanel();
        panel.add(carPanel);

        String fileName = "data/car.fcl";
        fis = FIS.load(fileName,true);
        // Error while loading?
        if( fis == null ) {
            System.err.println("Can't load file: '"
                    + fileName + "'");
        }


        showChartButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                JFuzzyChart.get().chart(fis);
            }
        });
        runCarButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                if(timer != null)
                {
                    timer.stop();
                }
                carPanel.setMaxDistance(maxDistance);
                timer = new Timer((int)updateTime, timerListener);
                timer.start();
            }
        });
        resetCarButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                if(timer != null)
                {
                    timer.stop();
                    timer = null;
                    distance = maxDistance;
                    speed = 0;
                    acceleration = 0;
                    carPanel.setDistance(maxDistance);
                }
            }
        });
        delay.addChangeListener(new ChangeListener() {
            @Override
            public void stateChanged(ChangeEvent e) {
                updateTime = delay.getValue();
            }
        });
    }

    public static void main(String[] args) {
        MainWindow main = new MainWindow();
        main.setVisible(true);
    }
}
