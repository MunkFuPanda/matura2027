"""
author: Markus Spitzer
file: tictactoe.py
"""

validators = {
    "play_again": lambda x: not len(x) or x.lower() == "y",
    "character": lambda x: len(x) and x.upper() in "OX",
}

prompts = {
    "play_again": "Do you want to play again? (y/N):",
    "pick_char": "Do you want to play X or O:",
    "pick_start": "Who should start? X or O:",
    "pick_spot": "Input where to place:",
}


def force_input(prompt, cb):
    while True:
        x = input(prompt)
        try:
            if cb(x):
                return x
        except:
            pass
        print("Invalid input!")


class Board:

    def __init__(self, playerCharacter, startingCharacter):
        self.board = {i: " " for i in range(9)}
        self.player = playerCharacter
        self.bot = "O" if playerCharacter == "X" else "X"
        self.win_moves = [
            [0, 1, 2],
            [3, 4, 5],
            [6, 7, 8],
            [0, 4, 8],
            [2, 4, 6],
            [0, 3, 6],
            [1, 4, 7],
            [2, 5, 8],
        ]

    def validate_move(self, position):
        return 10 > int(position) > 0 and self.board[int(position) - 1] == " "

    def minimax(self, letter, maxOut):
        enemy = "X" if letter == "O" else "O"

        if self.get_winner() == self.player:
            return -1
        elif self.get_winner() == self.bot:
            return 1
        elif self.is_draw():
            return 0

        best_score = -1 if maxOut else 1
        for i in self.board.keys():
            if self.board[i] == " ":
                self.board[i] = letter if maxOut else enemy
                score = self.minimax(letter, not maxOut)
                self.board[i] = " "
                if (score if maxOut else best_score) > (best_score if maxOut else score):
                    best_score = score
        return best_score

    def move(self, position):
        self.board[position] = self.player
        best_move = 0
        best_score = -1

        for i in self.board.keys():
            if self.board[i] == " ":
                self.board[i] = self.bot
                score = self.minimax(self.bot, False)
                self.board[i] = " "
                if score > best_score:
                    best_score = score
                    best_move = i

        self.board[best_move] = self.bot

    def output(self):
        for i in range(9):
            field = str(i + 1) if self.board[i] == " " else self.board[i]
            print(f'|{field}', end="")
            if i % 3 == 2:
                print("|")

    def get_winner(self):
        for combo in self.win_moves:
            if all(self.board[x] == self.bot for x in combo):
                return self.bot
            if all(self.board[x] == self.player for x in combo):
                return self.player

    def is_draw(self):
        return  " " not in self.board.values()


    def is_finished(self):
        return self.is_draw() or self.get_winner()


if __name__ == "__main__":
    print("Welcome to TicTacToe!\n")
    player = force_input(prompts["pick_char"], validators["character"])
    starter = force_input(prompts["pick_start"], validators["character"])
    db = True
    while db or force_input(prompts["play_again"], validators["play_again"]):
        db = False    
        board = Board(player, starter)

        while not board.is_finished():
            board.output()

            board.move(int(force_input(prompts["pick_spot"], board.validate_move)) - 1)

        print(board.get_winner() or "Noone", "won!")
