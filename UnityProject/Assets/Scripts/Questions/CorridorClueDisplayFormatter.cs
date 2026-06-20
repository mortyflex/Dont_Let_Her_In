using System.Collections.Generic;
using System.Text;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Pure mapping/formatting for the static corridor clue board (Phase 7G). Reads the
    /// evidence data model (<see cref="PrototypeEvidenceFloorSet"/>) and produces, for a given
    /// displayed floor number (5..1), the localized clue lines the corridor board shows.
    ///
    /// No Unity dependency, so it stays fully testable in EditMode. The playable trials still
    /// come from <see cref="PrototypeFloorSet"/>; this formatter only supplies the visible
    /// "observed clues" board, which is consistent with the floor's playable content by theme
    /// and evidence value. Never returns null text.
    /// </summary>
    public static class CorridorClueDisplayFormatter
    {
        /// <summary>
        /// Build the clue display entries for a displayed floor number (5..1).
        /// Returns an empty list when the floor is unknown (safe fallback).
        /// </summary>
        public static IReadOnlyList<CorridorClueDisplayEntry> BuildEntries(int floorDisplayNumber)
        {
            var entries = new List<CorridorClueDisplayEntry>();
            foreach (FloorObservationSet floor in PrototypeEvidenceFloorSet.BuildAll())
            {
                if (floor == null || floor.FloorDisplayNumber != floorDisplayNumber) continue;

                foreach (CorridorClue clue in floor.Clues)
                {
                    if (clue == null) continue;
                    entries.Add(new CorridorClueDisplayEntry(clue.Id, clue.Type, clue.Label, clue.EvidenceValue));
                }
                break;
            }
            return entries;
        }

        /// <summary>Localized board header ("OBSERVED CLUES" / "INDICES OBSERVÉS").</summary>
        public static string Header(GameLanguage language) => PrototypeLocalization.ObservedClues.Get(language);

        /// <summary>
        /// Build the full localized board text for a floor: a header followed by one
        /// bulleted line per clue. Always returns a non-null, non-empty string (the header
        /// is present even when a floor has no clues).
        /// </summary>
        public static string BuildBoardText(int floorDisplayNumber, GameLanguage language)
        {
            var sb = new StringBuilder();
            sb.Append(Header(language));

            foreach (CorridorClueDisplayEntry entry in BuildEntries(floorDisplayNumber))
            {
                sb.Append('\n').Append("- ").Append(entry.GetLine(language));
            }
            return sb.ToString();
        }
    }
}
