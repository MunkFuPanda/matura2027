"""
author: Markus Spitzer
file: name_spaces2.py
"""

name = input("Geben Sie einen Namen an:")


def name_spaces(string):
    summe = ""
    for character in string:
        summe += character + " "
    return summe


print(name_spaces(name))

