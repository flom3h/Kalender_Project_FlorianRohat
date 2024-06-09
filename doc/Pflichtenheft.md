# Pflichtenheft (Florian, Rohat)

## Softwarevoraussetzungen
  - .Net (8.0)
  - ExtendedWPFToolkit (4.6.0)
  - FirebaseDatabase.net (4.2.0)
  - Serilog (4.0.0)
  - Serilog.Sinks.Console (5.1.0-dev-00943)
  - Serilog.Sinks.File (5.0.1-dev-00972)

## Funktionsblockdiagramm
  - Nicht ganz verstanden
  ![Funkitonsblockdiagramm](Funcdiag.PNG)

## Architektur
  - Aufbau:
    Die Objektklassen (Event, Note, Todo) werden mit den jeweiligen Collections verbunden wo die Objekte in Listen eingelagert werden.
    </br>
    Die daten von den Collection klassen werden an die jeweiligen Pages weitergeleitet und dort angezeigt.
    </br>
    Die Pages werden mithilfe von der Navigator Klasse auf dem MainWindow angezeigt.
    </br>
    Verschiedene ItemControls (für Note, Event, Todo) werden in die Collecitons (in der Draw funktion) eingebunden um diese dann als das Grafische Element der Objekte anzuzeigen.
    </br>
    Das Mainwindow wird dann als Startup Fenster unter App.Xaml gewählt und somit ist das Programm vollständig.
    </br>
    Besonderheiten:
      TodoCollection wird an MainPage, AllTodosView & an ImportantView weitergegeben.
      </br>
      Todo & Note haben jeweils ein AddWindow, welches durch die jeweiligen Pages aufgerufen wird.
      </br>
      Der ColorConverter ist eine Klasse welche als Bining unter allen Pages mit Kalender.
      </br>
      CalendarMonthView ist besonders da die Objektklassen direkt an die Page gegeben werden, ohne zuerst diese in einer Collection zu sammeln, um trotzdem gesammelt zu werden wird ein Converter verwendet.
      </br>
      Auch wenn CalendarMonthView keinen Kalender besitzt, hat die Page auch eine Klasse (ColorConverterCurrentDay), welche als Farbbinding für den Kalender verwendet wird.
      </br>
  
## Detaillierte Beschreibung
  Wenn man das Programm startet, befindet man sich auf dem Homescreen, auf der linken Seite hat man die Menüleiste, mit welcher man durch die Einzelnen Tage des Jahres navigieren kann, durch das Kalenderelement, und mit welcher man auf andere Pages kommen kann, um andere funktionen des Programms zu entdecken. Der Kalender bietet auch die funktion Todos & Events auf den Tagen anzuzeigen, um genau zu sehen wo sich was befindet (Problematisch).
  </br>

  Oben befindet sich die Topleiste, welche verschiedene Funktionen wie (Add, Save & Delete) beinhalten, sowie eine Uhr welche die jetzige Zeit anzeigt.
  </br>

  Auf den Todo Objekten befinden sich, der Titel, das Datum der Wichtig Button, mit welchem man das Todo als Wichtig markieren kann. Ein Edit Button mit welchem man das Datum, sowie den Titel des Todos bearbeiten kann. Ein Check Button welcher das Todo als Gemacht erscheinen lässt und ein Delete Button, mit welchem man das Todo entfernen kann.
  </br>

  Auf der Kalenderseite sieht man den jetzigen Monat sowei die Todos, welche sich auf den Tagen befinden, falls man auf ein Tag des Kalenders klickt, kommt man auf die Todo Seite des jeweiligen Tages.
  </br>

  Bei Notizen befindet sich eine Seitenleiste, welche die Titel der jeweiligen Notizen beinhalten. Drückt man auf einen Titel, so öffnet sich die Notiz und man kann direkt mit dem bearbeiten der Notiz anfangen.
  </br>

  Auf der Events Seite befinden sich alle Feiertage / Ferien des nächsten Jahres
  </br>

  Fügt man nun ein(e) Todo / Notiz hinzu, so öffnet sich ein Fenster indem man den Titel sowie (bei Todo) den Tag einstellen kann.

## Umsetzung
  Wir haben angefangen mit den Todos, da wir schon einige Erfahrung mit Todos in WPF gemacht haben. Firebase haben wir als Datenbank gewählt, da wir 1. schon Erfahrung gesammelt haben und 2. eine No-SQL Datenbank, welche in der Cloud gehostet wird, als simple Lösung für die verschiedenen Anwendungsbereiche gilt. Nachdem wir die Todo sowie die Collection gemacht haben. Haben wir uns daran gesetzt die Todos Richtig anzuzeigen. Nachdem dies gemeistert wurde. Haben wir uns jeweils an die anderen Features des Programms drangesetzt (Ich an Notitzen, Rohat an Kalender) und diese nach dem selben Prinzip zu Lösen. Nachdem die Grundfunktionen unserer Applikation gestanden sind. Haben wir angefangen das Design sowie einzelne Ferfeinerungen zu machen. Zum Schluss wurden noch Exceptions sowie Logging eingefügt.

## Probleme und Ihre Lösung
1. **Pages Navigieren**
  Problem:
    Da wir den Code der MainPage ins MainWinow geschrieben haben, konnte man nicht richtig durch die Pages navigieren, da der Frame zu tief im Code gesetzt wurde.

   Fix:
    Wir haben den Code vom MainWindow in eine Eigene Page gemacht wodruch das MainWindow nur noch aus den PageEvents und dem Frame besteht.

2. **Kalender Binding**
   Problem:
    Das Binding für die Kalendertage wird erst Ausgelöst und Angezeigt, wenn man entweder den Kalender (durch Monat wechseln) oder die Seite neu ladet.

    Fix (Potenziell):
     Man sollte alle Events & Todos in einer JSON nach schließen des Programms speichern, somit können alle Todos und Events bei Start reingeladen werden. (Auch potenzieller fix mit Firebase Verbindung möglich, aber dadurch werden Ladezeiten sehr langsam.)