#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>

@interface ImagePickerLib : UIViewController <UINavigationControllerDelegate, UIImagePickerControllerDelegate, UIPopoverControllerDelegate>;

@property (retain, nonatomic) UIImagePickerController* imagePicker;
@property (retain, nonatomic) UIPopoverController *popoverPicker;
@property (retain, nonatomic) UIImage *loadedImage;

@property (retain, nonatomic) UIImage *savingImage;

@end
