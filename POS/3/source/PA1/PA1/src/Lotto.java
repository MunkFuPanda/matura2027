import java.util.*;
import java.util.Set;

public class Lotto {
    public Lotto(int range) {
        this.range = range;
    }

    private int range;

    Random rand = new Random();

    Collection<Integer> getZahlen(int anzahl)
    {
        Collection<Integer> zahlen = new ArrayList<>();
        Collection<Integer> numbersExisting = new HashSet<>();

        while (numbersExisting.size() < anzahl) {
            int newNum = rand.nextInt(range) + 1;
            if (!numbersExisting.contains(newNum)) {
                numbersExisting.add(newNum);
                zahlen.add(newNum);
            }
        }

        return zahlen;
    }
}