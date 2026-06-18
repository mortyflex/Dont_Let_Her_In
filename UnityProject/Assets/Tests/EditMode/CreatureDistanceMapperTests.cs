using NUnit.Framework;
using UnityEngine;
using DontLetHerIn.Creature;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class CreatureDistanceMapperTests
    {
        // ---- Default-threshold boundary coverage -----------------------------
        // Mapping: >80 Far, >60 Visible, >40 MidCorridor, >25 NearDoor, >0 AtDoor, <=0 Attack.

        [Test]
        public void Distance100_ReturnsFarPhase()
        {
            Assert.AreEqual(CreaturePhase.Far, CreatureDistanceMapper.GetPhase(100f));
        }

        [Test]
        public void Distance81_ReturnsFarPhase()
        {
            Assert.AreEqual(CreaturePhase.Far, CreatureDistanceMapper.GetPhase(81f));
        }

        [Test]
        public void Distance80_ReturnsVisiblePhase()
        {
            Assert.AreEqual(CreaturePhase.Visible, CreatureDistanceMapper.GetPhase(80f));
        }

        [Test]
        public void Distance61_ReturnsVisiblePhase()
        {
            Assert.AreEqual(CreaturePhase.Visible, CreatureDistanceMapper.GetPhase(61f));
        }

        [Test]
        public void Distance60_ReturnsMidCorridorPhase()
        {
            Assert.AreEqual(CreaturePhase.MidCorridor, CreatureDistanceMapper.GetPhase(60f));
        }

        [Test]
        public void Distance41_ReturnsMidCorridorPhase()
        {
            Assert.AreEqual(CreaturePhase.MidCorridor, CreatureDistanceMapper.GetPhase(41f));
        }

        [Test]
        public void Distance40_ReturnsNearDoorPhase()
        {
            Assert.AreEqual(CreaturePhase.NearDoor, CreatureDistanceMapper.GetPhase(40f));
        }

        [Test]
        public void Distance26_ReturnsNearDoorPhase()
        {
            Assert.AreEqual(CreaturePhase.NearDoor, CreatureDistanceMapper.GetPhase(26f));
        }

        [Test]
        public void Distance25_ReturnsAtDoorPhase()
        {
            Assert.AreEqual(CreaturePhase.AtDoor, CreatureDistanceMapper.GetPhase(25f));
        }

        [Test]
        public void Distance1_ReturnsAtDoorPhase()
        {
            Assert.AreEqual(CreaturePhase.AtDoor, CreatureDistanceMapper.GetPhase(1f));
        }

        [Test]
        public void Distance0_ReturnsAttackPhase()
        {
            Assert.AreEqual(CreaturePhase.Attack, CreatureDistanceMapper.GetPhase(0f));
        }

        [Test]
        public void NegativeDistance_ReturnsAttackPhase()
        {
            Assert.AreEqual(CreaturePhase.Attack, CreatureDistanceMapper.GetPhase(-15f));
        }

        [Test]
        public void DistanceAboveMaximum_ReturnsFarPhase()
        {
            // Over-100 input is handled safely and still maps to the farthest phase.
            Assert.AreEqual(CreaturePhase.Far, CreatureDistanceMapper.GetPhase(150f));
        }

        // ---- CreatureData threshold handling ---------------------------------

        [Test]
        public void CustomCreatureDataThresholds_AreUsed()
        {
            // Compressed thresholds: a distance of 55 is Far here but Visible by default.
            var data = CreatureData.Create(
                farThreshold: 50f,
                visibleThreshold: 35f,
                midCorridorThreshold: 20f,
                nearDoorThreshold: 10f,
                atDoorThreshold: 0f);

            Assert.AreEqual(CreaturePhase.Far, CreatureDistanceMapper.GetPhase(55f, data));        // > 50
            Assert.AreEqual(CreaturePhase.Visible, CreatureDistanceMapper.GetPhase(40f, data));    // > 35
            Assert.AreEqual(CreaturePhase.MidCorridor, CreatureDistanceMapper.GetPhase(25f, data)); // > 20
            Assert.AreEqual(CreaturePhase.NearDoor, CreatureDistanceMapper.GetPhase(15f, data));    // > 10
            Assert.AreEqual(CreaturePhase.AtDoor, CreatureDistanceMapper.GetPhase(5f, data));       // > 0
            Assert.AreEqual(CreaturePhase.Attack, CreatureDistanceMapper.GetPhase(0f, data));       // <= 0

            Object.DestroyImmediate(data);
        }

        [Test]
        public void NullCreatureData_UsesDefaultThresholds()
        {
            // A null reference must not throw and must fall back to the default mapping.
            Assert.AreEqual(CreaturePhase.Far, CreatureDistanceMapper.GetPhase(81f, null));
            Assert.AreEqual(CreaturePhase.Visible, CreatureDistanceMapper.GetPhase(80f, null));
            Assert.AreEqual(CreaturePhase.Attack, CreatureDistanceMapper.GetPhase(0f, null));
        }

        [Test]
        public void NaNDistance_IsHandledSafelyAsFar()
        {
            // Invalid input must never trigger a false death (Attack).
            Assert.AreEqual(CreaturePhase.Far, CreatureDistanceMapper.GetPhase(float.NaN));
        }
    }
}
