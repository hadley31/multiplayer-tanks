using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntUnityEvent : UnityEvent<int> { }

[System.Serializable]
public class StringUnityEvent : UnityEvent<string> { }

[System.Serializable]
public class FloatUnityEvent : UnityEvent<float> { }

[System.Serializable]
public class BoolUnityEvent : UnityEvent<bool> { }

[System.Serializable]
public class ColorUnityEvent : UnityEvent<Color> { }

[System.Serializable]
public class EntUnityEvent : UnityEvent<Entity> { }