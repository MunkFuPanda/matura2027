"""
author: Markus Spitzer
file: div_numbers.py
"""

def div_numbers(numbers, i1, i2):
    if i1 < 0 or i1 > len(numbers) -1:
        return None
    if i2 < 0 or i2 > len(numbers) -1:
        return None

    dividend = numbers[i1]
    divisor = numbers[i2]

    if divisor == 0:
        return float("nan")
    else:
        ergebnis = dividend / divisor
        return ergebnis

a = int(input("input 1:"))
b = int(input("input 2:"))
tupel = (2, 5, 0, 3, 6, "3", 1, 9, 8, 0, 2, 8)

print(div_numbers(tupel, a, b))
