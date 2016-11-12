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
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _RaymarchingTexture;

			fixed4 frag (v2f_img i) : SV_Target
			{
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
