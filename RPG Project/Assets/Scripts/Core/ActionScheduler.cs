using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction curAction;
        // Start is called before the first frame update
        public void StartAction(IAction action)
        {
            if (curAction == action) return;
            if (curAction != null)
            {
                curAction.Cancel();
            }
            curAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }

    }
}

