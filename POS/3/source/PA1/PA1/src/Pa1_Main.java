public class Pa1_Main {
    public static void main(String[] args) {
        //Test Aufgabe 1
        Set s = new Set();
        s.insert(15);
        s.insert(12);
        s.insert(17);
        s.insert(6);
        s.insert(-6);
        s.insert(10);
        s.insert(22);
        System.out.println(s); //[-6 6 10 12 15 17 22]
        System.out.println(s.getDivisible(3)); //[-6 6 12 15]
        System.out.println(s.getDivisible(9)); //[ ]

        //Test Aufgabe 3
        Lotto lotto = new Lotto(45);
        System.out.println(lotto.getZahlen(6));
        //Lotto lotto2 = new Lotto(10);
        //System.out.println(lotto2.getZahlen(10));

        MainWindow mw = new MainWindow();
        mw.setVisible(true);
    }
}