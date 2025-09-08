"""
author: Markus Spitzer
file: 8.py
"""

categories = [
{
    "weight": 3500,
    "color": 'grey',
    "origin": 'Africa',
},
{
    "weight": 3500,
    "color": 'brown',
    "origin": 'Africa',
},
{
    "weight": 3500,
    "color": 'white',
    "origin": 'Africa',
},
{
    "weight": 4000,
    "color": 'grey',
    "origin": 'Africa',
},
{
    "weight": 4000,
    "color": 'brown',
    "origin": 'Africa',
},
{
    "weight": 4000,
    "color": 'brown',
    "origin": 'India',
},
{
    "weight": 4000,
    "color": 'white',
    "origin": 'India',
},
{
    "weight": 4500,
    "color": 'grey',
    "origin": 'India',
},
{
    "weight": 4500,
    "color": 'brown',
    "origin": 'India',
},
{
    "weight": 4500,
    "color": 'white',
    "origin": 'India',
},
]


def elephants1(weight, color, origin):
    for i, v in enumerate(categories):
        i += 1
        if v["weight"] == weight and v["color"] == color and v["origin"] == origin:
            print(i, v)
            return i
