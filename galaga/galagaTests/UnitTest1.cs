using NUnit.Framework;
using System;
using galaga.GalagaStates;

namespace galagaTests {
    public class Tests {

        //We will test all statements/branches
        [SetUp]
        public void Setup() {
        }

        // TransformStringToState testing
        [Test]
        public void TestPath0() {
            Assert.AreEqual(GameStateType.StateTransformer.TransformStringToState("GAME_RUNNING"), GameStateType.GameRunning);
        }
        [Test]
        public void TestPath1() {
            Assert.AreEqual(GameStateType.StateTransformer.TransformStringToState("GAME_PAUSED"), GameStateType.GamePaused);
        }
        [Test]
        public void TestPath2() {
           Assert.AreEqual(GameStateType.StateTransformer.TransformStringToState("GAME_MAIN"), GameStateType.MainMenu);
        }
        [Test]
        public void TestPath3() {
            Assert.AreEqual(GameStateType.StateTransformer.TransformStringToState("G"), GameStateType.GameRunning);
        }

        //TransformStateToString testing
        [Test]
        public void TestPath0() {
            Assert.AreEqual(GameStateType.StateTransformer.TransformStateToString(GameStateType.GameRunning), "GAME_RUNNING");
        }
        [Test]
        public void TestPath1() {
            Assert.AreEqual(GameStateType.StateTransformer.TransformStateToString(GameStateType.GamePaused), "GAME_PAUSED");
        }
        [Test]
        public void TestPath2() {
            Assert.AreEqual(GameStateType.StateTransformer.TransformStateToString(GameStateType.MainMenu), "GAME_MAIN");
        }
    }
}