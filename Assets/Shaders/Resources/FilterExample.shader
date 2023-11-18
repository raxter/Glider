Shader "Hidden/FilterExample"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off //ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			sampler2D _RaymarchingTexture;

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

			fixed4 frag (v2f_img i) : SV_Target
			{
				i.uv.y = 1- i.uv.y;
				fixed4 col = tex2D(_MainTex, i.uv);

				fixed4 rm = tex2D(_RaymarchingTexture, i.uv).aaaa;
				//return float4(0,0,col.b, 1);
				col = lerp(tex2D(_RaymarchingTexture, i.uv), col, 1-rm.a);
				return col;
			}
			ENDCG
		}
	}
}
