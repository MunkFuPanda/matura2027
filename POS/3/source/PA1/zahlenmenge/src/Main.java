public class Main {
	public static void main(String[] args) {
		Set s1 = new Set();
		s1.set(-3);
		s1.set(-5);
		s1.set(2);
		s1.set(-9);
		s1.set(-4);
		s1.set(0);
		s1.set(4);
		s1.set(10);

		Set s2 = new Set();
		s2.set(1);
		s2.set(-3);
		s2.set(7);
		s2.set(-5);
		s2.set(0);
		s2.set(2);
		s2.set(9);

		s1.print();
		s1.union(s2).print();
		s1.intersect(s2).print();
		s1.range(0, 10).print();
		s2.range(-10, 0).print();
		s2.remove(0);
		s2.remove(2);
		s1.intersect(s2).print();
		s1.diff(s2).print();
		s1.print();
		System.out.printf("Total: %d\n", s1.total());
		System.out.printf("Avg: %f\n", s1.average());
		System.out.printf("Min: %d\n", s1.min());
		System.out.printf("Max: %d\n", s1.max());
	}
}