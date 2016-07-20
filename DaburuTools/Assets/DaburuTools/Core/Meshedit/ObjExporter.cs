using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Text;

// Credits: Done by KeliHlodversson, extracted from http://wiki.unity3d.com/index.php/ObjExporter
public class ObjExporter {

    public static string MeshToString(MeshFilter mf) 
	{
		Mesh m = mf.sharedMesh;
        Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;

        StringBuilder sb = new StringBuilder();

        sb.Append("g ").Append(mf.name).Append("\n");
        foreach(Vector3 v in m.vertices) {
            sb.Append(string.Format("v {0} {1} {2}\n",v.x,v.y,v.z));
        }
        sb.Append("\n");
        foreach(Vector3 v in m.normals) {
            sb.Append(string.Format("vn {0} {1} {2}\n",v.x,v.y,v.z));
        }
        sb.Append("\n");
        foreach(Vector3 v in m.uv) {
            sb.Append(string.Format("vt {0} {1}\n",v.x,v.y));
        }
        for (int material=0; material < m.subMeshCount; material ++) {
            sb.Append("\n");
            sb.Append("usemtl ").Append(mats[material].name).Append("\n");
            sb.Append("usemap ").Append(mats[material].name).Append("\n");

            int[] triangles = m.GetTriangles(material);
            for (int i=0;i<triangles.Length;i+=3) {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                    triangles[i]+1, triangles[i+1]+1, triangles[i+2]+1));
            }
        }

        return sb.ToString();
    }

    public static void MeshToFile(MeshFilter mf, string filename) 
	{
		string newAssetPath = "Assets/" + filename + ".obj";

		if(File.Exists(newAssetPath) == false)	// Do not overwrite
		{
			using (StreamWriter sw = new StreamWriter(newAssetPath))
			{
				sw.Write(MeshToString(mf));
			}

			AssetDatabase.Refresh();

			// Highlight the asset.
			Selection.activeObject = (Object)AssetDatabase.LoadAssetAtPath(newAssetPath, typeof(Object));
			EditorGUIUtility.PingObject(Selection.activeObject);
		}
		else
		{
			Debug.Log ("D:");
		}
    }
}
