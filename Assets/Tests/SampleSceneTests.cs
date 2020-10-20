using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class SampleSceneTests : SnapshotTestCase
    {
        [SetUp]
        protected override void Setup()
        {
            RecordMode = false;
            base.Setup();
            SceneManager.LoadScene("SampleScene");
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator SampleScenePlayModeTestsWithEnumeratorPasses()
        {
            var cam = Object.FindObjectOfType<Camera>();
            if (cam == null)
            {
                Assert.Fail("Missing camera for graphic tests.");
            }

            var background = Object.FindObjectOfType<SpriteRenderer>();
            if (background == null)
            {
                Assert.Fail("Missing background for graphic tests.");
            }

            Assert.AreEqual(background.color, Color.white, "Needs to be white");

            var audioSource = Object.FindObjectOfType<AudioSource>();
            if (audioSource != null)
            {
                Assert.Fail("AudioSoruce already in Scene tests.");
            }
            
            yield return new WaitForSeconds(2);

            audioSource = GameObject.FindObjectOfType<AudioSource>();
            if (audioSource == null)
            {
                Assert.Fail("Missing AudioSoruce for tests.");
            }
        }
        
        
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator SampleSceneTestAudioSource()
        {
            var audioSource = GameObject.FindObjectOfType<AudioSource>();
            if (audioSource != null)
            {
                Assert.Fail("AudioSoruce already in Scene tests.");
            }
            
            yield return new WaitForSeconds(2);

            audioSource = GameObject.FindObjectOfType<AudioSource>();
            if (audioSource == null)
            {
                Assert.Fail("Missing AudioSoruce for tests.");
            }
        }

        [UnityTest]
        public IEnumerator TestScreenCaptureReadPixels()
        {
            // Wait for 4 frames
            for (int i = 0; i < 4; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            SnapshotVerifyView();
        }
        
        [UnityTest]
        public IEnumerator TestScreenCaptureReadPixelsAsAnother()
        {
            SceneManager.LoadScene("SampleScene");
            Time.timeScale = 20;
            //Wait for 2 Seconds
            yield return new WaitForSeconds(2);
            yield return new WaitForEndOfFrame();
            SnapshotVerifyView();
            Time.timeScale = 1;
        }
        
        [UnityTest]
        public IEnumerator TestScreenCaptureReadPixelsAfter11Minutes()
        {
            Time.timeScale = 100;
            //Wait for 2 Seconds
            yield return new WaitForSeconds(660);
            yield return new WaitForEndOfFrame();
            SnapshotVerifyView();
            Time.timeScale = 1;
        }
    }
}
