package org.example;

import org.example.roman.Numerals;
import org.example.roman.States;

// import States.* for prettier code
import static org.example.roman.States.*;

public class RomanNumbers {


    // tab to navigate in roman num
    private static final States[][] tab = {
            // I V X L C D M E
            {A, C, F, I, K, N, P, X}, // *S*
            {B, Z, Z, X, X, X, X, X}, // A
            {Z, X, X, X, X, X, X, X}, // B
            {D, X, X, X, X, X, X, X}, // C
            {B, X, X, X, X, X, X, X}, // D
            {A, C, G, H, H, X, X, X}, // F
            {A, C, H, X, X, X, X, X}, // G
            {A, C, X, X, X, X, X, X}, // H
            {A, C, J, X, X, X, X, X}, // I
            {A, C, G, X, X, X, X, X}, // J
            {A, C, F, I, L, M, M, X}, // K
            {A, C, F, I, M, X, X, X}, // L
            {A, C, F, I, X, X, X, X}, // M
            {A, C, F, I, O, X, X, X}, // N
            {A, C, F, I, L, X, X, X}, // O
            {A, C, F, I, K, N, P, X}, // P
            {X, X, X, X, X, X, X, X}, // Z
            {X, X, X, X, X, X, X, X}, // *X*
    };

    // val to calculate roman num
    private static final int[][] val = {
            // I V X L C D M E
            {1, 5, 10, 50, 100, 500, 100, 0}, // *S*
            {1, 0, 0, 0, 0, 0, 0, 0}, // A
            {1, 0, 0, 0, 0, 0, 0, 0}, // B
            {1, 0, 0, 0, 0, 0, 0, 0}, // C
            {1, 0, 0, 0, 0, 0, 0, 0}, // D
            {1, 5, 10, 30, 80, 0, 0, 0}, // F
            {1, 5, 10, 0, 0, 0, 0, 0}, // G
            {1, 5, 0, 0, 0, 0, 0, 0}, // H
            {1, 5, 10, 0, 0, 0, 0, 0}, // I
            {1, 5, 10, 0, 0, 0, 0, 0}, // J
            {1, 5, 10, 50, 100, 300, 800, 0}, // K
            {1, 5, 10, 50, 100, 0, 0, 0}, // L
            {1, 5, 10, 50, 0, 0, 0, 0}, // M
            {1, 5, 10, 50, 100, 0, 0, 0}, // N
            {1, 5, 10, 50, 100, 0, 0, 0}, // O
            {1, 5, 10, 50, 100, 500, 1000, 0}, // P
            {0, 0, 0, 0, 0, 0, 0, 0}, // Z
            {0, 0, 0, 0, 0, 0, 0, 0}, // *X*
    };

    public static int romanToInt(String roman) {

        // States current = States.S;
        States current = S; // pos in val
        int res = 0;

        for (char letter : roman.toCharArray()) {
            Numerals num = Numerals.which(letter);
            res += val[current.ordinal()][num.ordinal()];
            current = tab[current.ordinal()][num.ordinal()]; // .ordinal() gets "index" from enum
        }

        // if on Start or not valid roman int exit
        if (current == S ||current == X) {
            return -1;
        }

        return res;
    }

    public static void main(String[] args) {
        int res = romanToInt("MCMLXXXVI");
        if (res != -1) {
            System.out.println("Roman number: " + res);
            return;
        }

        System.out.println("Roman number invalid: " + res);
    }
}
