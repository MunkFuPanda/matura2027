"""
author: Markus Spitzer
file: sets2.py
"""


def cross_product(a, b):
    return {(x, y) for x in a for y in b}


def in_set(e, a):
    for x in a:
        if x == e:
            return True


def output_mul_table(n):
    text = ""

    for i in range(n + 1):
        text += "\n"
        for j in range(n + 1):
            i2 = i or 1
            j2 = j or 1
            
            text += " {:>2}".format(str("x" if i + j == 0 else j2 * i2))

    return text
    
    
def reverse_list(lst):
    out = ""

    for x in lst:
        out = str(x) + "," + out

    return out[:-1]


def to_str(lst):
    out = ""

    for x in lst:
        out += str(x) + ","

    return out[:-1]


def reverse_number(n):
    return int(str(n)[::-1])


def is_palindrome(n):
    return reverse_number(n) == n


def add(lst):
    res = 0

    for x in lst:
        res += sum(x)

    return res


def minmax(lst):
    mx = None
    mn = None

    for x in lst:
        a, b = min(x), max(x)
        if mn is None or a < mn:
            mn = a
        if mx is None or b > mx:
            mx = b

    return (mn, mx)


def change_case(s):
    ##return s.swapcase() gefällt pilhar ned :3
    for i in s:
        if i.lower() == i:
            s2 += i.upper()
        else:
            s2 += i.lower()

    return s2


