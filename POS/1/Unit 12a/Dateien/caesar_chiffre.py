"""
author: Markus Spitzer
file: caesar_chiffre.py
"""

alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
encryption = "DEFGHIJKLMNOPQRSTUVWXYZABC"

encrypter = {}

for i, v in enumerate(alphabet):
    encrypter[v] = encryption[i]


def encrypt(msg, encrypter):
    new_msg = ""

    for x in msg:
        if x.upper() in encrypter:
            if x.upper() == x:
                new_msg += encrypter[x]
            elif x.lower() == x:
                new_msg += encrypter[x.upper()].lower()
        else:
            new_msg += x

    return new_msg
        

def invert_dict(d):
    n = {}

    for x, y in d.items():
        n[y] = x

    return n


def decrypt(msg, encrypter):
    return encrypt(msg, invert_dict(encrypter))
