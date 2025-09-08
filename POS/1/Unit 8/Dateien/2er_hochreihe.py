"""
author: Markus Spitzer
file: tst.py
"""

for p in range(17):
    dez = 2 ** p
    binaer = bin(dez)
    hexadez = hex(dez)
    print(f'2^{p:<2} = {dez:<5} binär: {binaer:<19} hex: {hexadez}')
