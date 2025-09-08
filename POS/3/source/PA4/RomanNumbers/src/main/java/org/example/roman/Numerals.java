package org.example.roman;

public enum Numerals {
    I('I'), V('V'), X('X'), L('L'), C('C'), D('D'), M('M'), E('*');

    private char n;

    private Numerals(char num) {
        n = num;
    }

    public static Numerals which(char letter) {
        for (Numerals symbol : Numerals.values()) {
            if (symbol.n == letter) {
                return symbol;
            }
        }
        return E;
    }
}
