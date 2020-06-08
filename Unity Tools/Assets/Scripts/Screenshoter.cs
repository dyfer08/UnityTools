using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;

public enum Format { PNG, JPG, RAW };

public class Screenshoter : MonoBehaviour{

	public static Screenshoter Instance;
	[SerializeField]
	string FolderName = "Screenshots";
	[SerializeField]
	string FileName = "Screenshot";
	[SerializeField]
	Format FileFormat = Format.PNG;
	[SerializeField]
	bool AlphaBackground = false;
	[SerializeField]
	Camera Camera = null;
	[SerializeField]
	[RangeAttribute(1, 10)]
	int SuperSize = 1;
	[SerializeField]
	Vector2 FixedResolution = Vector2.zero;
	string FilePath = "";
    int ScreenshotWidth;
    int ScreenshotHeight;
    bool Capture = false;

    void Start(){
    	if(Instance == null){
    		Instance = this;
            DontDestroyOnLoad(gameObject);
    	}else{
    		Destroy(gameObject);
    	}
    	CreateFolder();
    }

	void CreateFolder(){

		string Folder = "";

		switch(Application.platform){
			
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
				Folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
			break;

			case RuntimePlatform.Android:
				Folder = Application.persistentDataPath;
				System.IO.Directory.CreateDirectory("/storage/emulated/0/Pictures/"+FolderName);
			break;
		}
		Folder += "/"+FolderName;
		
		System.IO.Directory.CreateDirectory(Folder);
		FilePath = Folder;
	}

	public void OpenFolder(){
		CreateFolder();
        if(Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor){
            bool openInsidesOfFolder = true;
            string macPath = "\"" + FilePath + "\"";
            string arguments = (openInsidesOfFolder ? "" : "-R ") + macPath;
            System.Diagnostics.Process.Start("open", arguments);
        }else if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor){
            bool openInsidesOfFolder = true;
            string myWinPath = FilePath;
            string winPath = myWinPath.Replace("/", "\\");
            System.Diagnostics.Process.Start("explorer.exe", (openInsidesOfFolder ? "/root," : "/select,") + winPath);
        }
    }

    public void TakeScreenshot(){
    	Screenshoter.TakeScreenshot();
    }

	public static void TakeScreenshot(int NewSuperSize = 0, Vector2 Resolution = default(Vector2)){
		if(Resolution == Vector2.zero){
			if(Instance.FixedResolution == Vector2.zero){
				Instance.FixedResolution = new Vector2(Screen.width, Screen.height);
			}
		}else{
			Instance.FixedResolution = Resolution;
		}
		if(NewSuperSize == 0){
			NewSuperSize = Instance.SuperSize;
		}
		Instance.ScreenshotWidth = (int)Instance.FixedResolution.x * NewSuperSize;
		Instance.ScreenshotHeight = (int)Instance.FixedResolution.y * NewSuperSize;
		Instance.Capture = true;
	}

	public void TakeEditorScreenshot(){
		CreateFolder();

		if(FixedResolution == Vector2.zero){
    		System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
    		System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
    		System.Object Res = GetSizeOfMainGameView.Invoke(null,null);
    		Vector2 GameViewResolution = (Vector2)Res;
    		ScreenshotWidth = (int)GameViewResolution.x * SuperSize;
			ScreenshotHeight = (int)GameViewResolution.y * SuperSize;
		}else{
			ScreenshotWidth = (int)FixedResolution.x * SuperSize;
			ScreenshotHeight = (int)FixedResolution.y * SuperSize;
		}

		CaptureImage();
	}

	void LateUpdate(){
		if(Capture){
			Capture = false;
			CaptureImage();
		}
    }

    void CaptureImage(){
    	Rect Rect  = new Rect(0, 0, ScreenshotWidth, ScreenshotHeight);
		RenderTexture RenderTexture = new RenderTexture(ScreenshotWidth, ScreenshotHeight, 24);
		Texture2D Screenshot = new Texture2D(ScreenshotWidth, ScreenshotHeight, TextureFormat.ARGB32, false);
	
		if(Camera == null){
			Camera = Camera.main;
		}
		Camera.targetTexture = RenderTexture;

		var ClearFlags = Camera.clearFlags;
		var CameraColor = Camera.backgroundColor;
		
		if (AlphaBackground) {
		    Camera.clearFlags = CameraClearFlags.SolidColor;
		    Camera.backgroundColor = new Color (0, 0, 0, 0);
		}

		Camera.Render();
	
		RenderTexture.active = RenderTexture;
		Screenshot.ReadPixels(Rect, 0, 0);
	
		Camera.targetTexture = null;
		RenderTexture.active = null;

		Camera.clearFlags = ClearFlags;
		Camera.backgroundColor = CameraColor;
		
		string Mask = string.Format("{0}*.{1}", FileName, FileFormat.ToString().ToLower());
		string FileNumber = "_"+DateTime.Now.ToString("yyyyMMdd-HHmmss");
		
		string NewFile = FilePath+"/"+FileName+FileNumber+"."+FileFormat.ToString().ToLower();

		byte[] FileData = null;
	
		switch(FileFormat){
			case Format.RAW:
				FileData = Screenshot.GetRawTextureData();
			break;
			case Format.PNG:
				FileData = Screenshot.EncodeToPNG();
			break;
			case Format.JPG:
				FileData = Screenshot.EncodeToJPG();
			break;
		}
			
		new System.Threading.Thread(() =>{
			var File = System.IO.File.Create(NewFile);
			File.Write(FileData, 0, FileData.Length);
			File.Close();
		}).Start();

		if(Application.platform == RuntimePlatform.Android){
			StartCoroutine(RefreshGallery("/"+FileName+FileNumber+"."+FileFormat.ToString().ToLower()));
		}

    }

    IEnumerator RefreshGallery(string NewFilePath){
    	yield return new WaitForSeconds(2);
    	
    	File.Move(FilePath+NewFilePath, "/storage/emulated/0/Pictures/"+FolderName+NewFilePath);

    	using (AndroidJavaClass ClassPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject ClassActivity = ClassPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (AndroidJavaObject ObjectContext = ClassActivity.Call<AndroidJavaObject>("getApplicationContext"))
        using (AndroidJavaClass ClassMediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection"))
        using (AndroidJavaClass ClassEnvironment = new AndroidJavaClass("android.os.Environment"))
        using (AndroidJavaObject ObjectExDir = ClassEnvironment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory")){
            ClassMediaScannerConnection.CallStatic("scanFile", ObjectContext, new string[] { "/storage/emulated/0/Pictures/"+FolderName+NewFilePath }, null, null);
        }
    }
}


