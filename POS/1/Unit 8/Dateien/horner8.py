"""
author: Markus Spitzer
file: horner8.py
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

Zahlenlaenge = len(gefilterte_Eingabe) -1
#sie wendet die hornermethode an.
for Zaehler, Stelle in enumerate(gefilterte_Eingabe):
    erg += (int(Stelle) * 8 ** (Zahlenlaenge - Zaehler))
    
print(erg)

