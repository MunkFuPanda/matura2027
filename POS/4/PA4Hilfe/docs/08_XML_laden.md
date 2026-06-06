# XML-Datendateien laden (wie Roboter-Feld aus XML)

Die Roboter-Angabe laedt das Spielfeld aus einer XML-Datei. Falls die PA
Datendateien mitliefert, musst du sie einlesen statt eine feste Feldgroesse zu
nehmen. Zwei Wege - LINQ-to-XML ist meist schneller getippt.

## Weg A: LINQ to XML (XDocument) - empfohlen
```csharp
using System.Xml.Linq;

XDocument doc = XDocument.Load(pfad);

// Attribute des Wurzel-Elements lesen
int width  = (int)doc.Root.Attribute("width");
int height = (int)doc.Root.Attribute("height");

// Kind-Elemente durchgehen
foreach (XElement cell in doc.Root.Elements("Cell"))
{
    int x = (int)cell.Attribute("x");
    int y = (int)cell.Attribute("y");
    string content = (string)cell.Attribute("content");  // z.B. "A" oder "OBSTACLE"
    // ... ins Feld eintragen
}
```
Beispiel-XML, das dazu passt:
```xml
<Field width="6" height="6">
  <Cell x="2" y="0" content="A"/>
  <Cell x="5" y="3" content="OBSTACLE"/>
</Field>
```

## Weg B: XmlSerializer (wenn du Klassen aufs XML mappst)
```csharp
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Field")]
public class FieldData
{
    [XmlAttribute("width")]  public int Width  { get; set; }
    [XmlAttribute("height")] public int Height { get; set; }
    [XmlElement("Cell")]     public List<CellData> Cells { get; set; }
}
public class CellData
{
    [XmlAttribute("x")] public int X { get; set; }
    [XmlAttribute("y")] public int Y { get; set; }
    [XmlAttribute("content")] public string Content { get; set; }
}

// Laden:
var ser = new XmlSerializer(typeof(FieldData));
using (var fs = File.OpenRead(pfad))
    FieldData field = (FieldData)ser.Deserialize(fs);
```

## Welchen Weg?
- **XDocument**: weniger Code, gut wenn das XML klein/simpel ist. Schnell in der PA.
- **XmlSerializer**: sauberer, wenn die Struktur groesser ist und du sowieso
  Klassen hast. Mehr Tipparbeit (Attribute annotieren).

## Stolperfallen
- `(int)attribute` wirft, wenn das Attribut fehlt -> bei optionalen Werten
  `(int?)` nehmen und auf null pruefen.
- Gross-/Kleinschreibung der Tag-/Attributnamen muss exakt zur Datei passen.
- Pfad relativ zum Arbeitsverzeichnis -> im Zweifel vollen Pfad aus
  `OpenFileDialog` nehmen.
