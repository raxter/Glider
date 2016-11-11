Shader "Custom/Sky Colour Box" {
	Properties{
		_Cube("Environment Map", Cube) = "white" {}
		_MainTex("Pallete", 2D) = "white" {}
		_Radius("Radius", float) = 0.05
		_Depth ("Depth", float) = 0
	}

		SubShader{
		Tags{ "Queue" = "Background" }

		Pass{
		ZWrite Off
		Cull Off

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		// User-specified uniforms
		samplerCUBE _Cube;

		sampler2D _MainTex;
		float4 _MainTex_TexelSize;

		float _Radius;
		float _Depth;

	struct vertexInput {
		float4 vertex : POSITION;
		float3 texcoord : TEXCOORD0;
	};

	struct vertexOutput {
		float4 vertex : SV_POSITION;
		float3 texcoord : TEXCOORD0;
	};


	vertexOutput vert(vertexInput input)
	{
		vertexOutput output;
		output.vertex = mul(UNITY_MATRIX_MVP, input.vertex);
		output.texcoord = input.texcoord;
		return output;
	}

	fixed4 RandomColour(float index)
	{
		index = round(index);

		index %= _MainTex_TexelSize.z;

		index /= _MainTex_TexelSize.z;

		return tex2D(_MainTex, float2(index, 0));
	}

	fixed4 frag(vertexOutput input) : COLOR
	{
		//return texCUBE(_Cube, input.texcoord);
		// TODO put this not in the fragment shader! this only needs to be done once
		int topIndex = floor(_Depth / 2);
		fixed4 colourBottom = RandomColour(topIndex);
		fixed4 colourTop = RandomColour(topIndex+1);

		float heightAdjust = 0;
		heightAdjust += sin(input.texcoord.x * 10 + _Time.y)*0.6;
		heightAdjust += sin(input.texcoord.x * 5.3 - _Time.y * 2)*0.5;

		heightAdjust += sin(input.texcoord.x * 52 + _Time.y*2)*0.2;
		heightAdjust += sin(input.texcoord.x * 23 - _Time.y * 2)*0.2;
		//heightAdjust *= sin(input.texcoord.x * 3 - _Time.y*2.234);
		//heightAdjust = round(heightAdjust * 5) / 5;
		heightAdjust /= 20;
		//heightAdjust = round(input.texcoord.x * 10) / 10 /3;
		float radius = _Radius;
		float lerpFactor = input.texcoord.y;
		//lerpFactor = sin(lerpFactor*3.14159/2);
		lerpFactor += heightAdjust;
		lerpFactor /= radius;
		lerpFactor /= (sin(input.texcoord.x * 30 - _Time.y*0.234) + 5) / 7;
		lerpFactor /= (sin(input.texcoord.x * 27 + _Time.y*0.634) + 5) / 7;
		//lerpFactor /= sin(input.texcoord.x * 5 + _Time.y*1.634);
		lerpFactor = (lerpFactor + 1) / 2;

		lerpFactor -= frac(_Depth/2 - 1)*8 - 4;

		lerpFactor = round(lerpFactor * 5) / 5;

		return lerp(colourTop, colourBottom, saturate(lerpFactor));
	}
		ENDCG
	}
	}
}
