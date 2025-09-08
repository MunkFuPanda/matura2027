"""
author: Markus Spitzer
file: py.py
"""


def input_sentences():
    print("Bitte geben Sie die zu analysierenden Sätze ein (CTRL-D bricht ab):\n")
    sentences = []

    try:
        while True:
            sentences.append(input("Satz: "))
    except EOFError:
        return sentences


def split_sentences(lst):
    for i, x in enumerate(lst):
        for z in "\"',;.:-":
            x = x.replace(z, " ")
        x = x.split(" ")
        lst[i] = x
    
    return lst

def purge_bad_words(lst):
    for s_num in range(len(lst) -1, -1, -1):
        for w_num in range(len(lst[s_num]) -1, -1, -1):
            if not lst[s_num][w_num].isalpha() or len(lst[s_num][w_num]) < 2:
                del lst[s_num][w_num]
        
        if not len(lst[s_num]):
            del lst[s_num]

    return lst


def analyze_words(lst):
    counts = {}

    for x in lst:
        for y in x:
            y = y.lower()
            if y in counts:
                counts[y] += 1
            else:
                counts[y] = 1

    return counts


def analyze_letters(lst):
    counts = {}

    for x in lst:
        for y in x:
            for z in y:
                z = z.lower()
                if z in counts:
                    counts[z] += 1
                else:
                    counts[z] = 1

    return counts


def purge_analyzed_letters(dct):
    for x in "äöü":
        if x in dct:
            del dct[x]

    return dct


def sort_letters(dic):
    return sorted(dic.items(), key=lambda x: x[1], reverse=True)
