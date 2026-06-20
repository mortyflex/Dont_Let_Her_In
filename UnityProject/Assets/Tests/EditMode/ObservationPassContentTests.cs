using System.Collections.Generic;
using NUnit.Framework;
using DontLetHerIn.GameLoop;
using DontLetHerIn.Questions;

namespace DontLetHerIn.Tests.EditMode
{
    /// <summary>
    /// Phase 7H: observation overlay localization (EN/FR) and confirmation that the descent
    /// content (5 floors, 5 trials each) is unchanged by the observation pass.
    /// </summary>
    public sealed class ObservationPassContentTests
    {
        [TearDown]
        public void RestoreDefaultLanguage()
        {
            PrototypeLocalization.Language = PrototypeLocalization.DefaultLanguage;
        }

        [Test]
        public void ObservationTitle_HasEnglishAndFrench()
        {
            Assert.AreEqual("OBSERVE THE CORRIDOR", PrototypeLocalization.ObserveTitle.English);
            Assert.AreEqual("OBSERVE LE COULOIR", PrototypeLocalization.ObserveTitle.French);
        }

        [Test]
        public void ObservationSubtitle_HasEnglishAndFrench()
        {
            Assert.AreEqual("Look carefully. The answers are already here.",
                PrototypeLocalization.ObserveSubtitle.English);
            Assert.AreEqual("Regarde bien. Les réponses sont déjà là.",
                PrototypeLocalization.ObserveSubtitle.French);
        }

        [Test]
        public void ObservationText_ResolvesPerCurrentLanguage()
        {
            PrototypeLocalization.Language = GameLanguage.French;
            Assert.AreEqual("OBSERVE LE COULOIR",
                PrototypeLocalization.Current(PrototypeLocalization.ObserveTitle));

            PrototypeLocalization.Language = GameLanguage.English;
            Assert.AreEqual("OBSERVE THE CORRIDOR",
                PrototypeLocalization.Current(PrototypeLocalization.ObserveTitle));
        }

        [Test]
        public void FloorCount_RemainsFive()
        {
            Assert.AreEqual(5, PrototypeFloorSet.BuildAll().Count);
        }

        [Test]
        public void TrialCount_RemainsFivePerFloor()
        {
            IReadOnlyList<FloorDefinition> floors = PrototypeFloorSet.BuildAll();
            foreach (FloorDefinition floor in floors)
            {
                Assert.AreEqual(5, floor.TrialCount, $"Floor {floor.FloorIndex} should keep 5 trials.");
            }
        }
    }
}
