Shader "Custom/LabGradient"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_StartColor ("Start Color", Color) = (1,1,1,1)
		_FinishColor ("Finish Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		fixed4 _StartColor;
		fixed4 _FinishColor;

		
		struct LABColor {
			float L;
			float A;
			float B;
		};

		LABColor LABLerp(LABColor a, LABColor b, float t) {
			LABColor output;
			output.L = lerp(a.L, b.L, t);
			output.A = lerp(a.A, b.A, t);
			output.B = lerp(a.B, b.B, t);
			return output;
		}

		LABColor ToLAB (float4 c) {
			LABColor lab;
			float D65x = 0.9505;
			float D65y = 1.0;
			float D65z = 1.0890;
			float rLinear = c.r;
			float gLinear = c.g;
			float bLinear = c.b;
			float r = (rLinear > 0.04045f) ? pow((rLinear + 0.055) / (1.055), 2.2) : (rLinear / 12.92) ;
			float g = (gLinear > 0.04045f) ? pow((gLinear + 0.055) / (1.055), 2.2) : (gLinear / 12.92) ;
			float b = (bLinear > 0.04045f) ? pow((bLinear + 0.055) / (1.055), 2.2) : (bLinear / 12.92) ;
			float x = (r * 0.4124f + g * 0.3576f + b * 0.1805f);
			float y = (r * 0.2126f + g * 0.7152f + b * 0.0722f);
			float z = (r * 0.0193f + g * 0.1192f + b * 0.9505f);
			x = (x > 0.9505) ? 0.9505 : ((x < 0) ? 0 : x);
			y = (y > 1.0) ? 1.0 : ((y < 0) ? 0 : y);
			z = (z > 1.089) ? 1.089 : ((z < 0)? 0 : z);
			float fx = x / D65x;
			float fy = y / D65y;
			float fz = z / D65z;
			fx = ((fx > 0.008856) ? pow(fx, (1.0f/3.0)) : (7.787 * fx + 16.0 / 116.0));
			fy = ((fy > 0.008856) ? pow(fy, (1.0f/3.0)) : (7.787 * fy + 16.0 / 116.0));
			fz = ((fz > 0.008856) ? pow(fz, (1.0f/3.0)) : (7.787 * fz + 16.0 / 116.0));
			lab.L = 116.0 * fy - 16;
			lab.A = 500.0 * (fx - fy);
			lab.B = 200.0 * (fy - fz);
			return lab;
		}

		float4 FromLAB (LABColor lab) {
			float D65x = 0.9505;
			float D65y = 1.0;
			float D65z = 1.0890;
			float delta = 6.0/29.0;
			float fy = (lab.L+16)/116.0;
			float fx = fy + (lab.A/500.0);
			float fz = fy - (lab.B/200.0);
			float x = (fx > delta)? D65x * (fx*fx*fx) : (fx - 16.0/116.0)*3.0*(delta*delta)*D65x;
			float y = (fy > delta)? D65y * (fy*fy*fy) : (fy - 16.0/116.0)*3.0*(delta*delta)*D65y;
			float z = (fz > delta)? D65z * (fz*fz*fz) : (fz - 16.0/116.0)*3.0*(delta*delta)*D65z;
			float r = x*3.2410 - y*1.5374 - z*0.4986;
			float g = -x*0.9692 + y*1.8760 - z*0.0416;
			float b = x*0.0556 - y*0.2040 + z*1.0570;
			r = (r<=0.0031308)? 12.92*r : (1.055)* pow(r, (1.0/2.4)) - 0.055;
			g = (g<=0.0031308)? 12.92*g : (1.055)* pow(g, (1.0/2.4)) - 0.055;
			b = (b<=0.0031308)? 12.92*b : (1.055)* pow(b, (1.0/2.4)) - 0.055;
			r = (r<0)? 0 : r;
			g = (g<0)? 0 : g;
			b = (b<0)? 0 : b;
			return float4(r, g, b, 1.0);
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float4 col = FromLAB(LABLerp (ToLAB (_FinishColor), ToLAB (_StartColor), IN.uv_MainTex.y));
			col.a = lerp(_FinishColor.a, _StartColor.a, IN.uv_MainTex.y);
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * col;
            o.Albedo = c.rgb;
            o.Metallic = 0;
            o.Smoothness = 0;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
