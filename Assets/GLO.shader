Shader "Custom/SpriteGlow"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowThreshold ("Glow Threshold", Range(0, 1)) = 0.5
        _GlowIntensity ("Glow Intensity", Range(0, 10)) = 1
        _HaloSize ("Halo Size", Range(0, 1)) = 0.1
    }
 
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100
 
        Blend SrcAlpha OneMinusSrcAlpha
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float4 _Color;
            float4 _GlowColor;
            float _GlowThreshold;
            float _GlowIntensity;
            float _HaloSize;
 
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
 
                // Calculate the glow factor based on the pixel's brightness
                float brightness = col.r * 0.3 + col.g * 0.59 + col.b * 0.11;
                float glowFactor = saturate((brightness - _GlowThreshold) * _GlowIntensity);
 
                // Add the glow color to the sprite color
                col.rgb += _GlowColor.rgb * glowFactor;
 
                // Calculate the halo factor based on the UV distance from the center
                float2 centerUV = float2(0.5, 0.5);
                float2 uvOffset = i.uv - centerUV;
                float haloFactor = saturate(1.0 - length(uvOffset) / _HaloSize);
 
                // Add the halo color to the sprite color
                col.rgb += _GlowColor.rgb * haloFactor;
 
                return col;
            }
            ENDCG
        }
    }
}
