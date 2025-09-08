import java.util.Arrays;

public class Main {
    public static void main(String[] args) {
        PermutationGenerator g = new PermutationGenerator(6);
        for (Integer[] a : g) {
            System.out.println(Arrays.toString(a));
        }
    }
}
