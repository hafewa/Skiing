using UnityEditor;

public class EditorResourceSetting : AssetPostprocessor
{
    /// <summary>
    /// 导入贴图预处理
    /// </summary>
    void OnPreprocessTexture() {

        #region 界面UI贴图导入自动设置textureType
        if (assetPath.Contains("UI")) {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
        }
        #endregion

    }
}