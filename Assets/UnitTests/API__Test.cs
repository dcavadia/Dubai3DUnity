using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class API__Test
    {

        public string APIResult;
        public string UIResult;


        [OneTimeSetUp]
        public void LoadScene()
        {
            SceneManager.LoadScene("SampleScene");
        }

        [UnityTest]
        public IEnumerator API__GET__Test()
        {          
            GameObject Cam = GameObject.Find("Main Camera");

            yield return new WaitForSeconds(2f);

            Weather mainScript = Cam.GetComponent<Weather>();
            APIResult = mainScript.GetWeatherState(); 
            UIResult = mainScript.conditionUI.text;
            Assert.AreEqual(APIResult, UIResult);

        }
    }
}

