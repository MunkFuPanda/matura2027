"""
author: Markus Spitzer
file: leap_year.py
"""


def check_leap_year(year):
    if year <= 1582:
        return False

    if year % 100 == 0:
        return False

    if year % 4 == 0:
        return True
    return False


while True:
    year = int(input("Geben Sie eine Jahreszahl an: "))
    print(check_leap_year(year))
    
