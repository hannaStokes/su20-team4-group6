using NUnit.Framework;
using galaga.GalagaStates;

namespace galagaTests {
    public class Tests {
        //We will test all statements/branches
        [SetUp]
        public void Setup() {
        }

        // TransformStringToState testing
        [Test]
        public void StringToState1() {
            Assert.AreEqual(StateTransformer.TransformStringToState("GAME_RUNNING"), GameStateType.GameRunning);
        }
        
        [Test]
        public void StringToState2() {
            Assert.AreEqual(StateTransformer.TransformStringToState("GAME_PAUSED"), GameStateType.GamePaused);
        }

        [Test]
        public void StringToState3() {
           Assert.AreEqual(StateTransformer.TransformStringToState("GAME_MAIN"), GameStateType.MainMenu);
        }

        //TransformStateToString testing
        [Test]
        public void StateToString1() {
            Assert.AreEqual(StateTransformer.TransformStateToString(GameStateType.GameRunning), "GAME_RUNNING");
        }

        [Test]
        public void StateToString2() {
            Assert.AreEqual(StateTransformer.TransformStateToString(GameStateType.GamePaused), "GAME_PAUSED");
        }

        [Test]
        public void StateToString3() {
            Assert.AreEqual(StateTransformer.TransformStateToString(GameStateType.MainMenu), "GAME_MAIN");
        }
    }
}