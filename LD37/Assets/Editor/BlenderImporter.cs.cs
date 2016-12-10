using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
public class BlenderImporter : AssetPostprocessor{
	public void OnPreprocessModel (){
		UnityEngine.Debug.Log ("hey man");
		if(assetPath.EndsWith(".blend1")){
			string path = Directory.GetParent(Application.dataPath).ToString();
			string fbx = path + "/" + Path.GetDirectoryName(assetPath) + "/" + Path.GetFileNameWithoutExtension(assetPath) + ".fbx";

			ProcessStartInfo psi = new ProcessStartInfo();
			psi.FileName = "blender";
			psi.UseShellExecute = false;
			psi.RedirectStandardOutput = true;
			psi.Arguments = " --background /home/linkan/Programmering/LD37/blender_files/" + Path.GetFileNameWithoutExtension(assetPath) + ".blend" + " --python-expr 'import bpy; bpy.ops.export_scene.fbx(filepath="+'"'+fbx+'"'+",use_selection=False,use_mesh_modifiers=True)'";
			Process p = Process.Start(psi);
			string strOutput = p.StandardOutput.ReadToEnd();
			p.WaitForExit();
			UnityEngine.Debug.Log(strOutput);
			AssetDatabase.Refresh();
		}
	}
}