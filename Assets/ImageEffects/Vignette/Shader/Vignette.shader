Shader "Hidden/VignetteGrr" {
    Properties {
        _MainTex("-", 2D) = "" {}
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    sampler2D _MainTex;
    float2 _Aspect;
    float _Falloff;
	float _Strength;
	half4 _Color;

	float4 _MainTex_TexelSize;

	v2f_img vert(appdata_img v)
	{
		v2f_img o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord;
#if UNITY_UV_STARTS_AT_TOP
//		if (_MainTex_TexelSize.y < 0)
//			o.uv.y = 1 - o.uv.y;
#endif
		return o;
	}

    half4 frag(v2f_img i) : SV_Target {
        float2 coord = (i.uv - 0.5) * _Aspect * _Strength;
        float rf = sqrt(dot(coord, coord)) * _Falloff;

		float rf2_1 = rf * rf + 1.0;
        float e = 1.0 / (rf2_1 * rf2_1);

        half4 src = tex2D(_MainTex, i.uv);
        return (src - _Color) * e + _Color;
    }
    ENDCG

    SubShader {
        Pass {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
}
