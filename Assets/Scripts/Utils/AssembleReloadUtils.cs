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
                Debug.LogWarning("���¼��س����ѱ�������");
                EditorUtility.DisplayDialog("����", "���������¼��س��򼯣���ע�⣡����", "֪�����");
            }
        }

        private const string MENUKEY = "ToolKit/LockReloadAssemblies #R";
        private const string KEY = "LockReloadAssemblies";
        [MenuItem(MENUKEY, priority = int.MaxValue)]
        private static void SetLockReloadAssemblies()
        {
            if (LockReloadAssemblies)
            {
                Debug.Log("���¼��س����ѽ�����");
                EditorApplication.UnlockReloadAssemblies();
                LockReloadAssemblies = !LockReloadAssemblies;
                EditorPrefs.SetBool(KEY, false);
                Menu.SetChecked(MENUKEY, false);
            }
            else
            {
                if (EditorUtility.DisplayDialog("��ʾ", "�Ƿ����� ���¼��س��� \n\n�����Ժ��޷����¼��س���,\nҲ���ᴥ���ű����롣", "��������", "ȡ��"))
                {
                    Debug.Log("���¼��س�����������");
                    EditorApplication.LockReloadAssemblies();
                    LockReloadAssemblies = !LockReloadAssemblies;
                    EditorPrefs.SetBool(KEY, true);
                    Menu.SetChecked(MENUKEY, true);
                }
            }
        }
    }
}
