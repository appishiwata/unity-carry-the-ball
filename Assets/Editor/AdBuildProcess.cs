using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

public class AdBuildProcess
{
    // Info.plistに追加
    [PostProcessBuild(1)]
    public static void ModifyXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            PlistElementDict rootDict = plist.root;
            rootDict.SetString("NSUserTrackingUsageDescription", "あなたの好みに合わせた広告を表示するために使用されます");
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }

    // AppTrackingTransparency.frameworkの追加
    [PostProcessBuild(2)]
    public static void AddAppTrackingTransparencyFramework(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(projPath);
            string targetGuid = proj.GetUnityFrameworkTargetGuid();
            proj.AddFrameworkToProject(targetGuid, "AppTrackingTransparency.framework", false);
            proj.WriteToFile(projPath);
        }
    }
}