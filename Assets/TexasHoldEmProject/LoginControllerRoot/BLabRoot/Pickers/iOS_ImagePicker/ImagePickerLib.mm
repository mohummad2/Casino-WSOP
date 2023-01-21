//
//  ImagePickerLib.mm
//  ImagePickerLib
//
//  Created by ONODA on 13/05/23.
//  Copyright (c) 2013 WhiteDev All rights reserved.
//

#import "ImagePickerLib.h"

#import <AVFoundation/AVFoundation.h>

#define NON_ARC (UNITY_VERSION < 500)


extern UIViewController *UnityGetGLViewController(); // Root view controller of Unity screen.
extern UIView *UnityGetGLView();
extern "C" void UnitySendMessage(const char* obj, const char* method, const char* msg);

typedef struct {
	unsigned char r, g, b, a;
} Color32;

extern "C" void ImagePicker_CheckDevice(uint *iOSVersion, BOOL *isiPad);

extern "C" bool ImagePicker_bPopoverAutoClose;
extern "C" bool ImagePicker_bPopoverCenter;
extern "C" CGRect ImagePicker_rectPopoverTarget;


//
static const char *strSourceType_PhotoLibrary = "PhotoLibrary";	// Default
static const char *strSourceType_Camera = "Camera";
static const char *strSourceType_SavedPhotosAlbum = "SavedPhotosAlbum";

static const char *strCallbackResultMessage_Loaded = "Result: Loaded";
static const char *strCallbackResultMessage_Canceled = "Result: Canceled";
static const char *strCallbackResultMessage_Saved = "Result: Saved";
static const char *strCallbackResultMessage_SaveFailed = "Result: SaveFailed";

static UIImagePickerControllerSourceType sourceType = UIImagePickerControllerSourceTypePhotoLibrary;

static struct {
	char strGameObjectName[1024];
	char strMethodName[1024];
} callbackLoadedInfo = {0},
callbackSavedInfo = {0};

static void ImagePicker_callbackLoadFinished(bool loaded);
static void ImagePicker_callbackSaveFinished(bool saved);


// Temporary Buffer for Load and Save
static Color32 *s_nativeBuffer = NULL;
static void releaseNativeBuffer() {
	if (s_nativeBuffer) {
		delete[] s_nativeBuffer;
		s_nativeBuffer = NULL;
	}
}

static uint iOSVersion = 0;
static BOOL isiPad = NO;

// Options
static bool bDefaultFrontCamera = false;
//static char* strCustomAlbumName = NULL;


@interface ImagePickerLib (Private)
+ (ImagePickerLib *) instance;
+ (void) release;

- (bool) usePopover;
- (bool) showImagePicker:(const char *)sourceTypeText;
- (bool) getLoadedTexrure:(unsigned char **)nativeBuffer width:(int)width height:(int)height;

- (void) releaseImage;
- (void) releasePicker;
- (void) smartDismiss;

- (void) releaseSaveTmp;
- (void) saveToPhotoLibrary:(Color32 *)pixelBuffer width:(int)width height:(int)height asPng:(BOOL)asPng withTransparency:(BOOL)withTransparency;
- (void) saveFinished:(UIImage *)image didFinishSavingWithError:(NSError *)error contextInfo:(void *)contextInfo;


- (void) saveCaptureFormat;
- (void) restoreCaptureFormat;
@end

@implementation ImagePickerLib
@synthesize imagePicker, popoverPicker, loadedImage, savingImage;

static CGSize imageSize = CGSizeZero;

static ImagePickerLib *pInstance = nil;

static AVCaptureDeviceFormat *captureFormat;
static CMTime captureMinFrameRateDuration;
static CMTime captureMaxFrameRateDuration;


+ (ImagePickerLib *) instance {
	if (pInstance == nil) {
		pInstance = [[ImagePickerLib alloc] init];
		
		ImagePicker_CheckDevice(&iOSVersion, &isiPad);
	}
	return pInstance;
}

+ (void) release {
	if (pInstance != nil) {
		[pInstance releaseImage];
		[pInstance releasePicker];
		//[pInstance release];
	}
	pInstance = nil;
}

- (void) saveCaptureFormat
{
    // Vuforia 2.8 workaround on iOS7 - save the current format and frame rate durations
    if (floor(NSFoundationVersionNumber) > NSFoundationVersionNumber_iOS_6_1) {
        AVCaptureDevice *videoDevice = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];
        if( videoDevice != nil )
        {
            NSLog(@"ATTEMPTING Saving Original Camera Settings");
            captureFormat = videoDevice.activeFormat;
            captureMinFrameRateDuration = videoDevice.activeVideoMinFrameDuration;
            captureMaxFrameRateDuration = videoDevice.activeVideoMaxFrameDuration;
        
            
            NSLog(@"Saved Original Camera Settings");
            /*
            NSLog(@"captureMinFrameRateDuration: %d / %d", videoDevice.activeVideoMinFrameDuration.value, videoDevice.activeVideoMinFrameDuration.timescale);
            NSLog(@"captureMinFrameRateDuration: %d / %d", videoDevice.activeVideoMaxFrameDuration.value, videoDevice.activeVideoMaxFrameDuration.timescale);
             */
        }
        else
        {
            NSLog(@"FAILED Saving Original Camera Settings");
        }
    }
}
- (void) restoreCaptureFormat
{
    // Vuforia 2.8 workaround on iOS7 - restore the format and frame rate durations
    if (floor(NSFoundationVersionNumber) > NSFoundationVersionNumber_iOS_6_1) {
        AVCaptureDevice *videoDevice = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];
        if( videoDevice != nil){
            NSLog(@"ATTEMPTING to Restore Camera Settings");
            
            [videoDevice lockForConfiguration:nil];
            videoDevice.activeFormat = captureFormat;
            videoDevice.activeVideoMaxFrameDuration = captureMaxFrameRateDuration;
            videoDevice.activeVideoMinFrameDuration = captureMinFrameRateDuration;
            
            //videoDevice.activeVideoMaxFrameDuration = CMTimeMake(1, 0);
            //videoDevice.activeVideoMinFrameDuration = CMTimeMake(1, 0);
            
            [videoDevice unlockForConfiguration];
            NSLog(@"Restored Camera Settings");
        }
        else
        {
            NSLog(@"FAILED Restoring Camera Settings");
        }
    }
}


- (void) releaseImage {
	releaseNativeBuffer();
	loadedImage = nil;
	imageSize = CGSizeZero;
}
- (void) releasePicker {
	popoverPicker.delegate = nil;
	popoverPicker = nil;
	imagePicker.delegate = nil;
	imagePicker = nil;
}

- (void)dealloc {

	[self releaseImage];
	[self releasePicker];

#if NON_ARC
	[super dealloc];
#endif

	pInstance = nil;
}

- (bool) getLoadedTexrure:(unsigned char **)nativeBuffer width:(int)width height:(int)height {

	releaseNativeBuffer();
	s_nativeBuffer = new Color32[width*height];
	*nativeBuffer = (unsigned char *)s_nativeBuffer;

	assert(s_nativeBuffer);
	if (s_nativeBuffer) {
		if (loadedImage) {
			/* NG
			width = MIN(MAX(width, 64), 2048);
			height = MIN(MAX(height, 64), 2048);
			*/

#if true
			// Resize
			UIGraphicsBeginImageContext(CGSizeMake(width, height));
			CGRect drawRect = CGRectMake(0, 0, width, height);
			[loadedImage drawInRect:drawRect];
			UIImage *newImage = UIGraphicsGetImageFromCurrentImageContext();
			//int newImageWidth = (int)newImage.size.width;
			int newImageHeight = (int)newImage.size.height;
			UIGraphicsEndImageContext();

			// Pixel Data
			CGDataProviderRef dataProvider = CGImageGetDataProvider(newImage.CGImage);
			NSData *data = (NSData *) CFBridgingRelease(CGDataProviderCopyData(dataProvider));
			if (data) {
				assert([data length] >= (width*height*4));
				int newImagePitch = (int)[data length] / newImageHeight / 4;
				const Color32 *pSrcBase = (const Color32 *)[data bytes];
				for (int y=0; y<height; ++y) {
					const Color32 *pSrc = &pSrcBase[newImagePitch*y];
					Color32 *pDst = &s_nativeBuffer[width*((height-1)-y)];
#if true
					memcpy(pDst, pSrc, width*4);
#else
//					for (int x=0; x<width; ++x) {
//						pDst->r = pSrc->b;
//						pDst->g = pSrc->g;
//						pDst->b = pSrc->r;
//						pDst->a = pSrc->a;
//						++pSrc;
//						++pDst;
//					}
#endif
				}
			}
#else
//			CGImageRef image = [loadedImage CGImage];
//			if (image) {
//				//CGImageAlphaInfo info = CGImageGetAlphaInfo(image);
//				//bool hasAlpha = ((info == kCGImageAlphaPremultipliedLast) || (info == kCGImageAlphaPremultipliedFirst) || (info == kCGImageAlphaLast) || (info == kCGImageAlphaFirst) ? YES : NO);
//			
//				//int imageWidth = CGImageGetWidth(image);
//				//int imageHeight = CGImageGetHeight(image);
//
//				CGColorSpaceRef colorSpace = CGColorSpaceCreateDeviceRGB();
//				CGContextRef context = CGBitmapContextCreate(s_nativeBuffer, width, height, 8, 4*width, colorSpace, kCGImageAlphaPremultipliedLast|kCGBitmapByteOrderDefault);
//				CGColorSpaceRelease(colorSpace);
//				//CGContextClearRect(context, CGRectMake(0, 0, width, height));
//				//CGContextTranslateCTM(context, 0, height-imageHeight);
//				CGContextTranslateCTM(context, 0, height);
//				CGContextScaleCTM(context, 1.0f, -1.0f);
//				CGContextDrawImage(context, CGRectMake(0, 0, width, height), image);
//				CGContextRelease(context);
//			}
#endif

			return true;
		}
	}
	return false;
}


// PhotoLibrary
- (bool) usePopover
{
	return (isiPad && ((iOSVersion < 700) || (sourceType != UIImagePickerControllerSourceTypeCamera)));
}

- (bool) showImagePicker:(const char *)sourceTypeText
{
	[self saveCaptureFormat];
    
	[self smartDismiss];
	[self releaseImage];
	[self releasePicker];

	sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
	if (strcmp(sourceTypeText, strSourceType_PhotoLibrary) == 0) {
		sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
	} else if (strcmp(sourceTypeText, strSourceType_Camera) == 0) {
		sourceType = UIImagePickerControllerSourceTypeCamera;
	} else if (strcmp(sourceTypeText, strSourceType_SavedPhotosAlbum) == 0) {
		sourceType = UIImagePickerControllerSourceTypeSavedPhotosAlbum;
	}
	if ([UIImagePickerController isSourceTypeAvailable:sourceType] == false) {
		ImagePicker_callbackLoadFinished(/*loaded=*/false);
		return false;
	}

	imagePicker = [[UIImagePickerController alloc] init];
	imagePicker.sourceType = sourceType;
	imagePicker.videoQuality = UIImagePickerControllerQualityType640x480;
	imagePicker.allowsEditing = NO;
	if (bDefaultFrontCamera && [UIImagePickerController isCameraDeviceAvailable:UIImagePickerControllerCameraDeviceFront]) {
		imagePicker.cameraDevice = UIImagePickerControllerCameraDeviceFront;
	}
	imagePicker.delegate = self;
	
	// Present ImagePicker
	if ([self usePopover] == false) {
		[UnityGetGLViewController() presentViewController:imagePicker animated:YES completion:^ {
			}
		];
	} else {
		popoverPicker = [[UIPopoverController alloc] initWithContentViewController:imagePicker];
		popoverPicker.delegate = self;

		UIView *parentView = UnityGetGLViewController().view;
		UIPopoverArrowDirection arrow = 0;
		CGRect rect = imagePicker.view.bounds;
		if (ImagePicker_bPopoverCenter) {
			rect.origin.x = (parentView.bounds.size.width - rect.size.width) * 0.5f;
			rect.origin.y = (parentView.bounds.size.height - rect.size.height) * 0.5f;
			arrow = 0;
		} else {
			rect = ImagePicker_rectPopoverTarget;
			arrow = UIPopoverArrowDirectionAny;
		}
		[popoverPicker presentPopoverFromRect:rect inView:parentView permittedArrowDirections:arrow animated:YES];
	}

	return true;
}

- (void) smartDismiss {
	if ((popoverPicker == nil) || ([self usePopover] == false)) {
		[UnityGetGLViewController() dismissViewControllerAnimated:YES completion:^ {
			}
		];
	} else {
		if (popoverPicker) {
			[popoverPicker dismissPopoverAnimated:YES];
		}
		[self releasePicker];
	}
    
    // Restore the camera settings
    [self restoreCaptureFormat];
}

static BOOL IsImageRightOrLeft(UIImage *image) {
	switch ((int)image.imageOrientation) {
		case UIImageOrientationLeft:          // 90 deg CCW
		case UIImageOrientationRight:         // 90 deg CW
		case UIImageOrientationLeftMirrored:  // vertical flip
		case UIImageOrientationRightMirrored: // vertical flip
			return YES;
	}
	return NO;
}

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary *)info
{
	[self releaseImage];
	loadedImage = [info objectForKey:UIImagePickerControllerEditedImage];
	if (loadedImage == nil) {
		loadedImage = [info objectForKey:UIImagePickerControllerOriginalImage];
	}
	if (loadedImage) {
#if NON_ARC
		[loadedImage retain];
#endif

		// GetSize
		CGImageRef imageRef = loadedImage.CGImage;
		BOOL bRightOrLeft = IsImageRightOrLeft(loadedImage);
		if (bRightOrLeft) {
			// Change width and height
			imageSize.height = CGImageGetWidth(imageRef);
			imageSize.width = CGImageGetHeight(imageRef);
		} else {
			imageSize.width = CGImageGetWidth(imageRef);
			imageSize.height = CGImageGetHeight(imageRef);
		}
	} else {
		imageSize = CGSizeZero;
	}
	ImagePicker_callbackLoadFinished(/*loaded=*/true);
	if ((isiPad == false) || (ImagePicker_bPopoverAutoClose || (sourceType == UIImagePickerControllerSourceTypeCamera))) {
		[self smartDismiss];
		ImagePicker_callbackLoadFinished(/*loaded=*/false);
	}
}

- (void) imagePickerControllerDidCancel:(UIImagePickerController*)picker
{
	[self smartDismiss];
	ImagePicker_callbackLoadFinished(/*loaded=*/false);
}

- (void) popoverControllerDidDismissPopover:(UIPopoverController *)popoverController
{
	[self smartDismiss];
	ImagePicker_callbackLoadFinished(/*loaded=*/false);
}


// Save to PhotoLibrary
- (void) releaseSaveTmp {
	releaseNativeBuffer();
	savingImage = nil;
}

- (void) saveToPhotoLibrary:(Color32 *)pixelBuffer width:(int)width height:(int)height asPng:(BOOL)asPng withTransparency:(BOOL)withTransparency {
	[self releaseSaveTmp];

	while (pixelBuffer && (width > 0) && (height > 0)) {
		// Copy
		s_nativeBuffer = new Color32[width*height];
		if (s_nativeBuffer == nil) break;	// Failed
		const Color32 *pSrc = (const Color32 *)pixelBuffer;
		for (int y=0; y<height; ++y) {
			Color32 *pDst = &s_nativeBuffer[width*((height-1)-y)];
			memcpy(pDst, pSrc, width*sizeof(Color32));
			pSrc += width;
		}
	
		// Create UIImage
		CGDataProviderRef dataProviderRef = CGDataProviderCreateWithData(NULL, s_nativeBuffer, width*height*4, NULL);
		CGImageRef imageRef = CGImageCreate(
			width, height, 8, 32, width*4,
			CGColorSpaceCreateDeviceRGB(),
			(withTransparency) ? (kCGBitmapByteOrderDefault|kCGImageAlphaLast) : kCGBitmapByteOrderDefault,
			dataProviderRef,
			NULL, FALSE, kCGRenderingIntentDefault);

		if (asPng) {
			// as PNG
			savingImage = [UIImage imageWithCGImage:imageRef];
			NSData *pngData = UIImagePNGRepresentation(savingImage);
			savingImage = [UIImage imageWithData:pngData];
		} else {
			// as JPG
			savingImage = [UIImage imageWithCGImage:imageRef];
		}
#if NON_ARC
		[savingImage retain];
#endif

		CGDataProviderRelease(dataProviderRef);
		CGImageRelease(imageRef);

		UIImageWriteToSavedPhotosAlbum(savingImage, self, @selector(saveFinished:didFinishSavingWithError:contextInfo:), NULL);

		return;
		break;
	}

	// Failed
	ImagePicker_callbackSaveFinished(/*saved=*/false);
	[self releaseSaveTmp];
}

- (void) saveFinished:(UIImage *)image didFinishSavingWithError:(NSError *)error contextInfo:(void *)contextInfo {
	[self releaseSaveTmp];
	if (error == nil) {
		// 成功
		ImagePicker_callbackSaveFinished(/*saved=*/true);
	} else {
		// 失敗
		ImagePicker_callbackSaveFinished(/*saved=*/false);
	}
}

@end



//#pragma mark - Auto Rotation
// Auto Rotation
//@implementation UIImagePickerController (AutoRotation)
//- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
//	return [UnityGetGLViewController() shouldAutorotateToInterfaceOrientation:interfaceOrientation];
//}
//- (NSUInteger) supportedInterfaceOrientations {
//	return [UnityGetGLViewController() supportedInterfaceOrientations];
//}
//- (BOOL) shouldAutorotate {
//	return [UnityGetGLViewController() shouldAutorotate];
//}
//- (UIInterfaceOrientation) preferredInterfaceOrientationForPresentation {
//	return [UnityGetGLViewController() preferredInterfaceOrientationForPresentation];
//}
//@end


// Util
static char *strCopy(const char *str) {
	assert(str);
	char *copyStr = (char *)malloc(strlen(str)+1);
	assert(copyStr);
	strcpy(copyStr, str);
	return copyStr;
}



// Interface
extern "C" bool ImagePicker_showPicker(const char *sourceType, const char *callbackGameObjectName, const char *callbackMethodName) {
	strncpy(callbackLoadedInfo.strGameObjectName, callbackGameObjectName, sizeof(callbackLoadedInfo.strGameObjectName));
	strncpy(callbackLoadedInfo.strMethodName, callbackMethodName, sizeof(callbackLoadedInfo.strMethodName));
	return [[ImagePickerLib instance] showImagePicker:sourceType];
}

extern "C" bool ImagePicker_isLoaded() {
	return ([ImagePickerLib instance].loadedImage);
}
extern "C" int ImagePicker_getLoadedTexrureWidth() {
	if (ImagePicker_isLoaded()) {
		return imageSize.width;
	}
	return 0;
}
extern "C" int ImagePicker_getLoadedTexrureHeight() {
	if (ImagePicker_isLoaded()) {
		return imageSize.height;
	}
	return 0;
}
extern "C" bool ImagePicker_getLoadedTexrure(unsigned char **nativeBuffer, int width, int height) {
	return [[ImagePickerLib instance] getLoadedTexrure:nativeBuffer width:width height:height];
}

extern "C" void ImagePicker_releaseNativeBuffer() {
	releaseNativeBuffer();
}

extern "C" void ImagePicker_release() {
	[ImagePickerLib release];
}
extern "C" void ImagePicker_releaseLoadedImage() {
	[[ImagePickerLib instance] releaseImage];
}

// Options
extern "C" void ImagePicker_SetDefaultFrontCamera(bool on) {
	bDefaultFrontCamera = on;
}
//extern "C" void ImagePicker_setCustomAlbumName(const char *name) {
//	strCustomAlbumName = strCopy(name);
//}


// Save Image
extern "C" void Lib_SaveToPhotoLibrary(Color32 *pixelBuffer, int width, int height, const char *callbackGameObjectName, const char *callbackMethodName, bool asPng, bool withTransparency) {
	strncpy(callbackSavedInfo.strGameObjectName, callbackGameObjectName, sizeof(callbackSavedInfo.strGameObjectName));
	strncpy(callbackSavedInfo.strMethodName, callbackMethodName, sizeof(callbackSavedInfo.strMethodName));
	[[ImagePickerLib instance] saveToPhotoLibrary:pixelBuffer width:width height:height asPng:asPng withTransparency:withTransparency];
}


static void ImagePicker_callbackLoadFinished(bool loaded) {
	const char *msg = (loaded) ? strCallbackResultMessage_Loaded : strCallbackResultMessage_Canceled;
	UnitySendMessage(strCopy(callbackLoadedInfo.strGameObjectName), strCopy(callbackLoadedInfo.strMethodName), strCopy(msg));
}
static void ImagePicker_callbackSaveFinished(bool saved) {
	const char *msg = (saved) ? strCallbackResultMessage_Saved : strCallbackResultMessage_SaveFailed;
	UnitySendMessage(strCopy(callbackSavedInfo.strGameObjectName), strCopy(callbackSavedInfo.strMethodName), strCopy(msg));
}
