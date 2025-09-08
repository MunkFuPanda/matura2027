"""
Author: Markus Spitzer
File: main.py
"""

combinations = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9],
    [1, 4, 7],
    [2, 5, 8],
    [3, 6, 9],
    [1, 5, 9],
    [3, 5, 7],
]


def getNumber(inp):
    try:
        return int(inp)
    except ValueError:
        return False


class Game:
    def __init__(self):
        self.CurrentPlayer = "X"
        self.Field = [
            "1", "2", "3",
            "4", "5", "6",
            "7", "8", "9",
        ]

    def hasWon(self):
        for combo in combinations:
            if self.Field[combo[0] - 1] == self.Field[combo[1] - 1] == self.Field[combo[2] - 1]:
                return self.Field[combo[0] - 1]

    def set(self, coordinate):
        coordinate -= 1

        if getNumber(self.Field[coordinate]):
            self.Field[coordinate] = self.CurrentPlayer

            if self.CurrentPlayer == "X":
                self.CurrentPlayer = "O"
            else:
                self.CurrentPlayer = "X"
        else:
            print("====================\nField " +
                  str(coordinate + 1) + " is already set!")

    def output(self):
        string = "====================\nCurrent state: \n\n"

        for index, coordinate in enumerate(self.Field):
            string += "|" + str(coordinate) + "|"
            if index % 3 == 2:
                string += "\n"

        string += "\n\n Player " + self.CurrentPlayer + " is on their turn!"

        print(string)


while True:
    game = Game()
    game.output()

    while not game.hasWon():
        inp = getNumber(
            input("Player " + game.CurrentPlayer + ", Please set your mark:"))

        while not getNumber(inp) or inp < 1 or inp > 9:
            print("====================\n" +
                  str(inp) + " is not a valid input!")
            inp = getNumber(
                input("Player " + game.CurrentPlayer + ", Please set your mark:"))

        game.set(inp)

        game.output()

    print("Player " + game.CurrentPlayer + " won!")
    input("Press enter to continue...")


    1 + 1
