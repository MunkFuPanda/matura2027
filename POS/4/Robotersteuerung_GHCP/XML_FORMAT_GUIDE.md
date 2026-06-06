# XML-Format für Spielfelder

## Neues Format (Custom Control Compatible)

Das Programm unterstützt jetzt das offizielle Custom Control XML-Format:

### Struktur

```xml
<?xml version="1.0" encoding="utf-8"?>
<XML_Field>
  <Width>10</Width>
  <Height>10</Height>
  <Fields>
    <XML_Cell>
      <X>0</X>
      <Y>0</Y>
      <Type>robot</Type>
    </XML_Cell>
    <XML_Cell>
      <X>1</X>
      <Y>3</Y>
      <Type>A</Type>
    </XML_Cell>
    <XML_Cell>
      <X>3</X>
      <Y>2</Y>
      <Type>stone</Type>
    </XML_Cell>
  </Fields>
</XML_Field>
```

### Elemente

**XML_Field** - Root Element
- `Width` - Feldbreite (Integer)
- `Height` - Feldhöhe (Integer)
- `Fields` - Container für alle Zellen

**XML_Cell** - Einzelne Zelle
- `X` - X-Koordinate (0-basiert)
- `Y` - Y-Koordinate (0-basiert)
- `Type` - Zellentyp

### Zellentypen

| Type | Symbol | Beschreibung |
|------|--------|-------------|
| `robot` | R | Roboter-Startposition |
| `A-Z` | A-Z | Sammelbare Buchstaben |
| `stone` | # | Hindernis (nicht passierbar) |
| (leer) | ` ` | Leeres Feld (Leerzeichen/nicht definiert) |

### Beispiel: Einfaches Feld

```xml
<?xml version="1.0" encoding="utf-8"?>
<XML_Field>
  <Width>5</Width>
  <Height>5</Height>
  <Fields>
    <XML_Cell>
      <X>0</X>
      <Y>0</Y>
      <Type>robot</Type>
    </XML_Cell>
    <XML_Cell>
      <X>4</X>
      <Y>4</Y>
      <Type>A</Type>
    </XML_Cell>
  </Fields>
</XML_Field>
```

Darstellung:
```
R    



    A
```

### Beispiel: Komplexes Feld mit Hindernissen

```xml
<?xml version="1.0" encoding="utf-8"?>
<XML_Field>
  <Width>10</Width>
  <Height>10</Height>
  <Fields>
    <XML_Cell>
      <X>0</X>
      <Y>0</Y>
      <Type>robot</Type>
    </XML_Cell>
    <XML_Cell>
      <X>1</X>
      <Y>3</Y>
      <Type>A</Type>
    </XML_Cell>
    <XML_Cell>
      <X>9</X>
      <Y>0</Y>
      <Type>B</Type>
    </XML_Cell>
    <XML_Cell>
      <X>3</X>
      <Y>2</Y>
      <Type>stone</Type>
    </XML_Cell>
    <XML_Cell>
      <X>3</X>
      <Y>3</Y>
      <Type>stone</Type>
    </XML_Cell>
    <XML_Cell>
      <X>3</X>
      <Y>4</Y>
      <Type>stone</Type>
    </XML_Cell>
  </Fields>
</XML_Field>
```

Darstellung:
```
R       B
   #    
   #    
A #    
   #    





```

## Alte Formate (Noch unterstützt)

Das Programm unterstützt auch noch das alte Reihen-basierte Format für Kompatibilität:

```xml
<?xml version="1.0" encoding="utf-8"?>
<field width="9" height="9" startX="0" startY="0">
  <row>R        </row>
  <row>         </row>
  <row>         </row>
  <row>         </row>
  <row>         </row>
  <row>         </row>
  <row>         </row>
  <row>    A    </row>
  <row>         </row>
</field>
```

## Hinweise

1. **Koordinaten**: X und Y sind 0-basiert (oben-links = 0,0)
2. **Breite/Höhe**: Müssen positiv sein
3. **Duplikate**: Letzte Definition gewinnt wenn doppelte Koordinaten
4. **Grenzen**: Zellen außerhalb Width/Height werden ignoriert
5. **Leerzeichen**: Nicht definierte Zellen sind automatisch leer

## Verwendung im Programm

```
1. Pfad eingeben: C:\pfad\zur\field.xml
2. Button "Laden" klicken
3. Custom Control zeigt Feld
4. Programm ausführen
```

## Konvertierung Alt → Neu

Um alte row-basierte Dateien zu konvertieren:

**Alt:**
```xml
<field width="3" height="3" startX="0" startY="0">
  <row>R#A</row>
  <row>   </row>
  <row>B  </row>
</field>
```

**Neu:**
```xml
<XML_Field>
  <Width>3</Width>
  <Height>3</Height>
  <Fields>
    <XML_Cell><X>0</X><Y>0</Y><Type>robot</Type></XML_Cell>
    <XML_Cell><X>1</X><Y>0</Y><Type>stone</Type></XML_Cell>
    <XML_Cell><X>2</X><Y>0</Y><Type>A</Type></XML_Cell>
    <XML_Cell><X>0</X><Y>2</Y><Type>B</Type></XML_Cell>
  </Fields>
</XML_Field>
```
