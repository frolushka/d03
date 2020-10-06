using UnityEngine;
using UnityEngine.Events;

public class ShortcutManager : MonoBehaviour
{
    [System.Serializable]
    public class ShortcutItem
    {
        public KeyCode keyCode;
        public UnityEvent action;
    }

    [SerializeField] private ShortcutItem[] shortcuts;
    private void Update()
    {
        for (var i = 0; i < shortcuts.Length; i++)
        {
            if (Input.GetKeyDown(shortcuts[i].keyCode))
            {
                shortcuts[i].action?.Invoke();
            }
        }
    }
}
