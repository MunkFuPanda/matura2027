# POS2 - Programming & Software Engineering

Dieses Repository enthält die Lösungen zu den C++-Übungen im Unterrichtsfach **POS2** (Programming & Software Engineering). Hier findest du eine Anleitung, wie du das Repository klonst und die Übungen auf deinem Computer ausführst.

## Voraussetzungen

Um die Lösungen lokal auf deinem Rechner auszuführen, stelle sicher, dass folgende Voraussetzungen erfüllt sind:

1. **WSL (Windows Subsystem for Linux)** ist installiert und konfiguriert.
2. **Build-Essentials** (inkl. GCC und Make) sind installiert.

### Installation von WSL

Falls du noch kein WSL installiert hast, führe folgende Schritte aus:

1. Öffne PowerShell als Administrator und gib den folgenden Befehl ein:
    ```bash
   wsl --install
    ```
   Dadurch wird WSL zusammen mit Ubuntu installiert.

2. Starte dein System neu, falls erforderlich.

3. Öffne nach dem Neustart die WSL-Umgebung (Ubuntu) und richte dein Benutzerkonto ein.

### Installation von Build-Essentials

Nachdem WSL eingerichtet ist, öffne die WSL-Umgebung und installiere die benötigten Build-Tools:

1. Update der Paketliste:
    ```bash
   sudo apt update
    ```

2. Installation von Build-Essentials:
    ```bash
   sudo apt install build-essential
    ```

Das installiert den GNU Compiler (G++) und Make, um C++-Programme zu kompilieren.

## Repository klonen

Sobald WSL und die benötigten Tools installiert sind, kannst du das Repository klonen:

1. Navigiere in das Verzeichnis, in dem du das Repository speichern möchtest:
    ```bash
   cd /mnt/c/Users/DeinBenutzername/DeinPfadZumProjekt
    ```

2. Klone das Repository mit Git:
    ```bash
   git clone https://github.com/dein-repository-url.git
    ```

3. Wechsle in das Verzeichnis des geklonten Projekts:
    ```bash
   cd dein-repository-name
    ```

## Ausführen der C++-Übungen

1. Kompiliere den C++-Code:
    ```bash
   g++ -o programmname programmname.cpp
    ```

2. Führe das Programm aus:
    ```bash
   ./programmname
    ```

## Probleme und Fehlersuche

Falls Probleme bei der Installation oder Ausführung auftreten:

- Stelle sicher, dass WSL und Build-Essentials korrekt installiert sind.
- Überprüfe, ob du dich im richtigen Verzeichnis befindest und die Dateien korrekt benannt sind.
- Überprüfe Fehlermeldungen beim Kompilieren und führe gegebenenfalls Korrekturen durch.

Bei weiteren Fragen oder Problemen melde dich gerne!

---

Viel Erfolg bei den Übungen!
