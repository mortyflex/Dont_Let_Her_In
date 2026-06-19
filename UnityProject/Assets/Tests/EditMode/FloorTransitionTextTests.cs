using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class FloorTransitionTextTests
    {
        [Test]
        public void Titles_AreFloorFacing_NotEscape()
        {
            Assert.AreEqual("FLOOR CLEARED", FloorTransitionText.ClearedTitle);
            Assert.AreEqual("DOORS CLOSING", FloorTransitionText.DoorsClosingTitle);
            Assert.AreEqual("ASCENDING", FloorTransitionText.AscendingTitle);

            // A non-final floor clear must never read as the full escape.
            Assert.AreNotEqual("YOU ESCAPED", FloorTransitionText.ClearedTitle);
        }

        [Test]
        public void Subtitles_AreNonEmpty()
        {
            Assert.IsFalse(string.IsNullOrEmpty(FloorTransitionText.GetClearedSubtitle()));
            Assert.IsFalse(string.IsNullOrEmpty(FloorTransitionText.GetDoorsClosingSubtitle()));
        }

        [Test]
        public void AscendingSubtitle_RampsPerFloor()
        {
            Assert.AreEqual("The elevator climbs.", FloorTransitionText.GetAscendingSubtitle(2, 5));
            Assert.AreEqual("The lights flicker.", FloorTransitionText.GetAscendingSubtitle(3, 5));
            Assert.AreEqual("It is waiting above.", FloorTransitionText.GetAscendingSubtitle(4, 5));
        }

        [Test]
        public void AscendingSubtitle_FinalFloor_ReadsAsLastFloor()
        {
            string last = FloorTransitionText.GetAscendingSubtitle(5, 5);
            Assert.AreEqual("Last floor. Do not let her in.", last);
            // The mid-floor framing must differ from the final-floor framing.
            Assert.AreNotEqual(FloorTransitionText.GetAscendingSubtitle(2, 5), last);
        }

        [Test]
        public void AscendingSubtitle_NextFloorBeyondTotal_StillReadsAsLastFloor()
        {
            Assert.AreEqual("Last floor. Do not let her in.", FloorTransitionText.GetAscendingSubtitle(6, 5));
        }

        [Test]
        public void AscendingSubtitle_ZeroTotal_FallsBackWithoutThrowing()
        {
            // Defensive: totalFloors <= 0 must not be treated as "last floor".
            Assert.AreEqual("The elevator climbs.", FloorTransitionText.GetAscendingSubtitle(2, 0));
        }
    }
}
