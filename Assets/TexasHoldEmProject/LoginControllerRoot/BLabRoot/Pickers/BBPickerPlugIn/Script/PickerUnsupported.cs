using UnityEngine;

namespace com.blab.imagePicker
{
    internal class PickerUnsupported : IPicker
    {
        public void Show(string title, string outputFileName, int maxSize)
        {
            var message = "Unimgpicker is not supported on this platform.";
            Debug.LogError(message);

			var receiver = GameObject.Find("BLabImagePicker");
            if (receiver != null)
            {
                receiver.SendMessage("OnFailure", message);
            }
        }
    }
}