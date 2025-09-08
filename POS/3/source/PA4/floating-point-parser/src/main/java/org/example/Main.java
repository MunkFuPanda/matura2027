package org.example;

import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Main {
    public static void main(String[] args) {
        final String regex = "(?<zahl>(?<vor>[\\d]+)(?:(?<komma>[,.])(?<nach>[\\d]+))?)";
        final String string = "Invoice #2049: Total due 1299.99 EUR, paid 1200,75 EUR on 2023-04-15 at 09:45.\n"
                + "Next payment: 750.50 USD or 680,40 GBP — choose your currency.\n"
                + "Item codes: A1B2C3, D4E5F6 — Qty: 15, 8.0, 3,14159 and 2,718.28\n"
                + "Temperature logs: -4.5°C, 37.0°C, 0,0°C, and 98,6°F recorded.\n"
                + "Coordinates: Lat 48.2082, Long 16,3738 (Vienna); backup location at 40.7128° N, 74,0060° W (New York).\n"
                + "Measurements in cm: 10,5 × 20.75 × 15,0; volume ≈ 3141,59 cm³.\n"
                + "Bank transfer IDs: 000123456789, REF-2024.002, and #998776.\n\n"
                + "2.5\n"
                + "2,5";

        final Pattern pattern = Pattern.compile(regex, Pattern.MULTILINE);
        final Matcher matcher = pattern.matcher(string);

        while (matcher.find()) {
            System.out.println("Full match: " + matcher.group(0));

            for (int i = 1; i <= matcher.groupCount(); i++) {
                System.out.println("Group " + i + ": " + matcher.group(i));
            }
        }
    }
}