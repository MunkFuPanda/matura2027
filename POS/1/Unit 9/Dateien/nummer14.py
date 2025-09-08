"""
author: Markus Spitzer
file: nummer14.py
"""

a = {1, 2, 3, 4}
b = {3, 4, 5, 6}
c = {3, 4}

#Durchschnitt
durchschnitt = a.intersection(b)
durchschnitt = a & b

#Vereinigung
vereinigung = a.union(b, c)
vereinigung = a | b | c

#Differenz
differenz = a.difference(c)
differenz = a - c

#Symetrische Differenz
symdiff = a.symmetric_difference(b)
symdiff = a ^ b

# ist ein Wert in Menge enthalten
if 3 in a:
    print("enthalten")

# ist a eine Teilmenge von c
if a.issubset(c):
    print("a ist eine Teilmenge von c")
    
# ist a eine Teilmenge von c (alternativ)
if a <= c:
    print("a ist eine Teilmenge von c")

#ist eine echte Teilmenge von c
#(wenn A eine Teilmenge von B ist und die beiden Mengen
#ungleich sind, d.H. die Menge B muss auf jeden Fall
#zusätzliche Elemente enthalten, die nicht in A sind).
    
if a < c:
    print("a ist eine echte Teilmenge von c")
