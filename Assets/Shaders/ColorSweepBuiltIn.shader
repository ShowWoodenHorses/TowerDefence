Shader "Custom/ColorSweepPalette_URP"
{
    Properties
    {
        _PaletteTex ("Palette", 2D) = "white" {}
        _ColorIndex ("Color Index", Int) = 0
        _TargetIndex ("Target Index", Int) = 0
        _Sweep ("Sweep", Range(0,1)) = 0
        _MinY ("MinY", Float) = 0
        _MaxY ("MaxY", Float) = 1
        
        // Настройки линии
        [Toggle]_EnableLine ("Enable Line Effect", Float) = 1
        _LineWidth ("Line Width", Range(0, 0.2)) = 0.05
        _LineColor ("Line Color", Color) = (1, 1, 1, 1)
        _LineIntensity ("Line Intensity", Range(0, 2)) = 1
        _LineSoftness ("Line Softness", Range(0, 0.1)) = 0.02
        _LineGlow ("Line Glow", Range(0, 1)) = 0.3
        _GlowColor ("Glow Color", Color) = (1, 0.8, 0.4, 1)
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_PaletteTex);
            SAMPLER(sampler_PaletteTex);

            int _ColorIndex;
            int _TargetIndex;
            float _Sweep;
            float _MinY;
            float _MaxY;
            
            float _EnableLine;
            float _LineWidth;
            float4 _LineColor;
            float _LineIntensity;
            float _LineSoftness;
            float _LineGlow;
            float4 _GlowColor;

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS);
                o.worldPos = TransformObjectToWorld(v.positionOS).xyz;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float h = (i.worldPos.y - _MinY) / (_MaxY - _MinY);
                h = saturate(h);
    
                int colorIndex = (_Sweep < h) ? _TargetIndex : _ColorIndex;
                colorIndex = clamp(colorIndex, 0, 9);
                
                float y = h;
                float x = (colorIndex * 10 + 5) / 100.0;
                
                float3 col = SAMPLE_TEXTURE2D(_PaletteTex, sampler_PaletteTex, float2(x, y)).rgb;
                
                if (_EnableLine > 0.5)
                {
                    float lineDistance = abs(h - _Sweep);
                    
                    // Основная линия
                    float lineMask = 1 - smoothstep(0, _LineWidth, lineDistance);
                    
                    // Свечение (более широкая область)
                    float glowMask = 0;
                    if (_LineGlow > 0)
                    {
                        glowMask = 1 - smoothstep(0, _LineWidth * (1 + _LineGlow * 2), lineDistance);
                        glowMask = saturate(glowMask - lineMask) * _LineGlow;
                    }
                    
                    if (_LineSoftness > 0)
                    {
                        float softEdge = smoothstep(_LineWidth - _LineSoftness, _LineWidth, lineDistance);
                        lineMask = lineMask * (1 - softEdge) + softEdge;
                    }
                    
                    // Применяем цвета
                    col = lerp(col, _LineColor.rgb, lineMask * _LineIntensity);
                    col = lerp(col, _GlowColor.rgb, glowMask);
                }
                
                return float4(col, 1);
            }
            ENDHLSL
        }
    }
}