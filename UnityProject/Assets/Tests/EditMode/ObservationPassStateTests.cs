using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class ObservationPassStateTests
    {
        [Test]
        public void NewState_IsInactive_AnswersAndTimerAllowed()
        {
            var state = new ObservationPassState();
            Assert.AreEqual(ObservationPhase.Inactive, state.Phase);
            Assert.IsFalse(state.IsObserving);
            Assert.IsTrue(state.AnswersAllowed);
            Assert.IsTrue(state.TimerAllowed);
        }

        [Test]
        public void Begin_EntersObserving_DisablesAnswersAndTimer()
        {
            var state = new ObservationPassState();
            Assert.IsTrue(state.Begin());
            Assert.IsTrue(state.IsObserving);
            // Answers must not be active during observation.
            Assert.IsFalse(state.AnswersAllowed);
            // Timer must not be active during observation.
            Assert.IsFalse(state.TimerAllowed);
        }

        [Test]
        public void Begin_WhileObserving_ReturnsFalse_NoDuplicatePass()
        {
            var state = new ObservationPassState();
            Assert.IsTrue(state.Begin());
            Assert.IsFalse(state.Begin());
            Assert.IsTrue(state.IsObserving);
        }

        [Test]
        public void Complete_QuestionBecomesActiveAgain()
        {
            var state = new ObservationPassState();
            state.Begin();
            state.Complete();
            Assert.AreEqual(ObservationPhase.Completed, state.Phase);
            Assert.IsFalse(state.IsObserving);
            // After observation, answers and timer are allowed (first trial can start).
            Assert.IsTrue(state.AnswersAllowed);
            Assert.IsTrue(state.TimerAllowed);
        }

        [Test]
        public void Restart_CanBeginObservationAgain()
        {
            var state = new ObservationPassState();
            state.Begin();
            state.Complete();
            // A new run / restart can observe again.
            Assert.IsTrue(state.Begin());
            Assert.IsTrue(state.IsObserving);
        }

        [Test]
        public void Reset_ReturnsToInactive_AndCanBeginAgain()
        {
            var state = new ObservationPassState();
            state.Begin();
            state.Reset();
            Assert.AreEqual(ObservationPhase.Inactive, state.Phase);
            Assert.IsTrue(state.Begin());
        }

        // ---- Phase 7H.1: clue board is observation-only -------------------

        [Test]
        public void CluesVisible_TrueOnlyWhileObserving()
        {
            var state = new ObservationPassState();
            // Inactive (question/idle phase) must not require the clue board.
            Assert.IsFalse(state.CluesVisible);

            // Clue board should be visible during observation.
            state.Begin();
            Assert.IsTrue(state.CluesVisible);
        }

        [Test]
        public void CluesVisible_HidesWhenTrialStarts()
        {
            var state = new ObservationPassState();
            state.Begin();
            Assert.IsTrue(state.CluesVisible);

            // Completing observation = the first trial starts: clues hide.
            state.Complete();
            Assert.IsFalse(state.CluesVisible);
        }

        [Test]
        public void CluesVisible_QuestionPhase_DoesNotRequireClueBoard()
        {
            var state = new ObservationPassState();
            state.Begin();
            state.Complete(); // question phase
            Assert.IsFalse(state.CluesVisible);
        }

        [Test]
        public void CluesVisible_Restart_CanShowClueBoardAgain()
        {
            var state = new ObservationPassState();
            state.Begin();
            state.Complete();
            Assert.IsFalse(state.CluesVisible);

            // New run / restart observation shows the board again.
            Assert.IsTrue(state.Begin());
            Assert.IsTrue(state.CluesVisible);
        }
    }
}
