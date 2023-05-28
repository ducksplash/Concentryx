Shader "Custom/Glow2D" {
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1, 1, 1, 1)
        _GlowIntensity ("Glow Intensity", Range(0, 1)) = 1
        _GlowSize ("Glow Size", Range(0, 1)) = 0.1
        _HaloColor ("Halo Color", Color) = (1, 1, 1, 1)
        _HaloIntensity ("Halo Intensity", Range(0, 1)) = 1
        _HaloSize ("Halo Size", Range(0, 1)) = 0.2
    }
    
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha One
        Cull Off
        Lighting Off
        ZWrite Off
        
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _GlowColor;
            float _GlowIntensity;
            float _GlowSize;
            float4 _HaloColor;
            float _HaloIntensity;
            float _HaloSize;
            
            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                fixed4 glowColor = _GlowColor * _GlowIntensity;
                fixed4 haloColor = _HaloColor * _HaloIntensity;
                
                // Calculate the distance from the sprite's UV center
                float2 center = float2(0.5, 0.5);
                float2 dist = i.uv - center;
                
                // Calculate the glow amount based on the distance from the center
                float glowAmount = saturate(1.0 - length(dist) / _GlowSize);
                
                // Calculate the halo amount based on the distance from the center
                float haloAmount = saturate(length(dist) / _HaloSize);
                
                // Add the glow and halo to the sprite color
                fixed4 finalColor = texColor + glowColor * glowAmount + haloColor * (1.0 - glowAmount) * haloAmount;
                finalColor.a = texColor.a; // Preserve the sprite's original alpha
                
                return finalColor;
            }
            
            ENDCG
        }
    }
}
