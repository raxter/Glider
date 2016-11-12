Shader "Hidden/Raymarching"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ColorLight ("Color Light", Color) = (1,1,1,1)
		_ColorShadow ("Color Shadow", Color) = (1,1,1,1)
		_ColorBackground ("Color Background", Color) = (0,0,0,1)
		_Texture ("Texture", 2D) = "white" {}
		_RadiusSphere ("Radius Sphere", Float) = 4.0
		_FOV ("FOV", Range(0.1, 2.0)) = 1.0
		_DivisionThinckness ("Division Thinckness", Range(0.0, 1.0)) = 0.1
		_DivisionRange ("Division Range", Range(0.0, 1.0)) = 0.1
		_InputRatio ("Input Ratio", Range(0,1)) = 0
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "noise.cginc"
			#include "hg_sdf.cginc"
			
			sampler2D _MainTex;
			sampler2D _Texture;
			float4 _MainTex_ST;
			float3 _CameraForward;
			float3 _CameraUp;
			float3 _CameraRight;
			float _RadiusSphere;
			float4 _ColorLight;
			float4 _ColorShadow;
			float4 _ColorBackground;
			float _FOV;
			float _DivisionThinckness;
			float _DivisionRange;
			float _InputRatio;

			#define rayEpsilon 0.0001
			#define rayMax 100.0
			#define rayStepMax 16

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0; 
				float3 viewDir : TEXCOORD1; 
			};

			v2f vert (appdata_full v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
				return o;
			}

			float wrapUV (float2 uv)
			{
				return fmod(abs(uv), 1.0);
			}

			float reflectance(float3 a, float3 b) { 
				return dot(normalize(a), normalize(b)) * 0.5 + 0.5; 
			}

			// GLKITTY 2016.
			float noise(float3 p, float speed){

				float t = _Time.y;
				float3 np = normalize(p);

				// kind of bi-planar mapping
				float a = tex2D(_Texture,abs(frac(speed*t/20.+np.xy))).x;      
				float b = tex2D(_Texture,abs(frac(speed*t/20.+.77+np.yz))).x;

				a = lerp(a,.5,abs(np.x));
				b = lerp(b,.5,abs(np.z));

				float noise = a+b-.4;    
				noise = lerp(noise,.5,abs(np.y)/2.);

				return noise;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 color = _ColorBackground;

				float3 eye = _WorldSpaceCameraPos;
				eye.y = 1;
				eye.z = -10;
				float3 front = _CameraForward * _FOV;
				float3 up = _CameraUp;
				float3 right = _CameraRight;

				float radiusSphere = 0.5;

				float2 uv = i.uv.xy;
				uv.y = 1.0 - uv.y;
				uv = uv * 2.0 - 1.0;
				uv.x *= _ScreenParams.x / _ScreenParams.y;
				float3 ray = normalize(front + right * uv.x + up * uv.y);
				
				float stepTotal = 0.0;
				for (int index = 0; index < rayStepMax; ++index) {
					float3 position = eye + ray * stepTotal;
					float angle = atan2(position.z, position.x);
					float movement = sin(angle + _Time.y * 4.) * 0.02;
					movement *=  _InputRatio;
					float displacementDiv = _DivisionThinckness / smoothstep(0.0, _DivisionRange, movement + abs(position.y / 5.));
					
					position.xz /= 1.0 + displacementDiv * _InputRatio;
					float sphere = length(position) - _RadiusSphere;
					float displacementBen = noise(position, 1);
					float dist = sphere + displacementBen;

					if (dist < rayEpsilon) {
						float4 c = _ColorLight;
						float ratio = (float)index / (float)rayStepMax;
						color = lerp(c, _ColorShadow, ratio);
						color = lerp(color, _ColorBackground, stepTotal / rayMax);
						break;
					}

					stepTotal += dist;
				}
				return color;
				// float4 background = tex2D(_MainTex, i.uv);
				// return lerp(color, background, background.a);
			}
			ENDCG
		}
	}
}
