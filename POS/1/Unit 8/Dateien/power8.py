"""
author: Markus Spitzer
file: power8.py
"""

Eingabe = input("Geben Sie eine Zahl an:")
erg = 0
gefilterte_Eingabe = ""

#geht durch eingabe und packt jede stelle, die eine zahl ist in die gefilterte eingabe.
for Stelle in Eingabe:
    try:
        if int(Stelle) < 8:
            gefilterte_Eingabe += Stelle
        else:
            print("Ziffer ist zu groß:" + Stelle)
    except:
        print("Ungültige Eingabe:" + Stelle)

hoechste_Hochzahl = len(gefilterte_Eingabe)-1
Zaehler = 0
#sie wendet die potenzmethode an.
for Stelle in gefilterte_Eingabe:
    Stelle_int = int(Stelle)
    erg += Stelle_int * 8 ** (hoechste_Hochzahl - Zaehler)
    Zaehler = Zaehler + 1

print(erg)









