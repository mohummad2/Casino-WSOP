#if UNITY_ANDROID
using UnityEngine;

namespace com.blab.imagePicker
{
    internal class PickerAndroid : IPicker
    {
		private static readonly string PickerClass = "imagepicker.blab.com.plugin.BBPicker";

        public void Show(string title, string outputFileName, int maxSize)
        {
            using (var picker = new AndroidJavaClass(PickerClass))
            {
				Debug.Log("PickerAndroid Show : " + outputFileName + " : " + title);
                picker.CallStatic("show", title, outputFileName, maxSize);
            }
        }
    }
}
#endif