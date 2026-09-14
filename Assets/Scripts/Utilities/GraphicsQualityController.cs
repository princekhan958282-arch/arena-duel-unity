using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ArenaDuel.Utilities
{
    public static class GraphicsQualityController
    {
        public static void Apply(int tier)
        {
            tier = Mathf.Clamp(tier, 0, 3);
            int available = Mathf.Max(0, QualitySettings.names.Length - 1);
            QualitySettings.SetQualityLevel(Mathf.Min(tier, available), true);
            QualitySettings.shadowDistance = new[] { 18f, 28f, 42f, 60f }[tier];
            QualitySettings.antiAliasing = new[] { 0, 2, 4, 4 }[tier];
            QualitySettings.globalTextureMipmapLimit = tier == 0 ? 1 : 0;
            Application.targetFrameRate = tier == 0 ? 30 : 60;
            if (QualitySettings.renderPipeline is UniversalRenderPipelineAsset urp)
                urp.renderScale = new[] { .75f, .9f, 1f, 1f }[tier];
        }
    }
}
