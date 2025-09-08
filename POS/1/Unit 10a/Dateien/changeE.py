"""
author: Markus Spitzer
file: changeE.py
"""

def changeE(string):
    new_string = ""

    for char in string:
        if char == "e":
            char = "E"
        new_string += char
    return new_string