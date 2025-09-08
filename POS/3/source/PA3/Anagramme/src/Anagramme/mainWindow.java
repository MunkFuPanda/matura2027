package Anagramme;

import javax.swing.*;
import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.*;

public class mainWindow extends JFrame {
    private JTextField textField1;
    private JList list1;
    private JButton weiterButton;
    private JPanel panel1;
    private JButton auswertenButton;
    private JSlider slider1;
    private JButton wahrscheinlichsteButton;
    private int count;
    private ArrayList<String> list = new ArrayList<>();
    private String abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private String[] alphabet = abc.split("");
    private Hashtable<String, Integer> hashtable = new Hashtable<>();

    public mainWindow() {
        super("Anagramme Generator");
        setContentPane(panel1);
        setVisible(true);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setSize(1000, 800);


        String Bigramme = "";
        String dateipfad = "Resources/Deutsch.csv";
        try (BufferedReader br = new BufferedReader(new FileReader(dateipfad))) {
            String zeile;

            while ((zeile = br.readLine()) != null) {
                Bigramme += zeile + ";";
            }
        } catch (IOException e) {
            System.err.println("Fehler beim Lesen der Datei: " + e.getMessage());
        }


        ArrayList<String> bigramme = new ArrayList<>(Arrays.asList(Bigramme.split(";")));
        bigramme.remove(bigramme.size() - 1);
        HashSet<String> bigrammeSet = new HashSet<>(bigramme);
        bigramme = new ArrayList<>(bigrammeSet);
        for (String s : bigramme) {
            for (String ss : s.split(".")) {
                if (Integer.parseInt(ss) > 51) {
                    bigramme.remove(s);
                }
            }
        }

        ArrayList<String> finalBigramme1 = bigramme;
        auswertenButton.addActionListener(e -> {
            list.clear();
            hashtable.clear();
            String input = textField1.getText();
            if (input.length() != 5) {
                textField1.setText("");
                return;
            }
            String[] anagramme = input.split("");
            PermutationenGenerator g = new PermutationenGenerator(5);
            HashSet<String> set = new HashSet<>();
            String temp = "";
            for (int[] a : g) {
                for (Integer i : a) {
                    temp += anagramme[i];
                }
                set.add(temp);
                temp = "";
            }
            list.addAll(set);
            list.sort(String::compareTo);







            for (String s : list) {
                for (String bigram : finalBigramme1) {
                    System.out.println(bigram);
                    String string1 = "";
                    String string2 = "";

                    int string_count = 1;
                    for (String ss : bigram.split("")) {
                        if (Objects.equals(ss, ".")) {
                            string_count++;
                            continue;
                        }
                        if (string_count == 1) {
                            string1 += ss;
                        }
                        if (string_count == 2) {
                            string2 += ss;
                        }

                    }
                    string_count = 1;

                    if (Integer.parseInt(string1) > 52 || Integer.parseInt(string2) > 52) {
                        continue;
                    }

                    String bigram2 = alphabet[Integer.parseInt(string1)] + alphabet[Integer.parseInt(string2)];
                    System.out.println(bigram2);
                    if (s.contains(bigram2)) {
                        hashtable.put(s, hashtable.getOrDefault(s, 0) + 1);
                    }
                    else {
                        hashtable.put(s, hashtable.getOrDefault(s, 0));
                    }
                }
            }


            wahrscheinlichsteButton.addActionListener(e1 -> {
                int amount = 0;
                if (slider1.getValue() > list.size()) {
                    amount = list.size();
                }
                else {
                    amount = slider1.getValue();
                }


                int counter = 0;
                String[] array = new String[amount];


                for (int i = 0; i < 100; i++) {
                    for (Map.Entry<String, Integer> entry : hashtable.entrySet()) {
                        if (counter == amount) {
                            break;
                        }
                        if (entry.getValue() == i) {
                            array[counter] = entry.getKey();
                            counter++;
                        }
                    }
                }
                list1.setListData(array);



            });


            count = slider1.getValue();
            String[] array = new String[count];
            if (count > list.size()) {
                count = list.size();
            }
            for (int i = 0; i < count; i++) {
                array[i] = list.get(i);
            }
            list1.setListData(array);
        });

        weiterButton.addActionListener(e -> {
            int slidercount = slider1.getValue();
            if (count + slidercount > list.size()) {
                slidercount = list.size() - count;
            }
            if (slidercount == 0) {
                return;
            }
            try {
                String[] array = new String[slidercount];
                if (count + slidercount > list.size()) {
                    slidercount = list.size() - count;
                }
                for (int i = count; i < count + slidercount; i++) {
                    array[i - count] = list.get(i);
                }
                count += slidercount;
                list1.setListData(array);
            }
            catch (ArrayIndexOutOfBoundsException ex) {
            }
        });
    }


    public static void main(String[] args) {
        mainWindow mainWindow = new mainWindow();
    }
}
