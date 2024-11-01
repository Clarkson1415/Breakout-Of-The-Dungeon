Shader "UI/BottomToTopFadeWithCutoff" {
    Properties {
        _Color ("Bottom Color", Color) = (1, 0, 0, 1) // Red color with full opacity
        _Alpha ("Max Transparency", Range(0, 1)) = 1.0 // Max transparency control
        _Cutoff ("Transparency Cutoff", Range(-100, 100)) = 0.5 // Y-axis cutoff point for transparency (default set to middle)
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _Color;
            float _Alpha;
            float _Cutoff;

            v2f vert (appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy; // Assuming y-coordinates are normalized between 0 and 1
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Calculate the fade based on the _Cutoff value
                float fadeFactor = saturate((1.0 - i.uv.y) / _Cutoff);
                
                // Ensure that cutoff is never zero to avoid division by zero issues
                fadeFactor = fadeFactor * (i.uv.y > _Cutoff ? 0.0 : 1.0); 

                fixed4 color = _Color;
                color.a *= fadeFactor * _Alpha; // Apply fade based on cutoff and transparency
                return color;
            }
            ENDCG
        }
    }
}
