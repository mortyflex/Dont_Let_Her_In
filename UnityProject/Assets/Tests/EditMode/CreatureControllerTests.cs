using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using DontLetHerIn.Creature;

namespace DontLetHerIn.Tests.EditMode
{
    /// <summary>
    /// EditMode coverage for the minimal <see cref="CreatureController"/>.
    /// Anchors are private serialized fields with no public setter, so the move
    /// test injects one via reflection rather than authoring a scene.
    /// </summary>
    public sealed class CreatureControllerTests
    {
        private GameObject _creatureObject;

        [TearDown]
        public void TearDown()
        {
            if (_creatureObject != null)
            {
                Object.DestroyImmediate(_creatureObject);
            }
        }

        private CreatureController MakeController()
        {
            _creatureObject = new GameObject("CreatureUnderTest");
            return _creatureObject.AddComponent<CreatureController>();
        }

        [Test]
        public void ApplyDistance_UpdatesCurrentPhase()
        {
            CreatureController controller = MakeController();

            controller.ApplyDistance(90f);
            Assert.AreEqual(CreaturePhase.Far, controller.CurrentPhase);

            controller.ApplyDistance(50f);
            Assert.AreEqual(CreaturePhase.MidCorridor, controller.CurrentPhase);

            controller.ApplyDistance(0f);
            Assert.AreEqual(CreaturePhase.Attack, controller.CurrentPhase);
            Assert.AreEqual(0f, controller.CurrentDistance);
        }

        [Test]
        public void ApplyDistance_WithMissingAnchors_DoesNotThrow()
        {
            CreatureController controller = MakeController();
            Vector3 startPosition = controller.transform.position;

            // No anchors are assigned: the call must be safe and keep the position.
            Assert.DoesNotThrow(() => controller.ApplyDistance(5f));
            Assert.AreEqual(CreaturePhase.AtDoor, controller.CurrentPhase);
            Assert.AreEqual(startPosition, controller.transform.position);
        }

        [Test]
        public void ApplyDistance_WithAnchor_MovesToAnchor()
        {
            CreatureController controller = MakeController();

            var anchor = new GameObject("FarAnchor").transform;
            anchor.position = new Vector3(3f, 0f, 12f);
            SetPrivateAnchor(controller, "farAnchor", anchor);

            controller.ApplyDistance(95f); // Far phase -> snaps to farAnchor.

            Assert.AreEqual(CreaturePhase.Far, controller.CurrentPhase);
            Assert.AreEqual(anchor.position, controller.transform.position);

            Object.DestroyImmediate(anchor.gameObject);
        }

        [Test]
        public void PhaseChanged_FiresOnlyWhenPhaseChanges()
        {
            CreatureController controller = MakeController();
            int raisedCount = 0;
            controller.PhaseChanged += _ => raisedCount++;

            controller.ApplyDistance(90f); // Far (changes from default Far? default is Far)
            controller.ApplyDistance(85f); // still Far -> no event
            controller.ApplyDistance(50f); // MidCorridor -> event

            // Default phase is Far, so the first Far apply raises nothing; only MidCorridor does.
            Assert.AreEqual(1, raisedCount);
        }

        private static void SetPrivateAnchor(CreatureController controller, string fieldName, Transform value)
        {
            FieldInfo field = typeof(CreatureController).GetField(
                fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Expected private field '{fieldName}' on CreatureController.");
            field.SetValue(controller, value);
        }
    }
}
