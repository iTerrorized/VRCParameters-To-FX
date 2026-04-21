using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Terrorized.Tools
{
    public class VRCParametersToFXWindow : EditorWindow
    {
        private VRCExpressionParameters _vrcParams;
        private AnimatorController _fxController;
        private string _statusMessage = "";
        private MessageType _statusType = MessageType.None;

        [MenuItem("Terrorized/Tools/VRCParameters To FX")]
        public static void Open()
        {
            var window = GetWindow<VRCParametersToFXWindow>("VRCParameters To FX");
            window.minSize = new Vector2(420, 170);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(8);
            EditorGUILayout.LabelField("VRCParameters → FX Controller", EditorStyles.boldLabel);
            EditorGUILayout.Space(4);

            _vrcParams = (VRCExpressionParameters)EditorGUILayout.ObjectField(
                "VRC Parameters", _vrcParams, typeof(VRCExpressionParameters), false);

            _fxController = (AnimatorController)EditorGUILayout.ObjectField(
                "FX Controller", _fxController, typeof(AnimatorController), false);

            EditorGUILayout.Space(10);

            using (new EditorGUI.DisabledScope(_vrcParams == null || _fxController == null))
            {
                if (GUILayout.Button("Add Parameters to FX", GUILayout.Height(30)))
                    RunSync();
            }

            if (!string.IsNullOrEmpty(_statusMessage))
            {
                EditorGUILayout.Space(4);
                EditorGUILayout.HelpBox(_statusMessage, _statusType);
            }
        }

        private void RunSync()
        {
            var existingIndex = new Dictionary<string, int>();
            var paramsList    = new List<AnimatorControllerParameter>(_fxController.parameters);
            for (int i = 0; i < paramsList.Count; i++)
                existingIndex[paramsList[i].name] = i;

            int added = 0, updated = 0;

            foreach (var param in _vrcParams.parameters)
            {
                if (param.name.Contains("-----"))
                    continue;

                var type = MapType(param.valueType);
                var entry = new AnimatorControllerParameter
                {
                    name         = param.name,
                    type         = type,
                    defaultFloat = param.defaultValue,
                    defaultInt   = (int)param.defaultValue,
                    defaultBool  = param.defaultValue != 0f
                };

                if (existingIndex.TryGetValue(param.name, out int idx))
                {
                    paramsList[idx] = entry;
                    updated++;
                }
                else
                {
                    existingIndex[param.name] = paramsList.Count;
                    paramsList.Add(entry);
                    added++;
                }
            }

            _fxController.parameters = paramsList.ToArray();

            EditorUtility.SetDirty(_fxController);
            AssetDatabase.SaveAssets();

            _statusMessage = $"Done — added {added}, updated {updated}.";
            _statusType    = MessageType.Info;
        }

        private static AnimatorControllerParameterType MapType(VRCExpressionParameters.ValueType valueType) =>
            valueType switch
            {
                VRCExpressionParameters.ValueType.Int   => AnimatorControllerParameterType.Int,
                VRCExpressionParameters.ValueType.Float => AnimatorControllerParameterType.Float,
                VRCExpressionParameters.ValueType.Bool  => AnimatorControllerParameterType.Bool,
                _                                       => AnimatorControllerParameterType.Float
            };
    }
}
