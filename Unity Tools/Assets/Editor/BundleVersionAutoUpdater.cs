using UnityEditor;
using UnityEditor.Callbacks;

public class BundleVersionAutoUpdater {
    
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget BuildTarget, string Path) {

    	switch(BuildTarget){
    		case BuildTarget.iOS:
    			PlayerSettings.iOS.buildNumber = "" + (int.Parse(PlayerSettings.iOS.buildNumber)+1);
    		break;

    		case BuildTarget.Android:
    			PlayerSettings.Android.bundleVersionCode ++;
    		break;
    	}
	}
}
