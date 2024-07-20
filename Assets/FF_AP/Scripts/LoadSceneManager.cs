using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FF_ArApp
{
    public class LoadSceneManager : MonoBehaviour
    {
        [SerializeField] private Animator foreground;
        public void LoadMainScene()
        {
            this.foreground.SetTrigger("Hide");
            StartCoroutine(LoadDelay(0.5f, () => SceneManager.LoadScene("Main", LoadSceneMode.Single)));
        }
        private IEnumerator LoadDelay(float delayTime, Action loadAction)
        {
            yield return new WaitForSeconds(delayTime);
            loadAction?.Invoke();
        }
    }
}