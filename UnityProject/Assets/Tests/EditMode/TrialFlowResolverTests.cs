using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class TrialFlowResolverTests
    {
        [Test]
        public void NotFinalTrial_Alive_MovesToNextTrialSameFloor()
        {
            Assert.AreEqual(TrialResolution.NextTrialSameFloor,
                TrialFlowResolver.Resolve(isDead: false, isFinalTrialInFloor: false, isFinalFloor: false, doorSealReached: false));
            // Even on the final floor, a non-final trial just advances to the next trial,
            // regardless of the current Door Seal.
            Assert.AreEqual(TrialResolution.NextTrialSameFloor,
                TrialFlowResolver.Resolve(isDead: false, isFinalTrialInFloor: false, isFinalFloor: true, doorSealReached: true));
        }

        [Test]
        public void FinalTrial_NonFinalFloor_Sealed_ClearsFloor()
        {
            Assert.AreEqual(TrialResolution.FloorCleared,
                TrialFlowResolver.Resolve(isDead: false, isFinalTrialInFloor: true, isFinalFloor: false, doorSealReached: true));
        }

        [Test]
        public void FinalTrial_NonFinalFloor_NotSealed_Fails()
        {
            Assert.AreEqual(TrialResolution.SealFailed,
                TrialFlowResolver.Resolve(isDead: false, isFinalTrialInFloor: true, isFinalFloor: false, doorSealReached: false));
        }

        [Test]
        public void FinalTrial_FinalFloor_Sealed_Escapes()
        {
            Assert.AreEqual(TrialResolution.Escaped,
                TrialFlowResolver.Resolve(isDead: false, isFinalTrialInFloor: true, isFinalFloor: true, doorSealReached: true));
        }

        [Test]
        public void FinalTrial_FinalFloor_NotSealed_Fails()
        {
            Assert.AreEqual(TrialResolution.SealFailed,
                TrialFlowResolver.Resolve(isDead: false, isFinalTrialInFloor: true, isFinalFloor: true, doorSealReached: false));
        }

        [Test]
        public void Death_OverridesEveryProgression_IncludingDoorSeal()
        {
            Assert.AreEqual(TrialResolution.Lost,
                TrialFlowResolver.Resolve(isDead: true, isFinalTrialInFloor: false, isFinalFloor: false, doorSealReached: false));
            // Loss wins even on a final trial that had reached the seal.
            Assert.AreEqual(TrialResolution.Lost,
                TrialFlowResolver.Resolve(isDead: true, isFinalTrialInFloor: true, isFinalFloor: true, doorSealReached: true));
        }
    }
}
