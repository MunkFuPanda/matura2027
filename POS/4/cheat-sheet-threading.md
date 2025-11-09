Gerne erstelle ich eine noch prägnantere, praxislastige Zusammenfassung in Abschnitten mit Code-Snippets, die direkt auf die C#-ImplementGerne erstelle ich eine noch prägnantere, praxislastige Zusammenfassung in Abschnitten mit Code-Snippets, die direkt auf die C#-Implementierungen und Synchronisationsmechanismen fokussiert ist.

---

# C# Multithreading & Synchronisation – Praxis-Snippets

## I. Basis Thread-Erstellung und -Kontrolle

Um einen neuen Thread zu starten, muss der `System.Threading`-Namespace verwendet werden.

### 1. Thread-Start und asynchrone Ausführung

Ein `ThreadStart` Delegate wird genutzt, um die Methode zu referenzieren, die asynchron laufen soll.

**Snippet:**

```csharp
using System.Threading;
// ...
public static void WorkerThreadMethod() { Console.WriteLine("Worker thread started"); }

public static void Main()
{
    // 1. Delegate erstellen
    ThreadStart worker = new ThreadStart(WorkerThreadMethod); 
    
    // 2. Thread instanziieren und starten
    Thread t = new Thread(worker);
    t.Start(); // Startet die WorkerThreadMethod asynchron
}
```

### 2. Blockierende Methoden

Diese Funktionen setzen den Thread in den Zustand `blocked` und geben die CPU-Zeitscheibe sofort frei, bis die Bedingung erfüllt ist.

**Snippet: Warten auf Thread-Ende (`Join`)**

```csharp
Thread t = new Thread (Go);
t.Start();
t.Join(); // Wartet, bis t beendet ist
Console.WriteLine ("Thread t has ended!"); 

static void Go() { /* Längere Aufgabe */ }
```

**Snippet: Pausieren (`Sleep`)**

```csharp
Thread.Sleep (500); // Pausiert für 500 Millisekunden

// Beendet nur das aktuelle Timeslice
Thread.Sleep(0); 
```

### 3. Thread-Zustände und Priorität

Threads können in den Zuständen `wartend`, `bereit` oder `laufend` sein.

**Snippet: Priorität setzen**

Die Standardpriorität ist `Normal`. Threads mit höherer Priorität haben Vorrang.

```csharp
thread2.Priority = ThreadPriority.AboveNormal; 
```

**Snippet: Auf Blockierung prüfen (Fehlerdiagnose)**

```csharp
// Prüft auf Wait, Sleep oder Join
bool blocked = (someThread.ThreadState & ThreadState.WaitSleepJoin) != 0; 
```

## II. Thread-Pool (Ressourcenschonende Ausführung)

Der Thread-Pool (`ThreadPool`) reduziert den Overhead durch Wiederverwendung von Threads und ist ideal für Multicore-Prozessoren.

### 1. Arbeit an den Pool übergeben

Die Arbeit wird als `WaitCallback` Delegate an den Pool in die Warteschlange gestellt.

**Snippet: Aufgabe zuweisen und auf Fertigstellung warten**

```csharp
// ready dient zur Signalisierung der Fertigstellung durch den Worker
AutoResetEvent ready = new AutoResetEvent(false); 

// Task an ThreadPool übergeben
ThreadPool.QueueUserWorkItem(new WaitCallback(Calculate), ready);

Console.WriteLine("Der Hauptthread wartet ...");
ready.WaitOne(); // Hauptthread blockiert
Console.WriteLine("Sekundärthread ist fertig.");

// Die Worker-Methode muss object akzeptieren
public static void Calculate(object obj) 
{
    Console.WriteLine("Im Sekundärthread");
    Thread.Sleep(5000); 
    // Signalisiert Fertigstellung
    ((AutoResetEvent)obj).Set();
}
```

### 2. Effizientes, nicht-blockierendes Warten

Wenn auf ein `WaitHandle` gewartet wird, ohne einen Thread-Pool-Thread zu blockieren.

**Snippet: Registrierung für Wartevorgang**

```csharp
static ManualResetEvent _starter = new ManualResetEvent (false);

// Registriert eine Methode, die auf einem Pool-Thread ausgeführt wird,
// sobald _starter signalisiert wird (-1 = kein Timeout)
RegisteredWaitHandle reg = ThreadPool.RegisterWaitForSingleObject(
    _starter, 
    Go, 
    "Some Data", 
    -1, 
    true); 

// ...
_starter.Set(); // Löst Go(..) aus
reg.Unregister (_starter); // Aufräumen

public static void Go (object data, bool timedOut) 
{
    Console.WriteLine ("Started - " + data);
}
```

## III. Synchronisation: Exklusive Sperren (Locking)

Sorgt dafür, dass nur ein Thread gleichzeitig auf kritische Daten zugreift.

### 1. `lock` Ausdruck

`lock` ist die syntaktische Abkürzung für `Monitor.Enter/Exit` im `try/finally`-Block.

**Snippet: Thread-sicherer Code-Abschnitt**

```csharp
class ThreadSafe
{
    static readonly object _locker = new object();
    static int _val1, _val2;

    static void Go()
    {
        lock (_locker) // Exklusive Sperre auf _locker
        {
            if (_val2 != 0) Console.WriteLine (_val1 / _val2);
            _val2 = 0;
        }
    }
}
```

### 2. `Monitor` ab Framework 4.0 (Sichere Implementierung)

Verhindert *leaked locks*, falls `Abort` oder eine Exception während `Enter` auftritt.

**Snippet:**

```csharp
static void Go()
{
    bool lockTaken = false;
    try
    {
        Monitor.Enter (_locker, ref lockTaken); // Übergabe von ref lockTaken
        // ... Kritischer Code ...
    }
    finally { if (lockTaken) Monitor.Exit (_locker); }
}
```

### 3. `Wait` und `Pulse` (Kommunikation im Lock)

Wird zur effizienten Kommunikation verwendet. `Wait` blockiert den Thread und **gibt die Sperre frei**.

**Snippet: Produzent-Konsument-Puffer**

```csharp
public void Put(char ch)
{
    lock (this)
    {
        while (n >= size) Monitor.Wait(this); // Warten, Puffer voll
        // ... Einfügen ...
        Monitor.Pulse(this); // Weckt wartenden Get-Thread
    }
}
```

**Achtung:** Wird kein `Pulse` aufgerufen, läuft der wartende Thread nie wieder (Deadlock-Gefahr).

### 4. Prozessübergreifende Sperre (`Mutex`)

Wird verwendet, um Synchronisation über Prozessgrenzen hinweg zu realisieren (z.B. Einzelstart einer Anwendung).

**Snippet: Prüfen auf Erststart der Anwendung**

```csharp
public static bool IsApplicationStarted() 
{
    string mutexName = Application.ProductName;
    mutex = new Mutex(false, mutexName); // Mutex erstellen
    
    // Versucht, den Mutex sofort zu erhalten. Timeout 0 ms.
    if (mutex.WaitOne(0, true)) 
        return false; // Mutex erhalten, App ist nicht gestartet
    else
        return true; // App läuft bereits
}
```

### 5. Methode komplett synchronisieren

```csharp
using System.Runtime.CompilerServices;
// ...
[MethodImpl(MethodImplOptions.Synchronized)] // Sperrt die ganze Methode
public void Calculate() 
{
    // Anweisungen... (Nur ein Thread gleichzeitig)
}
```

## IV. Synchronisation: Signaling und Zählende Ressourcen

### 1. Semaphor (`SemaphoreSlim`)

Verwaltet beschränkte, zählbare Ressourcen (Kapazität).

**Snippet: Kapazitätskontrolle (Maximal 3 Threads erlaubt)**

```csharp
static SemaphoreSlim _sem = new SemaphoreSlim (3); // Kapazität von 3

static void Enter (object id)
{
    Console.WriteLine (id + " wants to enter");
    _sem.Wait(); // Reservieren/Blockiert, wenn Kapazität 0
    Console.WriteLine (id + " is in!"); 
    // ... Arbeit ... 
    _sem.Release(); // Freigeben/Zähler erhöhen
}
```

### 2. Events (`AutoResetEvent` und `ManualResetEvent`)

Warten auf Signale, um Polling zu vermeiden.

#### AutoResetEvent (Drehkreuz)

Gibt genau einen wartenden Thread frei und schließt sich automatisch (`auto`).

**Snippet:**

```csharp
static EventWaitHandle _waitHandle = new AutoResetEvent (false);

// ... In Thread 1 ...
Console.WriteLine ("Waiting...");
_waitHandle.WaitOne(); // Blockiert, bis Set() aufgerufen wird
Console.WriteLine ("Notified");

// ... In Thread 2 ...
_waitHandle.Set(); // Weckt den Waiter auf
```

#### ManualResetEvent (Tor)

Gibt **alle** wartenden Threads frei, wenn `Set()` aufgerufen wird, und bleibt offen, bis `Reset()` aufgerufen wird.

**Snippet:**

```csharp
EventWaitHandle manual = new ManualResetEvent (false); 

manual.Set(); // Öffnet das Tor. Alle warten Threads werden freigegeben.
// ...
manual.Reset(); // Schließt das Tor wieder.
```

### 3. `CountdownEvent`

Blockiert, bis eine bestimmte Anzahl von Signalen von Worker-Threads empfangen wurde.

**Snippet:**

```csharp
static CountdownEvent _countdown = new CountdownEvent (3); // Warten auf 3 Signale

static void Main()
{
    // Starte 3 Threads...
    _countdown.Wait(); // Blockiert, bis Zähler auf 0 ist
    Console.WriteLine ("All threads finished!"); 
}

static void SaySomething (object thing)
{
    // ... Arbeit ... 
    _countdown.Signal(); // Zähler wird um 1 verringert
}
```

### 4. Komplexes Warten

Warten auf mehrere Handles gleichzeitig.

**Snippet:**

```csharp
WaitHandle[] handles = new WaitHandle[] { handle1, handle2 };

// Blockiert, bis das erste Handle signalisiert
int index = WaitHandle.WaitAny(handles); 

// Blockiert, bis alle Handles signalisiert sind
WaitHandle.WaitAll(handles); 
```