public class Main {
	public static void main(String[] args) {
		Zahlenmenge<Integer> s1 = new Zahlenmenge<>();
		s1.set(-9);
		s1.set(-5);
		s1.set(-4);
		s1.set(-3);
		s1.set(0);
		s1.set(2);
		s1.set(4);
		s1.set(10);

		Zahlenmenge<Integer> s2 = new Zahlenmenge<>();
		s2.set(-5);
		s2.set(-3);
		s2.set(0);
		s2.set(1);
		s2.set(2);
		s2.set(7);
		s2.set(9);

		s1.print();
		s2.print();
		System.out.println(s1.total());
		System.out.println(s2.average());
	}
}
