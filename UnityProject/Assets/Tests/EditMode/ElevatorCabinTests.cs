using NUnit.Framework;
using DontLetHerIn.GameLoop;
using DontLetHerIn.UI;

namespace DontLetHerIn.Tests.EditMode
{
    /// <summary>
    /// Phase 7I cabin frame: pure cabin data/format and the door framing rule that the doors
    /// only cover a central aperture (side cabin stays visible).
    /// </summary>
    public sealed class ElevatorCabinTests
    {
        [Test]
        public void FloorPlateText_FormatsFloors_5_to_1()
        {
            Assert.AreEqual("5", ElevatorCabin.FloorPlateText(5));
            Assert.AreEqual("4", ElevatorCabin.FloorPlateText(4));
            Assert.AreEqual("3", ElevatorCabin.FloorPlateText(3));
            Assert.AreEqual("2", ElevatorCabin.FloorPlateText(2));
            Assert.AreEqual("1", ElevatorCabin.FloorPlateText(1));
        }

        [Test]
        public void ButtonFloors_AreFiveDownToOne()
        {
            Assert.AreEqual(new[] { 5, 4, 3, 2, 1 }, ElevatorCabin.ButtonFloors);
        }

        [Test]
        public void GroundButton_HasLabel()
        {
            Assert.AreEqual("G", ElevatorCabin.GroundButtonLabel);
        }

        [Test]
        public void DoorApertureWidthRatio_IsCentralAperture_NotFullScreen()
        {
            float ratio = GameplayUIController.DoorApertureWidthRatio;
            // Doors cover only a central aperture (Phase 7I framing): not full screen.
            Assert.Less(ratio, 1.0f);
            // Adjusted target range for the cabin framing.
            Assert.GreaterOrEqual(ratio, 0.62f);
            Assert.LessOrEqual(ratio, 0.72f);
        }

        [Test]
        public void SideMargins_RemainVisible_AroundAperture()
        {
            float ratio = GameplayUIController.DoorApertureWidthRatio;
            float apertureLeft = 0.5f - ratio * 0.5f;
            float apertureRight = 0.5f + ratio * 0.5f;
            // There is a non-zero cabin margin on each side of the central aperture.
            Assert.Greater(apertureLeft, 0f);
            Assert.Less(apertureRight, 1f);
        }
    }
}
