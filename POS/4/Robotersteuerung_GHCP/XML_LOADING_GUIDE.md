# XML-Laden Troubleshooting

## Problem: "Error 2:2" beim XML-Laden

### Mögliche Ursachen:

1. **Falscher Pfad**
   - Datei existiert nicht
   - Relativer Pfad wird nicht aufgelöst

2. **XML-Format-Fehler**
   - Zeichen-Länge stimmt nicht mit `width` überein
   - Ungültige Zeichen in XML

3. **Encoding-Fehler**
   - BOM (Byte Order Mark) Problem
   - Falsche Zeichenkodierung

## Lösung: Richtige Pfade eingeben

### Option 1: Relative Pfade (empfohlen)
```
Feld-Pfad: Examples/field1.xml
Programm-Pfad: Examples/program1.txt
```

Das Programm konvertiert diese automatisch zu absoluten Pfaden basierend auf dem Startverzeichnis.

### Option 2: Absolute Pfade
```
Feld-Pfad: C:\Users\Markus\matura2027\POS\4\Robotersteuerung\Examples\field1.xml
```

### Option 3: %AppDirectory% Pfade
```
Das Programm verwendet AppDomain.CurrentDomain.BaseDirectory
= Bin-Verzeichnis der ausführbaren Datei
```

## Validierung der XML-Dateien

### field1.xml - Struktur:
```xml
<field width="9" height="9" startX="0" startY="0">
  <row>R        </row>  <!-- Genau 9 Zeichen! -->
  <row>         </row>  <!-- Genau 9 Zeichen! -->
  <!-- ... 9 Reihen total ... -->
</field>
```

✅ **Wichtig**: 
- Jede `<row>` MUSS genau `width` Zeichen lang sein
- Verwende Leerzeichen zum Auffüllen
- Reihen-Anzahl MUSS `height` entsprechen

### Validierungstool:
```powershell
# In Visual Studio Code oder Editor:
1. Datei öffnen
2. Am Ende der jeder Zeile prüfen
3. Spaces zählen (sollte width Zeichen sein)
```

## Debug: Pfad-Ausgabe

Das Programm zeigt jetzt:
- Den eingegebenen Pfad
- Den aufgelösten absoluten Pfad
- Ob die Datei existiert

Falls noch Fehler:
→ Error-Message wird mit vollem Pfad angezeigt

## Beispiel-Verwendung:

```
1. Programm starten
2. Im Feld "XML-Spielfeld" eingeben: Examples/field1.xml
3. Button "Laden" klicken
4. Das Feld wird geladen und im Custom Control angezeigt

Falls Fehler:
→ Fehlermeldung mit Pfad anzeigen
→ Pfad überprüfen
→ Datei manuell öffnen um Inhalt zu validieren
```

## Häufige Fehler:

| Fehler | Ursache | Lösung |
|--------|--------|--------|
| "Datei nicht gefunden" | Pfad falsch | Relativen oder absoluten Pfad überprüfen |
| "Error 2:2" | XML ungültig | Alle Reihen müssen genau `width` Zeichen lang sein |
| "Ungültige Zeichen" | Nicht-ASCII Zeichen | Nur ASCII verwenden (R, A-Z, #, Leerzeichen) |

## XML-Format überprüfen:

```powershell
# PowerShell:
[xml]$xml = Get-Content "Examples/field1.xml"
$xml.field.row | ForEach-Object { Write-Host $_.Length }
```

Alle Längen sollten gleich `width` sein.
