using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu{
	public class UISettingsManagerOfPause : MonoBehaviour {

		public enum Platform {Desktop};
		public Platform platform;
		// toggle buttons

		[Header("CONTROLS SETTINGS")]
		// sliders
		public GameObject musicSlider;
		public GameObject sensitivityXSlider;
		public GameObject sensitivityYSlider;

		private float sliderValue = 0.0f;
		private float sliderValueXSensitivity = 0.0f;
		private float sliderValueYSensitivity = 0.0f;
		private bool initialized = false;

		public void  Start (){
			// check slider values
			musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("musicVolume");
			sliderValueXSensitivity = PlayerPrefs.GetFloat("XSensitivity");
			sliderValueYSensitivity = PlayerPrefs.GetFloat("YSensitivity");	
            // Initialize slider values from PlayerPrefs
            sensitivityXSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("XSensitivity", 5f); // Default 5 if not set
            sensitivityYSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("YSensitivity", 5f); // Default 5 if not set

            // Add listeners
            sensitivityXSlider.GetComponent<Slider>().onValueChanged.AddListener(OnXSensitivityChanged);
            sensitivityYSlider.GetComponent<Slider>().onValueChanged.AddListener(OnYSensitivityChanged);

            initialized = true;
		}
		void OnXSensitivityChanged(float value) {
            if (!initialized) return;
            PlayerPrefs.SetFloat("XSensitivity", value);
            PlayerPrefs.Save();
            Debug.Log($"X Sensitivity saved: {value}");
        }

        void OnYSensitivityChanged(float value) {
            if (!initialized) return;
            PlayerPrefs.SetFloat("YSensitivity", value);
            PlayerPrefs.Save();
            Debug.Log($"Y Sensitivity saved: {value}");
        }

		public void Update (){
			sliderValue = musicSlider.GetComponent<Slider>().value;
			sliderValueXSensitivity = sensitivityXSlider.GetComponent<Slider>().value;
			sliderValueYSensitivity = sensitivityYSlider.GetComponent<Slider>().value;
		}

		public void MusicSlider (){
			//PlayerPrefs.SetFloat("MusicVolume", sliderValue);
			PlayerPrefs.SetFloat("musicVolume", musicSlider.GetComponent<Slider>().value);
		}

		public void SensitivityXSlider (PlayerLook player){
			player.xSensitivity = sliderValueXSensitivity;
			PlayerPrefs.SetFloat("XSensitivity", sliderValueXSensitivity);
			PlayerPrefs.Save();
		}

		public void SensitivityYSlider (PlayerLook player){
			player.ySensitivity = sliderValueYSensitivity;
			PlayerPrefs.SetFloat("YSensitivity", sliderValueYSensitivity);
			PlayerPrefs.Save();
		}
	}
}