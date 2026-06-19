using System.Collections.Generic;
using NUnit.Framework;
using DontLetHerIn.Questions;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class PrototypeQuestionCueSetTests
    {
        [Test]
        public void BuildAll_ReturnsFiveCues()
        {
            IReadOnlyList<QuestionCue> cues = PrototypeQuestionCueSet.BuildAll();
            Assert.AreEqual(PrototypeQuestionCueSet.Count, cues.Count);
            Assert.AreEqual(5, cues.Count);
        }

        [Test]
        public void EveryQuestion_HasMatchingCue()
        {
            IReadOnlyDictionary<string, QuestionCue> cues = PrototypeQuestionCueSet.BuildById();
            foreach (QuestionData question in PrototypeQuestionSet.BuildAll())
            {
                Assert.IsTrue(cues.ContainsKey(question.Id),
                    $"Question '{question.Id}' should have a matching cue.");
            }
        }

        [Test]
        public void EveryCue_HasLabelAndAtLeastOneLine()
        {
            foreach (QuestionCue cue in PrototypeQuestionCueSet.BuildAll())
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(cue.Label),
                    $"Cue '{cue.QuestionId}' should have a label.");
                Assert.Greater(cue.Lines.Count, 0,
                    $"Cue '{cue.QuestionId}' should have at least one line.");
            }
        }

        [Test]
        public void Floor1Cue_ShowsBlinkingRoomNumber()
        {
            QuestionCue cue = PrototypeQuestionCueSet.BuildById()["floor-1-observation"];
            CollectionAssert.Contains((System.Collections.ICollection)cue.Lines, "104");
        }

        [Test]
        public void Floor2Cue_HighlightsKeyInTheCenter()
        {
            QuestionCue cue = PrototypeQuestionCueSet.BuildById()["floor-2-short-memory"];
            Assert.AreEqual(3, cue.Lines.Count, "Three symbols are expected.");
            Assert.IsTrue(cue.HasHighlight, "The center symbol should be highlighted.");
            Assert.AreEqual("Key", cue.Lines[cue.HighlightLineIndex]);
            Assert.AreEqual(1, cue.HighlightLineIndex, "Key should be the centered symbol.");
        }

        [Test]
        public void Floor3Cue_ShowsWallMessage()
        {
            QuestionCue cue = PrototypeQuestionCueSet.BuildById()["floor-3-environmental"];
            CollectionAssert.Contains((System.Collections.ICollection)cue.Lines, "DO NOT LOOK LEFT");
        }

        [Test]
        public void Floor4Cue_ShowsAudioProxy()
        {
            QuestionCue cue = PrototypeQuestionCueSet.BuildById()["floor-4-audio-clue"];
            CollectionAssert.Contains((System.Collections.ICollection)cue.Lines, "VOICE: 272");
        }

        [Test]
        public void Floor5Cue_ShowsConflictingInstructions()
        {
            QuestionCue cue = PrototypeQuestionCueSet.BuildById()["floor-5-sang-froid"];
            Assert.AreEqual(2, cue.Lines.Count, "Two conflicting orders are expected.");
            CollectionAssert.Contains((System.Collections.ICollection)cue.Lines, "ELEVATOR: PRESS EXIT NOW");
            CollectionAssert.Contains((System.Collections.ICollection)cue.Lines, "WALL: WAIT");
        }
    }
}
