# MiniTextAdventure

Game.cs:
  -De hoofdklasse die de spel-lus (Start()) beheert.
  -Verwerkt de input van de speler (help, look, go, take, fight, inventory, quit).
  -Beheert de interacties met kamers (Rooms) en inventaris (Inventory).
  
Rooms.cs / Room.cs:
  -Room: definieert een kamer met eigenschappen zoals naam, beschrijving, lethale status, monsters, items, en verbindingen met andere kamers.
  -Rooms: beheert alle kamers in het spel, houdt bij waar de speler zich bevindt, en bevat methoden om te bewegen (Go) en items op te pakken (Take).

Inventory.cs / Item.cs:
  -Inventory: beheert de items van de speler, met methoden om items toe te voegen, verwijderen en te tonen.
  -Item: beschrijft een individueel item met id, name en description.

Monster.cs / CombatService.cs:
  -Monster: beschrijft een monster met een naam en levend-status.
  -CombatService: regelt gevechten met monsters, controleert of de speler een wapen heeft en of het monster al verslagen is.

Enums:
  -Direction: de vier richtingen (N, E, S, W) voor beweging.
  -MoveResult: mogelijke uitkomsten bij bewegen (bijv. Moved, Died, Won).
  -FightResult: mogelijke uitkomsten van een gevecht (bijv. Victory, NoWeapon).

Testaanpak
Om te testen of het spel correct werkt, kan een systematische aanpak worden gevolgd:

Kamers en beweging:
  -Test of alle richtingen correct werken (n, e, s, w).
  -Test wat gebeurt bij lethale kamers (Valkamer) → speler moet sterven.
  -Test wat gebeurt als een sleutel nodig is (Deur) → speler kan pas door met sleutel.

Items:
  -Test of items correct kunnen worden opgepakt (key, sword).
  -Controleer of inventaris correct wordt bijgewerkt en duplicaten worden voorkomen.

Gevechten:
  -Test gevechten in kamers met een monster (Monsterkamer).
  -Test scenario’s zonder wapen of als het monster al dood is.

Algemene spel-logica:
  -Test commando’s zoals look, inventory, help en quit.
  -Test het winnen van het spel door sleutel op te pakken en de deur te openen.
  -Test foutieve input (xyz) en check of er een correcte foutmelding komt.
