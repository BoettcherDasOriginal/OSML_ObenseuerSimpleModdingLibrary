using System;
using System.Collections;
using UnityEngine;

namespace OSML
{
    public class SceneRuntimeObject : MonoBehaviour
    {
        private float _updateWaitTime = 5f;
        private bool _trigger = true;

        private void Update()
        {
            if (_trigger)
            {
                _trigger = false;
                StartCoroutine(updateWaitRoutine());
            }
        }

        IEnumerator updateWaitRoutine()
        {
            yield return new WaitForSeconds(_updateWaitTime);

            PublicVars.instance.firstUpdateFinished = true;
            Debug.Log("[OSML] SceneRuntimeObject finished first Update!");

            Notifications.instance.CreateNotification("OSML", "SceneRuntimeObject finished first Update!", false);
        }

        void OnDisable()
        {
            PublicVars.instance.firstUpdateFinished = false;
            Debug.Log("[OSML] SceneRuntimeObject disabled!");
        }
    }
}
