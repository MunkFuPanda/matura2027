"""
author: Markus Spitzer
file: geom.py
"""

import geomlib

print("Willkommen beim geometrischen Wunderzwerg!\n")
selection = int(input("""
Bitte wählen Sie:

(1) Berechnung der Entfernung: eindimensional
(2) Berechnung der Entfernung: zweidimensional
(3) Berechnung der Entfernung: dreidimensional
(4) Regelmäßiges Vieleck: zeichnen
(5) Unregelmäßiges Vieleck: zeichnen
(6) Sternmuster: console output
>"""))

def tryIntExit(string):
    try:
        return int(string)
    except:
        print("Invalid Input")
        return exit()

def tryInt(string):
    try:
        return int(string)
    except:
        return None

if selection == 1:
    x1 = int(input("Bitte geben Sie den x-Wert von Punkt 1 ein:"))
    x2 = int(input("Bitte geben Sie den x-Wert von Punkt 2 ein:"))
    print(f'\nDie Entfernung zwischen dem Punkt {x1} und dem Punkt {x2} beträgt {geomlib.distance1(x1, x2)}.')
elif selection == 2:
    x1 = int(input("Bitte geben Sie den x-Wert von Punkt 1 ein:"))
    x2 = int(input("Bitte geben Sie den y-Wert von Punkt 1 ein:"))
    x3 = int(input("Bitte geben Sie den x-Wert von Punkt 2 ein:"))
    x4 = int(input("Bitte geben Sie den y-Wert von Punkt 2 ein:"))
    print(f'\nDie Entfernung zwischen dem Punkt ({x1}, {x2}) und dem Punkt ({x3}, {x4}) beträgt {geomlib.distance2(x1, x2, x3, x4)}.')
elif selection == 3:
    x1 = tryIntExit(input("Bitte geben Sie den x-Wert von Punkt 1 ein:"))
    x2 = tryIntExit(input("Bitte geben Sie den y-Wert von Punkt 1 ein:"))
    x3 = tryIntExit(input("Bitte geben Sie den z-Wert von Punkt 1 ein:"))
    x4 = tryIntExit(input("Bitte geben Sie den x-Wert von Punkt 2 ein:"))
    x5 = tryIntExit(input("Bitte geben Sie den y-Wert von Punkt 2 ein:"))
    x6 = tryIntExit(input("Bitte geben Sie den z-Wert von Punkt 2 ein:"))
    print(f'\nDie Entfernung zwischen dem Punkt ({x1}, {x2}, {x3}) und dem Punkt ({x4}, {x5}, {x6}) beträgt {geomlib.distance3(x1, x2, x3, x4, x5, x6)}.')
elif selection == 4:
    x1 = tryIntExit(input("Bitte geben Sie den Wert der x-Koordinate ein:"))
    x2 = tryIntExit(input("Bitte geben Sie den Wert der y-Koordinate ein:"))
    x3 = tryIntExit(input("Bitte geben Sie die Anzahl der Ecken ein:"))
    x4 = tryIntExit(input("Bitte geben Sie die Seitenlänge ein:"))
    print(f'\nDer Umfang des Vielecks beträgt {geomlib.regular_polygon(x1, x2, x3, x4)}.')
elif selection == 5:
    x1 = tryIntExit(input("Bitte geben Sie den x-Wert von Punkt 1 ein:"))
    x2 = tryIntExit(input("Bitte geben Sie den y-Wert von Punkt 1 ein:"))
    x3 = tryIntExit(input("Bitte geben Sie den x-Wert von Punkt 2 ein:"))
    x4 = tryIntExit(input("Bitte geben Sie den y-Wert von Punkt 2 ein:"))
    x5 = tryIntExit(input("Bitte geben Sie den x-Wert von Punkt 3 ein:"))
    x6 = tryIntExit(input("Bitte geben Sie den y-Wert von Punkt 3 ein:"))

    x7 = tryInt(input("Bitte geben Sie den x-Wert von Punkt 4 ein:"))

    if x7:
        x8 = tryInt(input("Bitte geben Sie den y-Wert von Punkt 4 ein:"))
        x9 = tryInt(input("Bitte geben Sie den x-Wert von Punkt 5 ein:"))
        if x9:
            x10 = tryInt(input("Bitte geben Sie den y-Wert von Punkt 5 ein:"))
            print(f'\nDer Umfang des Vielecks beträgt {geomlib.polygon(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10)}.')
        else:
            print(f'\nDer Umfang des Vielecks beträgt {geomlib.polygon(x1, x2, x3, x4, x5, x6, x7, x8)}.')
    else:
        print(f'\nDer Umfang des Vielecks beträgt {geomlib.polygon(x1, x2, x3, x4, x5, x6)}.')
elif selection == 6:
    amount = tryIntExit(input("Bitte geben Sie die Anzahl der Sterne ein:"))
    print(f'\nDas Muster mit {amount} Sternen sieht so aus: \n\n{geomlib.stars_triangle(amount)}.')    
else:
    print("Invalid Input")
