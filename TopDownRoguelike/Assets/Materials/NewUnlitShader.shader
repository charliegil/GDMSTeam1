// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GlowLineShader"
{
    Properties
    {
        _Color ("Line Color", Color) = (1, 1, 1, 1)
        _GlowColor ("Glow Color", Color) = (1, 1, 1, 1)
        _GlowIntensity ("Glow Intensity", Float) = 1.0
        _MainTex ("Base (RGB)", 2D) = "white" { }
    }
    SubShader
    {
        Tags { "Queue"="Overlay" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata_t {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct v2f {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            uniform float _GlowIntensity;
            uniform float4 _GlowColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Apply glow color
                return half4(i.color.rgb + _GlowIntensity * _GlowColor.rgb, i.color.a);
            }
            ENDCG
        }
    }
}
