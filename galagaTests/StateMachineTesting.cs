using DIKUArcade.EventBus;
using DIKUArcade;
using galaga.GalagaStates;
using galaga;
using System.Collections.Generic;
using NUnit.Framework;

namespace galagaTests {
    [TestFixture]
    public class StateMachineTesting {
        private StateMachine stateMachine;
        private Window win;
        
        public GameEventBus<object> eventBus;

        [SetUp]
        public void InitiateStateMachine() {
            DIKUArcade.Window.CreateOpenGLContext();

            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType>() {
                GameEventType.GameStateEvent, // changes to the game state
                GameEventType.InputEvent
            });
    
            stateMachine = new StateMachine();

            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
        }

        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }

        [Test]
        public void TestEventGamePaused() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_PAUSED", ""));

            GalagaBus.GetBus().ProcessEventsSequentially();

            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
        }

        [Test]
        public void TestEventGameRunning() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_RUNNING", ""));

            GalagaBus.GetBus().ProcessEventsSequentially();

            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>());
        }

        [Test]
        public void TestEventGMainMenu() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_MAIN", ""));

            GalagaBus.GetBus().ProcessEventsSequentially();

            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }
    }
}
