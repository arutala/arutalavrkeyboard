# The Interactive Keyboard VR
The Interactive Keyboard VR is a toolkit from ARUTALA that was designed to ease the burden of the developer to create a Virtual Reality application using Unity and Oculus.

This toolkit has been developed and tested using Unity 2019.1.7f1 with Oculus Rift S and Oculus Quest

## Requirement
- Oculus SDK
- TextMeshPro

## Installation & Usage
- Download this project’s Unity package. [link]
- In your own Unity project, import the Interactive Keyboard VR by choosing “Assets” → “Import Package” → “Custom Package” → “AruKeyboard.unitypackage” from the Unity editor’s menus. 

- Go to **KeyboardSDK** folder, then go to **Prefabs** folder.
- Drag **AruKeyboard** prefab into your scene.

- In the inspector windows of **AruKeyboard** prefab, drag your **LaserPointer** to **OVRRaycaster.Pointer** component
- Drag **InputFieldSingle** prefab or **InputFieldTextArea** prefab into your scene.
	- You can also use your own InputField with this interactive keyboard.
- Drag your **InputField** to **AruKeyboardManager.InputField** component

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update the tests as appropriate.

## License
MIT License
Copyright (c) 2020 PT. ARUTALA DIGITAL INOVASI

