"""
author: Markus Spitzer
file: calculate2.py
"""

import mymath

prompt_type = input("1 = power\n2 = sqrt\n3 = minimum\nselect your method:")

if prompt_type == "1":
    x = int(input("Input für basis:"))
    y = int(input("Input für exponent:"))
    
    print(x, "^", y, "=", mymath.power(y, x))
elif prompt_type == "2":
    print(mymath.sqrt(float(input("Input für sqrt:"))))
elif prompt_type == "3":
    x0 = float(input("Erste Zahl:"))
    x1 = float(input("Zweite Zahl:"))
    x2 = float(input("Dritte Zahl:"))
    
    print(mymath.minimum(x0, x1, x2))
