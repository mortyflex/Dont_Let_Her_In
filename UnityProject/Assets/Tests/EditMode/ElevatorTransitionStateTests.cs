using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class ElevatorTransitionStateTests
    {
        [Test]
        public void NewState_IsInactive_DoorsOpen_AnswersAndTimerAllowed()
        {
            var state = new ElevatorTransitionState();
            Assert.AreEqual(ElevatorTransitionPhase.Inactive, state.Phase);
            Assert.IsFalse(state.IsActive);
            Assert.IsTrue(state.DoorsOpen);
            Assert.IsTrue(state.AnswersAllowed);
            Assert.IsTrue(state.TimerAllowed);
            Assert.IsFalse(state.CreatureHidden);
        }

        [Test]
        public void Begin_ClosesDoors_GatesAnswersTimerCluesCreature()
        {
            var state = new ElevatorTransitionState();
            Assert.IsTrue(state.Begin());
            Assert.AreEqual(ElevatorTransitionPhase.DoorsClosing, state.Phase);
            Assert.IsTrue(state.IsActive);
            // Answers must not be active during the transition.
            Assert.IsFalse(state.AnswersAllowed);
            // Timer must not be active during the transition.
            Assert.IsFalse(state.TimerAllowed);
            // Clue board must not be visible during the transition.
            Assert.IsFalse(state.ClueBoardVisible);
            // Creature must be hidden during the transition.
            Assert.IsTrue(state.CreatureHidden);
        }

        [Test]
        public void Begin_WhileActive_ReturnsFalse_NoDuplicate()
        {
            var state = new ElevatorTransitionState();
            Assert.IsTrue(state.Begin());
            Assert.IsFalse(state.Begin());
            Assert.AreEqual(ElevatorTransitionPhase.DoorsClosing, state.Phase);
        }

        [Test]
        public void EnterDescending_IsActive_AndStillGated()
        {
            var state = new ElevatorTransitionState();
            state.Begin();
            state.EnterDescending();
            Assert.AreEqual(ElevatorTransitionPhase.Descending, state.Phase);
            Assert.IsTrue(state.IsActive);
            Assert.IsTrue(state.CreatureHidden);
            Assert.IsFalse(state.AnswersAllowed);
        }

        [Test]
        public void BeginOpening_IsActive()
        {
            var state = new ElevatorTransitionState();
            state.Begin();
            state.EnterDescending();
            state.BeginOpening();
            Assert.AreEqual(ElevatorTransitionPhase.DoorsOpening, state.Phase);
            Assert.IsTrue(state.IsActive);
        }

        [Test]
        public void Complete_ReopensDoors_AnswersAndTimerAllowedAgain()
        {
            var state = new ElevatorTransitionState();
            state.Begin();
            state.EnterDescending();
            state.BeginOpening();
            state.Complete();
            Assert.AreEqual(ElevatorTransitionPhase.Completed, state.Phase);
            Assert.IsFalse(state.IsActive);
            Assert.IsTrue(state.DoorsOpen);
            Assert.IsTrue(state.AnswersAllowed);
            Assert.IsTrue(state.TimerAllowed);
            Assert.IsFalse(state.CreatureHidden);
        }

        [Test]
        public void Reset_ReturnsToInactive_AndCanBeginAgain()
        {
            var state = new ElevatorTransitionState();
            state.Begin();
            state.Reset();
            Assert.AreEqual(ElevatorTransitionPhase.Inactive, state.Phase);
            Assert.IsTrue(state.Begin());
        }
    }
}
