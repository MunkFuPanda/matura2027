"""
author: Markus Spitzer
file: 5.py
"""

brutto = int(input("Jahresbruttogehalt eingeben:"))

if brutto < 3000:
    print("Nettogehalt:", brutto*0.9)
elif brutto < 80000:
    print("Nettogehalt:", brutto*0.78)
elif brutto < 100000:
    print("Nettogehalt:", brutto*0.68)
elif brutto < 150000:
    print("Nettogehalt:", brutto*0.58)
elif brutto >= 150000:
    print("Nettogehalt:", brutto*0.5)
