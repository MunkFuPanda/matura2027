import java.util.ArrayList;
import java.util.List;
import java.util.Random;

public class Player {
    private String name;
    private List<DominoTile> tiles;

    public Player(String name) {
        this.name = name;
        this.tiles = new ArrayList<>();
    }

    public String getName() {
        return name;
    }

    public List<DominoTile> getTiles() {
        return tiles;
    }

    public void addTile(DominoTile tile) {
        tiles.add(tile);
    }

    public DominoTile playTile(int index) {
        return tiles.remove(index);
    }

    public boolean hasMatchingTile(int num) {
        return tiles.stream().anyMatch(tile -> tile.matchesLeft(num) || tile.matchesRight(num));
    }

    public boolean isEmpty() {
        return tiles.isEmpty();
    }
}
