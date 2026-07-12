<div align="center">

# 🌙 Under Quiet Skies ✨

*Ein leises 2D-Spiel unterm Sternenhimmel — gebaut mit Godot 4*

![Engine](https://img.shields.io/badge/Godot-4.x-ff2ea6?style=for-the-badge&logo=godotengine&logoColor=white)
![Language](https://img.shields.io/badge/GDScript-💖-c9b6ff?style=for-the-badge)
![Style](https://img.shields.io/badge/Style-2D-ffe9a8?style=for-the-badge)


```
· · · · · · · · · · · · · · · · · · · · · · · ·
  ⋆｡°✩  ⁺ ✩  °｡⋆   🌙   ⋆｡° ✩⁺  ✩ °｡⋆
· · · · · · · · · · · · · · · · · · · · · · · ·
```

</div>

## 🪐 Worum geht's hier eigentlich?

**Under Quiet Skies** ist im Rahmen eines Schulprojekts mit der **Godot Engine 4** entstanden. Ziel war es, Game Design, Projektplanung und Programmierung nicht nur zu lernen, sondern auch mal *wirklich anzuwenden*.

> 🌌 **Vibe-Check:** ruhige Atmosphäre, viel Erkunden, und ein Inventarsystem, das tatsächlich funktioniert — kein Platzhalter-Quatsch.

---

## 🧰 Der Werkzeugkasten

| | |
|---|---|
| 🎮 **Engine** | Godot Engine 4 |
| 📜 **Sprache** | GDScript |
| 🖼️ **Grafikstil** | 2D |
| 🎓 **Projektart** | Schulprojekt / Teamarbeit |

---

## 🗂️ Wo sich alles versteckt

Der Hauptordner heißt schlicht `Gdot_game` und enthält das komplette Godot-Projekt.

<details>
<summary>📦 <strong>Inhalt anzeigen</strong> (klick mich)</summary>

- 🖼️ **Assets** — Grafiken für Charaktere, Umgebung, UI
- 🗺️ **Szenen** — Spielwelten, MainScene, Loadingscreen
- 🎒 **UI** — Inventarsystem und Menüs
- ⚙️ **Skripte** — die eigentliche Magie dahinter

</details>

---

## ✨ Was man im Spiel so tun kann

- 🚶 Sich bewegen *(Grundvoraussetzung, aber wichtig)*
- 🌀 Zwischen Szenen wechseln, inklusive Loadingscreen statt Cliffhanger
- 🎒 Ein Inventar öffnen, befüllen, bestaunen
- 💬 Mit der UI interagieren, ohne dass etwas explodiert
- 🌙 Eine selbstgebaute Spielwelt erkunden

---

## 🤝 Wer hat was gebaut?

<table>
<tr>
<td valign="top" width="50%">

### 🌸 Mein Revier

- Projektplanung & -organisation
- Gesamtes Spieldesign
- Die komplette Spielwelt / Map, eigenhändig gepixelt
- Grafik-Asset-Auswahl *(im Team)*
- Szenenstruktur: MainScene, Loadingscreen, Levelwechsel
- Projektstruktur in Godot aufgebaut

</td>
<td valign="top" width="50%">

### ⚡ Alex' Revier

- Hauptverantwortlich fürs Programmieren
- Spielerbewegung implementiert
- Spielerfunktionen umgesetzt
- Spieler mit dem Inventar-UI verkabelt
- Logik, damit Player und UI sich verstehen

</td>
</tr>
</table>

> 🙈 **Disclaimer:** Ein Kisten- oder Container-System gibt es nicht. Bewusst weggelassen, nicht vergessen — versprochen.

---

## 🚀 Und los geht's

1. 🟢 Godot Engine 4 öffnen
2. 📂 Auf **„Importieren"** klicken
3. 📁 Den Ordner `Gdot_game` auswählen
4. ▶️ Über die MainScene starten — fertig, viel Spaß

---

## 🧾 Kleingedrucktes

- 🍎 `.DS_Store`-Dateien sind nur Mac-Krümel und völlig harmlos
- 🎓 Das Projekt ist rein schulischen Zwecken gewidmet
- 🔧 Erweiterungen wären möglich, sind aber *(noch)* nicht Teil des aktuellen Stands

---

## 🎬 Fazit

**Under Quiet Skies** beweist: Teamarbeit funktioniert, Godot 4 macht Spaß, und ein vollständiges Spielprojekt auf die Beine zu stellen ist machbar — solange man Kreativität und Technik unter einen Hut bringt.

<div align="center">

```
· · · · · · · · · · · · · · · · · · · · · · · ·
       ✩  T E C H N I S C H E R   T E I L  ✩
· · · · · · · · · · · · · · · · · · · · · · · ·
```

</div>

## 🔬 Jetzt wird's technisch: Die komplette Programmerkennung

Dieses Kapitel geht ans Eingemachte: der vollständige technische Aufbau von **Under Quiet Skies**.

### 🛠️ Die Entwicklungsumgebung

Gebaut mit **Godot Engine 4**, gesteuert von **GDScript** — einer objektorientierten Sprache, die Godot extra für Spiele mitgebracht hat.

### 🧩 Szenen & Nodes, das Dreamteam

Godot tickt in **Szenen und Nodes**: Eine Szene ist eine geschlossene Einheit (Spieler, UI, Map), eine Szene besteht aus mehreren Nodes, und jeder Node hat genau einen Job.

Ein paar Node-Promis aus dem Projekt:

```
Node2D            → das Fundament für so ziemlich alles
CharacterBody2D    → der Spieler, inklusive Bewegung & Kollision
TileMap            → die Spielwelt
CanvasLayer        → die Benutzeroberfläche
Control            → einzelne UI-Elemente
```

<details>
<summary>🕹️ <strong>Der Spieler</strong> — Details anzeigen</summary>

Eigene Szene, gebaut auf einem `CharacterBody2D`. Verarbeitet Tastatureingaben, bewegt sich durch die Welt, fragt Kollisionen ab und kommuniziert mit dem Inventarsystem.

- Bewegung läuft über `_physics_process(delta)`
- Eingaben kommen über Godots Input-System
- Geschwindigkeit wird über Vektoren gesteuert
- Kollisionen übernimmt die Physik-Engine automatisch

</details>

<details>
<summary>🎒 <strong>Das Inventarsystem</strong> — Details anzeigen</summary>

Lebt als eigene UI-Szene, eingebunden über einen `CanvasLayer`. Per Tastendruck auf- und zuklappbar, der Spieler liefert die Daten, die UI zeigt sie sofort an.

- Klare Trennung von **Datenlogik** (Spieler) und **Darstellung** (UI)
- Kommunikation über Variablen und Signale
- Keine feste Verdrahtung zwischen Player und UI

</details>

<details>
<summary>🪟 <strong>UI, Loadingscreen & Map</strong> — Details anzeigen</summary>

- UI besteht aus mehreren `Control`-Nodes und bleibt immer sichtbar, egal was in der Welt passiert
- Szenenmanagement: aktive Szene beenden → Loadingscreen → neue Szene laden → nahtloser Übergang
- Spielwelt via `TileMaps`, mehrere Layer (Boden, Objekte, Kollision), direkt an die Physik-Engine angebunden

</details>

### 🗃️ Projektstruktur, Teamarbeit & was fehlt

Modular durch und durch: jede größere Funktion hat ihr eigenes Skript, Spieler/UI/Map sind sauber getrennt, Szenen klar benannt. Projektstruktur und Szenenorganisation wurden zentral geplant, die Spielerlogik entstand unabhängig von der UI.

> ⏳ **Bewusst nicht umgesetzt:** Kisten-/Container-System und Item-Transfer zwischen Inventaren — Stoff für eine mögliche Fortsetzung.

---

<div align="center">

### ✨ Technisches Fazit ✨

Godots Szenen-Node-System verstanden · Logik und Darstellung sauber getrennt
GDScript strukturiert geschrieben · mehrere Spielsysteme, die tatsächlich miteinander reden

```
⋆｡°✩ ✩ ✩ °｡⋆   gute nacht, schlaft gut   ⋆｡° ✩ ✩ ✩ °｡⋆
```

</div>
