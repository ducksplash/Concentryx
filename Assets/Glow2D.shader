Shader "Custom/Glow2D" {
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1, 1, 1, 1)
        _GlowIntensity ("Glow Intensity", Range(0, 1)) = 1
        _GlowSize ("Glow Size", Range(0, 1)) = 0.1
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
            
            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                fixed4 glowColor = _GlowColor * _GlowIntensity;
                
                // Calculate the glow amount based on the distance from the sprite's UV center
                float2 center = float2(0.5, 0.5);
                float distance = distance(i.uv, center);
                float glowAmount = smoothstep(0, _GlowSize, distance);
                
                // Add the glow to the sprite color
                fixed4 finalColor = texColor + glowColor * glowAmount;
                finalColor.a = texColor.a; // Preserve the sprite's original alpha
                
                return finalColor;
            }
            
            ENDCG
        }
    }
}
