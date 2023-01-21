using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;
using System;

#if UNITY_IPHONE
public class LoadTextureFromImagePicker {

	private const string strSourceType_PhotoLibrary = "PhotoLibrary";	// Default
	private const string strSourceType_Camera = "Camera";
	private const string strSourceType_SavedPhotosAlbum = "SavedPhotosAlbum";


	// Interface
	public static bool ShowPhotoLibrary (string callbackGameObjectName, string callbackMethodName) {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			return ImagePicker_showPicker(strSourceType_PhotoLibrary, callbackGameObjectName, callbackMethodName);
		}
		return false;
	}

	public static bool ShowCamera (string callbackGameObjectName, string callbackMethodName) {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			return ImagePicker_showPicker(strSourceType_Camera, callbackGameObjectName, callbackMethodName);
		}
		return false;
	}

	public static void Release () {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			ImagePicker_release();
		}
	}
	public static void ReleaseLoadedImage () {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			ImagePicker_releaseLoadedImage();
		}
	}


	public static bool IsLoaded () {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			return ImagePicker_isLoaded();
		}
		return false;
	}
	public static int GetLoadedTextureWidth () {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			return ImagePicker_getLoadedTexrureWidth();
		}
		return 0;
	}
	public static int GetLoadedTextureHeight () {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			return ImagePicker_getLoadedTexrureHeight();
		}
		return 0;
	}

	public static Texture2D GetLoadedTexture (string message, int width, int height) {
		return _GetLoadedTexture(message, width, height, /*mipmap=*/true, /*linear=*/false);
	}
	public static Texture2D GetLoadedTexture (string message, int width, int height, bool mipmap) {
		return _GetLoadedTexture(message, width, height, mipmap, /*linear=*/false);
	}
	public static Texture2D GetLoadedTexture (string message, int width, int height, bool mipmap, bool linear) {
		return _GetLoadedTexture(message, width, height, mipmap, linear);
	}


	// Option
	public static void SetDefaultFrontCamera (bool enable) {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			ImagePicker_SetDefaultFrontCamera(enable);
		}
	}


	// Save to PhotoLibrary
	public static void SaveAsJpgToPhotoLibrary (Texture texture, string callbackGameObjectName, string callbackMethodName) {
		if (texture != null) {
			Texture2D tex2D = (Texture2D)texture;
			Lib_SaveToPhotoLibrary(tex2D.GetPixels32(), tex2D.width, tex2D.height, callbackGameObjectName, callbackMethodName, /*asPng=*/false, /*withTransparency=*/false);
		}
	}
	public static void SaveAsPngToPhotoLibrary (Texture texture, string callbackGameObjectName, string callbackMethodName) {
		if (texture != null) {
			Texture2D tex2D = (Texture2D)texture;
			Lib_SaveToPhotoLibrary(tex2D.GetPixels32(), tex2D.width, tex2D.height, callbackGameObjectName, callbackMethodName, /*asPng=*/true, /*withTransparency=*/false);
		}
	}
	public static void SaveAsPngWithTransparencyToPhotoLibrary (Texture texture, string callbackGameObjectName, string callbackMethodName) {
		if (texture != null) {
			Texture2D tex2D = (Texture2D)texture;
			Lib_SaveToPhotoLibrary(tex2D.GetPixels32(), tex2D.width, tex2D.height, callbackGameObjectName, callbackMethodName, /*asPng=*/true, /*withTransparency=*/true);
		}
	}


	// Save/Load to local storage
	public static void SaveToLocalFile (string filepath, Texture texture) {
		if (string.IsNullOrEmpty(filepath) == false && texture != null) {
			Texture2D tex2D = (Texture2D)texture;
			System.IO.File.WriteAllBytes(filepath, tex2D.EncodeToPNG());
		}
	}
	public static Texture LoadFromLocalFile (string filepath) {
		if (string.IsNullOrEmpty(filepath) == false && System.IO.File.Exists(filepath)) {
			byte[] bytes = System.IO.File.ReadAllBytes(filepath);
			var tex2D = new Texture2D(1, 1);
			tex2D.LoadImage(bytes);
			return tex2D;
		}
		return null;
	}
	public static void DeleteLocalFile (string filepath) {
		if (string.IsNullOrEmpty(filepath) == false && System.IO.File.Exists(filepath)) {
			System.IO.File.Delete(filepath);
		}
	}


	// (Popover Auto Close for iPad)
	public static void SetPopoverAutoClose (bool autoclose) {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			ImagePicker_SetPopoverAutoClose(autoclose);
		}
	}

	// (Position of Popover for iPad)
	public static void SetPopoverToCenter () {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			ImagePicker_SetPopoverToCenter();
		}
	}
	public static void SetPopoverTargetRect (float x, float y, float width, float height) {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			ImagePicker_SetPopoverTargetRect(x, y, width, height);
		}
	}



	
	// Implementation
	[DllImport ("__Internal")] private static extern bool ImagePicker_showPicker(string sourceType, string callbackGameObjectName, string callbackMethodName);
	[DllImport ("__Internal")] private static extern bool ImagePicker_isLoaded();
	[DllImport ("__Internal")] private static extern int ImagePicker_getLoadedTexrureWidth();
	[DllImport ("__Internal")] private static extern int ImagePicker_getLoadedTexrureHeight();
	[DllImport ("__Internal")] private static extern bool ImagePicker_getLoadedTexrure(ref IntPtr nativeBuffer, int width, int height);
	[DllImport ("__Internal")] private static extern void ImagePicker_releaseNativeBuffer();
	[DllImport ("__Internal")] private static extern void ImagePicker_releaseLoadedImage();
	[DllImport ("__Internal")] private static extern void ImagePicker_release();

	// Option
	[DllImport ("__Internal")] private static extern void ImagePicker_SetDefaultFrontCamera(bool enable);

	// Popover for iPad
	[DllImport ("__Internal")] private static extern bool ImagePicker_IsPopoverAutoClose();
	[DllImport ("__Internal")] private static extern void ImagePicker_SetPopoverAutoClose(bool autoclose);
	[DllImport ("__Internal")] private static extern void ImagePicker_SetPopoverToCenter();
	[DllImport ("__Internal")] private static extern void ImagePicker_SetPopoverTargetRect(float x, float y, float width, float height);

	// Save Image
	[DllImport ("__Internal")] private static extern void Lib_SaveToPhotoLibrary(Color32[] pixelBuffer, int width, int height, string callbackGameObjectName, string callbackMethodName, bool asPng, bool withTransparency);

	public const string strCallbackResultMessage_Loaded = "Result: Loaded";
	//public  const string strCallbackResultMessage_Canceled = "Result: Canceled";
	public const string strCallbackResultMessage_Saved = "Result: Saved";
	//public  const string strCallbackResultMessage_SaveFailed = "Result: SaveFailed";

	public static Texture2D _GetLoadedTexture(string message, int width, int height, bool mipmap, bool linear) {
		Texture2D texture = null;
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			if (message.Equals(strCallbackResultMessage_Loaded)) {
				if (width <= 0) width = 16;
				if (height <= 0) height = 16;
				int pixelCount = (width * height);
				int pixelBufferSize = 4/*sizeof(Color32)*/ * pixelCount;
//				Color32[] pixelData = new Color32[pixelCount];
				IntPtr nativeBuffer = IntPtr.Zero;
				if (ImagePicker_getLoadedTexrure(ref nativeBuffer, width, height)) {
					var byteBuffer = new byte[pixelBufferSize];
					Marshal.Copy(nativeBuffer, byteBuffer, 0, pixelBufferSize);
					ImagePicker_releaseNativeBuffer();
					Color32[] pixelData = new Color32[pixelCount];
					for (var i=0; i<pixelCount; ++i) {
						pixelData[i].r = byteBuffer[i*4+2];
						pixelData[i].g = byteBuffer[i*4+1];
						pixelData[i].b = byteBuffer[i*4+0];
						pixelData[i].a = byteBuffer[i*4+3];
					}
					byteBuffer = null;
					texture = new Texture2D(width, height, TextureFormat.ARGB32, mipmap, linear);
					texture.SetPixels32(pixelData);
					texture.Apply(mipmap);
					pixelData = null;
				}
			}
		}
		return texture;
	}
}
#endif
