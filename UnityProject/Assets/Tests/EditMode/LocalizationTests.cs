using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class LocalizationTests
    {
        [TearDown]
        public void RestoreDefaultLanguage()
        {
            // Language is global static state; reset after each test.
            PrototypeLocalization.Language = PrototypeLocalization.DefaultLanguage;
        }

        [Test]
        public void DefaultLanguage_IsEnglish()
        {
            Assert.AreEqual(GameLanguage.English, PrototypeLocalization.DefaultLanguage);
            Assert.AreEqual(GameLanguage.English, PrototypeLocalization.Language);
        }

        [Test]
        public void LocalizedText_ReturnsPerLanguage()
        {
            var text = new LocalizedText("Hello", "Bonjour");
            Assert.AreEqual("Hello", text.Get(GameLanguage.English));
            Assert.AreEqual("Bonjour", text.Get(GameLanguage.French));
        }

        [Test]
        public void Language_CanBeSwitchedInCode()
        {
            PrototypeLocalization.Language = GameLanguage.French;
            Assert.AreEqual("COMMENCER LA DESCENTE", PrototypeLocalization.Current(PrototypeLocalization.BeginDescent));

            PrototypeLocalization.Language = GameLanguage.English;
            Assert.AreEqual("BEGIN DESCENT", PrototypeLocalization.Current(PrototypeLocalization.BeginDescent));
        }

        [Test]
        public void EnglishIntro_MentionsFifthFloorAndGroundFloor()
        {
            string intro = PrototypeLocalization.Intro.English;
            StringAssert.Contains("5th floor", intro);
            StringAssert.Contains("ground floor", intro);
            StringAssert.Contains("Do not let her in", intro);
        }

        [Test]
        public void FrenchIntro_MentionsFifthFloorAndGroundFloor()
        {
            string intro = PrototypeLocalization.Intro.French;
            StringAssert.Contains("5e étage", intro);
            StringAssert.Contains("rez-de-chaussée", intro);
            StringAssert.Contains("Ne la laisse pas entrer", intro);
        }

        [Test]
        public void TransitionLabels_HaveEnglishAndFrench()
        {
            Assert.AreEqual("DESCENDING", PrototypeLocalization.Descending.English);
            Assert.AreEqual("DESCENTE", PrototypeLocalization.Descending.French);
            Assert.AreEqual("DOORS CLOSING", PrototypeLocalization.DoorsClosing.English);
            Assert.AreEqual("PORTES EN FERMETURE", PrototypeLocalization.DoorsClosing.French);
            Assert.AreEqual("GROUND FLOOR", PrototypeLocalization.GroundFloor.English);
            Assert.AreEqual("REZ-DE-CHAUSSÉE", PrototypeLocalization.GroundFloor.French);
        }

        [Test]
        public void ResultAndFeedbackLabels_HaveBothLanguages()
        {
            Assert.AreEqual("YOU ESCAPED", PrototypeLocalization.YouEscaped.English);
            Assert.AreEqual("TU ES SORTI", PrototypeLocalization.YouEscaped.French);
            Assert.AreEqual("SHE GOT IN", PrototypeLocalization.SheGotIn.English);
            Assert.AreEqual("ELLE EST ENTRÉE", PrototypeLocalization.SheGotIn.French);
            Assert.AreEqual("WRONG — SHE MOVES", PrototypeLocalization.Wrong.English);
            Assert.AreEqual("FAUX — ELLE AVANCE", PrototypeLocalization.Wrong.French);
        }

        [Test]
        public void FloorAndTrialLabels_FormatPerLanguage()
        {
            PrototypeLocalization.Language = GameLanguage.English;
            Assert.AreEqual("FLOOR 5", PrototypeLocalization.FloorLabel(5));
            Assert.AreEqual("TRIAL 1 / 5", PrototypeLocalization.TrialLabel(1, 5));

            PrototypeLocalization.Language = GameLanguage.French;
            Assert.AreEqual("ÉTAGE 5", PrototypeLocalization.FloorLabel(5));
            Assert.AreEqual("ÉPREUVE 1 / 5", PrototypeLocalization.TrialLabel(1, 5));
        }

        [Test]
        public void OutcomeMessage_IsLocalizedAndNonEmptyForAllOutcomes()
        {
            foreach (AnswerOutcome outcome in System.Enum.GetValues(typeof(AnswerOutcome)))
            {
                Assert.IsFalse(string.IsNullOrEmpty(PrototypeLocalization.OutcomeMessage(outcome)),
                    $"Outcome {outcome} should have a message.");
            }
        }
    }
}
