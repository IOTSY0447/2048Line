#if UNITY_IPHONE || UNITY_IOS 
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;

namespace komal.Editor
{
	public class CommonSettings : ISDKSettings
	{
		public void updateProject (BuildTarget buildTarget, string buildPath, string projectPath, string plistPath)
		{
#region project settings
			// PBXProject project = new PBXProject ();
			// project.ReadFromString (File.ReadAllText (projectPath));

			// string targetId = project.TargetGuidByName (PBXProject.GetUnityTargetName ());

			// // Required System Frameworks
			// // project.AddFrameworkToProject (targetId, "MessageUI.framework", false);
			// // project.AddFileToBuild (targetId, project.AddFile ("usr/lib/libz.1.2.5.tbd", "Frameworks/libz.1.2.5.tbd", PBXSourceTree.Sdk));

			// // Custom Link Flag
			// // project.AddBuildProperty (targetId, "OTHER_LDFLAGS", "-ObjC");

			// File.WriteAllText (projectPath, project.WriteToString ());
#endregion

#region plist settings
            // do nothing
#endregion
		}
	}
}
#endif
