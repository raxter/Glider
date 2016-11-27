Shader "Hidden/RadialBlurGrr" {
	Properties{
		_MainTex("-", 2D) = "" {}
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	sampler2D _MainTex;
	float4 _MainTex_TexelSize; 
	float _BlurAmount;
	float2 _Offset;

	float2 itr(int n, v2f_img i) {
		float ba = 1 - n * _BlurAmount / 20;
		return (i.uv*ba) + ((1 - ba) / 2) * (1-_Offset);
	}
	
	v2f_img vert(appdata_img v)
	{
		v2f_img o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord;
#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			o.uv.y = 1 - o.uv.y;
#endif
		return o;
	}

	half4 frag(v2f_img i) : SV_Target{
		half4 src = tex2D(_MainTex, i.uv);
		src += tex2D(_MainTex, itr(1, i));
		src += tex2D(_MainTex, itr(2, i));
		src += tex2D(_MainTex, itr(3, i));
		src += tex2D(_MainTex, itr(4, i));
		src += tex2D(_MainTex, itr(5, i));
		src += tex2D(_MainTex, itr(6, i));
		src += tex2D(_MainTex, itr(7, i));
		src += tex2D(_MainTex, itr(8, i));
		src += tex2D(_MainTex, itr(9, i));

		return src/10;
	}
	ENDCG

	SubShader {
		Pass{
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
}
