namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Lightweight, central registry of the prototype's visible player-facing text in
    /// English and French (Phase 7B.4). No Unity Localization package, no asset pipeline:
    /// just pure <see cref="LocalizedText"/> entries plus a switchable current language,
    /// so the UI reads localized strings and tests can flip the language in code.
    /// Code identifiers stay English; only displayed strings are localized here.
    /// Question/answer/cue content remains English-only for now (see report).
    /// </summary>
    public static class PrototypeLocalization
    {
        /// <summary>Language used when none has been chosen.</summary>
        public const GameLanguage DefaultLanguage = GameLanguage.English;

        /// <summary>Current prototype language. Switchable from code/tests (no settings UI yet).</summary>
        public static GameLanguage Language { get; set; } = DefaultLanguage;

        // ---- Intro ----
        public static readonly LocalizedText Title =
            new LocalizedText("DON'T LET HER IN", "NE LA LAISSE PAS ENTRER");

        public static readonly LocalizedText Intro = new LocalizedText(
            "You wake up on the 5th floor.\n\n" +
            "The elevator is open.\nThe hallway should be empty.\n\n" +
            "It is not.\n\n" +
            "Answer the trials.\nDo not let her in.\nReach the ground floor.",
            "Tu te réveilles au 5e étage.\n\n" +
            "L’ascenseur est ouvert.\nLe couloir devrait être vide.\n\n" +
            "Il ne l’est pas.\n\n" +
            "Réponds aux épreuves.\nNe la laisse pas entrer.\nAtteins le rez-de-chaussée.");

        public static readonly LocalizedText BeginDescent =
            new LocalizedText("BEGIN DESCENT", "COMMENCER LA DESCENTE");

        // ---- Transition (descent) ----
        public static readonly LocalizedText FloorCleared =
            new LocalizedText("FLOOR CLEARED", "ÉTAGE FRANCHI");

        public static readonly LocalizedText DoorsClosing =
            new LocalizedText("DOORS CLOSING", "PORTES EN FERMETURE");

        public static readonly LocalizedText Descending =
            new LocalizedText("DESCENDING", "DESCENTE");

        // ---- Elevator descent transition (Phase 7I) ----
        public static readonly LocalizedText DoorsOpening =
            new LocalizedText("DOORS OPENING", "PORTES EN OUVERTURE");

        public static readonly LocalizedText GroundFloor =
            new LocalizedText("GROUND FLOOR", "REZ-DE-CHAUSSÉE");

        // ---- Corridor clue board (Phase 7G) ----
        public static readonly LocalizedText ObservedClues =
            new LocalizedText("OBSERVED CLUES", "INDICES OBSERVÉS");

        // ---- Observation pass (Phase 7H) ----
        public static readonly LocalizedText ObserveTitle =
            new LocalizedText("OBSERVE THE CORRIDOR", "OBSERVE LE COULOIR");

        public static readonly LocalizedText ObserveSubtitle =
            new LocalizedText("Look carefully. The answers are already here.",
                              "Regarde bien. Les réponses sont déjà là.");

        // ---- Result ----
        public static readonly LocalizedText YouEscaped =
            new LocalizedText("YOU ESCAPED", "TU ES SORTI");

        public static readonly LocalizedText SheGotIn =
            new LocalizedText("SHE GOT IN", "ELLE EST ENTRÉE");

        public static readonly LocalizedText WinSubtitle =
            new LocalizedText("The doors finally close. You reach the ground floor.",
                              "Les portes se ferment enfin. Tu atteins le rez-de-chaussée.");

        public static readonly LocalizedText LossSubtitleCaught =
            new LocalizedText("She reached the elevator.", "Elle a atteint l’ascenseur.");

        public static readonly LocalizedText Restart =
            new LocalizedText("RESTART", "RECOMMENCER");

        // ---- Outcome feedback ----
        public static readonly LocalizedText CorrectFast =
            new LocalizedText("FAST — KEEP MOVING", "VITE — CONTINUE");

        public static readonly LocalizedText CorrectNormal =
            new LocalizedText("CORRECT — KEEP MOVING", "CORRECT — CONTINUE");

        public static readonly LocalizedText CorrectSlow =
            new LocalizedText("TOO SLOW", "TROP LENT");

        public static readonly LocalizedText Wrong =
            new LocalizedText("WRONG — SHE MOVES", "FAUX — ELLE AVANCE");

        public static readonly LocalizedText Timeout =
            new LocalizedText("TOO LATE — SHE HEARD YOU", "TROP TARD — ELLE T’A ENTENDU");

        /// <summary>Resolve a localized entry in the current language.</summary>
        public static string Current(LocalizedText text) => text.Get(Language);

        // ---- Formatted labels ----

        /// <summary>e.g. "FLOOR 5" / "ÉTAGE 5".</summary>
        public static string FloorLabel(int floorNumber)
        {
            return Language == GameLanguage.French ? $"ÉTAGE {floorNumber}" : $"FLOOR {floorNumber}";
        }

        /// <summary>e.g. "TRIAL 1 / 5" / "ÉPREUVE 1 / 5".</summary>
        public static string TrialLabel(int current, int total)
        {
            return Language == GameLanguage.French
                ? $"ÉPREUVE {current} / {total}"
                : $"TRIAL {current} / {total}";
        }

        /// <summary>Combined HUD line, e.g. "FLOOR 5   —   TRIAL 1 / 5".</summary>
        public static string FloorAndTrial(int floorNumber, int trial, int totalTrials)
        {
            return $"{FloorLabel(floorNumber)}   —   {TrialLabel(trial, totalTrials)}";
        }

        /// <summary>Localized status message for a resolved answer outcome.</summary>
        public static string OutcomeMessage(AnswerOutcome outcome)
        {
            switch (outcome)
            {
                case AnswerOutcome.CorrectFast: return Current(CorrectFast);
                case AnswerOutcome.CorrectNormal: return Current(CorrectNormal);
                case AnswerOutcome.CorrectSlow: return Current(CorrectSlow);
                case AnswerOutcome.Wrong: return Current(Wrong);
                case AnswerOutcome.Timeout: return Current(Timeout);
                default: return string.Empty;
            }
        }
    }
}
