# Custom-Control-DLL einbinden (falls Vorgabe eine .dll mitliefert)

Roboter (`AbcRobotCore.dll` / `RobotLibrary.dll`) und Painter (`Painter.dll`)
liefern ein fertiges Custom Control. Dann zeichnest du **nicht** selbst Zellen,
sondern rufst die Methoden des Controls auf. Der ganze Sprach-Kram (Lexer,
Parser, Interpreter) bleibt unveraendert - du tauschst nur die "Zeichnen"-Schicht.

## 1. DLL referenzieren
- Visual Studio: Rechtsklick aufs Projekt -> **Add -> Project Reference ->
  Browse** -> die `.dll` auswaehlen.
- Pruefen, ob die DLL im Vorgabe-Projekt schon referenziert ist (meist ja).

## 2. Control im XAML einbinden
```xml
<Window ...
        xmlns:ctrl="clr-namespace:NAMESPACE_DER_DLL;assembly=DLL_DATEINAME">
    <ctrl:RobotControl x:Name="Robot"/>
</Window>
```
- `NAMESPACE_DER_DLL` und der Klassenname stehen in der Doku/Angabe oder im
  Object Browser (DLL im VS oeffnen). Bei Roboter/Painter nennt die Angabe die
  relevanten Funktionen direkt.

## 3. Bekannte Methoden (aus den Angaben)
**Roboter** (`AbcRobotCore` / `RobotLibrary`):
```csharp
Robot.LoadField(pfadOderDaten);   // Feld aus XML laden
Robot.Move(direction);            // einen Schritt bewegen
Robot.Collect();                  // Buchstaben einsammeln
bool b1 = Robot.IsLetter(dir, 'A'); // ist dort ein Buchstabe?
bool b2 = Robot.IsObstacle(dir);    // ist dort ein Hindernis?
```
**Painter** (`Painter.dll`): Methoden zum Drehen/Zeichnen/Farbe (genaue Namen aus
der Painter-Doku ablesen - typisch `Turn`, `Draw`, `SetColor`).

## 4. Wo das deinen Code beruehrt
NUR im `Context` / in der Visualisierung. Statt
```csharp
_cells[r, c].Background = NameToBrush(color);   // eigenes Feld
```
rufst du die Control-Methode:
```csharp
Robot.Move("DOWN");   // Control aktualisiert sich selbst
```
Die `Interpret(...)`-Methoden, der Parser und der Lexer bleiben **identisch**.
Du leitest den Aufruf nur an das Control weiter, statt ein eigenes Gitter zu malen.

## 5. Getaktet abspielen
Genauso wie ohne DLL: zwischen den Control-Aufrufen `await Task.Delay(1000)` oder
`DispatcherTimer` (siehe `05_WPF_Cheatsheet.md`). Die 1-Sekunde-Pause verlangt die
Roboter-Angabe explizit.

## Stolperfallen
- **Plattform/Bitness:** Wenn die DLL fuer x86 gebaut ist, muss dein Projekt auch
  auf x86 laufen, sonst `BadImageFormatException`. Im Zweifel Build-Plattform
  anpassen.
- **Falsches `OpenFileDialog`:** weiterhin `Microsoft.Win32`, nicht WinForms.
- **DLL nicht gefunden zur Laufzeit:** sie muss im Ausgabeordner (`bin/...`)
  landen -> "Copy Local" / im SDK-Projekt klappt das ueber die Reference meist
  automatisch.
