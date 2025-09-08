package State;

public enum Numerals {
    I('I'), V('V'), X('X'), L('L'), C('C'), D('D'), M('M'), E('*');
    char n;

    private Numerals(char num) {
        n = num;
    }

    public static Numerals which (char zeichen) {
        for (Numerals num : Numerals.values()) {
            if (num.n == zeichen) {
                return num;
            }
        }
        return E;
    }
}
