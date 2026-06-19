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
                TrialFlowResolver.Resolve(isDead: false, isFinalTrialInFloor: false, isFinalFloor: false));
            // Even on the final floor, a non-final trial just advances to the next trial.
            Assert.AreEqual(TrialResolution.NextTrialSameFloor,
                TrialFlowResolver.Resolve(isDead: false, isFinalTrialInFloor: false, isFinalFloor: true));
        }

        [Test]
        public void FinalTrial_NonFinalFloor_Alive_ClearsFloor()
        {
            Assert.AreEqual(TrialResolution.FloorCleared,
                TrialFlowResolver.Resolve(isDead: false, isFinalTrialInFloor: true, isFinalFloor: false));
        }

        [Test]
        public void FinalTrial_FinalFloor_Alive_Escapes()
        {
            Assert.AreEqual(TrialResolution.Escaped,
                TrialFlowResolver.Resolve(isDead: false, isFinalTrialInFloor: true, isFinalFloor: true));
        }

        [Test]
        public void Death_OverridesEveryProgression()
        {
            Assert.AreEqual(TrialResolution.Lost,
                TrialFlowResolver.Resolve(isDead: true, isFinalTrialInFloor: false, isFinalFloor: false));
            Assert.AreEqual(TrialResolution.Lost,
                TrialFlowResolver.Resolve(isDead: true, isFinalTrialInFloor: true, isFinalFloor: false));
            // Loss wins even on the final trial of the final floor.
            Assert.AreEqual(TrialResolution.Lost,
                TrialFlowResolver.Resolve(isDead: true, isFinalTrialInFloor: true, isFinalFloor: true));
        }
    }
}
