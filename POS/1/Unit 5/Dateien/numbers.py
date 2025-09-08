"""
author: Markus Spitzer
file: numbers.py
"""

n = int(input("Zahl zwischen 1 - 100 eingeben:"))

def n2w(n):
    einer = n % 10
    zehner = n // 10
    verbindung = ""
    ergebnis = ""

    if einer == 1:
        einer = "eins"
    elif einer == 2:
        einer = "zwei"
    elif einer == 3:
        einer = "drei"
    elif einer == 4:
        einer = "vier"
    elif einer == 5:
        einer = "fünf"
    elif einer == 6:
        einer = "sechs"
    elif einer == 7:
        einer = "sieben"
    elif einer == 8:
        einer = "acht"
    elif einer == 9:
        einer = "neuen"

    if zehner == 0:
        ergebnis = einer
        zehner = ""
    elif zehner == 1:
        zehner = "zehn"
    elif zehner == 2:
        zehner = "zwanzig"
        if type(einer) == str:
            verbindung = "und"
    elif zehner == 3:
        zehner = "dreißig"
        if type(einer) == str:
            verbindung = "und"
    elif zehner == 4:
        zehner = "vierzig"
        if type(einer) == str:
            verbindung = "und"
    elif zehner == 5:
        zehner = "fünfzig"
        if type(einer) == str:
            verbindung = "und"
    elif zehner == 6:
        zehner = "sechzig"
        if type(einer) == str:
            verbindung = "und"
    elif zehner == 7:
        zehner = "siebzig"
        if type(einer) == str:
            verbindung = "und"
    elif zehner == 8:
        zehner = "achtzig"
        if type(einer) == str:
            verbindung = "und"
    elif zehner == 9:
        zehner = "neunzig"
        if type(einer) == str:
            verbindung = "und"

    if n == 11:
        ergebnis = "elf"
    elif n == 12:
        ergebnis = "zwölf"
    elif n == 17:
        ergebnis = "siebzehn"
    elif n % 10 == 1 and n // 10 > 0:
        ergebnis = "einund" + zehner
    elif n == 100:
        ergebnis = "hundert"
    else:
        ergebnis = einer + verbindung + zehner

    return ergebnis

if n >= 1 and n <= 100:
    print(n2w(n))
else:
    print("Ungültige Eingabe!")
