# AbcRobotCore Custom Control Integration

## Übersicht

Das Projekt integrtiert das WPF Custom Control `AbcRobotCore.RobotField` für die visuelle Darstellung des Spielfelds und den Roboter.

## DLL-Referenz

Die `AbcRobotCore.dll` wird in folgende Struktur eingebunden:

```
Robotersteuerung/
├── Lib/
│   └── AbcRobotCore.dll          ← Custom Control DLL
├── Robotersteuerung.csproj       ← Projekt-Referenz konfiguriert
└── ...
```

### Projekt-Konfiguration

Die `.csproj` Datei enthält die Referenz:

```xml
<ItemGroup>
  <Reference Include="AbcRobotCore">
    <HintPath>Lib\AbcRobotCore.dll</HintPath>
  </Reference>
</ItemGroup>
```

## XAML-Integration

Das Custom Control wird in `MainWindow.xaml` verwendet:

```xaml
xmlns:local="clr-namespace:AbcRobotCore;assembly=AbcRobotCore"

<!-- Im Grid: -->
<local:RobotField x:Name="RobotFieldControl" Height="600" Width="600"/>
```

## Code-Behind Integration

In der `MainWindow.xaml.cs`:

```csharp
using AbcRobotCore;

// Laden des Feldes
RobotFieldControl.LoadField(xmlFilePath);

// Wrapper erstellen
_fieldWrapper = new RobotFieldWrapper(RobotFieldControl);

// Im Interpreter verwenden
_interpreter = new RobotInterpreter(_fieldWrapper);
```

## RobotFieldWrapper

Die Wrapper-Klasse vereinfacht die Verwendung des Custom Controls:

```csharp
public class RobotFieldWrapper
{
    public void LoadField(string xmlPath)
    public void Move(AbcRobotCore.RobotField.Direction direction)
    public void Collect()
    public bool IsLetter(string letter, AbcRobotCore.RobotField.Direction direction)
    public bool IsObstacle(AbcRobotCore.RobotField.Direction direction)
}
```

## Direction-Konvertierung

Das Projekt nutzt interne `Direction` enums, die zu AbcRobotCore konvertiert werden:

```csharp
public static class DirectionConverter
{
    public static AbcRobotCore.RobotField.Direction ToAbcDirection(Direction dir)
    {
        return dir switch
        {
            Direction.UP => AbcRobotCore.RobotField.Direction.Up,
            Direction.DOWN => AbcRobotCore.RobotField.Direction.Down,
            Direction.LEFT => AbcRobotCore.RobotField.Direction.Left,
            Direction.RIGHT => AbcRobotCore.RobotField.Direction.Right,
            _ => AbcRobotCore.RobotField.Direction.Up
        };
    }
}
```

## Custom Control API

### Methoden

| Methode | Parameter | Rückgabe | Beschreibung |
|---------|-----------|----------|-------------|
| LoadField | string xmlFilePath | void | Lädt das Spielfeld aus XML |
| Move | Direction direction | bool | Bewegt den Roboter (Rückgabe: Erfolg) |
| Collect | - | string | Sammelt Buchstabe ein, gibt ihn zurück |
| IsLetter | string letter, Direction direction | bool | Prüft ob Buchstabe in Richtung liegt |
| IsObstacle | Direction direction | bool | Prüft ob Hindernis/Grenze in Richtung |

### Direction Enum

```csharp
public enum Direction
{
    Up,
    Down,
    Left,
    Right
    // Potentiell: DiagonaleRichtungen in Erweiterung
}
```

## XML-Format Support

Das Custom Control unterstützt folgendes XML-Format:

```xml
<?xml version="1.0" encoding="utf-8"?>
<field width="9" height="9" startX="0" startY="0">
  <row>R       </row>
  <row>        </row>
  <row>    A   </row>
  <row>        </row>
  <!-- weitere Reihen -->
</field>
```

Symbole:
- `R` - Roboter-Startposition
- `#` - Hindernis
- ` ` - Leeres Feld
- `A-Z` - Sammelbare Buchstaben

## Ausführungsablauf

1. **Spielfeld laden**
   ```csharp
   RobotFieldControl.LoadField(xmlPath)
   ```
   - Custom Control liest XML und initialisiert Feld
   - Roboter wird auf Startposition positioniert
   - Control wird gerendert

2. **Programm ausführen**
   ```csharp
   _interpreter.Execute(program)
   ```
   - Für jeden Command:
     - Falls MOVE: `_fieldWrapper.Move(direction)`
     - Falls COLLECT: `_fieldWrapper.Collect()`
     - Falls IF/UNTIL: Bedingung mit `IsLetter`/`IsObstacle` prüfen
   - Custom Control aktualisiert sich visuell nach jedem Schritt
   - 1-Sekunden-Pause vor nächstem Schritt

3. **Gesammelte Buchstaben anzeigen**
   ```csharp
   CollectedLetters.Text = _interpreter.GetCollectedLetters()
   ```

## Erweiterungsmöglichkeiten

Die `AbcRobotCore.dll` Datei wurde bereits für diagonale Bewegungen erweitert. Zukünftige Erweiterungen:

1. **Diagonale Bewegungen**
   ```csharp
   Direction.UP_LEFT, Direction.UP_RIGHT, 
   Direction.DOWN_LEFT, Direction.DOWN_RIGHT
   ```

2. **Additional Commands**
   - Custom Control API erweitern
   - Parser/Interpreter anpassen

3. **Feldmanipulation**
   - Gegenstände hinzufügen
   - Dynamische Feldbeschaffenheit

## Fehlerbehandlung

Fehler bei der Custom Control Verwendung:

```csharp
try
{
    RobotFieldControl.LoadField(path);
    _fieldWrapper = new RobotFieldWrapper(RobotFieldControl);
}
catch (Exception ex)
{
    ErrorDisplay.Text = $"Fehler: {ex.Message}";
}
```

Häufige Fehler:
- XML-Datei nicht gefunden
- Ungültiges XML-Format
- Ungültige Richtung
- Roboter außerhalb des Feldes

## Performance

- Custom Control rendert bei jedem Move neu
- 1-Sekunden-Pausen ermöglichen visuelles Tracking
- Für große Felder (>100x100) kann Rendering verlangsamt sein

## Debugging

Im Visual Studio:
1. Breakpoints im Interpreter setzen
2. Custom Control-Zustand in Live Visual Tree inspizieren
3. Execution History in TextBox verfolgbar

## Kompatibilität

- .NET 10
- WPF
- Windows 7+ (für WPF)

## Version History

- v1.0: Initial Integration
- v1.1: Direction Konvertierung hinzugefügt
- v1.2: RobotFieldWrapper erstellt
