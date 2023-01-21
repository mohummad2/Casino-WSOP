
/*
	About iOS Device
*/

extern "C" void ImagePicker_CheckDevice(uint *iOSVersion, BOOL *isiPad) {
	*iOSVersion = (uint)([[[UIDevice currentDevice] systemVersion] floatValue] * 100.0f);
	
	*isiPad = NO;
	{
		#define UI_USER_INTERFACE_IDIOM() \
		   ([[UIDevice currentDevice] respondsToSelector:@selector(userInterfaceIdiom)] ? \
		   [[UIDevice currentDevice] userInterfaceIdiom] : \
		   UIUserInterfaceIdiomPhone)
		if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad) {
			*isiPad = YES;
		}
	}
}



/*
	Popover for iPad
*/

#import "DisplayManager.h"

extern UIView* UnityGetGLView();

extern "C" bool ImagePicker_bPopoverAutoClose = true;
extern "C" bool ImagePicker_bPopoverCenter = true;
extern "C" CGRect ImagePicker_rectPopoverTarget = {0};

extern "C" bool ImagePicker_IsPopoverAutoClose() {
	return ImagePicker_bPopoverAutoClose;
}
extern "C" void ImagePicker_SetPopoverAutoClose(bool autoclose) {
	ImagePicker_bPopoverAutoClose = autoclose;
}

extern "C" void ImagePicker_SetPopoverToCenter() {
	ImagePicker_bPopoverCenter = true;
}

static float GetUnitySurfaceToUIViewScale() {
	float scale = 1.0f;
	float viewScale = UnityGetGLView().contentScaleFactor;
#if (UNITY_VERSION > 462)
	scale = ((float)GetMainDisplaySurface()->systemW / viewScale) / (float)GetMainDisplaySurface()->targetW;
#elif (UNITY_VERSION == 462)
    #ifdef _TRAMPOLINE_UNITY_DISPLAYMANAGER_H_
	scale = ((float)GetMainRenderingSurface()->systemW / viewScale) / (float)GetMainRenderingSurface()->targetW;
    #else
	scale = ((float)GetMainDisplaySurface()->systemW / viewScale) / (float)GetMainDisplaySurface()->targetW;
    #endif
#else//#elif (UNITY_VERSION >= 410)
	scale = ((float)GetMainDisplay()->surface.systemW / viewScale) / (float)GetMainDisplay()->surface.targetW;
#endif
	return scale;
}

extern "C" void ImagePicker_SetPopoverTargetRect(float x, float y, float width, float height) {
	float scale = GetUnitySurfaceToUIViewScale();
	ImagePicker_rectPopoverTarget = CGRectMake(x*scale, y*scale, width*scale, height*scale);
	ImagePicker_bPopoverCenter = false;
}

