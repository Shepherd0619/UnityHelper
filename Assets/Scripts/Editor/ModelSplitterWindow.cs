// ModelSplitterWindow
// 朱梓瑞 Shepherd0619
// 主要将模型的部件拆分成单个Prefab方便做换装
// 注意：最好让美术将东西拆成一个个FBX模型而不是让程序做这个事情，
// 这个脚本并不一定适用于所有的美术流程，仅供思路参考。
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace UnityHelper.Editor
{
	public class ModelSplitterWindow : EditorWindow
	{
		private GameObject model;

		[MenuItem("Shepherd0619/Model Splitter")]
		public static void ShowWindow()
		{
			GetWindow<ModelSplitterWindow>("Model Splitter");
		}

		private void OnGUI()
		{
			GUILayout.Label("Model Splitter", EditorStyles.boldLabel);
			GUILayout.Label("by Shepherd0619");

			GUILayout.Space(5);

			GUILayout.Label("It is better to let the artist break things into FBX models (with full armature inside) instead of letting the programmer do this.\n\n" +
							"This script may not be applicable to all art asset creation/maintain processes, it is for reference only.", EditorStyles.helpBox);

			model = EditorGUILayout.ObjectField("Model", model, typeof(GameObject), true) as GameObject;

			if (GUILayout.Button("Split Model"))
			{
				if (model != null)
				{
					SplitModel();
				}
				else
				{
					Debug.LogError("Please assign a model to split.");
				}
			}
		}

		private void SplitModel()
		{
			SkinnedMeshRenderer[] skinnedMeshRenderers = model.GetComponentsInChildren<SkinnedMeshRenderer>();
			if (skinnedMeshRenderers.Length == 0)
			{
				Debug.LogError("No SkinnedMeshRenderer found in the model.");
				return;
			}

			foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
			{
				GameObject root = new GameObject(renderer.gameObject.name);
				GameObject part = new GameObject(renderer.gameObject.name);
				part.transform.SetParent(root.transform);

				Transform hips = Instantiate(model.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Hips), root.transform);
				hips.gameObject.name = hips.gameObject.name.Replace("(Clone)", "").TrimEnd();
				SkinnedMeshRenderer newRenderer = part.AddComponent<SkinnedMeshRenderer>();
				newRenderer.sharedMesh = renderer.sharedMesh;
				newRenderer.sharedMaterials = renderer.sharedMaterials;
				newRenderer.rootBone = hips;
				newRenderer.bones = new Transform[renderer.bones.Length];
				Transform[] oldBones = hips.GetComponentsInChildren<Transform>();
				for (int i = 0; i < renderer.bones.Length; i++)
				{
					newRenderer.bones[i] = oldBones.First(search => search.gameObject.name == renderer.bones[i].gameObject.name.Replace("(Clone)", "").TrimEnd());
				}

				string folderPath = "Assets/Prefabs/" + model.name + "/" + root.name;
				if (!Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
				}

				PrefabUtility.SaveAsPrefabAsset(root, folderPath + "/" + root.name + ".prefab");
				DestroyImmediate(root);
			}
		}
	}
}
