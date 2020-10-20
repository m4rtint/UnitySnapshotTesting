using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class TimerSceneTests : SnapshotTestCase
    {
        [SetUp]
        protected override void Setup()
        {
            RecordMode = false;
            Width = 2560;
            Height = 1600;
            base.Setup();
            SceneManager.LoadScene("TimerScene");
        }

        [UnityTest]
        public IEnumerator TestTimerSceneInitial()
        {
            yield return new WaitForEndOfFrame();
            SnapshotVerifyView();
        }
        
        [UnityTest]
        public IEnumerator TestTimerSceneAfter10Minutes()
        {
            Time.timeScale = 100;
            // Wait for 4 frames
            for (int i = 0; i < 4; i++)
            {
                yield return null;
            }
            yield return new WaitForSeconds(610);
            yield return new WaitForEndOfFrame();
            SnapshotVerifyView();

            Time.timeScale = 1;
        }
    }
}
