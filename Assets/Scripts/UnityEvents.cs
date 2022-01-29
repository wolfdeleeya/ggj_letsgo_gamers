using System;
using UnityEngine.Events;

[Serializable]
public class UnityEventMessageSO: UnityEvent<MessageSO> {}

[Serializable]
public class UnityEventFloat: UnityEvent<float> {}

[Serializable]
public class UnityEventBool: UnityEvent<bool> {}