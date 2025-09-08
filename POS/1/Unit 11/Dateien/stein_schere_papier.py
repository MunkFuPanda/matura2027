"""
author: Markus Spitzer
file: stein_schere_papier.py
"""

import random
from operator import itemgetter

players = {}

print("Willkommen zu Stein, Papier und Schere!")

# spieleranzahl
spieleranzahl = 0
while spieleranzahl < 1:
    raw_input = input("Spieleranzahl:")
    try:
        spieleranzahl = int(raw_input)
    except:
        pass

# punkteanzahl
needed_wins = 0
while needed_wins < 1:
    raw_input = input("Wieviele Punkte sind zum Sieg nötig?")
    try:
        needed_wins = int(raw_input)
    except:
        pass
    
# spieler definieren
for i in range(spieleranzahl):
    # spielername
    Mensch = ""
    while len(Mensch) > 5 or len(Mensch) < 1:
        Mensch = input(f"Spielername {i + 1} (max. 5 Zeichen):")

    players[Mensch] = 0

for Mensch, human_wins in players.items():
    print(f"Spieler {Mensch} ist jetzt dran")
    robot_wins = 0
    options = {
        1: 3,
        2: 1,
        3: 2
    }

    while human_wins < needed_wins and robot_wins < needed_wins:
        selection = None

        while not selection:
            selection = input("Wähle: (1)Stein (2)Papier (3)Schere   ")
            try:
                selection = int(selection)
            finally:
                if selection > 3 or selection < 1:
                    selection = None

        robot_choice = random.randint(1,3)

        if robot_choice == selection:
            print("Unentschieden!")
        elif options[selection] == robot_choice:
            print(f"{Mensch} gewinnt!")
            human_wins += 1
        else:
            print("Roboter gewinnt!")
            robot_wins += 1

        print(f'Punktestand: {Mensch} {human_wins}    Computer {robot_wins}')

    players[Mensch] = human_wins

# gewinner entscheiden

sortiert = sorted(players.items(), key=itemgetter(1), reverse=True)

for key, value in sortiert:
    print(f'Spieler {key} hat {value} Punkte')

print(f"\n{sortiert[0][0]} hat gewonnen!")
