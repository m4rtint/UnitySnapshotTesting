using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class MenuSceneTests : SnapshotTestCase
    {
        protected override void Setup()
        {
            RecordMode = false;
            base.Setup();
            SceneManager.LoadScene("MenuScene");
        }
        
        [UnityTest]
        public IEnumerator TestMenuSceneInitialStartup()
        {
            // Wait for load
            yield return new WaitForEndOfFrame();
            SnapshotVerifyView();
        }
        
        [UnityTest]
        public IEnumerator TestMenuSceneOptionButtonHighlight()
        {
            // Wait for load
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(GameObject.Find("Option"));
            for (int i = 0; i < 4; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            SnapshotVerifyView();
        }

    }
}
