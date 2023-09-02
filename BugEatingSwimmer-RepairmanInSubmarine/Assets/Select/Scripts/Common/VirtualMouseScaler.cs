using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// <see href="https://nekojara.city/unity-input-system-virtual-mouse#%E3%82%AB%E3%82%B9%E3%82%BF%E3%83%A0Processor%E3%81%AE%E5%AE%9F%E8%A3%85">
/// 【Unity】Input Systemからマウスカーソルを操作する > カスタムProcessorの実装
/// </see>
/// </summary>
[InitializeOnLoad]
#endif
public class VirtualMouseScaler : InputProcessor<Vector2>
{
    public float scale = 1;

    private const string ProcessorName = nameof(VirtualMouseScaler);

#if UNITY_EDITOR
    static VirtualMouseScaler() => Initialize();
#endif
    // Processorの登録処理
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        // 重複登録すると、Input ActionのProcessor一覧に正しく表示されない事があるため、
        // 重複チェックを行う
        if (InputSystem.TryGetProcessor(ProcessorName) == null)
            InputSystem.RegisterProcessor<VirtualMouseScaler>(ProcessorName);
    }

    // 独自のProcessorの処理定義
    public override Vector2 Process(Vector2 value, InputControl control)
    {
        // VirtualMouseの場合のみ、座標系問題が発生するためProcessorを適用する
        if (control.device.name == "VirtualMouse")
            value *= scale;

        return value;
    }
}
