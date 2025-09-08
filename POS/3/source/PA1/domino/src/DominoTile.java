public class DominoTile {
    private int left;
    private int right;

    public DominoTile(int left, int right) {
        this.left = Math.min(left, right);
        this.right = Math.max(left, right);
    }

    public int getLeft() {
        return left;
    }

    public int getRight() {
        return right;
    }

    @Override
    public String toString() {
        return left + "|" + right;
    }

    public boolean matchesLeft(int num) {
        return right == num;
    }

    public boolean matchesRight(int num) {
        return left == num;
    }
}
