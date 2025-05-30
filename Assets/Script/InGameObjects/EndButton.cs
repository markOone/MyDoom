using MyDoom.InGameObjects;
using MyDoom.GeneralSystems;
using UnityEngine;

namespace Script.InGameObjects
{
    public class EndButton : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            GameManager.Instance.levelEndScreenManager.ShowLevelEndScreen();
        }
    }
}