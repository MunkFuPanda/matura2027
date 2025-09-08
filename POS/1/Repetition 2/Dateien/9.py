"""
author: Markus Spitzer
file: 9.py
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

def elephants2(weight, color, origin):
    cat = None
    
    for i, v in enumerate(categories):
        if v["color"] == color and v["origin"] == origin and weight <= v["weight"]:
            if cat == None:
                cat = i
            elif categories[cat]["weight"] > v["weight"]:
                cat = i
                

    return categories[cat]
                
