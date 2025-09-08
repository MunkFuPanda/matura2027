"""
author: Markus Spitzer
file: caesar_chiffre2.py
"""

from getpass import getpass


def encrypt(msg, k):
    new_msg = ""

    for x in msg:
        new_msg += chr(ord(x) + k)
            
    return new_msg


def decrypt(msg, k):
    return encrypt(msg, -k)


def translate_msg(mode, msg, k):
    return encrypt(msg, k) if mode == 1 else decrypt(msg, k)


def brute_force(msg):
    for x in range(26):
        print(f'Key: {x} Result: {decrypt(msg, x)}')


def input_encrypt():
    msg = None
    while not msg:
        msg = getpass("Nachricht eingeben:")
        
        for x in msg:
            if not (x.isalpha() or x.isspace()):
                msg = None
    
    k = None
    while True:
        k = int(input("Schlüssel (int) eingeben:"))
        
        if k <= 26 and k > 1:
            break

    return translate_msg(1, msg, k)


def input_decrypt():
    msg = None
    while not msg:
        msg = getpass("Nachricht eingeben:")
        
        for x in msg:
            if not (x.isalpha() or x.isspace()):
                msg = None
    
    k = None
    while True:
        k = int(input("Schlüssel (int) eingeben:"))
        
        if k <= 26 and k > 1:
            break

    return translate_msg(0, msg, k)


def input_brute_force():
    msg = None
    while not msg:
        msg = getpass("Nachricht eingeben:")
        
        for x in msg:
            if not (x.isalpha() or x.isspace()):
                msg = None

    return brute_force(msg)


def menu(title, actions):
    while True:
        sel = input(title)
        if len(sel) > 0:
            sel = sel[0]
            if sel in actions:
                res = actions[sel]()
                if res:
                    print(res)
            elif sel == "q":
                exit()


menu("""Verschlüsseln einer Nachricht [e/encrypt]
Entschlüsseln einer Nachricht [d/decrypt]
Bruteforce [b/brute]
Beenden [q/quit]
Bitte wählen Sie! """, {"e": input_encrypt,
"encrypt": input_encrypt,
"d": input_decrypt,
"decrypt": input_decrypt,
"b": input_brute_force,
"brute": input_brute_force})
