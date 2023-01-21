using UnityEngine;
using System.Collections;

namespace com.blab.imagePicker
{

	public class BLabImagePicker : MonoBehaviour
    {
#if UNITY_ANDROID

        public delegate void ImageDelegate(string path);

        public delegate void ErrorDelegate(string message);

        public event ImageDelegate Completed;

        public event ErrorDelegate Failed;

        private IPicker picker = 
        #if UNITY_IOS && !UNITY_EDITOR
            //new PickeriOS();
        #elif UNITY_ANDROID && !UNITY_EDITOR
            new PickerAndroid();
        #else
            new PickerUnsupported();
        #endif

        public void Show(string title, string outputFileName, int maxSize)
        {
            picker.Show(title, outputFileName, maxSize);
        }

        private void OnComplete(string path)
        {
			Debug.Log("OnComplete path : " + path);

            var handler = Completed;
            if (handler != null)
            {
                handler(path);
            }
        }

        private void OnFailure(string message)
        {

			Debug.Log("OnFailure : " + message);
            var handler = Failed;
            if (handler != null)
            {
                handler(message);
            }
        }
#endif
    }

}