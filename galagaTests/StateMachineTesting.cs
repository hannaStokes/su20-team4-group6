using DIKUArcade.EventBus;
using galaga.GalagaStates;
using galaga;
using System.Collections.Generic;
using NUnit.Framework;

namespace galagaTests {
    [TestFixture]
    public class StateMachineTesting {
        private StateMachine stateMachine;
        
        public GameEventBus<object> eventBus;

        [SetUp]
        public void InitiateStateMachine() {
            DIKUArcade.Window.CreateOpenGLContext();

            eventBus = GalagaBus.GetBus();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.GameStateEvent // changes to the game state
            });
    
            stateMachine = new StateMachine();

            eventBus.Subscribe(GameEventType.GameStateEvent, stateMachine);
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
    }
}
