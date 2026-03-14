Shader "Custom/ColorSweepBuiltIn"
{
    Properties
    {
        _ColorA ("Old Color", Color) = (1,0,0,1)
        _ColorB ("New Color", Color) = (0,1,0,1)

        _Sweep ("Sweep", Range(0,1)) = 0
        _EdgeWidth ("Edge Width", Range(0.001,0.2)) = 0.05
        _EdgeColor ("Edge Color", Color) = (1,1,1,1)

        _MinY ("MinY", Float) = 0
        _MaxY ("MaxY", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float height : TEXCOORD0;
            };

            float4 _ColorA;
            float4 _ColorB;

            float _Sweep;
            float _EdgeWidth;
            float4 _EdgeColor;

            float _MinY;
            float _MaxY;

            v2f vert(appdata v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);

                float h = (v.vertex.y - _MinY) / (_MaxY - _MinY);
                o.height = saturate(h);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float t = step(_Sweep, i.height);

                float4 col = lerp(_ColorA, _ColorB, t);

                float edge =
                    smoothstep(_Sweep - _EdgeWidth, _Sweep, i.height) -
                    smoothstep(_Sweep, _Sweep + _EdgeWidth, i.height);

                col.rgb += edge * _EdgeColor.rgb;

                return col;
            }

            ENDCG
        }
    }
}