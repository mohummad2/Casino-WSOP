using UnityEngine;
using System.Collections;

namespace com.blab.imagePicker
{
    public class PickerController : MonoBehaviour
    {
#if UNITY_ANDROID
        [SerializeField]
		private BLabImagePicker imagePicker;

        [SerializeField]
        private MeshRenderer imageRenderer;

        void Awake()
        {
            imagePicker.Completed += (string path) =>
            {
                StartCoroutine(LoadImage(path, imageRenderer));
            };
        }

        public void OnPressShowPicker()
        {
            imagePicker.Show("Select Image", "AppBlabImagePicker", 1024);
        }

        private IEnumerator LoadImage(string path, MeshRenderer output)
        {
            var url = "file://" + path;
            var www = new WWW(url);
            yield return www;

            var texture = www.texture;
            if (texture == null)
            {
                Debug.LogError("Failed to load texture url:" + url);
            }

            output.material.mainTexture = texture;
        }
#endif
    }
}