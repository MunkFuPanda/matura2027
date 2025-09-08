import java.util.Scanner;

public class Main {
	public static void main(String[] args) {
		Scanner scanner = new Scanner(System.in);
		Suitcase suitcase = new Suitcase();

		System.out.print("Gib bitte die Anzahl der möglichen Fehler an: ");
		int fehler = scanner.nextInt();
		scanner.nextLine();

		while (fehler > 0) {
			for (int i = 0; i < 20; i++) {
				System.out.println();
			}

			Piece first = suitcase.getRoot();

			if (first == null) {
				System.out.println("Pack bitte einen neuen Gegenstand ein: ");
				String name = scanner.nextLine();
				suitcase.addNewPiece(name);
				continue;
			}

			while (first != null) {
				System.out.println("Nenne den nächsten Gegenstand im Koffer: ");
				String name = scanner.nextLine();
				if (!first.getName().equals(name)) {
					fehler--;
					System.out.println("Das war leider nicht korrekt!");
					System.out.println("Der Inhalt ist: " + suitcase);
					System.out.println("Eingabe zum fortsetzen!");
					scanner.nextLine();
					break;
				}
				first = first.getNext();

				if (first == null) {
					System.out.println("Pack bitte einen neuen Gegenstand ein: ");
					String piece = scanner.nextLine();
					suitcase.addNewPiece(piece);
				}
			}

			System.out.println("Du hast erfolgreich " + suitcase.size() + " Gegenstände eingepackt!");
		}
	}
}