using System.Collections.Generic;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Code-authored, evidence-backed prototype content for the Phase 7E data model:
    /// 5 floors (displayed 5..1, descent order), 5 evidence trials each, 25 trials total.
    /// Every trial references an existing corridor clue, has exactly 4 answers with exactly
    /// one correct option matching the clue's evidence, and provides EN/FR text for prompts
    /// and answers.
    ///
    /// This is DATA ONLY (Phase 7E is DATA_MODEL_ONLY): it does not drive the runtime trial
    /// flow yet — the live game still uses <see cref="PrototypeFloorSet"/>. Pure data (no
    /// Unity dependency) so it stays fully testable in EditMode and validatable via
    /// <see cref="EvidenceTrialValidator"/>. The visual clue rendering and the observation
    /// camera pass remain future phases (see Docs/CORRIDOR_OBSERVATION_DESIGN.md).
    /// </summary>
    public static class PrototypeEvidenceFloorSet
    {
        /// <summary>Number of floors in the prototype.</summary>
        public const int FloorCount = 5;

        /// <summary>Trials per floor in the prototype.</summary>
        public const int TrialsPerFloor = 5;

        /// <summary>Build the ordered list of evidence floors (displayed 5 first, down to 1).</summary>
        public static IReadOnlyList<FloorObservationSet> BuildAll()
        {
            return new List<FloorObservationSet>
            {
                BuildFloor5(),
                BuildFloor4(),
                BuildFloor3(),
                BuildFloor2(),
                BuildFloor1(),
            };
        }

        // ---- Floor 5 — Observation (descent start) ------------------------------------

        private static FloorObservationSet BuildFloor5()
        {
            const int floor = 5;
            const float time = 8f;
            const int diff = 1;

            var clues = new List<CorridorClue>
            {
                Clue("f5-clue-room", CorridorClueType.DoorNumber, floor,
                    "ROOM DISPLAY", "NUMÉRO DE PORTE",
                    "Room 104 blinks above the first door.", "La chambre 104 clignote au-dessus de la première porte.",
                    "Corridor.Door1Plate", "104"),
                Clue("f5-clue-arrow", CorridorClueType.DirectionInstruction, floor,
                    "ELEVATOR PANEL", "PANNEAU ASCENSEUR",
                    "The lit arrow points up.", "La flèche allumée pointe vers le haut.",
                    "Elevator.PanelArrow", "Up"),
                Clue("f5-clue-light", CorridorClueType.LightState, floor,
                    "PANEL LIGHT", "VOYANT",
                    "Only the green light stays on.", "Seul le voyant vert reste allumé.",
                    "Corridor.PanelLight", "Green"),
                Clue("f5-clue-glitch", CorridorClueType.Anomaly, floor,
                    "FLOOR DISPLAY", "AFFICHEUR D'ÉTAGE",
                    "The floor digit 4 flickers wrongly.", "Le chiffre d'étage 4 clignote anormalement.",
                    "Elevator.FloorDisplay", "4"),
                Clue("f5-clue-symbol", CorridorClueType.Symbol, floor,
                    "DOOR MARK", "MARQUE SUR LA PORTE",
                    "An eye symbol is painted on the door.", "Un symbole d'œil est peint sur la porte.",
                    "Corridor.DoorMark", "Eye"),
            };

            var trials = new List<EvidenceTrial>
            {
                Trial("f5-trial-1", "f5-clue-room",
                    "Which room number blinked?", "Quel numéro de chambre clignotait ?", time, diff,
                    A("f5-trial-1-1", "104", "104", true),
                    A("f5-trial-1-2", "101", "101"),
                    A("f5-trial-1-3", "140", "140"),
                    A("f5-trial-1-4", "401", "401")),
                Trial("f5-trial-2", "f5-clue-arrow",
                    "Which arrow was lit?", "Quelle flèche était allumée ?", time, diff,
                    A("f5-trial-2-1", "Up", "Haut", true),
                    A("f5-trial-2-2", "Down", "Bas"),
                    A("f5-trial-2-3", "Left", "Gauche"),
                    A("f5-trial-2-4", "Right", "Droite")),
                Trial("f5-trial-3", "f5-clue-light",
                    "Which light stayed on?", "Quel voyant est resté allumé ?", time, diff,
                    A("f5-trial-3-1", "Red", "Rouge"),
                    A("f5-trial-3-2", "Green", "Vert", true),
                    A("f5-trial-3-3", "Blue", "Bleu"),
                    A("f5-trial-3-4", "White", "Blanc")),
                Trial("f5-trial-4", "f5-clue-glitch",
                    "Which floor number glitched?", "Quel chiffre d'étage a bugué ?", time, diff,
                    A("f5-trial-4-1", "2", "2"),
                    A("f5-trial-4-2", "4", "4", true),
                    A("f5-trial-4-3", "6", "6"),
                    A("f5-trial-4-4", "8", "8")),
                Trial("f5-trial-5", "f5-clue-symbol",
                    "Which symbol was on the door?", "Quel symbole était sur la porte ?", time, diff,
                    A("f5-trial-5-1", "Eye", "Œil", true),
                    A("f5-trial-5-2", "Key", "Clé"),
                    A("f5-trial-5-3", "Hand", "Main"),
                    A("f5-trial-5-4", "Door", "Porte")),
            };

            return new FloorObservationSet(floor, clues, trials);
        }

        // ---- Floor 4 — Short memory ---------------------------------------------------

        private static FloorObservationSet BuildFloor4()
        {
            const int floor = 4;
            const float time = 7f;
            const int diff = 1;

            var clues = new List<CorridorClue>
            {
                Clue("f4-clue-center-symbol", CorridorClueType.Symbol, floor,
                    "SYMBOLS", "SYMBOLES",
                    "Three symbols; the key is in the center.", "Trois symboles ; la clé est au centre.",
                    "Corridor.SymbolRow", "Key"),
                Clue("f4-clue-twice-word", CorridorClueType.WallMessage, floor,
                    "WALL WORDS", "MOTS SUR LE MUR",
                    "The word WAIT appears twice on the wall.", "Le mot ATTENDS apparaît deux fois sur le mur.",
                    "Corridor.WallWords", "Wait"),
                Clue("f4-clue-moved-symbol", CorridorClueType.Anomaly, floor,
                    "SYMBOLS", "SYMBOLES",
                    "The triangle symbol moved since the last glance.", "Le symbole triangle a bougé depuis le dernier regard.",
                    "Corridor.SymbolRow", "Triangle"),
                Clue("f4-clue-whisper-name", CorridorClueType.AudioProxy, floor,
                    "WHISPER", "MURMURE",
                    "The intercom whispers the name Mara.", "L'interphone murmure le nom Mara.",
                    "Corridor.Speaker", "Mara"),
                Clue("f4-clue-open-door", CorridorClueType.DoorState, floor,
                    "HALL DOORS", "PORTES DU COULOIR",
                    "Only the center door stands open.", "Seule la porte du centre est ouverte.",
                    "Corridor.Doors", "Center"),
            };

            var trials = new List<EvidenceTrial>
            {
                Trial("f4-trial-1", "f4-clue-center-symbol",
                    "Which symbol was in the center?", "Quel symbole était au centre ?", time, diff,
                    A("f4-trial-1-1", "Eye", "Œil"),
                    A("f4-trial-1-2", "Key", "Clé", true),
                    A("f4-trial-1-3", "Hand", "Main"),
                    A("f4-trial-1-4", "Door", "Porte")),
                Trial("f4-trial-2", "f4-clue-twice-word",
                    "Which word appeared twice?", "Quel mot est apparu deux fois ?", time, diff,
                    A("f4-trial-2-1", "Wait", "Attends", true),
                    A("f4-trial-2-2", "Open", "Ouvre"),
                    A("f4-trial-2-3", "Run", "Cours"),
                    A("f4-trial-2-4", "Hide", "Cache-toi")),
                Trial("f4-trial-3", "f4-clue-moved-symbol",
                    "Which symbol moved?", "Quel symbole a bougé ?", time, diff,
                    A("f4-trial-3-1", "Circle", "Cercle"),
                    A("f4-trial-3-2", "Square", "Carré"),
                    A("f4-trial-3-3", "Triangle", "Triangle", true),
                    A("f4-trial-3-4", "Cross", "Croix")),
                Trial("f4-trial-4", "f4-clue-whisper-name",
                    "Which name was whispered?", "Quel nom a été murmuré ?", time, diff,
                    A("f4-trial-4-1", "Anna", "Anna"),
                    A("f4-trial-4-2", "Mara", "Mara", true),
                    A("f4-trial-4-3", "Lena", "Lena"),
                    A("f4-trial-4-4", "Sara", "Sara")),
                Trial("f4-trial-5", "f4-clue-open-door",
                    "Which door was open?", "Quelle porte était ouverte ?", time, diff,
                    A("f4-trial-5-1", "Left", "Gauche"),
                    A("f4-trial-5-2", "Right", "Droite"),
                    A("f4-trial-5-3", "Center", "Centre", true),
                    A("f4-trial-5-4", "None", "Aucune")),
            };

            return new FloorObservationSet(floor, clues, trials);
        }

        // ---- Floor 3 — Environmental instruction --------------------------------------

        private static FloorObservationSet BuildFloor3()
        {
            const int floor = 3;
            const float time = 6f;
            const int diff = 2;

            var clues = new List<CorridorClue>
            {
                Clue("f3-clue-wall-left", CorridorClueType.WallMessage, floor,
                    "WALL", "MUR",
                    "The wall reads: DO NOT LOOK LEFT.", "Le mur indique : NE REGARDE PAS À GAUCHE.",
                    "Corridor.WallMessage", "Do not look left"),
                Clue("f3-clue-panel-warning", CorridorClueType.WallMessage, floor,
                    "PANEL WARNING", "AVERTISSEMENT PANNEAU",
                    "A warning marks the Door Open button as dangerous.", "Un avertissement marque le bouton Ouverture des portes comme dangereux.",
                    "Elevator.ButtonPanel", "Door Open"),
                Clue("f3-clue-note-still", CorridorClueType.WallMessage, floor,
                    "NOTE", "NOTE",
                    "A note says the safe instruction is to stay still.", "Une note dit que la consigne sûre est de rester immobile.",
                    "Corridor.Note", "Stay still"),
                Clue("f3-clue-red-button", CorridorClueType.ColorCue, floor,
                    "RED BUTTON", "BOUTON ROUGE",
                    "The alarm button glows red and is labelled ALARM.", "Le bouton d'alarme brille en rouge et porte la mention ALARME.",
                    "Elevator.ButtonPanel", "Alarm"),
                Clue("f3-clue-her", CorridorClueType.WallMessage, floor,
                    "WALL", "MUR",
                    "Fresh writing: DO NOT LOOK AT HER.", "Inscription fraîche : NE LA REGARDE PAS.",
                    "Corridor.WallMessage", "Look at her"),
            };

            var trials = new List<EvidenceTrial>
            {
                Trial("f3-trial-1", "f3-clue-wall-left",
                    "What did the wall say?", "Qu'a dit le mur ?", time, diff,
                    A("f3-trial-1-1", "Do not run", "Ne cours pas"),
                    A("f3-trial-1-2", "Do not look left", "Ne regarde pas à gauche", true),
                    A("f3-trial-1-3", "Do not answer", "Ne réponds pas"),
                    A("f3-trial-1-4", "Do not lie", "Ne mens pas")),
                Trial("f3-trial-2", "f3-clue-panel-warning",
                    "Which button should you avoid?", "Quel bouton faut-il éviter ?", time, diff,
                    A("f3-trial-2-1", "Alarm", "Alarme"),
                    A("f3-trial-2-2", "Door Open", "Ouverture portes", true),
                    A("f3-trial-2-3", "Floor 3", "Étage 3"),
                    A("f3-trial-2-4", "Light", "Lumière")),
                Trial("f3-trial-3", "f3-clue-note-still",
                    "Which instruction was safe?", "Quelle consigne était sûre ?", time, diff,
                    A("f3-trial-3-1", "Run", "Cours"),
                    A("f3-trial-3-2", "Stay still", "Reste immobile", true),
                    A("f3-trial-3-3", "Scream", "Crie"),
                    A("f3-trial-3-4", "Knock", "Frappe")),
                Trial("f3-trial-4", "f3-clue-red-button",
                    "Which button glowed red?", "Quel bouton brillait en rouge ?", time, diff,
                    A("f3-trial-4-1", "Alarm", "Alarme", true),
                    A("f3-trial-4-2", "Open", "Ouvrir"),
                    A("f3-trial-4-3", "Close", "Fermer"),
                    A("f3-trial-4-4", "Call", "Appel")),
                Trial("f3-trial-5", "f3-clue-her",
                    "What must you not do?", "Que ne dois-tu pas faire ?", time, diff,
                    A("f3-trial-5-1", "Breathe", "Respirer"),
                    A("f3-trial-5-2", "Blink", "Cligner"),
                    A("f3-trial-5-3", "Look at her", "La regarder", true),
                    A("f3-trial-5-4", "Wait", "Attendre")),
            };

            return new FloorObservationSet(floor, clues, trials);
        }

        // ---- Floor 2 — Audio proxy / codes --------------------------------------------

        private static FloorObservationSet BuildFloor2()
        {
            const int floor = 2;
            const float time = 5f;
            const int diff = 2;

            var clues = new List<CorridorClue>
            {
                Clue("f2-clue-intercom", CorridorClueType.AudioProxy, floor,
                    "INTERCOM", "INTERPHONE",
                    "The intercom display shows 2 · 7 · 2.", "L'afficheur de l'interphone montre 2 · 7 · 2.",
                    "Corridor.Intercom", "272"),
                Clue("f2-clue-scratched", CorridorClueType.ScratchedCode, floor,
                    "SCRATCHED CODE", "CODE GRAVÉ",
                    "914 is scratched into the wall.", "914 est gravé dans le mur.",
                    "Corridor.WallScratch", "914"),
                Clue("f2-clue-display", CorridorClueType.ScratchedCode, floor,
                    "DISPLAY CODE", "CODE AFFICHÉ",
                    "The display shows 358.", "L'afficheur montre 358.",
                    "Elevator.DigitalDisplay", "358"),
                Clue("f2-clue-tone", CorridorClueType.AudioProxy, floor,
                    "SPEAKER", "HAUT-PARLEUR",
                    "A low tone repeats from the speaker.", "Un son grave se répète dans le haut-parleur.",
                    "Corridor.Speaker", "Low"),
                Clue("f2-clue-red-digits", CorridorClueType.Anomaly, floor,
                    "RED DIGITS", "CHIFFRES ROUGES",
                    "The digits 06 flash red.", "Les chiffres 06 clignotent en rouge.",
                    "Elevator.DigitalDisplay", "06"),
            };

            var trials = new List<EvidenceTrial>
            {
                Trial("f2-trial-1", "f2-clue-intercom",
                    "What code came through the intercom?", "Quel code est passé par l'interphone ?", time, diff,
                    A("f2-trial-1-1", "272", "272", true),
                    A("f2-trial-1-2", "227", "227"),
                    A("f2-trial-1-3", "722", "722"),
                    A("f2-trial-1-4", "277", "277")),
                Trial("f2-trial-2", "f2-clue-scratched",
                    "Which code was scratched into the wall?", "Quel code était gravé dans le mur ?", time, diff,
                    A("f2-trial-2-1", "914", "914", true),
                    A("f2-trial-2-2", "941", "941"),
                    A("f2-trial-2-3", "491", "491"),
                    A("f2-trial-2-4", "149", "149")),
                Trial("f2-trial-3", "f2-clue-display",
                    "Which code appeared on the display?", "Quel code est apparu à l'écran ?", time, diff,
                    A("f2-trial-3-1", "358", "358", true),
                    A("f2-trial-3-2", "385", "385"),
                    A("f2-trial-3-3", "538", "538"),
                    A("f2-trial-3-4", "583", "583")),
                Trial("f2-trial-4", "f2-clue-tone",
                    "Which tone repeated?", "Quel son s'est répété ?", time, diff,
                    A("f2-trial-4-1", "Low", "Grave", true),
                    A("f2-trial-4-2", "High", "Aigu"),
                    A("f2-trial-4-3", "Mid", "Médium"),
                    A("f2-trial-4-4", "None", "Aucun")),
                Trial("f2-trial-5", "f2-clue-red-digits",
                    "Which digits flashed red?", "Quels chiffres clignotaient en rouge ?", time, diff,
                    A("f2-trial-5-1", "60", "60"),
                    A("f2-trial-5-2", "06", "06", true),
                    A("f2-trial-5-3", "66", "66"),
                    A("f2-trial-5-4", "00", "00")),
            };

            return new FloorObservationSet(floor, clues, trials);
        }

        // ---- Floor 1 — Final panic / sang-froid (last before escape) -------------------

        private static FloorObservationSet BuildFloor1()
        {
            const int floor = 1;
            const float time = 4f;
            const int diff = 3;

            var clues = new List<CorridorClue>
            {
                Clue("f1-clue-final-warning", CorridorClueType.WallMessage, floor,
                    "FINAL WARNING", "DERNIER AVERTISSEMENT",
                    "The wall says to answer calmly, do not open.", "Le mur dit de répondre calmement, ne pas ouvrir.",
                    "Corridor.WallMessage", "Answer calmly"),
                Clue("f1-clue-dark", CorridorClueType.WallMessage, floor,
                    "IN THE DARK", "DANS LE NOIR",
                    "A note: when the lights die, stay silent.", "Une note : quand les lumières meurent, reste silencieux.",
                    "Corridor.Note", "Stay silent"),
                Clue("f1-clue-whisper", CorridorClueType.AudioProxy, floor,
                    "WHISPER", "MURMURE",
                    "She whispers your name; the rule is do not answer.", "Elle murmure ton nom ; la règle est de ne pas répondre.",
                    "Corridor.Speaker", "Do not answer"),
                Clue("f1-clue-seal", CorridorClueType.WallMessage, floor,
                    "FINAL", "FIN",
                    "Last second: hold your breath while the doors seal.", "Dernière seconde : retiens ton souffle pendant que les portes se scellent.",
                    "Corridor.WallMessage", "Hold breath"),
                Clue("f1-clue-door-closed", CorridorClueType.DoorState, floor,
                    "HALL DOOR", "PORTE DU COULOIR",
                    "The last hall door is now fully closed.", "La dernière porte du couloir est maintenant fermée.",
                    "Corridor.Doors", "Closed"),
            };

            var trials = new List<EvidenceTrial>
            {
                Trial("f1-trial-1", "f1-clue-final-warning",
                    "She is at the door. What should you do?", "Elle est à la porte. Que dois-tu faire ?", time, diff,
                    A("f1-trial-1-1", "Hold the door", "Tenir la porte"),
                    A("f1-trial-1-2", "Answer calmly", "Répondre calmement", true),
                    A("f1-trial-1-3", "Open it", "Ouvrir"),
                    A("f1-trial-1-4", "Look closer", "Regarder de plus près")),
                Trial("f1-trial-2", "f1-clue-dark",
                    "The lights die. What do you do?", "Les lumières s'éteignent. Que fais-tu ?", time, diff,
                    A("f1-trial-2-1", "Scream", "Crier"),
                    A("f1-trial-2-2", "Stay silent", "Rester silencieux", true),
                    A("f1-trial-2-3", "Run out", "S'enfuir"),
                    A("f1-trial-2-4", "Knock back", "Frapper en retour")),
                Trial("f1-trial-3", "f1-clue-whisper",
                    "She whispers your name. What do you do?", "Elle murmure ton nom. Que fais-tu ?", time, diff,
                    A("f1-trial-3-1", "Answer", "Répondre"),
                    A("f1-trial-3-2", "Do not answer", "Ne pas répondre", true),
                    A("f1-trial-3-3", "Open door", "Ouvrir la porte"),
                    A("f1-trial-3-4", "Look", "Regarder")),
                Trial("f1-trial-4", "f1-clue-seal",
                    "The doors must seal. What do you do?", "Les portes doivent se sceller. Que fais-tu ?", time, diff,
                    A("f1-trial-4-1", "Hold breath", "Retenir son souffle", true),
                    A("f1-trial-4-2", "Panic", "Paniquer"),
                    A("f1-trial-4-3", "Force doors", "Forcer les portes"),
                    A("f1-trial-4-4", "Scream", "Crier")),
                Trial("f1-trial-5", "f1-clue-door-closed",
                    "What state was the hall door in?", "Dans quel état était la porte du couloir ?", time, diff,
                    A("f1-trial-5-1", "Open", "Ouverte"),
                    A("f1-trial-5-2", "Ajar", "Entrouverte"),
                    A("f1-trial-5-3", "Closed", "Fermée", true),
                    A("f1-trial-5-4", "Broken", "Cassée")),
            };

            return new FloorObservationSet(floor, clues, trials);
        }

        // ---- Authoring helpers ---------------------------------------------------------

        private static LocalizedText L(string english, string french) => new LocalizedText(english, french);

        private static CorridorClue Clue(
            string id, CorridorClueType type, int floor,
            string labelEn, string labelFr, string descEn, string descFr,
            string anchor, string evidence, int weight = 1)
        {
            return new CorridorClue(id, type, floor, L(labelEn, labelFr), L(descEn, descFr),
                anchor, evidence, weight, isRequiredForTrial: true);
        }

        private static EvidenceAnswerOption A(string id, string en, string fr, bool correct = false)
        {
            return new EvidenceAnswerOption(id, L(en, fr), correct);
        }

        private static EvidenceTrial Trial(
            string id, string clueId, string promptEn, string promptFr,
            float timeLimitSeconds, int difficulty, params EvidenceAnswerOption[] answers)
        {
            return new EvidenceTrial(id, clueId, L(promptEn, promptFr), answers, timeLimitSeconds, difficulty);
        }
    }
}
