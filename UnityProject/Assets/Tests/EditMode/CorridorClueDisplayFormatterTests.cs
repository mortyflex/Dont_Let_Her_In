using System.Collections.Generic;
using NUnit.Framework;
using DontLetHerIn.GameLoop;
using DontLetHerIn.Questions;

namespace DontLetHerIn.Tests.EditMode
{
    /// <summary>
    /// Phase 7G: the static corridor clue board maps the current displayed floor (5..1) to
    /// 5 clue display entries sourced from PrototypeEvidenceFloorSet, localized EN/FR, with a
    /// safe fallback for unknown floors. Pure formatter logic (no Unity scene needed).
    /// </summary>
    public sealed class CorridorClueDisplayFormatterTests
    {
        [SetUp]
        public void SetEnglishDefault()
        {
            PrototypeLocalization.Language = GameLanguage.English;
        }

        [TearDown]
        public void RestoreDefaultLanguage()
        {
            PrototypeLocalization.Language = PrototypeLocalization.DefaultLanguage;
        }

        [TestCase(5)]
        [TestCase(4)]
        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        public void CurrentFloor_MapsToFiveClueEntries(int floorDisplayNumber)
        {
            IReadOnlyList<CorridorClueDisplayEntry> entries =
                CorridorClueDisplayFormatter.BuildEntries(floorDisplayNumber);
            Assert.AreEqual(5, entries.Count, $"Floor {floorDisplayNumber} should expose 5 clue entries.");
        }

        [Test]
        public void Entries_UsePrototypeEvidenceFloorSetData()
        {
            // Floor 5, first clue is the room-number evidence (104) from PrototypeEvidenceFloorSet.
            CorridorClueDisplayEntry first = CorridorClueDisplayFormatter.BuildEntries(5)[0];
            Assert.AreEqual("f5-clue-room", first.ClueId);
            Assert.AreEqual(CorridorClueType.DoorNumber, first.Type);
            Assert.AreEqual("104", first.EvidenceValue);
            Assert.AreEqual("ROOM DISPLAY", first.GetLabel(GameLanguage.English));
        }

        [Test]
        public void AllPrototypeFloors_HaveClueEntries()
        {
            foreach (int floor in new[] { 5, 4, 3, 2, 1 })
            {
                Assert.Greater(CorridorClueDisplayFormatter.BuildEntries(floor).Count, 0,
                    $"Floor {floor} should have clue entries.");
            }
        }

        [Test]
        public void EnglishBoard_ReturnsEnglish()
        {
            PrototypeLocalization.Language = GameLanguage.English;
            string board = CorridorClueDisplayFormatter.BuildBoardText(5, GameLanguage.English);

            StringAssert.Contains("OBSERVED CLUES", board);
            StringAssert.Contains("ROOM DISPLAY", board);
            StringAssert.Contains("104", board);
        }

        [Test]
        public void FrenchBoard_ReturnsFrench()
        {
            PrototypeLocalization.Language = GameLanguage.French;
            string board = CorridorClueDisplayFormatter.BuildBoardText(5, GameLanguage.French);

            StringAssert.Contains("INDICES OBSERVÉS", board);
            StringAssert.Contains("NUMÉRO DE PORTE", board);
            StringAssert.Contains("104", board);
        }

        [Test]
        public void Header_IsLocalized()
        {
            Assert.AreEqual("OBSERVED CLUES", CorridorClueDisplayFormatter.Header(GameLanguage.English));
            Assert.AreEqual("INDICES OBSERVÉS", CorridorClueDisplayFormatter.Header(GameLanguage.French));
        }

        [Test]
        public void Entry_GetLine_CombinesLabelAndEvidence()
        {
            CorridorClueDisplayEntry first = CorridorClueDisplayFormatter.BuildEntries(5)[0];
            Assert.AreEqual("ROOM DISPLAY: 104", first.GetLine(GameLanguage.English));
            Assert.AreEqual("NUMÉRO DE PORTE: 104", first.GetLine(GameLanguage.French));
        }

        [Test]
        public void SwitchingLanguage_DoesNotChangeClueCount()
        {
            PrototypeLocalization.Language = GameLanguage.English;
            int en = CorridorClueDisplayFormatter.BuildEntries(5).Count;

            PrototypeLocalization.Language = GameLanguage.French;
            int fr = CorridorClueDisplayFormatter.BuildEntries(5).Count;

            Assert.AreEqual(en, fr);
            Assert.AreEqual(5, fr);
        }

        [Test]
        public void MissingFloor_ReturnsEmptyEntries_AndSafeBoardText()
        {
            Assert.AreEqual(0, CorridorClueDisplayFormatter.BuildEntries(99).Count);

            string board = CorridorClueDisplayFormatter.BuildBoardText(99, GameLanguage.English);
            Assert.IsNotNull(board);
            Assert.AreEqual("OBSERVED CLUES", board); // header only, no clue lines
        }

        [Test]
        public void BoardText_IsNeverNull_ForEveryFloorAndLanguage()
        {
            foreach (GameLanguage language in new[] { GameLanguage.English, GameLanguage.French })
            {
                foreach (int floor in new[] { 5, 4, 3, 2, 1, 0, 99 })
                {
                    string board = CorridorClueDisplayFormatter.BuildBoardText(floor, language);
                    Assert.IsNotNull(board, $"Board text null for floor {floor} / {language}.");
                    Assert.IsNotEmpty(board, $"Board text empty for floor {floor} / {language}.");
                }
            }
        }

        [Test]
        public void Entries_AreNeverNull_AndHaveNonNullLines()
        {
            foreach (int floor in new[] { 5, 4, 3, 2, 1 })
            {
                foreach (CorridorClueDisplayEntry entry in CorridorClueDisplayFormatter.BuildEntries(floor))
                {
                    Assert.IsNotNull(entry);
                    Assert.IsNotNull(entry.GetLine(GameLanguage.English));
                    Assert.IsNotNull(entry.GetLine(GameLanguage.French));
                    Assert.IsNotEmpty(entry.GetLine(GameLanguage.English));
                }
            }
        }
    }
}
