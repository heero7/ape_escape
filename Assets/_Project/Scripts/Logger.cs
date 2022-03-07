using UnityEngine;

namespace ApeEscape
{
    public class Logger : MonoBehaviour
    {
        [Header("Settings")] 
        
        [SerializeField] private bool showLogs;
        [SerializeField] private string prefix = default!;
        [SerializeField] private Color prefixColor;

        public void Log(object message, UnityEngine.Object sender)
        {
            if (showLogs) return;
            
            Debug.Log($"<color={prefixColor}>{prefix}: {message}", sender);
        }
    }
}