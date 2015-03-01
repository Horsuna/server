//
//  ObjExporter.cs
//
//  Author:
//       Paál Gyula <paalgyula@gmail.com>
//
//  Copyright (c) 2015 (c) GW-Systems Kft. All Rights Reserved!
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//------------------------------------------------------------------------------
// <auto-generated>
//     Ezt a kódot eszköz generálta.
//     Futásidejű verzió:4.0.30319.0
//
//     Ennek a fájlnak a módosítása helytelen viselkedést eredményezhet, és elvész, ha
//     a kódot újragenerálják.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	struct ObjMaterial
	{
		public string name;
		public string textureName;
	}

	public class ObjExporter {
	
		public const String TARGET_DIR = "target/";

		private static int vertexOffset = 0;
		private static int normalOffset = 0;
		private static int uvOffset = 0;
		
		
		//User should probably be able to change this. It is currently left as an excercise for
		//the reader.
		private static string targetFolder = TARGET_DIR;
		
		
		private static string MeshToString(Component mf, Dictionary<string, ObjMaterial> materialList) 
		{
			if ( !Directory.Exists(TARGET_DIR) )
				Directory.CreateDirectory(TARGET_DIR);

			Mesh m;
			Material[] mats;
			
			if(mf is MeshFilter)
			{
				m = (mf as MeshFilter).mesh;
				mats = mf.renderer.sharedMaterials;
			}
			else if(mf is SkinnedMeshRenderer)
			{
				m = (mf as SkinnedMeshRenderer).sharedMesh;
				mats = (mf as SkinnedMeshRenderer).sharedMaterials;
			}
			else
			{
				return "";
			}
			
			StringBuilder sb = new StringBuilder();
			
			sb.Append("g ").Append(mf.name).Append("\n");
			foreach(Vector3 lv in m.vertices) 
			{
				Vector3 wv = mf.transform.TransformPoint(lv);
				
				//This is sort of ugly - inverting x-component since we're in
				//a different coordinate system than "everyone" is "used to".
				sb.Append(string.Format("v {0} {1} {2}\n",-wv.x,wv.y,wv.z));
			}
			sb.Append("\n");
			
			foreach(Vector3 lv in m.normals) 
			{
				Vector3 wv = mf.transform.TransformDirection(lv);
				
				sb.Append(string.Format("vn {0} {1} {2}\n",-wv.x,wv.y,wv.z));
			}
			sb.Append("\n");
			
			foreach(Vector3 v in m.uv) 
			{
				sb.Append(string.Format("vt {0} {1}\n",v.x,v.y));
			}
			
			for (int material=0; material < m.subMeshCount; material ++) {
				sb.Append("\n");
				sb.Append("usemtl ").Append(mats[material].name).Append("\n");
				sb.Append("usemap ").Append(mats[material].name).Append("\n");
				
				//See if this material is already in the materiallist.
				try
				{
					ObjMaterial objMaterial = new ObjMaterial();
					
					objMaterial.name = mats[material].name;
					
					if (!mats[material].mainTexture)
						// FIXME fix material name
						//objMaterial.textureName = EditorUtility.GetAssetPath(mats[material].mainTexture);
					//else 
						objMaterial.textureName = null;
					
					materialList.Add(objMaterial.name, objMaterial);
				}
				catch (ArgumentException)
				{
					//Already in the dictionary
				}
				
				
				int[] triangles = m.GetTriangles(material);
				for (int i=0;i<triangles.Length;i+=3) 
				{
					//Because we inverted the x-component, we also needed to alter the triangle winding.
					sb.Append(string.Format("f {1}/{1}/{1} {0}/{0}/{0} {2}/{2}/{2}\n", 
					                        triangles[i]+1 + vertexOffset, triangles[i+1]+1 + normalOffset, triangles[i+2]+1 + uvOffset));
				}
			}
			
			vertexOffset += m.vertices.Length;
			normalOffset += m.normals.Length;
			uvOffset += m.uv.Length;
			
			return sb.ToString();
		}
		
		private static void Clear()
		{
			vertexOffset = 0;
			normalOffset = 0;
			uvOffset = 0;
		}
		
		private static Dictionary<string, ObjMaterial> PrepareFileWrite()
		{
			Clear();
			
			return new Dictionary<string, ObjMaterial>();
		}
		
		private static void MaterialsToFile(Dictionary<string, ObjMaterial> materialList, string folder, string filename)
		{
			using (StreamWriter sw = new StreamWriter(folder + "/" + filename + ".mtl")) 
			{
				foreach( KeyValuePair<string, ObjMaterial> kvp in materialList )
				{
					sw.Write("\n");
					sw.Write("newmtl {0}\n", kvp.Key);
					sw.Write("Ka  0.6 0.6 0.6\n");
					sw.Write("Kd  0.6 0.6 0.6\n");
					sw.Write("Ks  0.9 0.9 0.9\n");
					sw.Write("d  1.0\n");
					sw.Write("Ns  0.0\n");
					sw.Write("illum 2\n");
					
					if (kvp.Value.textureName != null)
					{
						string destinationFile = kvp.Value.textureName;
						
						
						int stripIndex = destinationFile.LastIndexOf('/');//FIXME: Should be Path.PathSeparator;
						
						if (stripIndex >= 0)
							destinationFile = destinationFile.Substring(stripIndex + 1).Trim();
						
						
						string relativeFile = destinationFile;
						
						destinationFile = folder + "/" + destinationFile;
						
						Debug.Log("Copying texture from " + kvp.Value.textureName + " to " + destinationFile);
						
						try
						{
							//Copy the source file
							File.Copy(kvp.Value.textureName, destinationFile);
						}
						catch
						{
							
						}    
						
						
						sw.Write("map_Kd {0}", relativeFile);
					}
					
					sw.Write("\n\n\n");
				}
			}
		}

		public static void MeshToFile(Component mf, string filename) 
		{
			MeshToFile(mf, TARGET_DIR, filename);
		}

		private static void MeshToFile(Component mf, string folder, string filename) 
		{
			Dictionary<string, ObjMaterial> materialList = PrepareFileWrite();
			
			Directory.CreateDirectory(folder);
			
			using (StreamWriter sw = new StreamWriter(folder +"/" + filename + ".obj")) 
			{
				sw.Write("mtllib ./" + filename + ".mtl\n");
				
				sw.Write(MeshToString(mf, materialList));
			}
			
			MaterialsToFile(materialList, folder, filename);
		}
	}
}
