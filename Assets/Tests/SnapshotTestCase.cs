﻿using System.Diagnostics;
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
        protected int Width = 1920;
        protected int Height = 1080;

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
        
        protected void SnapshotVerifyView(string identifier = null, float errorTolerance = 0.1f)
        {
            // MUST GET THE FILE PATH HERE, NAME OF METHOD DEPENDS ON REFLECTION/CALL STACK.
            // IF YOU CALL IT FROM ANYWHERE ELSE, THE METHOD NAME WILL BE INCONSISTENT
            var filePath = GetFilePath(identifier);
            var imageBytes = ScreenCaptureInBytes();
            var existingImageInBytes = GetFileIfExists(filePath);

            if (RecordMode)
            {
                #if UNITY_EDITOR
                File.WriteAllBytes(filePath, imageBytes);
                AssetDatabase.Refresh();
                #endif
                Assert.Fail("Recording Image");
            }
            
            if (existingImageInBytes == null)
            {
                Assert.Fail("Image does not exist");
            }
            else if (existingImageInBytes.Length != imageBytes.Length)
            {
                Assert.Fail("Image resolution different");
            }
            else
            {
                float amountOfBytesEqual = 0;
                for (var i = 0; i < existingImageInBytes.Length; i++)
                {
                    if (existingImageInBytes[i] == imageBytes[i])
                    {
                        amountOfBytesEqual++;
                    }
                }

                var percentageOfError = 1 - (amountOfBytesEqual / existingImageInBytes.Length);
                Assert.LessOrEqual(percentageOfError, errorTolerance);
            }
            
            if (RecordMode)
            {
                Assert.Fail("Still Recording ScreenShots");
            }
        }
        
        #region Helper

        private void SetupDirectoryIfNeeded()
        {
            
            if (!Directory.Exists(GetDirectoryPath()))
            {
                Directory.CreateDirectory(GetDirectoryPath());
            }
        }
        
        private byte[] ScreenCaptureInBytes()
        {
            Texture2D screenImage = new Texture2D(Width, Height);
            //Get Image from screen
            /*
             * Issues - because different devices have different resolutions, you can't screenshot
             * larger than the resolution than your device. and the bytes would be different
             * Solve - How to take a screen shot on specific resolution.
             */
            screenImage.ReadPixels(new Rect(0, 0, Width, Height), 0, 0);
            screenImage.Apply();
            //Convert to png
            return screenImage.EncodeToPNG();
        }

        private byte[] GetFileIfExists(string path)
        {
            return File.Exists(path) ? File.ReadAllBytes(path) : null;
        }

        private string GetFilePath(string suffix = null)
        {
            var nameSuffix = "";
            if (suffix != null)
            {
                nameSuffix = "_" + suffix;
            }
            return Path + _directoryName +"/"+ GetCurrentMethodName() + nameSuffix + FileFormat;
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
