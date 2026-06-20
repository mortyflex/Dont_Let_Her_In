using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    /// <summary>
    /// Phase 7I: elevator transition localization (EN/FR) and the rule that the transition runs
    /// only after a NON-final floor is cleared (the final Floor 1 escape shows the result instead).
    /// </summary>
    public sealed class ElevatorTransitionContentTests
    {
        [TearDown]
        public void RestoreDefaultLanguage()
        {
            PrototypeLocalization.Language = PrototypeLocalization.DefaultLanguage;
        }

        [Test]
        public void DoorsOpening_HasEnglishAndFrench()
        {
            Assert.AreEqual("DOORS OPENING", PrototypeLocalization.DoorsOpening.English);
            Assert.AreEqual("PORTES EN OUVERTURE", PrototypeLocalization.DoorsOpening.French);
        }

        [Test]
        public void ExistingTransitionLabels_AreReused_EN_FR()
        {
            // Phase 7I reuses these existing labels for the door/descent beats.
            Assert.AreEqual("DOORS CLOSING", PrototypeLocalization.DoorsClosing.English);
            Assert.AreEqual("PORTES EN FERMETURE", PrototypeLocalization.DoorsClosing.French);
            Assert.AreEqual("DESCENDING", PrototypeLocalization.Descending.English);
            Assert.AreEqual("DESCENTE", PrototypeLocalization.Descending.French);
            Assert.IsFalse(string.IsNullOrEmpty(PrototypeLocalization.FloorCleared.English));
            Assert.IsFalse(string.IsNullOrEmpty(PrototypeLocalization.FloorCleared.French));
        }

        [Test]
        public void DoorsOpening_ResolvesPerCurrentLanguage()
        {
            PrototypeLocalization.Language = GameLanguage.French;
            Assert.AreEqual("PORTES EN OUVERTURE", PrototypeLocalization.Current(PrototypeLocalization.DoorsOpening));
            PrototypeLocalization.Language = GameLanguage.English;
            Assert.AreEqual("DOORS OPENING", PrototypeLocalization.Current(PrototypeLocalization.DoorsOpening));
        }

        [Test]
        public void FloorLabel_FormatsPerLanguage()
        {
            PrototypeLocalization.Language = GameLanguage.English;
            Assert.AreEqual("FLOOR 4", PrototypeLocalization.FloorLabel(4));
            PrototypeLocalization.Language = GameLanguage.French;
            Assert.AreEqual("ÉTAGE 4", PrototypeLocalization.FloorLabel(4));
        }

        [Test]
        public void Transition_RunsOnly_AfterNonFinalFloorClear()
        {
            // Non-final floor, last trial survived -> FloorCleared -> elevator descent transition.
            Assert.AreEqual(TrialResolution.FloorCleared,
                TrialFlowResolver.Resolve(isDead: false, isFinalTrialInFloor: true, isFinalFloor: false));
        }

        [Test]
        public void Transition_DoesNotRun_AfterFinalFloorEscape()
        {
            // Final floor, last trial survived -> Escaped (ground floor) -> NO descent transition.
            Assert.AreEqual(TrialResolution.Escaped,
                TrialFlowResolver.Resolve(isDead: false, isFinalTrialInFloor: true, isFinalFloor: true));
            Assert.AreNotEqual(TrialResolution.FloorCleared,
                TrialFlowResolver.Resolve(isDead: false, isFinalTrialInFloor: true, isFinalFloor: true));
        }
    }
}
