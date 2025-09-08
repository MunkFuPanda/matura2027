"""
author: Markus Spitzer
file: gruss.py
"""

def gruss(hallo, ort, meinung, gestern, gestern_meinung, name):
    return """{}!
Hier in {} gefällt es mir {}.
Gestern waren wir {}. Das war {}!
Bis bald,
{}""".format(hallo, ort, meinung, gestern, gestern_meinung, name)


hallo = input("Begrüßung:")
ort = input("Ort:")
meinung = input("Meinung:")
gestern = input("Wo waren Sie gestern?")
meinung_gestern = input("Wie fanden Sie das?")
name = input("Wie heißen Sie?")

print(gruss(hallo, ort, meinung, gestern, meinung_gestern, name))
