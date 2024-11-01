Shader "UI/GradientColorShaderWithSmoothTransparency" {
    Properties {
        _ColorTop ("Top Color", Color) = (1, 0, 0, 1)
        _ColorBottom ("Bottom Color", Color) = (0, 0, 1, 0.2)
        _Alpha ("Transparency", Range(0, 1)) = 1.0
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

            float4 _ColorTop;
            float4 _ColorBottom;
            float _Alpha;

            v2f vert (appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Interpolate color based on vertical position (uv.y) with smoother alpha blending
                fixed4 color = lerp(_ColorBottom, _ColorTop, i.uv.y);
                color.a *= _Alpha * lerp(_ColorBottom.a, _ColorTop.a, i.uv.y); // Smooth transparency transition
                return color;
            }
            ENDCG
        }
    }
}
