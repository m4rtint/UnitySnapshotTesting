using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests
{
    /// <summary>
    /// Base Test Class
    /// - Place this inside the Test folder
    /// </summary>
    public class SnapshotTestCase
    {
        private const string Path = "Assets/Tests/ScreenShots/";
        private const string FileFormat = ".png";
        
        protected bool RecordMode = false;
        protected int Width = Screen.width;
        protected int Height = Screen.height;

        private string _directoryName = "GenericTests";
        
        [SetUp]
        protected virtual void Setup()
        {
            SetupCurrentClassName();
            if (RecordMode)
            {
                SetupDirectoryIfNeeded();
            }
        }
        
        protected void SnapshotVerifyView()
        {
            // MUST GET THE FILE PATH HERE, NAME OF METHOD DEPENDS ON REFLECTION/CALL STACK.
            // IF YOU CALL IT FROM ANYWHERE ELSE, THE METHOD NAME WILL BE INCONSISTENT
            var filePath = GetFilePath();
            var imageBytes = ScreenCaptureInBytes();
            var existingImageInBytes = GetFileIfExists(filePath);

            if (RecordMode)
            {
                #if UNITY_EDITOR
                System.IO.File.WriteAllBytes(filePath, imageBytes);
                AssetDatabase.Refresh();
                #endif
            }
            
            if (existingImageInBytes == null)
            {
                Assert.Fail("Image does not exist");
            }
            else
            {
                Assert.AreEqual(existingImageInBytes, imageBytes);
            }
            
            if (RecordMode)
            {
                Assert.Fail("Still Recording ScreenShots");
            }
        }
        
        #region Helper

        private void SetupDirectoryIfNeeded()
        {
            
            if (!System.IO.Directory.Exists(GetDirectoryPath()))
            {
                System.IO.Directory.CreateDirectory(GetDirectoryPath());
            }
        }
        private byte[] ScreenCaptureInBytes()
        {
            Texture2D screenImage = new Texture2D(Width, Height);
            //Get Image from screen
            screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenImage.Apply();
            //Convert to png
            return screenImage.EncodeToPNG();
        }

        private byte[] GetFileIfExists(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            return File.ReadAllBytes(path);
        }

        private string GetFilePath()
        {
            return Path + _directoryName +"/"+ GetCurrentMethodName() + FileFormat;
        }

        private string GetDirectoryPath()
        {
            return Path + _directoryName;
        }
        
        private string GetCurrentMethodName()
        {
            var st = new StackTrace();
            var methodName = st.GetFrame(3).GetMethod().DeclaringType.Name;
            int start = methodName.IndexOf("<") + 1;
            int end = methodName.IndexOf(">", start);
            string methodNameExtracted = methodName.Substring(start, end - start);
            return methodNameExtracted;
        }

        private void SetupCurrentClassName()
        {
            var st = new StackTrace();
            var className = st.GetFrame(2).GetMethod().ReflectedType.Name;
            _directoryName = className;
        }
        #endregion
    }
}
