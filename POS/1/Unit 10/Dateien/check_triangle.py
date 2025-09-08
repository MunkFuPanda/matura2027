"""
author: Markus Spitzer
file: check_triangle.py
"""

a = int(input("Geben Sie die Seite a des Dreiecks an: "))
b = int(input("Geben Sie die Seite b des Dreiecks an: "))
c = int(input("Geben Sie die Seite c des Dreiecks an: "))

if a + b >= c:
    print("gültiges Dreieck")
elif a + c >= b:
    print("gültiges Dreieck")
elif b + c >= a:
    print("gültiges Dreieck")
else:
    print("ungültiges Dreieck")

