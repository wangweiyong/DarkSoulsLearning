using UnityEditor;
using UnityEngine;

public class AssembleReloadUtils
{
    [InitializeOnLoad]
    internal class SetReloadAssemblies
    {
        private static bool LockReloadAssemblies = false;
        static SetReloadAssemblies()
        {
            LockReloadAssemblies = EditorPrefs.GetBool(KEY, false);
            Menu.SetChecked(MENUKEY, LockReloadAssemblies);
            if (LockReloadAssemblies)
            {
                EditorApplication.LockReloadAssemblies();
            }
            else
            {
                EditorApplication.UnlockReloadAssemblies();
            }
            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode && EditorPrefs.GetBool(KEY, false))
            {
                EditorApplication.isPlaying = false;
                Debug.LogWarning("重新加载程序集已被锁定。");
                EditorUtility.DisplayDialog("警告", "已锁定重新加载程序集，请注意！！！", "知道后果");
            }
        }

        private const string MENUKEY = "ToolKit/LockReloadAssemblies #R";
        private const string KEY = "LockReloadAssemblies";
        [MenuItem(MENUKEY, priority = int.MaxValue)]
        private static void SetLockReloadAssemblies()
        {
            if (LockReloadAssemblies)
            {
                Debug.Log("重新加载程序集已解锁。");
                EditorApplication.UnlockReloadAssemblies();
                LockReloadAssemblies = !LockReloadAssemblies;
                EditorPrefs.SetBool(KEY, false);
                Menu.SetChecked(MENUKEY, false);
            }
            else
            {
                if (EditorUtility.DisplayDialog("提示", "是否锁定 重新加载程序集 \n\n锁定以后无法重新加载程序集,\n也不会触发脚本编译。", "继续锁定", "取消"))
                {
                    Debug.Log("重新加载程序集已锁定。");
                    EditorApplication.LockReloadAssemblies();
                    LockReloadAssemblies = !LockReloadAssemblies;
                    EditorPrefs.SetBool(KEY, true);
                    Menu.SetChecked(MENUKEY, true);
                }
            }
        }
    }
}
