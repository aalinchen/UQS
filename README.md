# Under Quiet Skies

## Projektbeschreibung

**Under Quiet Skies** ist ein 2D-Spielprojekt, das im Rahmen eines Schulprojekts mit der **Godot Engine 4** entwickelt wurde.  
Ziel des Projekts war es, grundlegende Kenntnisse in Game Design, Projektplanung und Programmierung anzuwenden und zu vertiefen.

Das Spiel legt den Fokus auf eine ruhige Atmosphäre, Erkundung und die Umsetzung eines funktionierenden Inventarsystems.

---

## Verwendete Technologien

- **Game Engine:** Godot Engine 4
- **Programmiersprache:** GDScript
- **Grafikstil:** 2D
- **Projektart:** Schulprojekt / Teamarbeit

---

## Projektstruktur

Der Hauptordner des Projekts lautet:


Der Ordner **Gdot_game** enthält das vollständige Godot-Projekt inklusive Szenen, Skripten, Assets und Einstellungen.

### Wichtige Inhalte:

- **Assets:** Grafiken für Charaktere, Umgebung und Benutzeroberfläche
- **Szenen:** Spielwelten, MainScene und Ladebildschirm
- **UI:** Inventarsystem und Menüs
- **Skripte:** Spiellogik und Steuerung

---

## Spielmechaniken

- Spielerbewegung
- Szenenwechsel (inkl. Loadingscreen)
- Inventarsystem mit Benutzeroberfläche
- Interaktion zwischen Spieler und UI
- Erkunden einer selbst erstellten Spielwelt (Map)

---

## Arbeitsaufteilung

### Eigener Aufgabenbereich

- Planung und Organisation des gesamten Projekts
- Gestaltung des gesamten Spieldesigns
- Erstellung der Spielwelt / Map (eigenständig)
- Auswahl der grafischen Assets (gemeinsam im Team)
- Umsetzung der Szenenstruktur
  - MainScene
  - Loadingscreen
  - Szenen- und Levelwechsel
- Aufbau der Projektstruktur in Godot

### Aufgabenbereich von Alex

- Hauptverantwortlich für die Programmierung
- Implementierung der Spielerbewegung
- Umsetzung der Spielerfunktionen
- Verbindung des Spielers mit dem Inventar-UI
- Logik für die Zusammenarbeit zwischen Player und UI

> Hinweis: Ein Kisten- oder Container-System wurde nicht umgesetzt.

---

## Projektstart

1. Godot Engine 4 öffnen  
2. Auf **„Importieren“** klicken  
3. Den Ordner **Gdot_game** auswählen  
4. Projekt über die MainScene starten  

---

## Hinweise

- Enthaltene `.DS_Store` Dateien stammen vom Betriebssystem und haben keinen Einfluss auf das Projekt
- Das Projekt dient ausschließlich schulischen Zwecken
- Erweiterungen sind möglich, jedoch nicht Teil des aktuellen Projektstands

---

## Fazit

Das Projekt **Under Quiet Skies** zeigt die erfolgreiche Zusammenarbeit im Team sowie die Umsetzung eines vollständigen Spielprojekts mit Godot 4.  
Dabei wurden sowohl technische als auch kreative Aspekte der Spieleentwicklung berücksichtigt.

## Gesamte Programmerkennung und technische Umsetzung

Dieses Kapitel beschreibt den vollständigen technischen Aufbau des Projekts **Under Quiet Skies**. Es erklärt die Funktionsweise der einzelnen Systeme, deren Zusammenspiel sowie die grundlegenden Programmierkonzepte, die im Projekt verwendet wurden.

---

## Entwicklungsumgebung

Das Spiel wurde mit der **Godot Engine 4** entwickelt.  
Die gesamte Spiellogik basiert auf **GDScript**, einer von Godot bereitgestellten, objektorientierten Programmiersprache, die speziell für Spiele entwickelt wurde.

---

## Grundprinzip: Szenen- und Node-System

Godot verwendet ein **Szenen-Node-System**:

- Eine **Szene** ist eine in sich geschlossene Einheit (z. B. Spieler, UI, Map)
- Eine Szene besteht aus mehreren **Nodes**
- Jeder Node hat eine klar definierte Aufgabe

Beispiele für verwendete Node-Typen:
- `Node2D` → Basis für Spielobjekte
- `CharacterBody2D` → Spieler mit Bewegung und Kollision
- `TileMap` → Spielwelt
- `CanvasLayer` → Benutzeroberfläche (UI)
- `Control` → UI-Elemente

Diese Struktur sorgt für Übersichtlichkeit und Wiederverwendbarkeit.

---

## Spieler (Player)

Der Spieler ist als eigene Szene umgesetzt und basiert auf einem `CharacterBody2D`.

### Aufgaben des Spieler-Codes:
- Verarbeiten von Tastatureingaben
- Bewegung in der Spielwelt
- Kollisionsabfrage mit der Map
- Kommunikation mit dem Inventarsystem

### Bewegung:
- Die Bewegung wird in der Funktion `_physics_process(delta)` berechnet
- Eingaben werden über das Input-System von Godot abgefragt
- Die Geschwindigkeit wird über Vektoren gesteuert
- Kollisionen werden automatisch durch die Physik-Engine behandelt

Die Spiellogik ist so aufgebaut, dass sie leicht erweitert werden kann (z. B. Sprinten, Animationen).

---

## Inventarsystem

Das Inventar ist als **separate UI-Szene** umgesetzt und über einen `CanvasLayer` in das Spiel eingebunden.

### Funktionsweise:
- Das Inventar kann per Tastendruck geöffnet und geschlossen werden
- Die Inventardaten werden vom Spieler an das UI übergeben
- Das UI stellt die Items visuell dar
- Änderungen im Inventar werden direkt aktualisiert

### Technisches Konzept:
- Trennung von **Datenlogik** (Spieler) und **Darstellung** (UI)
- Kommunikation über Variablen und Signale
- Keine feste Kopplung zwischen Player und UI

Diese Lösung ermöglicht eine saubere Struktur und einfache Erweiterungen.

---

## Benutzeroberfläche (UI)

Die Benutzeroberfläche besteht aus mehreren `Control`-Nodes.

### Aufgaben:
- Anzeige des Inventars
- Darstellung von UI-Elementen
- Reaktion auf Benutzereingaben

Die UI ist unabhängig von der Spielwelt und bleibt stets sichtbar, unabhängig von der Kameraposition.

---

## Szenenmanagement und Loadingscreen

Für den Wechsel zwischen Spielabschnitten wurde ein eigenes **Szenenmanagement** umgesetzt.

### Ablauf:
1. Aktive Szene wird beendet
2. Loadingscreen wird angezeigt
3. Neue Szene wird geladen
4. Übergang zur Zielszene

Dieser Ablauf sorgt für:
- Saubere Übergänge
- Keine sichtbaren Ladeunterbrechungen
- Bessere Benutzererfahrung

---

## Spielwelt und Map

Die Spielwelt wurde mit **TileMaps** umgesetzt.

### Eigenschaften:
- Eigenständig erstellte Map
- Mehrere Layer (Boden, Objekte, Kollision)
- Klare Trennung zwischen visuellen Elementen und Kollisionen

Die Map ist direkt mit der Physik-Engine verbunden, wodurch der Spieler korrekt mit der Umgebung interagiert.

---

## Projektstruktur und Organisation

Das Projekt ist modular aufgebaut:

- Jede größere Funktion hat ein eigenes Skript
- Spieler, UI und Map sind getrennt
- Szenen sind klar benannt und strukturiert

Diese Struktur erleichtert:
- Teamarbeit
- Fehlerbehebung
- Erweiterungen
- Wartung des Codes

---

## Zusammenarbeit im Team (technisch)

- Die Projektstruktur und Szenenorganisation wurden zentral geplant
- Die Spielerlogik wurde unabhängig vom UI entwickelt
- Das Inventar wurde so umgesetzt, dass es mit dem Spieler zusammenarbeitet, aber eigenständig bleibt

---

## Nicht umgesetzte Funktionen

Folgende Funktionen wurden bewusst nicht umgesetzt:
- Kisten- oder Container-System
- Transfer von Items zwischen verschiedenen Inventaren

Diese Funktionen könnten in zukünftigen Erweiterungen ergänzt werden.

---

## Technische Zusammenfassung

Das Projekt **Under Quiet Skies** zeigt:
- Verständnis für das Szenen- und Node-System von Godot
- Saubere Trennung von Logik und Darstellung
- Strukturierte Programmierung in GDScript
- Funktionierende Zusammenarbeit mehrerer Spielsysteme

Das Programm ist übersichtlich, erweiterbar und erfüllt die Anforderungen eines vollständigen Schulprojekts im Bereich Game Development.


